using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Messaging.Model;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Adapter.Common.Paging;
using System.ComponentModel;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Integration.Northwind.Adapter.Feeds;

namespace Sage.Integration.Northwind.Adapter.Common.Handler
{
    class LinkingCRUD
    {
        private RequestContext _requestContext;
        IRequest _request;
        private const int DEFAULT_ITEMS_PER_PAGE = 10;

        public LinkingCRUD(IRequest request)
        {
            _requestContext = new RequestContext(request.Uri);
            _request = request;
        }

        public void Create(FeedEntry entry)
        {
            // only atom entry supported!!!

            if (_request.ContentType != Sage.Common.Syndication.MediaType.AtomEntry)
                throw new RequestException("Atom entry content type expected");

            string requestEndPointUrl;
            RequestContext entryResourceAsRequestContext;

            string url;             // The url that references an existing resource
            string localId;         // will be parsed from urlAttrValue and set to the key attribute of the result resource payload
            Guid uuid;              // the new uuid

            if (null == entry)
                throw new RequestException("sdata payload element missing");

            // the consumer MUST provide an sdata:url attribute that references an existing resource.
            url = entry.Uri;
            if (string.IsNullOrEmpty(url))
                throw new RequestException("sdata url attribute missing for resource payload element.");


            // Parse the url of thew url attribute to get the local id.
            // Additionally we check for equality of the urls of the request url and
            // in the linked element up to the resourceKind.
            requestEndPointUrl = _requestContext.OriginEndPoint;
            entryResourceAsRequestContext = new RequestContext(new Sage.Common.Syndication.SDataUri(url));    // TODO: not really nice here.
            string linkedResourceUrl = entryResourceAsRequestContext.OriginEndPoint;

            if (!string.Equals(requestEndPointUrl, linkedResourceUrl, StringComparison.InvariantCultureIgnoreCase))
                throw new RequestException("Request url and linked entry resource not matching.");

            string resourceKindName = _requestContext.ResourceKind.ToString();
            localId = entryResourceAsRequestContext.ResourceKey;

            ICorrelatedResSyncInfoStore correlatedResSyncInfoStore = NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_requestContext.SdataContext);

            CheckExisting(correlatedResSyncInfoStore, localId);

            // try to get the new uuid from uuid attribute
            // if this attribute is not set a new one is created
            if (null == entry.UUID || entry.UUID == Guid.Empty)
                uuid = Guid.NewGuid();
            else
                uuid = entry.UUID;


            // store a new correlation entry to the sync store
            ResSyncInfo newResSyncInfo = new ResSyncInfo(uuid, requestEndPointUrl, 0, string.Empty, DateTime.Now);
            CorrelatedResSyncInfo newInfo = new CorrelatedResSyncInfo(localId, newResSyncInfo);
            correlatedResSyncInfoStore.Add(resourceKindName, newInfo);



            // If the service consumer only needs to retrieve the URL, not the actual payload, 
            // it may do so by adding an empty select parameter to its request:
            // a) select parameter not exist -> return deep resource payload
            // b) select exists and empty -> return no resource payload
            // c) select exists and not empty -> return deep resource payload as we do not yet support payload filtering
            //    with select parameter.
            string tmpValue;
            // ?select
            bool includeResourcePayloads = true;    // default value, but check for select parameter now
            if (_requestContext.SdataUri.QueryArgs.TryGetValue("select", out tmpValue))
                if (string.IsNullOrEmpty(_requestContext.SdataUri.QueryArgs["select"]))
                    includeResourcePayloads = false;


            // Create an entity wrapper if resource data should be requested. Otherwise
            // leave wrapper null.
            IFeedEntryEntityWrapper wrapper = null;
            //if (includeResourcePayloads) //TODO: Comment this in as soon as there is a possibility to send empty payload objects
                wrapper = FeedEntryWrapperFactory.Create(_requestContext.ResourceKind, _requestContext);

            /* Create the response entry */
            _request.Response.FeedEntry = (FeedEntry)this.BuildFeedEntryForCorrelation(newInfo, wrapper);
            _request.Response.ContentType = MediaType.AtomEntry;

            _request.Response.StatusCode = System.Net.HttpStatusCode.Created;
            _request.Response.Protocol.SendUnknownResponseHeader("location", FeedMetadataHelpers.BuildLinkedEntryUrl(_requestContext, uuid));
        }

