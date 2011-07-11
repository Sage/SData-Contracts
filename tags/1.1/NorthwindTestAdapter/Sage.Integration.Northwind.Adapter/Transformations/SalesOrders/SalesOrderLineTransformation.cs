#region Usings

using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Application.Entities.Order.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using Sage.Sis.Sdata.Sync.Storage;

#endregion

namespace Sage.Integration.Northwind.Adapter.Transformations.SalesOrders
{
    public class SalesOrderLineTransformation : TransformationBase, ITransformation<LineItemDocument, SalesOrderLinePayload>
    {
        #region Class Variables

        #endregion

        #region Ctor.

        public SalesOrderLineTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.salesOrderLines)
        {
        }

        #endregion

        #region ITransformation<LineItemDocument,SalesOrderLinePayload> Members

        public LineItemDocument GetTransformedDocument(SalesOrderLinePayload payload, List<SyncFeedEntryLink> links)
        {
            LineItemDocument document = new LineItemDocument();
            salesOrderLinetype salesOrder = payload.SalesOrderLinetype;

#warning TODO:

            return document;
        }

        public SalesOrderLinePayload GetTransformedPayload(LineItemDocument document, out List<SyncFeedEntryLink> links)
        {
            SalesOrderLinePayload payload = new SalesOrderLinePayload();
            salesOrderLinetype salesOrderLine = new salesOrderLinetype();
            links = new List<SyncFeedEntryLink>();

            #region initial values

            //Many more things should set to default values

            #endregion


            payload.SalesOrderLinetype = salesOrderLine;

            return payload;
        }

        #endregion
    }
}
