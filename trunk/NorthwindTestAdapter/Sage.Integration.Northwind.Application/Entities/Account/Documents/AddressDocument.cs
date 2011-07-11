#region ©$year$ Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			$safeitemname$.cs
	Author:			Philipp Schuette
	DateCreated:	$time$
	DateChanged:	$time$
 ---------------------------------------------------------------------
	©$year$ Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	$time$	$username$		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Toolkit;
using System.Xml;
using Sage.Integration.Northwind.Application.Base;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class AddressDocument : Document
    {
        public AddressDocument()
            : base("address")
        {
            AddProperty("address1", TypeCode.String, 60);
            AddProperty("address2", TypeCode.String, 60);
            AddProperty("address3", TypeCode.String, 60);
            AddProperty("address4", TypeCode.String, 60);
            AddProperty("city", TypeCode.String, 15);
            AddProperty("state", TypeCode.String, 15);
            AddProperty("country", TypeCode.String, 15);
            AddProperty("postcode", TypeCode.String, 10);
            AddProperty("primaryaddress", TypeCode.String);
            //AddProperty("addresstype", TypeCode.String);

            this.TypeProperty = "primaryaddress";

        }

        

        public Property address1
        {
            get { return Properties["address1"] ; }
            set {  Properties["address1"] = value; }
        }


        public Property address2
        {
            get { return Properties["address2"] ; }
            set {  Properties["address2"] = value; }
        }

        public Property address3
        {
            get { return Properties["address3"] ; }
            set {  Properties["address3"] = value; }
        }

        public Property address4
        {
            get { return Properties["address4"] ; }
            set {  Properties["address4"] = value; }
        }

        public Property City
        {
            get { return Properties["city"] ; }
            set {  Properties["city"] = value; }
        }

        public Property state
        {
            get { return Properties["state"] ; }
            set {  Properties["state"] = value; }
        }

        public Property country
        {
            get { return Properties["country"] ; }
            set {  Properties["country"] = value; }
        }

        public Property postcode
        {
            get { return Properties["postcode"]; }
            set { Properties["postcode"] = value; }
        }

        public Property primaryaddress
        {
            get { return Properties["primaryaddress"]; }
            set { Properties["primaryaddress"] = value; }
        }

    }
}
