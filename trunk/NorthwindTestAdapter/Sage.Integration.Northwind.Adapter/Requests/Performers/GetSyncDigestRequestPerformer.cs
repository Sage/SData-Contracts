#region Usings

using System;
using Sage.Common.Syndication;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Sync;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class GetSyncDigestRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestProcess Members

        public void DoWork(IRequest request)
        {
            
            string resourceKind = _requestContext.ResourceKind.ToString();
            string endpoint = _requestContext.DatasetLink + resourceKind;
            SdataContext sdataContext = _requestContext.SdataContext;

            ISyncSyncDigestInfoStore syncDigestStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetSyncDigestStore(sdataContext);

            SyncDigestInfo syncDigestInfo = syncDigestStore.Get(resourceKind);

            SyncDigestPayload syncDigestPayload = new SyncDigestPayload();
            syncDigestPayload.Digest.Origin = endpoint;
            // create a new initial syncDigest and store it.
            if ((null == syncDigestInfo) || (syncDigestInfo.Count == 0))
            {
                SyncFeedDigestEntry entry = new SyncFeedDigestEntry();
                entry.ConflictPriority = 0;
                entry.Endpoint = endpoint;
                entry.Stamp = DateTime.Now;
                entry.Tick = -1;
                syncDigestPayload.Digest.Entries.Add(entry);
            }
            else
            {
                foreach (SyncDigestEntryInfo digestEntry in syncDigestInfo)
                {
                    SyncFeedDigestEntry entry = new SyncFeedDigestEntry();
                    entry.ConflictPriority = digestEntry.ConflictPriority;
                    entry.Endpoint = digestEntry.Endpoint;
                    //entry.Stamp = digestEntry.;
                    entry.Tick = digestEntry.Tick;
                    syncDigestPayload.Digest.Entries.Add(entry);

                }

            }
            SyncFeed feed = new SyncFeed();
            string url = endpoint + "/$syncDigest";
            FeedLink link = new FeedLink(url, LinkType.Self, MediaType.AtomEntry);
            feed.Links = new FeedLinkCollection();
            feed.Links.Add(link);
            feed.Id = url;
            feed.FeedType = FeedType.ResourceEntry;
            SyncFeedEntry syncFeedEntry = new SyncFeedEntry();
            syncFeedEntry.Payload = syncDigestPayload;
            syncFeedEntry.Title = "Synchronization digest";
            syncFeedEntry.Id = url;

            syncFeedEntry.Links.Add(link);
            feed.Entries.Add(syncFeedEntry);
           
            feed.Id = url;
            
            request.Response.Serializer = new SyncFeedSerializer();
            request.Response.Feed = feed;
            request.Response.ContentType = MediaType.AtomEntry;
        }

        void IRequestPerformer.Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion
    }
}
