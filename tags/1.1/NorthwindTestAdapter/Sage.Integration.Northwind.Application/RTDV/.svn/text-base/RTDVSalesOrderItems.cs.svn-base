#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
    File:        RTDVSalesOrderItems.cs
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
    public class RTDVSalesOrderItems : RTDVBase
    {
        #region Fields

        private Dictionary<string, RTDVField> _rtdvFieldDictionary;

        #endregion

        #region Ctor.

        public RTDVSalesOrderItems()
            : base(Constants.RTDVNames.SalesOrderItems, "soi")
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
                //OrderID
                case "OrderID":
                    return "[Order Details].OrderID";

                //ProductID
                case "ProductID":
                    return "[Order Details].ProductID";

                //UnitPrice
                case "UnitPrice":
                    return "[Order Details].UnitPrice";

                //Discount
                case "Discount":
                    return "[Order Details].Discount";

                //Quantity
                case "Quantity":
                    return "[Order Details].Quantity";
                default:
                    throw new ArgumentException(string.Format(Resources.ErrorMessages_InvalidFieldNameForReport, fieldName, Constants.RTDVNames.SalesInvoices));
            }
        }
        protected override string GetSqlQueryTemplate()
        {
            // it has the format: SELECT... FROM... WHERE {0} GROUPBY... HAVING {1} ORDERBY{2}
            return "select OrderID, ProductID, UnitPrice, Quantity, " +
                    "Round(Discount*100,0) as DicountPC, " +
                    "Round(UnitPrice * Quantity * (1-Discount),2) as Total " +
                    "from [order Details] " +
                    "{0} " +
                    "{1} {2}";
        }
        protected override SearchField[] GetHavingFields(SearchField[] searchFields)
        {
            return new SearchField[0];
        }

        protected override SearchField[] GetWhereFields(SearchField[] searchFields)
        {
            List<SearchField> whereFields;
            whereFields = new List<SearchField>();// (searchFields);
            foreach (SearchField sf in searchFields)
            {
                if (sf.Name.Equals("intforeignid"))
                    continue;
                whereFields.Add(sf);

            }
            return whereFields.ToArray();
        }
        protected override OleDbParameter CreateOleDbParameter(string fieldName, string fieldValue)
        {
            OleDbParameter newParameter;

            switch (fieldName)
            {
                case "intforeignid":
                //OrderID
                case "OrderID":
                    newParameter = new System.Data.OleDb.OleDbParameter("OrderID",
                    System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "OrderID",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;

                //ProductID
                case "ProductID":
                    newParameter = new System.Data.OleDb.OleDbParameter("ProductID",
                    System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "ProductID",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;

                //UnitPrice
                case "UnitPrice":
                    newParameter = new System.Data.OleDb.OleDbParameter("UnitPrice",
                    System.Data.OleDb.OleDbType.Decimal, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "UnitPrice",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDecimal(fieldValue);
                    return newParameter;

                //Discount
                case "Discount":
                    newParameter = new System.Data.OleDb.OleDbParameter("Discount",
                    System.Data.OleDb.OleDbType.Decimal, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "Discount",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDecimal(fieldValue);
                    return newParameter;

                //Quantity
                case "Quantity":
                    newParameter = new System.Data.OleDb.OleDbParameter("Quantity",
                    System.Data.OleDb.OleDbType.SmallInt, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "Quantity",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt16(fieldValue);
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

            //OrderID
            field = new RTDVField("OrderID", TypeCode.Int32);
            field.DisplayName = "OrderID";
            field.Size = 4;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("OrderID", field);


            //ProductID
            field = new RTDVField("ProductID", TypeCode.Int32);
            field.DisplayName = "ProductID";
            field.Size = 4;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ProductID", field);


            //UnitPrice
            field = new RTDVField("UnitPrice", TypeCode.Decimal);
            field.DisplayName = "UnitPrice";
            field.Size = 8;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("UnitPrice", field);


            //Discount
            field = new RTDVField("Discount", TypeCode.Decimal);
            field.DisplayName = "Discount";
            field.Size = 4;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Discount", field);


            //Quantity
            field = new RTDVField("Quantity", TypeCode.Int16);
            field.DisplayName = "Quantity";
            field.Size = 2;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Quantity", field);

            return dictionary;
        }

        #endregion
    }
}
