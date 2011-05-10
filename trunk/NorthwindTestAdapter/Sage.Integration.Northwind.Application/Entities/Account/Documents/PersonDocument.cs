using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Toolkit;
using System.Xml;
using Sage.Integration.Northwind.Application.Base;

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class PersonDocument : Document
    {

        public Property salutation
        {
            get { return Properties["salutation"] ; }
            set {  Properties["salutation"] = value; }
        }

        public Property firstname
        {
            get { return Properties["firstname"] ; }
            set { Properties["firstname"] = value;}
        }


        public Property middlename
        {
            get { return Properties["middlename"] ; }
            set {  Properties["middlename"] = value; }
        }

        public Property lastname
        {
            get { return Properties["lastname"] ; }
            set {  Properties["lastname"] = value; }
        }

        public Property suffix
        {
            get { return Properties["suffix"] ; }
            set {  Properties["suffix"] = value; }
        }

        public Property primaryperson
        {
            get { return Properties["primaryperson"]; }
            set { Properties["primaryperson"] = value; }
        }

        public Property title
        {
            get { return Properties["title"] ; }
            set {  Properties["title"] = value; }
        }

        public Property fullname
        {
            get { return Properties["fullname"]; }
            set { Properties["fullname"] = value; }
        }

        public PersonDocument()
            : base("person")
        {
            AddProperty("salutation", TypeCode.String, 30);
            AddProperty("firstname", TypeCode.String, 30);
            AddProperty("middlename", TypeCode.String, 30);
            AddProperty("lastname", TypeCode.String, 30);
            AddProperty("suffix", TypeCode.String, 20);
            AddProperty("primaryperson", TypeCode.String);
            AddProperty("title", TypeCode.String, 30);
            AddProperty("fullname", TypeCode.SByte, 255);
            this.TypeProperty = "primaryperson";
        }

       


    }
}
