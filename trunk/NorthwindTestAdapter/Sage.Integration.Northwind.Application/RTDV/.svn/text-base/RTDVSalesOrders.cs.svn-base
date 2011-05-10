#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
    File:        RTDVSalesOrders.cs
    Author:      pschuette
    DateCreated: 10-19-2007
    DateChanged: 10-19-2007
    ---------------------------------------------------------------------
    ©2007 Sage Technology Ltd., All Rights Reserved. 
=====================================================================*/

/*=====================================================================
    Date        Author       Changes     Reasons
    10-19-2007  pschuette  Create   
    

    Changes: Create, Refactoring, Upgrade 
=====================================================================*/
#endregion

#region Usings

using System;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.Xml;

using Sage.Integration.Northwind.Application.EntityResources;
using Sage.Integration.Northwind.Application.Toolkit;

using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Properties;

#endregion

namespace Sage.Integration.Northwind.Application.RTDV
{
    public class RTDVSalesOrders : RTDVBase
    {
        #region Fields

        private Dictionary<string, RTDVField> _rtdvFieldDictionary;

        #endregion

        #region Ctor.

        public RTDVSalesOrders()
            : base(Constants.RTDVNames.SalesInvoices, "sal")
        {

        }

        #endregion

        protected override Dictionary<string, RTDVField> RTDVFieldDictionary
        {
            get
            {
                if (null == _rtdvFieldDictionary)
                    _rtdvFieldDictionary = this.CreateRTDVFieldDictionary();

                return _rtdvFieldDictionary;
            }
        }
        protected override string IdFieldName
        {
            get { return "OrderID"; }
        }
        protected override string GetSqlQueryNameRepresentation(string fieldName)
        {
            switch (fieldName)
            {
                case "intforeignid":
                case "AccountID":
                    return "'C-' & Orders.CustomerID";
                case "Type":
                    return "'Invoice'";
                case "OrderID":
                    return "Orders.OrderID";
                case "CreatedBy":
                    return "Employees.FirstName + ' ' + Employees.LastName";
                case "Value":
                    return "SUM(([Order Details].Quantity * [Order Details].UnitPrice) * (1 - [Order Details].Discount)) + Orders.Freight";
                case "OrderDate":
                    return "Orders.OrderDate";
                case "DatePaymentDue":
                    return "'14'";
                default:
                    throw new ArgumentException(string.Format(Resources.ErrorMessages_InvalidFieldNameForReport, fieldName, Constants.RTDVNames.SalesInvoices));
            }
        }
        protected override string GetSqlQueryTemplate()
        {
            // it has the format: SELECT... FROM... WHERE {0} GROUPBY... HAVING {1} ORDERBY{2}
            return  "SELECT 'Invoice' AS Type, Orders.OrderID, Employees.FirstName + ' ' + Employees.LastName AS CreatedBy, " +
                    "SUM(([Order Details].Quantity * [Order Details].UnitPrice) * (1 - [Order Details].Discount)) + Orders.Freight AS [Value], " +
                    "Orders.OrderDate, '14' AS DatePaymentDue, 'C-' & Orders.CustomerID AS AccountID " +
                    "FROM (((Orders INNER JOIN Employees ON Orders.EmployeeID = Employees.EmployeeID) INNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID) INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID) " +
                    "{0} " +
                    "GROUP BY 'C-' & Orders.CustomerID, Orders.OrderID, Employees.LastName, Employees.FirstName, Customers.CustomerID, Orders.OrderDate, Orders.Freight, 'C-' & Orders.CustomerID " +
                    "{1} {2}";
        }
        protected override SearchField[] GetHavingFields(SearchField[] searchFields)
        {
            List<SearchField> havingFields;

            // initializations
            havingFields = new List<SearchField>();

            // iterate through all search fields and copy a reference of the 'having' fields to the result list.
            foreach (SearchField field in searchFields)
            {
                if (field.Name == "OrderID" || field.Name == "Value" || field.Name == "OrderDate" || field.Name == "AccountID" || field.Name == "intforeignid")
                {
                    havingFields.Add(field);
                }
            }

            return havingFields.ToArray(); 
        }
        protected override SearchField[] GetWhereFields(SearchField[] searchFields)
        {
             List<SearchField> whereFields;

            // initializations
            whereFields = new List<SearchField>();

            // iterate through all search fields and copy a reference of the 'where' fields to the result list.
            foreach (SearchField field in searchFields)
            {
                if (field.Name == "Type" || field.Name == "CreatedBy" || field.Name == "DatePaymentDue")
                {
                    whereFields.Add(field);
                }
            }

            return whereFields.ToArray(); 
        }
        protected override OleDbParameter CreateOleDbParameter(string fieldName, string fieldValue)
        {
            OleDbParameter newParameter;

            switch (fieldName)
            {
                case "intforeignid":
                case "AccountID":
                    newParameter = new System.Data.OleDb.OleDbParameter("AccountID", System.Data.OleDb.OleDbType.VarWChar, 1024, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Account", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "Type":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param1", System.Data.OleDb.OleDbType.VarChar, 1024, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "CreatedBy":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param2", System.Data.OleDb.OleDbType.VarWChar, 1024, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "DatePaymentDue":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param3", System.Data.OleDb.OleDbType.Integer, 1024, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;
                case "OrderID":
                    newParameter = new System.Data.OleDb.OleDbParameter("OrderID", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "OrderID", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;
                case "Value":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param5", System.Data.OleDb.OleDbType.Decimal, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDecimal(fieldValue);
                    return newParameter;
                case "OrderDate":
                    newParameter = new System.Data.OleDb.OleDbParameter("OrderDate", System.Data.OleDb.OleDbType.Date, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "OrderDate", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDateTime(fieldValue);
                    return newParameter;
                default:
                    throw new ArgumentException(string.Format(Resources.ErrorMessages_InvalidFieldNameForReport, fieldName, Constants.RTDVNames.SalesInvoices));
            }
        }


        #region Private Helper Members

        private Dictionary<string, RTDVField> CreateRTDVFieldDictionary()
        {
            Dictionary<string, RTDVField> dictionary;
            RTDVField field;

            // create a new dictionary
            dictionary = new Dictionary<string, RTDVField>();

            //AccountID
            field = new RTDVField("AccountID", TypeCode.String);
            field.DisplayName = "Account ID";
            field.Size = 1024;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("AccountID", field);

            //Type
            field = new RTDVField("Type", TypeCode.String);
            field.DisplayName = "Type";
            field.Size = 1024;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Type", field);
            
            //CreatedBy
            field = new RTDVField("CreatedBy", TypeCode.String);
            field.DisplayName = "Created By";
            field.Size = 1024;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("CreatedBy", field);

            //DatePaymentDue
            field = new RTDVField("DatePaymentDue", TypeCode.Int32);
            field.DisplayName = "Date Payment Due";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("DatePaymentDue", field);

            //OrderID
            field = new RTDVField("OrderID", TypeCode.Int32);
            field.DisplayName = "Order ID";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("OrderID", field);

            //Value
            field = new RTDVField("Value", TypeCode.Decimal);
            field.DisplayName = "Value";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Value", field);

            //Value
            field = new RTDVField("OrderDate", TypeCode.DateTime);
            field.DisplayName = "Order Date";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("OrderDate", field);


            return dictionary;
        }

        #endregion
    }
}
