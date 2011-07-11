#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
    File:        RTDVProductsPurchased.cs
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
    public class RTDVProductsPurchased : RTDVBase
    {
        #region Fields

        private Dictionary<string, RTDVField> _rtdvFieldDictionary;

        #endregion

        #region Ctor.

        public RTDVProductsPurchased()
            :base(Constants.RTDVNames.ProductsPurchased, "pro")
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
            get { return "ID"; }
        }
        protected override string GetSqlQueryNameRepresentation(string fieldName)
        {
            switch (fieldName)
            {
                case "ID":
                    return "[Order Details].ProductID & '-' & 'C-' & Orders.CustomerID";
                case "intforeignid":
                case "AccountID":
                    return "'C-' & Orders.CustomerID";
                case "ItemCode":
                    return "[Order Details].ProductID";
                case "ItemName":
                    return "Products.ProductName";
                case "Quantity":
                    return "SUM([Order Details].Quantity)";
                case "UnitPrice":
                    return "Products.UnitPrice";
                case "Value":
                    return "ROUND(SUM(([Order Details].UnitPrice * [Order Details].Quantity) * (1 - [Order Details].Discount)), 2)";
                case "LastPurchaseDate":
                    return "MAX(Orders.OrderDate)";
                case "Description":
                    return "NULL";
                case "Discount":
                    return "ROUND(SUM([Order Details].UnitPrice * [Order Details].Quantity * [Order Details].Discount), 2)";
                default:
                    throw new ArgumentException(string.Format(Resources.ErrorMessages_InvalidFieldNameForReport, fieldName, Constants.RTDVNames.ProductsPurchased));
            }
        }
        protected override string GetSqlQueryTemplate()
        {
            // it has the format: SELECT... FROM... WHERE {0} GROUPBY... HAVING {1} ORDERBY{2}
            return "SELECT [Order Details].ProductID & '-' & 'C-' & Orders.CustomerID AS [ID]," + 
                "[Order Details].ProductID AS [ItemCode], Products.ProductName AS [ItemName], " +
                "SUM([Order Details].Quantity) AS Quantity, Products.UnitPrice, " +
                "ROUND(SUM(([Order Details].UnitPrice * [Order Details].Quantity) * (1 - [Order Details].Discount)), 2) AS [Value], " +
                "MAX(Orders.OrderDate) AS [LastPurchaseDate], NULL AS Description, " +
                "ROUND(SUM([Order Details].UnitPrice * [Order Details].Quantity * [Order Details].Discount), 2) AS Discount, 'C-' & Orders.CustomerID AS AccountID " +
                "FROM (Products INNER JOIN (Orders INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID) ON Products.ProductID = [Order Details].ProductID) " +
                "{0} " +
                "GROUP BY 'C-' & Orders.CustomerID, [Order Details].ProductID, Products.ProductName, Products.UnitPrice, [Order Details].ProductID & '-' & 'C-' & Orders.CustomerID " +
                "{1} {2}";
        }
        protected override SearchField[] GetHavingFields(SearchField[] searchFields)
        {
            List<SearchField> havingFields;

            havingFields = new List<SearchField>(searchFields);

            return havingFields.ToArray();
        }
        protected override SearchField[] GetWhereFields(SearchField[] searchFields)
        {
            return new SearchField[0];
        }
        protected override OleDbParameter CreateOleDbParameter(string fieldName, string fieldValue)
        {
            OleDbParameter newParameter;

            switch (fieldName)
            {
                case "ID":
                    newParameter = new System.Data.OleDb.OleDbParameter("ID", System.Data.OleDb.OleDbType.VarWChar, 1024, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "AccountID", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "intforeignid":
                case "AccountID":
                    newParameter = new System.Data.OleDb.OleDbParameter("AccountID", System.Data.OleDb.OleDbType.VarWChar, 1024, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "AccountID", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "ItemCode":
                    newParameter = new System.Data.OleDb.OleDbParameter("ProductID", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "ItemCode", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;
                case "ItemName":
                    newParameter = new System.Data.OleDb.OleDbParameter("ProductName", System.Data.OleDb.OleDbType.WChar, 40, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "ItemName", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "Quantity":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param3", System.Data.OleDb.OleDbType.Decimal, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDecimal(fieldValue);
                    return newParameter;
                case "UnitPrice":
                    newParameter = new System.Data.OleDb.OleDbParameter("UnitPrice", System.Data.OleDb.OleDbType.Currency, 0, System.Data.ParameterDirection.Input, ((byte)(19)), ((byte)(0)), "UnitPrice", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDecimal(fieldValue);
                    return newParameter;
                case "Value":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param5", System.Data.OleDb.OleDbType.Decimal, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDecimal(fieldValue);
                    return newParameter;
                case "LastPurchaseDate":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param6", System.Data.OleDb.OleDbType.DBDate, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDateTime(fieldValue, XmlDateTimeSerializationMode.Unspecified);
                    return newParameter;
                case "Description":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param7", System.Data.OleDb.OleDbType.VarChar, 1024, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "Discount":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param8", System.Data.OleDb.OleDbType.Decimal, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDecimal(fieldValue);
                    return newParameter;
                default:
                    throw new ArgumentException(string.Format(Resources.ErrorMessages_InvalidFieldNameForReport, fieldName, Constants.RTDVNames.ProductsPurchased));
            }
        }


        #region Private Helper Members

        private Dictionary<string, RTDVField> CreateRTDVFieldDictionary()
        {
            Dictionary<string, RTDVField> dictionary;
            RTDVField field;

            // create a new dictionary
            dictionary = new Dictionary<string, RTDVField>();

            //ID
            field = new RTDVField("ID", TypeCode.String);
            field.DisplayName = "ID";
            field.Size = 1024;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ID", field);

            //AccountID
            field = new RTDVField("AccountID", TypeCode.String);
            field.DisplayName = "Account ID";
            field.Size = 1024;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("AccountID", field);

            //ItemCode
            field = new RTDVField("ItemCode", TypeCode.Int32);
            field.DisplayName = "Item Code";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ItemCode", field);

            //ItemName
            field = new RTDVField("ItemName", TypeCode.String);
            field.DisplayName = "Item Name";
            field.Size = 40;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ItemName", field);

            //Quantity
            field = new RTDVField("Quantity", TypeCode.Decimal);
            field.DisplayName = "Quantity";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Quantity", field);

            //UnitPrice
            field = new RTDVField("UnitPrice", TypeCode.Decimal);
            field.DisplayName = "Unit Price";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("UnitPrice", field);

            //Value
            field = new RTDVField("Value", TypeCode.Decimal);
            field.DisplayName = "Value";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Value", field);

            //LastPurchaseDate
            field = new RTDVField("LastPurchaseDate", TypeCode.DateTime);
            field.DisplayName = "Last Purchase Date";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("LastPurchaseDate", field);

            //Description
            field = new RTDVField("Description", TypeCode.String);
            field.DisplayName = "Description";
            field.Size = 1024;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Description", field);

            //Discount
            field = new RTDVField("Discount", TypeCode.Decimal);
            field.DisplayName = "Discount";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Discount", field);

            return dictionary;
        }

        #endregion
    }
}
