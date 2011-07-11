#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
    File:        RTDVEmployees.cs
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
    public class RTDVEmployees : RTDVBase
    {
        #region Fields

        private Dictionary<string, RTDVField> _rtdvFieldDictionary;

        #endregion

        #region Ctor.

        public RTDVEmployees()
            : base(Constants.RTDVNames.Employees, "emp")
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
            get { return "EmployeeID"; }
        }
        protected override string GetSqlQueryNameRepresentation(string fieldName)
        {
            switch (fieldName)
            {
                case "intforeignid":

                //BirthDate
                case "BirthDate":
                    return "Employees.BirthDate";

                //HireDate
                case "HireDate":
                    return "Employees.HireDate";

                //Photo
                case "Photo":
                    return "Employees.Photo";

                //EmployeeID
                case "EmployeeID":
                    return "Employees.EmployeeID";

                //ReportsTo
                case "ReportsTo":
                    return "Employees.ReportsTo";

                //Notes
                case "Notes":
                    return "Employees.Notes";

                //HomePhone
                case "HomePhone":
                    return "Employees.HomePhone";

                //Extension
                case "Extension":
                    return "Employees.Extension";

                //PhotoPath
                case "PhotoPath":
                    return "Employees.PhotoPath";

                //Region
                case "Region":
                    return "Employees.Region";

                //PostalCode
                case "PostalCode":
                    return "Employees.PostalCode";

                //Country
                case "Country":
                    return "Employees.Country";

                //TitleOfCourtesy
                case "TitleOfCourtesy":
                    return "Employees.TitleOfCourtesy";

                //Address
                case "Address":
                    return "Employees.Address";

                //City
                case "City":
                    return "Employees.City";

                //LastName
                case "LastName":
                    return "Employees.LastName";

                //FirstName
                case "FirstName":
                    return "Employees.FirstName";

                //Title
                case "Title":
                    return "Employees.Title";

                case "ReportsToName":
                    return "Manager.LastName";
                default:
                    throw new ArgumentException(string.Format(Resources.ErrorMessages_InvalidFieldNameForReport, fieldName, Constants.RTDVNames.SalesInvoices));
            }
        }
        protected override string GetSqlQueryTemplate()
        {
            // it has the format: SELECT... FROM... WHERE {0} GROUPBY... HAVING {1} ORDERBY{2}
            return "SELECT Employees.EmployeeID, Employees.LastName, " +
                "Employees.FirstName, Employees.Title, Employees.TitleOfCourtesy, " +
                "Employees.BirthDate, Employees.HireDate, Employees.Address, " +
                "Employees.City, Employees.Region, Employees.PostalCode, " +
                "Employees.Country, Employees.HomePhone, Employees.Extension, " +
                "Employees.Notes, Employees.ReportsTo, Manager.LastName as ReportsToName " +
                "FROM Employees left join Employees as Manager " +
                "on Employees.ReportsTo = Manager.EmployeeID " +
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
                    return null;
                //BirthDate
                case "BirthDate":
                    newParameter = new System.Data.OleDb.OleDbParameter("BirthDate",
                    System.Data.OleDb.OleDbType.Date, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "BirthDate",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDateTime(fieldValue, XmlDateTimeSerializationMode.Unspecified);
                    return newParameter;

                //HireDate
                case "HireDate":
                    newParameter = new System.Data.OleDb.OleDbParameter("HireDate",
                    System.Data.OleDb.OleDbType.Date, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "HireDate",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToDateTime(fieldValue, XmlDateTimeSerializationMode.Unspecified);
                    return newParameter;


                //EmployeeID
                case "EmployeeID":
                    newParameter = new System.Data.OleDb.OleDbParameter("EmployeeID",
                    System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "EmployeeID",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;

                //ReportsTo
                case "ReportsTo":
                    newParameter = new System.Data.OleDb.OleDbParameter("ReportsTo",
                    System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "ReportsTo",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = XmlConvert.ToInt32(fieldValue);
                    return newParameter;

                //Notes
                case "Notes":
                    newParameter = new System.Data.OleDb.OleDbParameter("Notes",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "Notes",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //HomePhone
                case "HomePhone":
                    newParameter = new System.Data.OleDb.OleDbParameter("HomePhone",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "HomePhone",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value =fieldValue;
                    return newParameter;

                //Extension
                case "Extension":
                    newParameter = new System.Data.OleDb.OleDbParameter("Extension",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "Extension",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //PhotoPath
                case "PhotoPath":
                    newParameter = new System.Data.OleDb.OleDbParameter("PhotoPath",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "PhotoPath",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //Region
                case "Region":
                    newParameter = new System.Data.OleDb.OleDbParameter("Region",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "Region",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //PostalCode
                case "PostalCode":
                    newParameter = new System.Data.OleDb.OleDbParameter("PostalCode",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "PostalCode",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //Country
                case "Country":
                    newParameter = new System.Data.OleDb.OleDbParameter("Country",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "Country",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //TitleOfCourtesy
                case "TitleOfCourtesy":
                    newParameter = new System.Data.OleDb.OleDbParameter("TitleOfCourtesy",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "TitleOfCourtesy",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //Address
                case "Address":
                    newParameter = new System.Data.OleDb.OleDbParameter("Address",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "Address",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //City
                case "City":
                    newParameter = new System.Data.OleDb.OleDbParameter("City",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "City",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //LastName
                case "LastName":
                    newParameter = new System.Data.OleDb.OleDbParameter("LastName",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "LastName",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //FirstName
                case "FirstName":
                    newParameter = new System.Data.OleDb.OleDbParameter("FirstName",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "FirstName",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
                    return newParameter;

                //Title
                case "Title":
                    newParameter = new System.Data.OleDb.OleDbParameter("Title",
                    System.Data.OleDb.OleDbType.VarChar, 0, System.Data.ParameterDirection.Input,
                    ((byte)(19)), ((byte)(0)), "Title",
                    System.Data.DataRowVersion.Current, false, null);
                    newParameter.Value = fieldValue;
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

            //BirthDate
            field = new RTDVField("BirthDate", TypeCode.DateTime);
            field.DisplayName = "BirthDate";
            field.Size = 8;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("BirthDate", field);


            //HireDate
            field = new RTDVField("HireDate", TypeCode.DateTime);
            field.DisplayName = "HireDate";
            field.Size = 8;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("HireDate", field);



            //EmployeeID
            field = new RTDVField("EmployeeID", TypeCode.Int16);
            field.DisplayName = "EmployeeID";
            field.Size = 4;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("EmployeeID", field);


            //ReportsTo
            field = new RTDVField("ReportsTo", TypeCode.Int16);
            field.DisplayName = "ReportsTo";
            field.Size = 4;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ReportsTo", field);


            //Notes
            field = new RTDVField("Notes", TypeCode.String); //ntext 
            field.DisplayName = "Notes";
            field.Size = 2000;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Notes", field);


            //HomePhone
            field = new RTDVField("HomePhone", TypeCode.String);
            field.DisplayName = "HomePhone";
            field.Size = 48;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("HomePhone", field);


            //Extension
            field = new RTDVField("Extension", TypeCode.String);
            field.DisplayName = "Extension";
            field.Size = 8;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Extension", field);

            //Region
            field = new RTDVField("Region", TypeCode.String);
            field.DisplayName = "Region";
            field.Size = 30;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Region", field);


            //PostalCode
            field = new RTDVField("PostalCode", TypeCode.String);
            field.DisplayName = "PostalCode";
            field.Size = 20;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("PostalCode", field);


            //Country
            field = new RTDVField("Country", TypeCode.String);
            field.DisplayName = "Country";
            field.Size = 30;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Country", field);


            //TitleOfCourtesy
            field = new RTDVField("TitleOfCourtesy", TypeCode.String);
            field.DisplayName = "TitleOfCourtesy";
            field.Size = 50;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("TitleOfCourtesy", field);


            //Address
            field = new RTDVField("Address", TypeCode.String);
            field.DisplayName = "Address";
            field.Size = 120;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Address", field);


            //City
            field = new RTDVField("City", TypeCode.String);
            field.DisplayName = "City";
            field.Size = 30;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("City", field);


            //LastName
            field = new RTDVField("LastName", TypeCode.String);
            field.DisplayName = "LastName";
            field.Size = 40;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("LastName", field);


            //FirstName
            field = new RTDVField("FirstName", TypeCode.String);
            field.DisplayName = "FirstName";
            field.Size = 20;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("FirstName", field);


            //Title
            field = new RTDVField("Title", TypeCode.String);
            field.DisplayName = "Title";
            field.Size = 60;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("Title", field);

            //ReportsToName
            field = new RTDVField("ReportsToName", TypeCode.String);
            field.DisplayName = "ReportsToName";
            field.Size = 40;
            field.MinOccurs = 0;
            field.MaxOccurs = 1;
            dictionary.Add("ReportsToName", field);

            return dictionary;
        }

        #endregion
    }
}
