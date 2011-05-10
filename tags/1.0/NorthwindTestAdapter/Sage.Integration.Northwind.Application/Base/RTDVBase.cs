#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
    File:        RealtimeDataViewingBase.cs
    Author:      msassanelli
    DateCreated: 04-27-2007
    DateChanged: 04-27-2007
    ---------------------------------------------------------------------
    ©2007 Sage Technology Ltd., All Rights Reserved. 
=====================================================================*/

/*=====================================================================
    Date        Author       Changes     Reasons
    04-27-2007  msassanelli  Create   
    

    Changes: Create, Refactoring, Upgrade 
=====================================================================*/
#endregion

#region Usings

using System;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.Collections.Generic;

using Sage.Integration.Northwind.Application.Properties;
using Sage.Integration.Northwind.Application.EntityResources;
using Sage.Integration.Northwind.Application.Toolkit;
using System.Xml.Schema;
using Sage.Integration.Northwind.Application.API;



#endregion

namespace Sage.Integration.Northwind.Application.Base
{
    public abstract class RTDVBase
    {
        #region Fields

        private string _reportName;     // the name of the report
        private string _reportPrefix;    // the prefix of the report (used in xml schema)

        #endregion

        #region Ctor.

        public RTDVBase(string reportName, string reportPrefix)
        {
            _reportName = reportName;
            _reportPrefix = reportPrefix;
        }

        #endregion

        #region Abstract Members

        abstract protected Dictionary<string, RTDVField> RTDVFieldDictionary { get; }
        abstract protected string IdFieldName { get; }
        abstract protected string GetSqlQueryTemplate();
        abstract protected SearchField[] GetHavingFields(SearchField[] list);
        abstract protected SearchField[] GetWhereFields(SearchField[] list);
        abstract protected OleDbParameter CreateOleDbParameter(string fieldName, string fieldValue);

        #endregion

        #region public Methods

        public XmlNode ViewRealTimeData(string entityName, string[] selectFields, SearchField[] searchFields, string[] orderFields, int rowsPerPage, int pageNumber, NorthwindConfig config)
        {
            // declarations
            string sqlQuery;
            DataSet resultDataSet;
            XmlDocument resultXmlDoc;
            SearchField[] whereFieldList;
            SearchField[] havingFieldList;
            List<OleDbParameter> oleDbParameterList;

            // initializations
            resultDataSet = null;
            oleDbParameterList = null;

            // fill the where and having field lists
            whereFieldList = this.GetWhereFields(searchFields);
            havingFieldList = this.GetHavingFields(searchFields);

            // Create an OleDbCommand .
            sqlQuery = this.CreateSqlQuery(whereFieldList, havingFieldList, orderFields, out oleDbParameterList);

            // Get data from database
            this.Fill(sqlQuery, oleDbParameterList, config, out resultDataSet);

            // Convert result xml document to the CRM contract format
            resultXmlDoc = this.ConvertToXmlDocument(resultDataSet, selectFields, pageNumber, rowsPerPage);
           
            // return the root node xml document
            return (XmlNode)resultXmlDoc.DocumentElement;
        }


        public XmlSchemaElement GetXmlSchemaElement()
        {
            // declarations
            XmlDocument xmlDoc;
            XmlSchemaElement xmlSchemaElement;

            // initializations
            xmlDoc = new XmlDocument();
            
            #region <xs:element name="RTDVName" prefix="reportPrefix" isprimarytable="y" idfield="IDField">

            xmlSchemaElement = new XmlSchemaElement();

            xmlSchemaElement.Name = _reportName;
            
            XmlAttribute attributePrefix = xmlDoc.CreateAttribute("prefix");
            attributePrefix.Value = _reportPrefix;
            XmlAttribute attributePrimaryTable = xmlDoc.CreateAttribute("isprimarytable");
            attributePrimaryTable.Value = "y";
            XmlAttribute attributeIdField = xmlDoc.CreateAttribute("idfield");
            attributeIdField.Value = this.IdFieldName;
            xmlSchemaElement.UnhandledAttributes = new XmlAttribute[] { attributePrefix, attributePrimaryTable, attributeIdField };

            #endregion

            #region <xs:complexType>

            XmlSchemaComplexType complexType = new XmlSchemaComplexType();
            xmlSchemaElement.SchemaType = complexType;



            #region <xs:sequence>

            XmlSchemaSequence sequence = new XmlSchemaSequence();
            complexType.Particle = sequence;

            #endregion

            foreach (RTDVField field in this.RTDVFieldDictionary.Values)
                sequence.Items.Add(field.GetSchemaElement(xmlDoc));

            #endregion

            return xmlSchemaElement;
        }

