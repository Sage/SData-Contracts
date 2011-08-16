using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Common.Syndication;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

namespace Sage.Integration.Northwind.Adapter.Data
{
    public interface IFeedEntryEntityWrapper
    {
        EntityBase Entity { get; }
        Identity GetIdentity(string id);

        // add
        SdataTransactionResult Add(FeedEntry payload);
        //SdataTransactionResult Add(ResourcePayloadContainer payload, syncstate, targetdigest);

        SdataTransactionResult Update(FeedEntry payload);

        SdataTransactionResult Delete(string id);


        // get
        string[] GetFeed();

        // get
        FeedEntry GetFeedEntry(string id);

        // sync source
        FeedEntry GetSyncSourceFeedEntry(CorrelatedResSyncInfo resSyncInfo);

        // sync target
        FeedEntry GetSyncTargetFeedEntry(SdataTransactionResult transactionResult);

        Document GetTransformedDocument(FeedEntry payload);
    }
}
