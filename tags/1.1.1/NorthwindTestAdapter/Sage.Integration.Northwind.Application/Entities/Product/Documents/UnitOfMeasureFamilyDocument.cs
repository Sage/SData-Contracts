#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			UnitOfMeasureFamilyDocument.cs
	Author:			Philipp Schuette
	DateCreated:	04/10/2007 16:30:37
	DateChanged:	04/10/2007 16:30:37
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	04/10/2007 16:30:37  	pschuette		Create			  
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
    public class UnitOfMeasureFamilyDocument : Document
    {
        #region constructor
        /// <summary>
        /// Creates a new UnitOfMeasureFamilyDocument Document
        /// </summary>
        public UnitOfMeasureFamilyDocument()
            : base(Constants.EntityNames.UnitOfMeasureFamily)
        {
            AddProperty("active", TypeCode.String);
            AddProperty("defaultvalue", TypeCode.Boolean);
            AddProperty("description", TypeCode.String);
            AddProperty("name", TypeCode.String);
        }

        


        #endregion

        #region public properties

        public Property active
        {
            get { return this.Properties["active"]; }
            set { this.Properties["active"] = value; }
        }


        public Property defaultvalue
        {
            get { return this.Properties["defaultvalue"]; }
            set { this.Properties["defaultvalue"] = value; }
        }

        public Property description
        {
            get { return this.Properties["description"]; }
            set { this.Properties["description"] = value; }
        }


        public Property name
        {
            get { return this.Properties["name"]; }
            set { this.Properties["name"] = value; }
        }

        #endregion
    }
}
