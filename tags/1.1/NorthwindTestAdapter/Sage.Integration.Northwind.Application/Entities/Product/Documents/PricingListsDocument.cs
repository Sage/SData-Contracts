//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Sage.Integration.Northwind.Application.Entities.Product.Documents
//{
//    class PricingListsDocument
//    {
//    }
//}
#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			PricingListsDocument.cs
	Author:			Philipp Schuette
	DateCreated:	05/31/2007 11:13:26
	DateChanged:	05/31/2007 11:13:26
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
    public class PricingListsDocument : Document
    {
        #region constructor
        /// <summary>
        /// Creates a new PriceDocument Document
        /// </summary>
        public PricingListsDocument()
            : base(Constants.EntityNames.PricingList)
        {
            AddProperty("active", TypeCode.String);
            AddProperty("defaultvalue", TypeCode.Boolean);
#warning should be boolean, but as workaround this will filled with 'Y'
            AddProperty("erpdefaultvalue", TypeCode.String);
            AddProperty("description", TypeCode.String);
            AddProperty("name", TypeCode.String);

        }



        #endregion

        #region public properties

        public Property active
        {
            get { return Properties["active"]; }
            set { Properties["active"] = value; }
        }

        public Property defaultvalue
        {
            get { return Properties["defaultvalue"]; }
            set { Properties["defaultvalue"] = value; }
        }
        public Property erpdefaultvalue
        {
            get { return Properties["erpdefaultvalue"]; }
            set { Properties["erpdefaultvalue"] = value; }
        }
        public Property description
        {
            get { return Properties["description"]; }
            set { Properties["description"] = value; }
        }
        public Property name
        {
            get { return Properties["name"]; }
            set { Properties["name"] = value; }
        }

        #endregion
    }
}
