#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			LineItemDocument.cs
	Author:			Philipp Schuette
	DateCreated:	04/11/2007 10:59:18
	DateChanged:	04/11/2007 10:59:18
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/11/2007 10:59:18  	pschuette		Create			  
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

namespace Sage.Integration.Northwind.Application.Entities.Order.Documents
{
    public class LineItemDocument : Document
    {
        #region constructor
        /// <summary>
        /// Creates a new LineItemDocument Document
        /// </summary>
        public LineItemDocument()
            : base(Constants.EntityNames.LineItem)
        {
            //description ?
            // discount 
            // discountsum?
            // donotreprice
            // erplocation
            // linetype
            // orderquoteid
            // productfamilyid
            // 
        
            AddProperty("productid", TypeCode.Int32);
            AddProperty("uomid", TypeCode.Int32);
            AddProperty("quantity", TypeCode.Decimal);
            AddProperty("listprice", TypeCode.Decimal);
            AddProperty("discountsum", TypeCode.Decimal);
            AddProperty("quotedprice", TypeCode.Decimal);
            AddProperty("taxrate", TypeCode.String);
            AddProperty("tax", TypeCode.Decimal);
            AddProperty("quotedpricetotal", TypeCode.Decimal);
        }

       


        #endregion


        #region public properties

        //public Property orderquouteid { get { return this.Properties["orderquouteid"]; } set { this.Properties["orderquouteid"] = value; } }
        public Property productid { get { return this.Properties["productid"]; } set { this.Properties["productid"] = value; } }
        public Property uomid { get { return this.Properties["uomid"]; } set { this.Properties["uomid"] = value; } }
        public Property quantity { get { return this.Properties["quantity"]; } set { this.Properties["quantity"] = value; } }
        public Property listprice { get { return this.Properties["listprice"]; } set { this.Properties["listprice"] = value; } }
        public Property discountsum { get { return this.Properties["discountsum"]; } set { this.Properties["discountsum"] = value; } }
        public Property quotedprice { get { return this.Properties["quotedprice"]; } set { this.Properties["quotedprice"] = value; } }
        public Property taxrate { get { return this.Properties["taxrate"]; } set { this.Properties["taxrate"] = value; } }
        public Property tax { get { return this.Properties["tax"]; } set { this.Properties["tax"] = value; } }
        public Property quotedpricetotal { get { return this.Properties["quotedpricetotal"]; } set { this.Properties["quotedpricetotal"] = value; } }

        #endregion
    }
}
