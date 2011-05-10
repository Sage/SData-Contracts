#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			ProductDocument.cs
	Author:			Philipp Schuette
	DateCreated:	04/05/2007 15:46:54
	DateChanged:	04/05/2007 15:46:54
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/05/2007 15:46:54  	pschuette		Create			  
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
    public class ProductDocument : Document
    {
        #region constructor
        /// <summary>
        /// Creates a new ProductDocument Document
        /// </summary>
        public ProductDocument()
            : base(Constants.EntityNames.Product)
        {
            //this.AddProperty("active", TypeCode.Boolean);
            this.AddProperty("active", TypeCode.String, 5);
            this.AddProperty("code", TypeCode.String, 40);
            this.AddProperty("name", TypeCode.String, 40);
            this.AddProperty("productfamilyid", TypeCode.Int32);
            this.AddProperty("uomcategory", TypeCode.String, 20);
            this.AddProperty("instock", TypeCode.Int32);
        }

        

        #endregion


        #region public properties

        public Property active
        {
            get { return this.Properties["active"]; }
            set { this.Properties["active"] = value; }
        }

        public Property code
        {
            get { return this.Properties["code"]; }
            set { this.Properties["code"] = value; }
        }
        public Property name
        {
            get { return this.Properties["name"]; }
            set { this.Properties["name"] = value; }
        }
        public Property productfamilyid
        {
            get { return this.Properties["productfamilyid"]; }
            set { this.Properties["productfamilyid"] = value; }
        }
        public Property uomcategory
        {
            get { return this.Properties["uomcategory"]; }
            set { this.Properties["uomcategory"] = value; }
        }
        public Property instock
        {
            get { return this.Properties["instock"]; }
            set { this.Properties["instock"] = value; }
        }

        #endregion
    }
}
