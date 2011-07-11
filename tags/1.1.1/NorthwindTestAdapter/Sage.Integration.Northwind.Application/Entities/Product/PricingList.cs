//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Sage.Integration.Northwind.Application.Entities.Product
//{
//    class PricingList
//    {
//    }
//}
#region ©2007 Sage Technology Ltd., All Rights Reserved.
/*=====================================================================
	File:			PricingList.cs
	Author:			Philipp Schuette
	DateCreated:	06/01/2007 09:35:10
	DateChanged:	06/01/2007 09:35:10
 ---------------------------------------------------------------------
	©2007 Sage Technology Ltd., All Rights Reserved.	
=====================================================================*/

/*=====================================================================
	Date			        Author		Changes			Reasons
	06/01/2007 09:35:10	pschuette		Create			  
	Changes: Create, Refactoring, Upgrade	
=====================================================================*/
#endregion

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Entities.Product.DataSets;
using System.Xml;
using Sage.Integration.Northwind.Application.Toolkit;
using System.Data;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Application.Entities.Product.DataSets.CategoryTableAdapters;
using System.Data.OleDb;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;
#endregion

namespace Sage.Integration.Northwind.Application.Entities.Product
{
    public class PricingList : EntityBase
    {
        #region constructor

        public PricingList()
            : base(Constants.EntityNames.PricingList)
        {
        }

        #endregion

        public override Document GetDocument(Identity identity, Token lastToken, NorthwindConfig config)
        {

            PricingListsDocument doc = new PricingListsDocument();
            if (identity.Id.Equals(Constants.DefaultValues.PriceList.ID))
            {
                doc.Id = Constants.DefaultValues.PriceList.ID;
                doc.description.Value = Constants.DefaultValues.PriceList.Name;
                doc.name.Value = Constants.DefaultValues.PriceList.Name;
            }
            else
            {
                doc.Id = Constants.DefaultValues.PriceListSpecial.ID;
                doc.description.Value = Constants.DefaultValues.PriceListSpecial.Name;
                doc.name.Value = Constants.DefaultValues.PriceListSpecial.Name;
            }
            doc.active.Value = "Y"; //Constants.DefaultValues.Active;
#warning should be boolean, but as workaround this will filled with "Y"
            doc.erpdefaultvalue.Value = "Y";
            doc.defaultvalue.Value = false;

            return doc;
        }


        public override int FillChangeLog(out DataTable table, NorthwindConfig config, Token lastToken)
        {
            table = new DataTable("PriceingList");
            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("Sequence", typeof(int));
            table.Rows.Add(new object[] { Constants.DefaultValues.PriceList.ID, 0 });
            //table.Rows.Add(new object[] { Constants.DefaultValues.PriceListSpecial.ID, 0 });
            return 1;
        }


        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, OleDbParameter[] oleDbParameters)
        {
            throw new NotImplementedException();
        }

        public override List<Identity> GetAll(NorthwindConfig config, string whereExpression, int startIndex, int count)
        {
            throw new NotImplementedException();
        }
    }
}
