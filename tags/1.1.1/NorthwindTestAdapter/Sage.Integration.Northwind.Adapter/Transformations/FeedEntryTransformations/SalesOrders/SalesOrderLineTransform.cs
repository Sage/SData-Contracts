using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Entities.Order.Documents;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Adapter.Common;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class SalesOrderLineTransform : TransformationBase, IFeedEntryTransformation<LineItemDocument, SalesOrderLineFeedEntry>
    {
        public SalesOrderLineTransform(RequestContext context) : base(context, Adapter.Common.SupportedResourceKinds.salesOrderLines) { }

        public LineItemDocument GetTransformedDocument(SalesOrderLineFeedEntry feedEntry)
        {
            LineItemDocument document = new LineItemDocument();
            //salesOrderLinetype salesOrder = payload.SalesOrderLinetype;

#warning TODO: The actual tranformation...

            return document;
        }

        public SalesOrderLineFeedEntry GetTransformedPayload(LineItemDocument document)
        {
            SalesOrderLineFeedEntry feedEntry = new SalesOrderLineFeedEntry();

            #region initial values

#warning Many more things should set to default values

            #endregion

            return feedEntry;
        }
    }
}