        public void Read()
        {
            string resourceKindName;
            EntryRequestType entryRequestType;  // Multi or Single
            bool includeResourcePayloads;
            ICorrelatedResSyncInfoStore correlatedResSyncStore;
            CorrelatedResSyncInfo[] correlatedResSyncInfos;
            int totalResult;                    // number of correlations found
            PageInfo normalizedPageInfo;        // normalized pageInfo -> will be used for paging, etc.

            resourceKindName = _requestContext.ResourceKind.ToString();

            // multi or single entry request?
            entryRequestType = (String.IsNullOrEmpty(_requestContext.ResourceKey)) ? EntryRequestType.Multi : EntryRequestType.Single;

            // get store to request the correlations
            correlatedResSyncStore = NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_requestContext.SdataContext);


            // receive correlated resource synchronization entries
            if (entryRequestType == EntryRequestType.Multi)
            {
                int pageNumber = PagingHelpers.GetPageNumber(_request.Uri.StartIndex, _request.Uri.Count);

                correlatedResSyncInfos = correlatedResSyncStore.GetPaged(resourceKindName, pageNumber, (int)(_request.Uri.Count ?? DEFAULT_ITEMS_PER_PAGE), out totalResult);
            }
            else
            {
                Guid uuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(_requestContext.ResourceKey);
                correlatedResSyncInfos = correlatedResSyncStore.GetByUuid(resourceKindName, new Guid[] { uuid });
                totalResult = correlatedResSyncInfos.Length;
                if (totalResult > 1)
                    throw new ApplicationException("More than one resource for uuid exists.");
                if (totalResult == 0)
                    throw new RequestException("No resource found");
            }


            // If the service consumer only needs to retrieve the URL, not the actual payload, 
            // it may do so by adding an empty select parameter to its request:
            // a) select parameter not exist -> return deep resource payload
            // b) select exists and empty -> return no resource payload
            // c) select exists and not empty -> return deep resource payload as we do not yet support payload filtering
            //    with select parameter.
            string tmpValue;
            // ?select
            includeResourcePayloads = true;    // default value, but check for select parameter now
            if (_requestContext.SdataUri.QueryArgs.TryGetValue("select", out tmpValue))
                if (string.IsNullOrEmpty(_requestContext.SdataUri.QueryArgs["select"]))
                    includeResourcePayloads = false;



