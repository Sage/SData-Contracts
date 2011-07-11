#region Usings

using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Order;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using System;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data.SalesOrders
{
    public class SalesInvoiceLineWrapper : EntityWrapperBase, IEntityWrapper, IEntityQueryWrapper
    {
                #region Ctor.

        public SalesInvoiceLineWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.salesInvoiceLines)
        {
            _entity = new Order();
            
        }

        #endregion
        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            throw new NotImplementedException();
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Add(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Update(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override SyncFeed  GetFeed()
        {
            throw new NotImplementedException();
        }

        public override SyncFeedEntry GetFeedEntry(string id)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region IEntityQueryWrapper Members

        public string GetDbFieldName(string propertyName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
