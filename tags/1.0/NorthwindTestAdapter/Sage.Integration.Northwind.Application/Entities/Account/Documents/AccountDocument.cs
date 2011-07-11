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
using Sage.Integration.Northwind.Application.API;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class AccountDocument : Document
    {

        #region constructor

        /// <summary>
        /// the constructor of the accout document.
        /// it sets the name and all
        /// </summary>
        public AccountDocument()
            : base(Constants.EntityNames.Account)
        {
            //AddProperty("type", TypeCode.String, 30);
            //AddProperty("subtype", TypeCode.String, 20);
            //AddProperty("groupid", TypeCode.String);
            AddProperty("name", TypeCode.String, 40);
            AddProperty("currencyid", TypeCode.String, 4);
            AddProperty("onhold", TypeCode.Boolean);
            AddCollection(new PeopleDocumentCollection());
            AddCollection(new AddressDocumentCollection());
            AddCollection(new PhonesDocumentCollection());
            AddCollection(new EmailsDocumentCollection());

            AddProperty("customerSupplierFlag", TypeCode.String, 40);

            this.TypeProperty = "accounttype";
        }

        
        #endregion

        #region public properties
        //public Property type
        //{
        //    get { return Properties["type"]; }
        //    set { Properties["type"] = value; }
        //}

        //public Property AccountSubType
        //{
        //    get { return Properties["subtype"]; }
        //    set { Properties["subtype"] = value; }
        //}

        //public Property AccountGroupID
        //{
        //    get { return Properties["groupid"]; }
        //    set { Properties["groupid"] = value; }
        //}

        public Property name
        {
            get { return Properties["name"]; }
            set { Properties["name"] = value; }
        }

        public Property currencyid
        {
            get { return Properties["currencyid"]; }
            set { Properties["currencyid"] = value; }
        }

        public Property onhold
        {
            get { return Properties["onhold"]; }
            set { Properties["onhold"] = value; }
        }

        public PeopleDocumentCollection people
        {
            get { return (PeopleDocumentCollection)Collections["people"]; }
            set { Collections["people"] = value; }
        }

        public AddressDocumentCollection addresses
        {
            get { return (AddressDocumentCollection)Collections["addresses"]; }
            set { Collections["addresses"] = value; }
        }

        public PhonesDocumentCollection phones
        {
            get { return (PhonesDocumentCollection)Collections["phones"]; }
            set { Collections["phones"] = value; }
        }

        public EmailsDocumentCollection emails
        {
            get { return (EmailsDocumentCollection) Collections["emails"]; }
            set { Collections["emails"] = value; }
        }

        public Property customerSupplierFlag
        {
            get { return Properties["customerSupplierFlag"]; }
            set { Properties["customerSupplierFlag"] = value; }
        }

        #endregion

    }
}
