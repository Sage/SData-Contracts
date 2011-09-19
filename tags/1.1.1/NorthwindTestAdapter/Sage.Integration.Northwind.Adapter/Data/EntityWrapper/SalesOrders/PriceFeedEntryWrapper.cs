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
    class PriceFeedEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        PriceTransform _transform;
        IFeedEntryEntityWrapper _commodityFeedEntryWrapper;

        public PriceFeedEntryWrapper(RequestContext context)
            : base(context, Adapter.Common.SupportedResourceKinds.prices)
        {
            _entity = new Price();
            _transform = new PriceTransform(context);
            _commodityFeedEntryWrapper = FeedEntryWrapperFactory.Create(SupportedResourceKinds.commodities, context);
        }


        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            return _transform.GetTransformedDocument(payload as PriceFeedEntry);
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            PriceFeedEntry result = _transform.GetTransformedPayload(document as PriceDocument);
            result.commodity = (CommodityFeedEntry)_commodityFeedEntryWrapper.GetFeedEntry(document.Id);
            return result;
        }

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "ProductID";
            if (propertyName.Equals("price", StringComparison.InvariantCultureIgnoreCase))
                return "UnitPrice";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

    }
}
