using System;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Feeds;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Application.Base;
using System.Collections.Generic;
using Sage.Integration.Northwind.Application.API;
namespace Sage.Integration.Northwind.Adapter.Data
{
    public interface IEntityWrapper
    {
        EntityBase Entity { get; }
        Identity GetIdentity(string id);

        SdataTransactionResult Add(PayloadBase payload, List<SyncFeedEntryLink> links);
        SdataTransactionResult Update(PayloadBase payload, List<SyncFeedEntryLink> links);
        SdataTransactionResult Delete(string id);
        
        SyncFeed GetFeed();

        SyncFeedEntry GetFeedEntry(string id);
        SyncFeedEntry GetFeedEntry(CorrelatedResSyncInfo resSyncInfo);
        SyncFeedEntry GetFeedEntry(SdataTransactionResult transactionResult);

        Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links);
    }
}
