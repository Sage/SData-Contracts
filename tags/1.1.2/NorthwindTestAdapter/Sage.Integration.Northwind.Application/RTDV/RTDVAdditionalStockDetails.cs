#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
    File:        RTDVAdditionalStockDetails.cs
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
    public class RTDVAdditionalStockDetails : RTDVBase
    {
        #region Fields

        private Dictionary<string, RTDVField> _rtdvFieldDictionary;

        #endregion

        #region Ctor.

        public RTDVAdditionalStockDetails()
            : base(Constants.RTDVNames.AdditionalStockDetails, "add")
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
            get { return "ProductID"; }
        }
        protected override string GetSqlQueryNameRepresentation(string fieldName)
        {
            switch (fieldName)
            {
                case "intforeignid":
                    return string.Empty;
                case "ProductID":
                case "ItemCode":
                    return "Products.ProductID";
                case "ProductName":
                    return "Products.ProductName";
                case "UnitsInStock":
                    return "Products.UnitsInStock";
                case "UnitsOnOrder":
                    return "Products.UnitsOnOrder";
                case "UnitsOnStockAfterOrder":
                    return "Products.UnitsInStock - Products.UnitsOnOrder";
                case "ReorderLevel":
                    return "Products.ReorderLevel";
                case "CompanyName":
                    return "Suppliers.CompanyName";
                case "ContactName":
                    return "Suppliers.ContactName";
                case "Address":
                    return "Suppliers.Address";
                case "City":
                    return "Suppliers.City";
                case "Region":
                    return "Suppliers.Region";
                case "PostalCode":
                    return "Suppliers.PostalCode";
                case "Country":
                    return "Suppliers.Country";
                case "Phone":
                    return "Suppliers.Phone";
                case "Fax":
                    return "Suppliers.Fax";
                default:
                    throw new ArgumentException(string.Format(Resources.ErrorMessages_InvalidFieldNameForReport, fieldName, Constants.RTDVNames.AdditionalStockDetails));
            }
        }
        protected override string GetSqlQueryTemplate()
        {
            // it has the format: SELECT... FROM... WHERE {0} GROUPBY... HAVING {1} ORDERBY{2}
            return  "SELECT Products.ProductID, Products.ProductName, Products.UnitsInStock, Products.UnitsOnOrder, Products.UnitsInStock - Products.UnitsOnOrder AS UnitsOnStockAfterOrder, " + 
                    "Products.ReorderLevel, Suppliers.CompanyName, Suppliers.ContactName, Suppliers.Address, Suppliers.City, Suppliers.Region, Suppliers.PostalCode, " +
                    "Suppliers.Country, Suppliers.Phone, Suppliers.Fax " +
                    "FROM (Products INNER JOIN Suppliers ON Products.SupplierID = Suppliers.SupplierID) " +
                    "{0} {1} {2}";
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
                case "ItemCode":
                case "ProductID":
                    newParameter = new System.Data.OleDb.OleDbParameter("ProductID", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "ProductID", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;
                case "ProductName":
                    newParameter = new System.Data.OleDb.OleDbParameter("ProductName", System.Data.OleDb.OleDbType.WChar, 40, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "ProductName", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "UnitsInStock":
                    newParameter = new System.Data.OleDb.OleDbParameter("UnitsInStock", System.Data.OleDb.OleDbType.SmallInt, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "UnitsInStock", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;
                case "UnitsOnOrder":
                    newParameter = new System.Data.OleDb.OleDbParameter("UnitsOnOrder", System.Data.OleDb.OleDbType.SmallInt, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "UnitsOnOrder", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;
                case "UnitsOnStockAfterOrder":
                    newParameter = new System.Data.OleDb.OleDbParameter("Param4", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;
                case "ReorderLevel":
                    newParameter = new System.Data.OleDb.OleDbParameter("ReorderLevel", System.Data.OleDb.OleDbType.SmallInt, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "ReorderLevel", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;
                case "CompanyName":
                    newParameter = new System.Data.OleDb.OleDbParameter("CompanyName", System.Data.OleDb.OleDbType.WChar, 40, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CompanyName", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "ContactName":
                    newParameter = new System.Data.OleDb.OleDbParameter("ContactName", System.Data.OleDb.OleDbType.WChar, 30, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "ContactName", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "Address":
                    newParameter = new System.Data.OleDb.OleDbParameter("Address", System.Data.OleDb.OleDbType.WChar, 60, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Address", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "City":
                    newParameter = new System.Data.OleDb.OleDbParameter("City", System.Data.OleDb.OleDbType.WChar, 15, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "City", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "Region":
                    newParameter = new System.Data.OleDb.OleDbParameter("Region", System.Data.OleDb.OleDbType.WChar, 15, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Region", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "PostalCode":
                    newParameter = new System.Data.OleDb.OleDbParameter("PostalCode", System.Data.OleDb.OleDbType.WChar, 10, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "PostalCode", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "Country":
                    newParameter = new System.Data.OleDb.OleDbParameter("Country", System.Data.OleDb.OleDbType.WChar, 15, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Country", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "Phone":
                    newParameter = new System.Data.OleDb.OleDbParameter("Phone", System.Data.OleDb.OleDbType.WChar, 24, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Phone", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                case "Fax":
                    newParameter = new System.Data.OleDb.OleDbParameter("Fax", System.Data.OleDb.OleDbType.WChar, 24, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "Fax", System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;
                default:
                    throw new ArgumentException(string.Format(Resources.ErrorMessages_InvalidFieldNameForReport, fieldName, Constants.RTDVNames.AdditionalStockDetails));
            }
        }

        #region Private Helper Members

        private Dictionary<string, RTDVField> CreateRTDVFieldDictionary()
        {
            Dictionary<string, RTDVField> dictionary;
            RTDVField field;

            // create a new dictionary
            dictionary = new Dictionary<string, RTDVField>();

            //ProductID
            field = new RTDVField("ProductID", TypeCode.Int32);
            field.DisplayName = "Product ID";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ProductID", field);

            //ProductName
            field = new RTDVField("ProductName", TypeCode.String);
            field.DisplayName = "Product Name";
            field.Size = 40;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ProductName", field);

            //UnitsInStock
            field = new RTDVField("UnitsInStock", TypeCode.Int16);
            field.DisplayName = "Units In Stock";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("UnitsInStock", field);

            //UnitsOnStockAfterOrder
            field = new RTDVField("UnitsOnStockAfterOrder", TypeCode.Int32);
            field.DisplayName = "Units On Stock After Order";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("UnitsOnStockAfterOrder", field);

            //ReorderLevel
            field = new RTDVField("ReorderLevel", TypeCode.Int16);
            field.DisplayName = "Reorder Level";
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ReorderLevel", field);

            //CompanyName
            field = new RTDVField("CompanyName", TypeCode.String);
            field.DisplayName = "Company Name";
            field.Size = 40;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("CompanyName", field);
            
            //CompanyName
            field = new RTDVField("ContactName", TypeCode.String);
            field.DisplayName = "Contact Name";
            field.Size = 30;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ContactName", field);
            
            //CompanyName
            field = new RTDVField("Address", TypeCode.String);
            field.DisplayName = "Address";
            field.Size = 60;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Address", field);

            //City
            field = new RTDVField("City", TypeCode.String);
            field.DisplayName = "City";
            field.Size = 15;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("City", field);

            //City
            field = new RTDVField("Region", TypeCode.String);
            field.DisplayName = "Region";
            field.Size = 15;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Region", field);

            //PostalCode
            field = new RTDVField("PostalCode", TypeCode.String);
            field.DisplayName = "Postal Code";
            field.Size = 10;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("PostalCode", field);

            //Country
            field = new RTDVField("Country", TypeCode.String);
            field.DisplayName = "Country";
            field.Size = 15;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Country", field);

            //Phone
            field = new RTDVField("Phone", TypeCode.String);
            field.DisplayName = "Phone";
            field.Size = 24;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Phone", field);

            //Fax
            field = new RTDVField("Fax", TypeCode.String);
            field.DisplayName = "Fax";
            field.Size = 24;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Fax", field);
            

            return dictionary;
        }

        #endregion
    }
}
