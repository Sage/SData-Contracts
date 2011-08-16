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
    class CommodityFeedEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        CommodityTransform _transform;

        public CommodityFeedEntryWrapper(RequestContext context)
            : base(context, Adapter.Common.SupportedResourceKinds.commodities)
        {
            _entity = new Product();
            _entity.DelimiterClause = @"Discontinued = false";
            _transform = new CommodityTransform(context);            
        }


        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            return _transform.GetTransformedDocument(payload as CommodityFeedEntry);
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            return _transform.GetTransformedPayload(document as ProductDocument);
        }

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                return "ProductName";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

    }
}
