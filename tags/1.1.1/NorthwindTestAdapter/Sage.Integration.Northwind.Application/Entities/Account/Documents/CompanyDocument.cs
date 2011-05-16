#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			CompanyDocument.cs
	Author:			Philipp Schuette
	DateCreated:	04/24/2007 11:28:43
	DateChanged:	04/24/2007 11:28:43
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/24/2007 11:28:43	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Base;
using System.Xml;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Integration.Northwind.Application.API;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class CompanyDocument : Document
    {
      #region constructor
        public CompanyDocument()
            : base(Constants.EntityNames.Company)
        {
            AddProperty("companyname", TypeCode.String, 40);
            AddProperty("companytype", TypeCode.String, 20);
            AddCollection(new AccountDocumentCollection());

            this.TypeProperty = "companytype";
        }

       
        #endregion

        #region public properties

        public Property companyname
        {
            get { return Properties["companyname"]; }
            set { Properties["companyname"] = value; }
        }

        public Property companytype
        {
            get { return Properties["companytype"]; }
            set { Properties["companytype"] = value; }
        }




        public AccountDocumentCollection accounts
        {
            get { return (AccountDocumentCollection)Collections["accounts"]; }
            set { Collections["accounts"] = value; }
        }

      
        #endregion

    }
}