        #endregion

        #region Protected Members

        protected virtual string CreateSqlQuery(SearchField[] whereFields, SearchField[] havingFields, string[] orderFields, out List<OleDbParameter> oleDbParameterList)
        {
            // declarations
            string sqlQuery;
            string sqlQueryTemplate;
            string sqlWhereClause;
            string sqlHavingClause;
            string sqlOrderClause;

            // initializations
            oleDbParameterList = new List<OleDbParameter>();

            // Get the SQL query template.
            // it has the format: SELECT... FROM... WHERE {0} GROUPBY... HAVING {1} ORDERBY{2}
            sqlQueryTemplate = this.GetSqlQueryTemplate();

            // Create the 'WHERE' clause of the SQL query
            sqlWhereClause = this.CreateWhereClause(whereFields, ref oleDbParameterList);

            // Create the 'HAVING' clause of the SQL query
            sqlHavingClause = this.CreateHavingClause(havingFields, ref oleDbParameterList);

            // Create the 'ORDERBY' clause of the SQL query
            sqlOrderClause = this.CreateOrderClause(orderFields);

            // create the complete SQL query
            sqlQuery = this.ConcatSqlQuery(sqlQueryTemplate, sqlWhereClause, sqlHavingClause , sqlOrderClause);

            return sqlQuery;
        }
        protected virtual string CreateWhereClause(SearchField[] searchFields, ref List<OleDbParameter> oleDbParameterList)
        {
            // declarations
            string whereClause;                                 // the result string.
            string fieldName;                                   // the field name to enter into the WHERE clause.
            OperatorValue operationTag;                        // the operator of the Search (WHERE or HAVING) clause.
            string fieldValue;                                  // the value to enter into the WHERE clause.

            string sqlFieldName;                                // the representation of the field name in the SQL query.
            string sqlOperationTag;                             // the representation of the operation tag in the SQL query.
            string sqlFieldValue;                               // the representation od the field value (i.e. add % when operator is LIKE)

            // initialize where clause
            whereClause = string.Empty;

            if (searchFields.Length == 0)
                return whereClause;
            else
                whereClause = " WHERE ";

            try
            {
                // iterate through the search fields and build the where clause 
                foreach (SearchField field in searchFields)
                {
                    fieldName = field.Name;                 // get the field name
                    operationTag = field.Operator;          // get the operator
                    fieldValue = field.Value;               // get the field value
                    
                    sqlFieldName = this.GetSqlQueryNameRepresentation(fieldName);                           // get the SQL query representation of the field name
                    if ((sqlFieldName == null) || (sqlFieldName == string.Empty))
                        continue;
                    
                    
                    sqlOperationTag = this.GetSqlQueryOperatorRepresentation(operationTag);                 // get the SQL query representation of the operation tag
                    sqlFieldValue = this.GetSqlQueryFieldValueRepresentation(fieldValue, operationTag);     // get the SQL query representation of the field value (i.e. for Contains: %myValue%)
                    
                    // Create a new parameter
                    oleDbParameterList.Add(this.CreateOleDbParameter(fieldName, sqlFieldValue));

                    // enter field name using the SQL representation.
                    whereClause += "(" + sqlFieldName;
                    
                    // enter operator tag
                    whereClause += sqlOperationTag;

                    // enter placeholder
                    whereClause += "?)";

                    // enter separator
                    whereClause += "AND ";
                }
            }
            catch (Exception exception)
            {
#if DEBUG
                throw new ArgumentException(string.Format(Resources.ErrorMessages_RTDVSQLQueryBuildError_Debug, _reportName, whereClause, this.GetSqlQueryTemplate()), exception);
#else
                throw new ArgumentException(string.Format(Resources.ErrorMessages_RTDVSQLQueryBuildError_Debug, _reportName), exception);
#endif
            }

            // remove the last separator
            whereClause = whereClause.Remove(whereClause.Length - 4);

            return whereClause;
        }
        protected virtual string CreateHavingClause(SearchField[] searchFields, ref List<OleDbParameter> oleDbParameterList)
        {
            // declarations
            string havingClause;                                // the result string.
            string fieldName;                                   // the field name to enter into the HAVING clause.
            OperatorValue operationTag;                        // the operator of the HAVING clause.  
            string fieldValue;                                  // the value to enter into the HAVING clause.

            string sqlFieldName;                                // the representation of the field name in the SQL query.
            string sqlOperationTag;                             // the representation of the operation tag in the SQL query.
            string sqlFieldValue;                               // the representation od the field value (i.e. add % when operator is LIKE)

            // initialize where clause
            havingClause = string.Empty;

            if (searchFields.Length == 0)
                return havingClause;
            else
                havingClause = " HAVING ";

            try
            {
                // iterate through the search fields and build the where clause 
                foreach (SearchField field in searchFields)
                {
			        fieldName = field.Name;                                                 // get the field name
			        operationTag = field.Operator;                                          // get the operator
			        fieldValue = field.Value;												// get the field value

                    sqlFieldName = this.GetSqlQueryNameRepresentation(fieldName);                           // get the SQL query representation of the field name
                    sqlOperationTag = this.GetSqlQueryOperatorRepresentation(operationTag);                 // get the SQL query representation of the operation tag
                    sqlFieldValue = this.GetSqlQueryFieldValueRepresentation(fieldValue, operationTag);     // get the SQL query representation of the field value (i.e. for Contains: %myValue%)

                    // Create a new parameter
                    oleDbParameterList.Add(this.CreateOleDbParameter(fieldName, sqlFieldValue));

                    // enter field name using the SQL representation.
                    havingClause += "(" + sqlFieldName;

                    // enter operator tag
                    havingClause += sqlOperationTag;

                    // enter placeholder
                    havingClause += "?)";

                    // enter separator
                    havingClause += "AND ";
                }
            }
            catch (Exception exception)
            {
#if DEBUG
                throw new ArgumentException(string.Format(Resources.ErrorMessages_RTDVSQLQueryBuildError_Debug, _reportName, havingClause + "_"), exception);
#else
                throw new ArgumentException(string.Format(Resources.ErrorMessages_RTDVSQLQueryBuildError_Debug, _reportName), exception);
#endif
            }

            // remove the last separator
            havingClause = havingClause.Remove(havingClause.Length - 4);

            return havingClause;
        }
        protected virtual string GetSqlQueryOperatorRepresentation(OperatorValue operationTag)
        {
            switch (operationTag)
            {
                case OperatorValue.Equals:
                    return " = ";
                case OperatorValue.GreaterThanEquals:
                    return " >= ";
                case OperatorValue.LessThanEquals:
                    return " <= ";
                case OperatorValue.NotEquals:
                    return " <> ";
                case OperatorValue.Begins:
                case OperatorValue.Contains:
                    return " LIKE ";
                default:
                    throw new FormatException(string.Format(Resources.ErrorMessages_OperationTagNotSupported, operationTag.ToString()));
            }
        }
        protected virtual string GetSqlQueryNameRepresentation(string fieldName)
        {
            return fieldName;
        }
        protected virtual string GetSqlQueryFieldValueRepresentation(string fieldValue, OperatorValue operationTag)
        {
            if (operationTag == OperatorValue.Begins)
                return fieldValue + "%";

            if (operationTag == OperatorValue.Contains)
                return "%" + fieldValue + "%";

            return fieldValue;
        }
        protected virtual string CreateOrderClause(string[] orderFields)
        {
            // declarations
            string orderClause;             // the result string
            string fieldName;               // the field name to enter into the ORDER clause.

            // initialize order clause
            orderClause = string.Empty;

            if (orderFields.Length == 0)
                return orderClause;
            else
                orderClause = " ORDER BY ";

            // iterate through the order fields and build the order clause 
            foreach (string field in orderFields)
            {
                fieldName = field;      // get the field name

                // enter field name using the SQL representation
                orderClause += this.GetSqlQueryNameRepresentation(fieldName);

                // enter separator
                orderClause += ", ";
            }

            // remove the last separator
            orderClause = orderClause.Remove(orderClause.Length - 2);

            return orderClause;
        }
        protected virtual string ConcatSqlQuery(string sqlQueryTemplate, params string[] replacers)
        {
            return string.Format(sqlQueryTemplate, replacers);
        }
        protected virtual int Fill(string sqlQuery, List<OleDbParameter> oleDbParameterList, NorthwindConfig config, out DataSet dataSet)
        {
            // declarations
            OleDbDataAdapter dataAdapter;
            int nOfRows;

            // initializations
            dataSet = null;
            nOfRows = 0;
            dataSet = new DataSet();

            // get the data base records using a table adapter.
            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            {
                dataAdapter = new OleDbDataAdapter(sqlQuery, connection);
                dataAdapter.SelectCommand.Parameters.AddRange(oleDbParameterList.ToArray());
                nOfRows = dataAdapter.Fill(dataSet, _reportName);
            }

            return nOfRows;
        }
        protected virtual XmlDocument ConvertToXmlDocument(DataSet dataSet, string[] selectedFields, int pageNumber, int rowsPerPage)
        {
            DataRowCollection dataRowCollection;
            XmlDocument xmlDoc;
            XmlNode rootElement;
            //XmlNode dataRowElement;
            XmlNode rowElement;
            DataRow dataRow;
            int startRecord;
            int endRecord;

            // initializations
            dataRowCollection = dataSet.Tables[_reportName].Rows;       // get the collection of rows
            xmlDoc = new XmlDocument();                                 // create a new XmlDocument

            // calculate start and end record for row iteration
            startRecord = this.ResponseCalcStartPosition(pageNumber, rowsPerPage);
            endRecord = startRecord + rowsPerPage - 1;

            // create the root node
            rootElement = this.ResponseCreateRootNode(xmlDoc, _reportName, dataRowCollection.Count);
            
            //// create row count node
            //rootElement.AppendChild(this.ResponseCreateRowCountNode(xmlDoc, dataRowCollection.Count));
            
            //// create data row node
            //dataRowElement = this.ResponseCreateDataRowNode(xmlDoc, _reportName);
            
            //
            // iterate through the records that are in the range of the requested page and create a field node for each.
            //
            for (int i = startRecord; (i <= endRecord && i < dataRowCollection.Count); i++)
            //int i = 0;
            {
                dataRow = dataRowCollection[i];     // get the data row at the index position

                rowElement = this.ResponseCreateRowNode(xmlDoc);    // create empty row node

                // iterate through all fields that should be added to the response
                foreach (string fieldName in selectedFields)
                    if (fieldName == "*")
                        foreach (DataColumn dc in dataRow.Table.Columns)
                            rowElement.AppendChild(this.ResponseCreateFieldNode(xmlDoc, dc.ColumnName, dataRow[dc.ColumnName]));
                    else
                        rowElement.AppendChild(this.ResponseCreateFieldNode(xmlDoc, fieldName, dataRow[fieldName]));

                // append the row element
                rootElement.AppendChild(rowElement);
            }

            //// append the data row element
            //rootElement.AppendChild(dataRowElement);

            // append the root node
            xmlDoc.AppendChild(rootElement);

            return xmlDoc;
        }

