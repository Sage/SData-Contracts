#region Usings

using System;
using System.ComponentModel;
using System.Diagnostics;
using Sage.Common.Syndication;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Integration.Northwind.Feeds;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Adapter.Common.Paging;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class GetLinkingRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            EntryRequestType entryRequestType;
            string resourceKindName;
            ICorrelatedResSyncInfoStore correlatedResSyncStore;
            bool includeDataPayloads;   //?includePayloads value

            CorrelatedResSyncInfo[] correlatedResSyncInfos;
            int totalResult;

            #region initialization
            
            // multi or single entry request?
            entryRequestType = (String.IsNullOrEmpty(_requestContext.ResourceKey)) ? EntryRequestType.Multi : EntryRequestType.Single;

            resourceKindName = _requestContext.ResourceKind.ToString();

            string tmpValue;
            // ?includePayloads
            includeDataPayloads = false;    // default value, but check for settings now
            if (_requestContext.SdataUri.QueryArgs.TryGetValue("includePayload", out tmpValue))
                includeDataPayloads = System.Xml.XmlConvert.ToBoolean(tmpValue);

            // get store to request the correlations
            correlatedResSyncStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_requestContext.SdataContext);

            #endregion

            // receive correlated resource synchronization entries
            if (entryRequestType == EntryRequestType.Multi)
            {
                int pageNumber = FeedMetadataHelpers.GetPageNumber(_requestContext);

                correlatedResSyncInfos = correlatedResSyncStore.GetPaged(resourceKindName, pageNumber, FeedMetadataHelpers.DEFAULT_ITEMS_PER_PAGE, out totalResult);
            }
            else
            {
                Guid uuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(_requestContext.ResourceKey);
                correlatedResSyncInfos = correlatedResSyncStore.GetByUuid(resourceKindName, new Guid[]{uuid});
                totalResult = correlatedResSyncInfos.Length;
            }

            // Create the feed
            SyncFeed feed = new SyncFeed();

            // initialize the feed
            feed.Id = FeedMetadataHelpers.BuildBaseUrl(_requestContext, FeedMetadataHelpers.RequestKeywordType.linked);
            feed.Title = string.Format("{0} Linking Feed", resourceKindName);

            #region PAGING & OPENSEARCH

            /* PAGING */
            feed.Links = FeedMetadataHelpers.CreatePageFeedLinks(_requestContext, totalResult, FeedMetadataHelpers.RequestKeywordType.linked);
            
            /* OPENSEARCH */
            PageController pageLinkBuilder = FeedMetadataHelpers.GetPageLinkBuilder(_requestContext, totalResult, FeedMetadataHelpers.RequestKeywordType.linked);

            feed.Opensearch_ItemsPerPageElement = pageLinkBuilder.GetOpensearch_ItemsPerPageElement();
            feed.Opensearch_StartIndexElement = pageLinkBuilder.GetOpensearch_StartIndexElement();
            feed.Opensearch_TotalResultsElement = pageLinkBuilder.GetOpensearch_TotalResultsElement();

            #endregion

            feed.FeedType = (entryRequestType == EntryRequestType.Multi) ? FeedType.Linked : FeedType.LinkedSingle;

            IEntityWrapper wrapper = null;
            if (includeDataPayloads)
                wrapper = EntityWrapperFactory.Create(_requestContext.ResourceKind, _requestContext);

            for (int i=0; i<correlatedResSyncInfos.Length; i++)
            {
                SyncFeedEntry entry = null;

                if (includeDataPayloads)
                {
                    entry = wrapper.GetFeedEntry(correlatedResSyncInfos[i].LocalId);
                }
                else
                {
                    entry = new SyncFeedEntry();
                }

                entry.Id = FeedMetadataHelpers.BuildLinkedEntryUrl(_requestContext, correlatedResSyncInfos[i].ResSyncInfo.Uuid);
                entry.Title = FeedMetadataHelpers.BuildLinkedEntryTitle(_requestContext, correlatedResSyncInfos[i].ResSyncInfo.Uuid);

                #region LINKED ELEMENT

                LinkedElement linkedElement = new LinkedElement();
                linkedElement.Resource = FeedMetadataHelpers.BuildEntryResourceUrl(_requestContext, correlatedResSyncInfos[i].LocalId);
                linkedElement.Uuid = correlatedResSyncInfos[i].ResSyncInfo.Uuid;
                entry.Linked = linkedElement;

                #endregion

                feed.Entries.Add(entry);
            }

            request.Response.Serializer = new SyncFeedSerializer();
            request.Response.Feed = (IFeed)feed;
            request.Response.ContentType = (entryRequestType == EntryRequestType.Multi) ? MediaType.Atom : MediaType.AtomEntry;
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion

        #region ENUM: EntryRequestType

        private enum EntryRequestType { Single, Multi }

        #endregion
    }
}