            if (entryRequestType == EntryRequestType.Single)
            {
                IFeedEntryEntityWrapper wrapper = null;
                //if (includeResourcePayloads) //TODO: Comment this in as soon as there is a possibility to send empty payload objects
                    wrapper = FeedEntryWrapperFactory.Create(_requestContext.ResourceKind, _requestContext);

                _request.Response.FeedEntry = this.BuildFeedEntryForCorrelation(correlatedResSyncInfos[0], wrapper);
                _request.Response.ContentType = MediaType.AtomEntry;
            }
            else
            {
                // Create an empty feed
                Feed<FeedEntry> feed = new Feed<FeedEntry>();
                feed.Author = new FeedAuthor();
                feed.Author.Name = "Northwind Adapter";
                feed.Category = new FeedCategory("http://schemas.sage.com/sdata/categories", "collection", "Resource Collection");
                DateTime updateTime = DateTime.Now;

                string feedUrl = _requestContext.SdataUri.ToString();   // the url requested
                string feedUrlWithoutQuery = (new Uri(feedUrl)).GetLeftPart(UriPartial.Path);   // the url without query

                normalizedPageInfo = PagingHelpers.Normalize(_request.Uri.StartIndex, _request.Uri.Count, totalResult);

                IFeedEntryEntityWrapper wrapper = null;
                //if (includeResourcePayloads) //TODO: Comment this in as soon as there is a possibility to send empty payload objects
                    wrapper = FeedEntryWrapperFactory.Create(_requestContext.ResourceKind, _requestContext);

                // create a feed entry for each correlation.
                IEnumerator<CorrelatedResSyncInfo> pagedCorrelationEnumerator = PagingHelpers.GetPagedEnumerator<CorrelatedResSyncInfo>(normalizedPageInfo, correlatedResSyncInfos);
                while (pagedCorrelationEnumerator.MoveNext())
                {
                    // Create and append a feed entry per each id
                    FeedEntry entry = this.BuildFeedEntryForCorrelation(pagedCorrelationEnumerator.Current, wrapper);
                    if(entry != null)
                        feed.Entries.Add(entry);
                }


                // set feed title
                feed.Title = string.Format("{0} linked {1}", feed.Entries.Count, resourceKindName);

                // set feed update
                feed.Updated = updateTime;

                // set id
                feed.Id = feedUrl;

                // add links (general)
                feed.Links.AddRange(LinkFactory.CreateFeedLinks(_requestContext, feedUrl));

                #region PAGING & OPENSEARCH

                // add links (paging)
                feed.Links.AddRange(LinkFactory.CreatePagingLinks(normalizedPageInfo, totalResult, feedUrlWithoutQuery));

                /* OPENSEARCH */
                feed.ItemsPerPage = normalizedPageInfo.Count;
                feed.StartIndex = normalizedPageInfo.StartIndex;
                feed.TotalResults = totalResult;

                #endregion

                _request.Response.Feed = feed;
                _request.Response.ContentType = MediaType.Atom;
            }
        }

        public void Update(FeedEntry entry)
        {
            // only atom entry supported!!!
            if (_request.ContentType != Sage.Common.Syndication.MediaType.AtomEntry)
                throw new RequestException("Atom entry content type expected");

            // resource key must exist in url
            if (string.IsNullOrEmpty(_requestContext.ResourceKey))
                throw new RequestException("ResourceKey missing in requested url.");

            string requestEndPointUrl;
            RequestContext entryResourceAsRequestContext;

            string url;             // The url that references an existing resource
            string localId;         // will be parsed from urlAttrValue and set to the key attribute of the result resource payload
            Guid uuid;              // the new uuid

            // retrieve the feed entry
            //entry = FeedComponentFactory.Create<ILinkingResourceFeedEntry>(request.Stream);


            // Get the uuid from query
            uuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(_requestContext.ResourceKey);


            if (null == entry)
                throw new RequestException("sdata payload element missing");

            // the consumer MUST provide an sdata:url attribute that references an existing resource.
            url = entry.Uri;
            if (string.IsNullOrEmpty(url))
                throw new RequestException("sdata url attribute missing for resource payload element.");


            // Parse the url of thew url attribute to get the local id.
            // Additionally we check for equality of the urls of the request url and
            // in the linked element up to the resourceKind.
            requestEndPointUrl = _requestContext.OriginEndPoint;
            entryResourceAsRequestContext = new RequestContext(new Sage.Common.Syndication.SDataUri(url));    // TODO: not really nice here.
            string linkedResourceUrl = entryResourceAsRequestContext.OriginEndPoint;

            if (!string.Equals(requestEndPointUrl, linkedResourceUrl, StringComparison.InvariantCultureIgnoreCase))
                throw new RequestException("Request url and linked entry resource not matching.");

            string resourceKindName = _requestContext.ResourceKind.ToString();
            localId = entryResourceAsRequestContext.ResourceKey;



            // The correlation store
            ICorrelatedResSyncInfoStore correlatedResSyncInfoStore = NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_requestContext.SdataContext);


            // update the correlation entry in the sync store.
            // if uuid is not in use -> throw exception
            // if resource is already linked -> remove existing link, reassign link
            // if resource is not yet linked -> reassign link
            CorrelatedResSyncInfo[] correlations;
            CorrelatedResSyncInfo correlationToModify = null;

            // retrieve the correlation we want to reassign by given uuid
            correlations = correlatedResSyncInfoStore.GetByUuid(resourceKindName, new Guid[] { uuid });
            if (correlations.Length > 0)
                correlationToModify = correlations[0];
            else
                throw new RequestException("Uuid not in use");

            //remember the old Correlation for the new localId, will be deleted after the update.
            CorrelatedResSyncInfo[] oldCorrelations = correlatedResSyncInfoStore.GetByLocalId(resourceKindName, new string[] { localId });

            // change the local ID to link to the new resource.
            // change the modification stamp and reset the tick
            correlationToModify.LocalId = localId;
            correlationToModify.ResSyncInfo.ModifiedStamp = DateTime.Now;
            correlationToModify.ResSyncInfo.Tick = 1;

            // update the correlation
            correlatedResSyncInfoStore.Update(resourceKindName, correlationToModify);

            //If updating went OK, delete the old correlation for the new localId (should be only 1, for-loop just to be sure to catch em all)
            foreach (CorrelatedResSyncInfo oldCorrelation in oldCorrelations)
            {
                correlatedResSyncInfoStore.Delete(resourceKindName, oldCorrelation.ResSyncInfo.Uuid);
            }

            // If the service consumer only needs to retrieve the URL, not the actual payload, 
            // it may do so by adding an empty select parameter to its request:
            // a) select parameter not exist -> return deep resource payload
            // b) select exists and empty -> return no resource payload
            // c) select exists and not empty -> return deep resource payload as we do not yet support payload filtering
            //    with select parameter.
            string tmpValue;
            // ?select
            bool includeResourcePayloads = true;    // default value, but check for select parameter now
            if (_requestContext.SdataUri.QueryArgs.TryGetValue("select", out tmpValue))
                if (string.IsNullOrEmpty(_requestContext.SdataUri.QueryArgs["select"]))
                    includeResourcePayloads = false;


            // Create an entity wrapper if resource data should be requested. Otherwise
            // leave wrapper null.
            IFeedEntryEntityWrapper wrapper = null;
            //if (includeResourcePayloads) //TODO: Comment this in as soon as there is a possibility to send empty payload objects
                wrapper = FeedEntryWrapperFactory.Create(_requestContext.ResourceKind, _requestContext);

            /* Create the response entry */
            _request.Response.FeedEntry = (FeedEntry)this.BuildFeedEntryForCorrelation(correlationToModify, wrapper);
            _request.Response.ContentType = MediaType.AtomEntry;
        }

        public void Delete()
        {
            if (String.IsNullOrEmpty(_requestContext.ResourceKey))
                throw new RequestException("Please use a uuid predicate.");
            // TODO: Check resourceKind for value None???

            Guid uuid;
            string resourceKindName;
            ICorrelatedResSyncInfoStore correlatedResSyncInfoStore;

            // Convert resourceKey into Guid instance
            uuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(_requestContext.ResourceKey);
            resourceKindName = _requestContext.ResourceKind.ToString();

            // retrieve correlations from store
            correlatedResSyncInfoStore = new Sage.Integration.Northwind.Sync.StoreLocator().GetCorrelatedResSyncStore(_requestContext.SdataContext);

            // remove the correlation from store
            correlatedResSyncInfoStore.Delete(resourceKindName, uuid);
        }

        private enum EntryRequestType { Single, Multi }

        #region Helpers

        protected FeedEntry BuildFeedEntryForCorrelation(CorrelatedResSyncInfo corrResSyncInfo, IFeedEntryEntityWrapper wrapper)
        {
            FeedEntry feedEntry;

            #region Payload

            if (null != wrapper)
            {
                // Get resource data
                feedEntry = wrapper.GetFeedEntry(corrResSyncInfo.LocalId);
            }
            else
            {
                // Create an empty payload container
                feedEntry = new FeedEntry();
                feedEntry.IsDeleted = false;

                feedEntry.Key = corrResSyncInfo.LocalId;
            }
            if (feedEntry != null)
            {
                // modify url and set uuid as we are requesting linked resources here.
                feedEntry.Uri = string.Format("{0}{1}('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString() , corrResSyncInfo.LocalId);
                feedEntry.UUID = corrResSyncInfo.ResSyncInfo.Uuid;

            #endregion

                // set id tag
                feedEntry.Id = feedEntry.Uri;

                // set title tag
                feedEntry.Title = string.Format("{0}('{1}') : {2}", _requestContext.ResourceKind.ToString(), corrResSyncInfo.LocalId, corrResSyncInfo.ResSyncInfo.Uuid);
                feedEntry.Updated = corrResSyncInfo.ResSyncInfo.ModifiedStamp.ToLocalTime();

                // set resource dependent  links (self, edit, schema, template, post, service)
                feedEntry.Links.AddRange(LinkFactory.CreateEntryLinks(_requestContext, feedEntry));

            }
            return feedEntry;
        }

        private void CheckExisting(ICorrelatedResSyncInfoStore correlatedResSyncInfoStore, string localId)
        {
            CorrelatedResSyncInfo[] existingCorrelations = correlatedResSyncInfoStore.GetByLocalId(_requestContext.ResourceKind.ToString(), new string[] { localId });
            if (existingCorrelations.Length > 0)
            {
                StringBuilder b = new StringBuilder(existingCorrelations.Length + " correlations for " + localId + " already exist ( ");
                foreach (CorrelatedResSyncInfo existingCorrelation in existingCorrelations)
                {
                    b.Append(existingCorrelation.ResSyncInfo.Uuid);
                    b.Append(" ");
                }
                b.Append(")");
                throw new RequestException(b.ToString());
            }
        }
        #endregion
    }
}
