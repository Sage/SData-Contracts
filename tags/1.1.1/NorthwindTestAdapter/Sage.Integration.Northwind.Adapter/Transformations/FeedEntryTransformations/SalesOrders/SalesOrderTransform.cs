using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Entities.Order.Documents;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Adapter.Common;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class SalesOrderTransform : TransformationBase, IFeedEntryTransformation<OrderDocument, SalesOrderFeedEntry>
    {
        private SalesOrderLineTransform _salesOrderLineTransform;

        public SalesOrderTransform(RequestContext context)
            : base(context, Adapter.Common.SupportedResourceKinds.salesOrders)
        {
            _salesOrderLineTransform = new SalesOrderLineTransform(context);
        }

        public OrderDocument GetTransformedDocument(SalesOrderFeedEntry feedEntry)
        {
            OrderDocument document = new OrderDocument();

#warning TODO:

            return document;
        }

        public SalesOrderFeedEntry GetTransformedPayload(OrderDocument document)
        {
            SalesOrderFeedEntry payload = new SalesOrderFeedEntry();

#warning TODO: the actual transformation?!

            #region salesorder lines

            int salesOrderLinesCount = document.orderitems.documents.Count;
            payload.salesOrderLines = new SalesOrderLineFeed();
            for (int index = 0; index < salesOrderLinesCount; index++)
            {
                LineItemDocument lineItem = document.orderitems.documents[index] as LineItemDocument;
                SalesOrderLineFeedEntry salesOrderLinePayload;
                salesOrderLinePayload = _salesOrderLineTransform.GetTransformedPayload(lineItem);
                payload.salesOrderLines.Entries.Add(salesOrderLinePayload);
            }

            #endregion

            return payload;
        }
    }
}
