#region Usings

using System;
using Sage.Integration.Messaging.Model;
using System.Xml;
using Sage.Integration.Northwind.Feeds;
using System.Diagnostics;
using System.ComponentModel;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Requests;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class PutLinkingRequestPerformer : IRequestPerformer
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

            // We check for equality of the urls of the request url and
            // in the linked element up to the resourceKind.

            string requestEndpointUrl = _requestContext.OriginEndPoint;
            RequestContext entryResourceAsRequestContext = new RequestContext(new Sage.Common.Syndication.SDataUri(entry.Linked.Resource));    // TODO: not really nice here.
            string linkedResourceUrl = entryResourceAsRequestContext.OriginEndPoint;

            if (!string.Equals(requestEndpointUrl, linkedResourceUrl, StringComparison.InvariantCultureIgnoreCase))
                throw new RequestException("Request url and linked entry resource not matching.");

            string resourceKindName = _requestContext.ResourceKind.ToString();

            Guid currentUuid =  (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(_requestContext.ResourceKey);
            Guid newUuid = entry.Linked.Uuid;
            string newLocalId = entryResourceAsRequestContext.SdataUri.CollectionPredicate;

            // update the correlation entry in the sync store.
            // if the uuid should be updated we have to remove the current correlation and then add a new one.
            // otherwise we update the current correlation.
            ICorrelatedResSyncInfoStore correlatedResSyncInfoStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_requestContext.SdataContext);
            // create the target correlation
            ResSyncInfo targetResSyncInfo = new ResSyncInfo(newUuid, requestEndpointUrl, 0, string.Empty, DateTime.Now);
            CorrelatedResSyncInfo targetInfo = new CorrelatedResSyncInfo(newLocalId, targetResSyncInfo);

            if (currentUuid == newUuid)
            {
                correlatedResSyncInfoStore.Update(resourceKindName, targetInfo);
            }
            else
            {
                correlatedResSyncInfoStore.Delete(resourceKindName, currentUuid);
                correlatedResSyncInfoStore.Add(resourceKindName, targetInfo);
            }

            // create response
            SyncFeed feed = new SyncFeed();
            feed.FeedType = FeedType.LinkedSingle;

            // create entry
            SyncFeedEntry responseEntry = new SyncFeedEntry();

            responseEntry.Id = FeedMetadataHelpers.BuildLinkedEntryUrl(_requestContext, targetInfo.ResSyncInfo.Uuid);
            responseEntry.Title = FeedMetadataHelpers.BuildLinkedEntryTitle(_requestContext, targetInfo.ResSyncInfo.Uuid);

            LinkedElement linkedElement = new LinkedElement();
            linkedElement.Resource = FeedMetadataHelpers.BuildEntryResourceUrl(_requestContext, targetInfo.LocalId);
            linkedElement.Uuid = targetInfo.ResSyncInfo.Uuid;
            responseEntry.Linked = linkedElement;

            feed.Entries.Add(responseEntry);

            request.Response.Serializer = new SyncFeedSerializer();
            request.Response.Feed = (IFeed)feed;
            request.Response.ContentType = MediaType.AtomEntry;
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion
    }
}