        #endregion

        #region Private Methods

        
        
        private int ResponseCalcStartPosition(int pageNumber, int rowsPerPage)
        {
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException("pageNumber", pageNumber, Resources.ErrorMessages_RTDVInvalidPageNumber);
            if (rowsPerPage < 0)
                throw new ArgumentOutOfRangeException("rowsPerPage", rowsPerPage, Resources.ErrorMessages_RTDVInvalidRowsPerPage);

            return (pageNumber - 1) * rowsPerPage;
        }

        private XmlNode ResponseCreateRootNode(XmlDocument xmlDoc, string reportName, int nOfRows)
        {

            //<datarow name="invoice" rowcount="52" xmlns="">
            XmlNode rootNode;
            XmlAttribute attribute;

            rootNode = xmlDoc.CreateElement("datarow");
            
            attribute = xmlDoc.CreateAttribute("name");
            //attribute.Value = Sage.Integration.Northwind.Common.Helpers.XmlHelper.xmlConvert(reportName);
            attribute.Value = Sage.Integration.Northwind.Common.XmlConverter.GetString(reportName);
            rootNode.Attributes.Append(attribute);

            attribute = xmlDoc.CreateAttribute("rowcount");
//            attribute.Value = Sage.Integration.Northwind.Common.Helpers.XmlHelper.xmlConvert(nOfRows);
            attribute.Value = Sage.Integration.Northwind.Common.XmlConverter.GetString(nOfRows);
            rootNode.Attributes.Append(attribute);


            attribute = xmlDoc.CreateAttribute("xmlns");
            attribute.Value = @"http://tempuri.org/";
            rootNode.Attributes.Append(attribute);

            return rootNode;
        }


