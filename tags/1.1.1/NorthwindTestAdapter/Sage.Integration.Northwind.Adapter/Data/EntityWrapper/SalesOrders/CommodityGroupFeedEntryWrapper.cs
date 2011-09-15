using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Application.Entities.Order;
using Sage.Integration.Northwind.Adapter.Common;
using System.Data.OleDb;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.ComponentModel;
using System.Data;
using System.Collections;
using Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters;
using Sage.Integration.Northwind.Application.Entities.Product;
using Sage.Integration.Northwind.Adapter.Transform;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;

namespace Sage.Integration.Northwind.Adapter.Data
{
    class CommodityGroupFeedEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        CommodityGroupTransform _transform;

        public CommodityGroupFeedEntryWrapper(RequestContext context)
            : base(context, Adapter.Common.SupportedResourceKinds.commodityGroups)
        {
            _entity = new ProductFamily();
            _transform = new CommodityGroupTransform(context);            
        }


        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            return _transform.GetTransformedDocument(payload as CommodityGroupFeedEntry);
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            return _transform.GetTransformedPayload(document as ProductFamilyDocument);
        }

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                return "CategoryName";
            else if (propertyName.Equals("label", StringComparison.InvariantCultureIgnoreCase))
                return "CategoryName";
            else if (propertyName.Equals("description", StringComparison.InvariantCultureIgnoreCase))
                return "Description";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

    }
}
