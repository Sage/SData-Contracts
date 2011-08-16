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
    class UnitOfMeasureFeedEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        UnitOfMeasureTransform _transform;

        public UnitOfMeasureFeedEntryWrapper(RequestContext context)
            : base(context, Adapter.Common.SupportedResourceKinds.unitsOfMeasure)
        {
            _entity = new UnitOfMeasure();
            _transform = new UnitOfMeasureTransform(context);            
        }


        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            return _transform.GetTransformedDocument(payload as UnitOfMeasureFeedEntry);
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            return _transform.GetTransformedPayload(document as UnitOfMeasureDocument);
        }

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "ProductID";
            else if (propertyName.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                return "QuantityPerUnit";
            else if (propertyName.Equals("Description", StringComparison.InvariantCultureIgnoreCase))
                return "QuantityPerUnit";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

    }
}
