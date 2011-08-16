#region Usings

using System;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Sync;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Sage.Integration.Adapter.Model;
using Sage.Integration.Northwind.Adapter.Data.FeedEntries;
using Sage.Integration.Messaging.Model;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    public class GetSyncDigestRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestProcess Members

        public void DoWork(IRequest request)
        {
           // ReturnSample(request);
            //return;
            // DECLARATIONS
            string resourceKindName;
            string EndPoint;
            SdataContext sdataContext;

            ISyncSyncDigestInfoStore syncDigestStore;
            SyncDigestInfo syncDigestInfo;

            // INITIALIZATIONS
            sdataContext = _requestContext.SdataContext;
            resourceKindName = _requestContext.ResourceKind.ToString();
            EndPoint = _requestContext.DatasetLink + resourceKindName;
            syncDigestStore = NorthwindAdapter.StoreLocator.GetSyncDigestStore(sdataContext);


            // Get the digest info from store
            syncDigestInfo = syncDigestStore.Get(resourceKindName);

            /* Create a digest payload and fill it with values retrived from digest store */
            //DigestPayload digestPayload = new DigestPayload();
            Digest digest = new Digest();


            digest.Origin = EndPoint;
                
            // set digest entries
            if ((null == syncDigestInfo) || (syncDigestInfo.Count == 0))
            {
                DigestEntry entry = new DigestEntry();
                entry.ConflictPriority = 1;
                entry.EndPoint = EndPoint;
                entry.Stamp = DateTime.Now;
                entry.Tick = 1;
                digest.Entries = new DigestEntry[] { entry };
            }
            else
            {
                digest.Entries = new DigestEntry[syncDigestInfo.Count];
                for (int i = 0; i < syncDigestInfo.Count; i++)
                {
                    DigestEntry entry = new DigestEntry();

                    entry.ConflictPriority = syncDigestInfo[i].ConflictPriority;
                    entry.EndPoint = syncDigestInfo[i].EndPoint;
                    entry.Stamp = syncDigestInfo[i].Stamp;
                    entry.Tick = (int)syncDigestInfo[i].Tick;
                    digest.Entries[i] = entry;
                }
            }

            // The url to this request
            string url = EndPoint + "/$syncDigest";

            // Create self link
            FeedLink link = new FeedLink(url, LinkType.Self, MediaType.AtomEntry);
            
            // Create FeedEntry
            // Set Response
           

            DigestFeedEntry digestFeedEntry = new DigestFeedEntry();
            digestFeedEntry.Digest = digest;
            digestFeedEntry.Title = "Synchronization digest";
            //digestFeedEntry.Id = url;
            digestFeedEntry.Links.Add(link);


            request.Response.FeedEntry = digestFeedEntry;
            request.Response.ContentType = MediaType.AtomEntry;
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }
        


        #endregion
    }
}