        //private XmlNode ResponseCreateRootNode(XmlDocument xmlDoc)
        //{
        //    XmlNode rootNode;
        //    XmlAttribute attribute;

        //    rootNode = xmlDoc.CreateElement("getrealtimedataresponse");

        //    attribute = xmlDoc.CreateAttribute("xmlns");
        //    attribute.Value = @"http://tempuri.org/";
        //    rootNode.Attributes.Append(attribute);

        //    return rootNode;
        //}
        //private XmlNode ResponseCreateRowCountNode(XmlDocument xmlDoc, int nOfRows)
        //{
        //    XmlNode rowCountNode;

        //    rowCountNode = xmlDoc.CreateElement("rowcount");
        //    rowCountNode.InnerText = Helpers.XmlHelper.xmlConvert(nOfRows);
        //    return rowCountNode;
        //}

        //private XmlNode ResponseCreateDataRowNode(XmlDocument xmlDoc, string reportName)
        //{
        //    XmlNode dataRowNode;
        //    XmlAttribute attribute;

        //    dataRowNode = xmlDoc.CreateElement("datarow");
        //    attribute = xmlDoc.CreateAttribute("name");
        //    attribute.Value = Helpers.XmlHelper.xmlConvert(reportName);
        //    dataRowNode.Attributes.Append(attribute);

        //    return dataRowNode;
        //}
        private XmlNode ResponseCreateRowNode(XmlDocument xmlDoc)
        {
            return xmlDoc.CreateElement("row");
        }
        private XmlNode ResponseCreateFieldNode(XmlDocument xmlDoc, string fieldName, object value)
        {
            XmlNode fieldNode;
            XmlNode nameNode;
            XmlNode valueNode;

            fieldNode = xmlDoc.CreateElement("field");
            nameNode = xmlDoc.CreateElement("name");
            valueNode = xmlDoc.CreateElement("value");

            nameNode.InnerText = fieldName;
            //valueNode.InnerText = Sage.Integration.Northwind.Common.Helpers.XmlHelper.xmlConvert(value);
            valueNode.InnerText = Sage.Integration.Northwind.Common.XmlConverter.GetString(value);

            fieldNode.AppendChild(nameNode);
            fieldNode.AppendChild(valueNode);

            return fieldNode;
        }

        #endregion
    }
}
