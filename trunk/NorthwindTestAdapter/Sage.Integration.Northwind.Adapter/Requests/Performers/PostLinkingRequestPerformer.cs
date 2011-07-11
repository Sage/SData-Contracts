#region Usings

using System;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Feeds;
using System.Xml;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.Diagnostics;
using Sage.Integration.Northwind.Etag;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Common.Syndication;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class PostLinkingRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            // only atom entry supported!!!
            if (request.ContentType != Sage.Common.Syndication.MediaType.AtomEntry)
                throw new RequestException("Atom entry content type expected");

            // deserialize the request stream to a SyncFeedEntry
            SyncFeedEntry entry = new SyncFeedEntry();
            XmlReader reader = XmlReader.Create(request.Stream);
            reader.MoveToContent();
            entry.ReadXml(reader, typeof(SyncDigestPayload));

            if (null == entry.Linked)
                throw new RequestException("Invalid content: element 'linked' missing");
            
            // Parse the resource url to get the local id.
            // Additionally we check for equality of the urls of the request url and
            // in the linked element up to the resourceKind.
            
            string requestEndpointUrl = _requestContext.OriginEndPoint;
            RequestContext entryResourceAsRequestContext = new RequestContext(new Sage.Common.Syndication.SDataUri(entry.Linked.Resource));    // TODO: not really nice here.
            string linkedResourceUrl = entryResourceAsRequestContext.OriginEndPoint;

            if (!string.Equals(requestEndpointUrl, linkedResourceUrl, StringComparison.InvariantCultureIgnoreCase))
                throw new RequestException("Request url and linked entry resource not matching.");

            string resourceKindName = _requestContext.ResourceKind.ToString();
            string localId = entryResourceAsRequestContext.SdataUri.CollectionPredicate;
            Guid uuid = entry.Linked.Uuid;

            // store a new correlation entry to the sync store
            ICorrelatedResSyncInfoStore correlatedResSyncInfoStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_requestContext.SdataContext);

            ResSyncInfo newResSyncInfo = new ResSyncInfo(uuid, requestEndpointUrl, 0, string.Empty, DateTime.Now);
            CorrelatedResSyncInfo newInfo = new CorrelatedResSyncInfo(localId, newResSyncInfo);
            correlatedResSyncInfoStore.Add(resourceKindName, newInfo);

            // Set the response values
            SyncFeed feed = new SyncFeed();
            feed.FeedType = FeedType.LinkedSingle;

            SyncFeedEntry feedEntry = new SyncFeedEntry();
            feedEntry.Title = FeedMetadataHelpers.BuildLinkedEntryTitle(_requestContext, uuid);
            feedEntry.Id = FeedMetadataHelpers.BuildLinkedEntryUrl(_requestContext, uuid);
            feedEntry.Updated = newInfo.ResSyncInfo.ModifiedStamp;
            feedEntry.Published = newInfo.ResSyncInfo.ModifiedStamp;
            feedEntry.Linked = entry.Linked;

            feed.Entries.Add(feedEntry);

            
            request.Response.Serializer = new SyncFeedSerializer();
            request.Response.Feed = (IFeed)feed;
            request.Response.ContentType = MediaType.AtomEntry;
            
            request.Response.StatusCode = System.Net.HttpStatusCode.Created;
            request.Response.Protocol.SendUnknownResponseHeader("location", FeedMetadataHelpers.BuildLinkedEntryUrl(_requestContext, uuid));

        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion
    }
}
