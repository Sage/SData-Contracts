using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Toolkit;
using System.Xml;
using Sage.Integration.Northwind.Application.Base;

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class EmailDocument : Document
    {

        public Property emailaddress
        {
            get { return Properties["emailaddress"] ; }
            set {  Properties["emailaddress"] = value; }
        }

        public Property type
        {
            get { return Properties["type"]; }
            set { Properties["type"] = value; }
        }

        

        public EmailDocument()
            : base("email")
        {
            AddProperty("emailaddress", TypeCode.String, 100);
            AddProperty("type", TypeCode.String, 20);
        }

    }
}

