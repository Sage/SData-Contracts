#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			UnitOfMeasureDocument.cs
	Author:			Philipp Schuette
	DateCreated:	04/10/2007 16:30:21
	DateChanged:	04/10/2007 16:30:21
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/10/2007 16:30:21  	pschuette		Create			  
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
    public class UnitOfMeasureDocument : Document
    {
        #region constructor
        /// <summary>
        /// Creates a new UnitOfMeasureDocument Document
        /// </summary>
        public UnitOfMeasureDocument()
            : base(Constants.EntityNames.UnitOfMeasure)
        {
            AddProperty("active", TypeCode.String);
            AddProperty("name", TypeCode.String);
            AddProperty("units", TypeCode.Int32);
            AddProperty("defaultvalue", TypeCode.Boolean);
            AddProperty("familyid", TypeCode.String);
            
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
            get { return this.Properties["name"]; }
            set { this.Properties["name"] = value; }
        }

        public Property units
        {
            get { return this.Properties["units"]; }
            set { this.Properties["units"] = value; }
        }
        public Property defaultvalue
        {
            get { return this.Properties["defaultvalue"]; }
            set { this.Properties["defaultvalue"] = value; }
        }


        public Property familyid
        {
            get { return this.Properties["familyid"]; }
            set { this.Properties["familyid"] = value; }
        }
        

        


        #endregion



    }
}
