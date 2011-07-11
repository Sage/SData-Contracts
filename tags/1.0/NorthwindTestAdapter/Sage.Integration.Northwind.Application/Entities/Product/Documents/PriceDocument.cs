#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			PriceDocument.cs
	Author:			Philipp Schuette
	DateCreated:	04/10/2007 11:13:26
	DateChanged:	04/10/2007 11:13:26
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/10/2007 11:13:26  	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using System.Xml;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Product.Documents
{
    public class PriceDocument : Document
    {
        #region constructor
        /// <summary>
        /// Creates a new PriceDocument Document
        /// </summary>
        public PriceDocument()
            : base(Constants.EntityNames.Price)
        {
            AddProperty("active", TypeCode.Boolean);
            AddProperty("price_cid", TypeCode.String);
            AddProperty("pricinglistid", TypeCode.String);
            AddProperty("productid", TypeCode.String);
            AddProperty("uomid", TypeCode.String);
            AddProperty("price", TypeCode.Decimal);
        }

        


        #endregion

        #region public properties

        public Property active
        {
            get { return Properties["active"] ; }
            set {  Properties["active"] = value; }
        }

        public Property price_cid
        {
            get { return Properties["price_cid"]; }
            set { Properties["price_cid"] = value; }
        }
        public Property pricinglistid
        {
            get { return Properties["pricinglistid"]; }
            set { Properties["pricinglistid"] = value; }
        }
        public Property productid
        {
            get { return Properties["productid"] ; }
            set {  Properties["productid"] = value; }
        }
        public Property uomid
        {
            get { return Properties["uomid"] ; }
            set { Properties["uomid"] = value; }
        }
        public Property price
        {
            get { return Properties["price"] ; }
            set {  Properties["price"] = value; }
        }

        #endregion
    }
}
