#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			ProductFamilyDocument.cs
	Author:			Philipp Schuette
	DateCreated:	04/12/2007 13:29:58
	DateChanged:	04/12/2007 13:29:58
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/12/2007 13:29:58  	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Product.Documents
{
    public class ProductFamilyDocument : Document
    {
        #region constructor
        /// <summary>
        /// Creates a new ProductFamilyDocument Document
        /// </summary>
        public ProductFamilyDocument()
            : base(Constants.EntityNames.ProductFamily)
        {
            this.AddProperty("active", TypeCode.String);
            this.AddProperty("name", TypeCode.String, 15);
            // Our field is of type MEMO, but the maximum length allowed by SageCRM is 4000
            // Setting it to 4000 though led to the limit for the row size to be exceeded
            this.AddProperty("description", TypeCode.String, 2000);
        }

        

        #endregion


        #region public properties

        public Property active
        {
            get { return this.Properties["active"]; }
            set { this.Properties["active"] = value; }
        }

        public Property name
        {
            get { return this.Properties["name"] ; }
            set { this.Properties["name"] = value; }
        }


        public Property description
        {
            get { return this.Properties["description"] ; }
            set { this.Properties["description"] = value; }
        }



        #endregion

    }
}
