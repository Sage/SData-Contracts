#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Messaging.Model;
using System.ComponentModel;
using System.Xml;
using Sage.Common.Syndication;

using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Integration.Northwind.Application.Entities.Account;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Etag;
using Sage.Sis.Sdata.Sync.Tick;
using Sage.Integration.Northwind.Adapter.Sync;
using System.Runtime.Remoting.Messaging;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Adapter.Transformations;
using System.Net;
using System.IO;
using Sage.Integration.Northwind.Sync;
using Sage.Integration.Northwind.Sync.Syndication;
using Sage.Integration.Northwind.Common;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Integration.Northwind.Adapter.Common.Paging;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{

    internal class PostSyncSourceRequestPerformer : ITrackingPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;
        private AsyncState _asyncStateObj;      // holds the state of the asynchronous call.
        
        //private SyncDigestInfo _syncDigestInfo;

        private InternalAsyncPerformer _asyncPerformer;
        //private NorthwindConfig _config;
       
        #endregion

        #region IRequestProcess Members

        public void DoWork(IRequest request)
        {
            // If an asyncState object already exists, an exception is thrown as the performer only accepts
            // one call after each other. The Request receiver has to manage a queued execution.
            
            // Before calling the performers execution implementation method a new AsyncState object is created
            // and set to an initial tracking state.

            lock (_asyncStateObj)
            {
                if (null != _asyncStateObj.Tracking)
                    throw new InvalidOperationException("The performer cannot be executed because it is already running.");

                SyncTracking tracking = new SyncTracking();
                tracking.ElapsedSeconds = 1;
                tracking.Phase = TrackingPhase.INIT;
                tracking.PhaseDetail = "Tracking Id was: " + _requestContext.TrackingId.ToString();
                tracking.PollingMillis = 100;
                tracking.RemainingSeconds = 100;

                _asyncStateObj.Tracking = tracking;
            }


            // *** Initialization for the async execution ***
            // - Read SyncDigest from request stream.
            // - Read trackingId from request URL

            //// Load input stream (throws RequestException if it fails.)
            //XmlDocument xmlInputDoc;
            //InputStreamHelpers.Get(request, out xmlInputDoc);

            //// load syncDigest info from input stream of the request
            //SyncDigestInfo syncDigestInfo = SyncDigestInfoHelpers.Load(xmlInputDoc);

            SyncFeedEntry entry = new SyncFeedEntry();
            XmlReader reader = XmlReader.Create(request.Stream);
            reader.MoveToContent();
            entry.ReadXml(reader, typeof(SyncDigestPayload));
            if (entry.Payload == null)
            {
                throw new RequestException("Invalid Content");
            }
            SyncFeedDigest syncDigestInfo = ((SyncDigestPayload)entry.Payload).Digest;

            // convert tracking ID from request to type Guid
            string strTrackingId = request.Uri.TrackingID;
            if (String.IsNullOrEmpty(strTrackingId))
                throw new RequestException("TrackingId is missing");
        
            GuidConverter converter = new GuidConverter();
            this.TrackingId = (Guid)converter.ConvertFrom(strTrackingId);


            
            // *** Do work asynchronously ***
            _asyncPerformer = new InternalAsyncPerformer(this);
            _asyncPerformer.DoWork(_requestContext.Config, syncDigestInfo);


            // *** set the tracking to the request response ***
            this.GetTrackingState(request);
        }

        void IRequestPerformer.Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
            _asyncStateObj = new AsyncState();
        }

        #endregion

        #region ITrackingPerformer Members

        public Guid TrackingId { get; private set; }
        public void GetTrackingState(IRequest request)
        {
            lock (_asyncStateObj)
            {
                if (null == _asyncStateObj.Tracking)
                    throw new InvalidOperationException("Performer has not been started.");


                if (_asyncStateObj.Tracking.Phase == TrackingPhase.FINISH)
                {
                    request.Response.ContentType = MediaType.Atom;
                    int startindex = Convert.ToInt32(request.Uri.StartIndex);
                    int count = Convert.ToInt32(request.Uri.Count);
                    SdataContext sdataContext = _requestContext.SdataContext;
                    ICorrelatedResSyncInfoStore correlatedResSyncInfoStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(sdataContext);

                    request.Response.Serializer = new SyncFeedSerializer();
                    SyncFeed syncFeed = _asyncPerformer.GetFeed(_requestContext.Config, startindex, count);
                    syncFeed.FeedType = FeedType.SyncSource;
                    request.Response.Feed = syncFeed;
                }
                else if (_asyncStateObj.Tracking.Phase == TrackingPhase.ERROR)
                {
                    request.Response.Xml = XmlSerializationHelpers.SerializeObjectToXml(_asyncStateObj.Tracking);
                    request.Response.ContentType = MediaType.Xml;
                    request.Response.StatusCode = HttpStatusCode.InternalServerError;
                    request.Response.Serializer = new XmlSerializer();
                    request.Response.Protocol.SendUnknownResponseHeader("location", String.Format("{0}{1}/$syncSource('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString(), _requestContext.TrackingId));
                }
                else
                {
                    request.Response.Xml = XmlSerializationHelpers.SerializeObjectToXml(_asyncStateObj.Tracking);
                    request.Response.ContentType = MediaType.Xml;
                    request.Response.StatusCode = HttpStatusCode.Accepted;
                    request.Response.Serializer = new XmlSerializer();
                    request.Response.Protocol.SendUnknownResponseHeader("location", String.Format("{0}{1}/$syncSource('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString(), _requestContext.TrackingId));
                }
            }
        }
        
        #endregion

        #region CLASS: InternalAsyncPerformer

        private class InternalAsyncPerformer
        {
            #region Delegates

            private delegate void ExecuteDelegate(NorthwindConfig config, SyncFeedDigest syncDigestInfo);

            #endregion

            #region Class Variables

            private readonly PostSyncSourceRequestPerformer _parentPerformer;

            #endregion

            #region Ctor.

            public InternalAsyncPerformer(PostSyncSourceRequestPerformer parentPerformer)
            {
                _parentPerformer = parentPerformer;
            }

            #endregion

            public void DoWork(NorthwindConfig config, SyncFeedDigest syncDigestInfo)
            {
                ExecuteDelegate worker = new ExecuteDelegate(Execute);
                AsyncCallback completedCallback = new AsyncCallback(ExecuteCompletedCallback);

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);

                // Begin asynchronous method call
                worker.BeginInvoke(config, syncDigestInfo, completedCallback, async);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config"></param>
            /// <returns></returns>
            /// <remarks>This method is not threadsafe as the performer must be finished when calling this method.</remarks>
            public SyncFeed GetFeed(NorthwindConfig config, int startIndex, int count)
            {
                SyncFeed feed;
                SdataContext sdataContext;
                SupportedResourceKinds resource;
                string resourceKind;
                string endpoint;
                Guid trackingId;


                List<CorrelatedResSyncInfo> correlatedResSyncInfos;

                sdataContext = _parentPerformer._requestContext.SdataContext;
                resource = _parentPerformer._requestContext.ResourceKind;
                resourceKind = resource.ToString();
                correlatedResSyncInfos = _parentPerformer._asyncStateObj.CorrelatedResSyncInfos;
                endpoint = _parentPerformer._requestContext.DatasetLink + resourceKind;
                trackingId = _parentPerformer._requestContext.TrackingId;


                ISyncSyncDigestInfoStore syncDigestStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetSyncDigestStore(sdataContext);
                SyncDigestInfo syncDigestInfo = syncDigestStore.Get(resourceKind);


                if (count == 0)
                    count = 10;

                feed = new SyncFeed();
                Token emptyToken = new Token();

                feed.Digest = new SyncFeedDigest();
                feed.Digest.Origin = _parentPerformer._requestContext.OriginEndPoint;
                feed.Digest.Entries = new List<SyncFeedDigestEntry>();

                if (syncDigestInfo != null)
                {
                    foreach (SyncDigestEntryInfo entryinfo in syncDigestInfo)
                    {
                        SyncFeedDigestEntry entry = new SyncFeedDigestEntry();
                        entry.ConflictPriority = entryinfo.ConflictPriority;
                        entry.Endpoint = entryinfo.Endpoint;
                        entry.Tick = entryinfo.Tick;
                        entry.Stamp = DateTime.Now;
                        feed.Digest.Entries.Add(entry);
                    }
                }



                IEntityWrapper wrapper = EntityWrapperFactory.Create(resource, _parentPerformer._requestContext);


                for (int index = startIndex;
                index < ((startIndex + count > correlatedResSyncInfos.Count) ? correlatedResSyncInfos.Count : startIndex + count);
                index++)
                {
                    CorrelatedResSyncInfo resSyncInfo = (CorrelatedResSyncInfo)correlatedResSyncInfos[index];
                    SyncFeedEntry entry = wrapper.GetFeedEntry(resSyncInfo);
                    if (entry != null)
                        feed.Entries.Add(entry);
                    else
                    {
                        entry = new SyncFeedEntry();
                        entry.HttpMethod = "DELETE";
                        entry.Uuid = resSyncInfo.ResSyncInfo.Uuid;
                        feed.Entries.Add(entry);
                    }


                }


                // initialize the feed
                string url = string.Format("{0}/$syncSource('{1}')", endpoint, trackingId);

                feed.Id = url;
                feed.Title = resourceKind;

                #region PAGING & OPENSEARCH

                int totalResults = correlatedResSyncInfos.Count;

                PageController pageController = new PageController(startIndex+1, FeedMetadataHelpers.DEFAULT_ITEMS_PER_PAGE, totalResults, count, url);

                /* PAGING */
                FeedLinkCollection feedLinks = new FeedLinkCollection();
                feedLinks.Add(new FeedLink(pageController.GetLinkSelf(), LinkType.Self, MediaType.Atom, "Current Page"));
                feedLinks.Add(new FeedLink(pageController.GetLinkFirst(), LinkType.First, MediaType.Atom, "First Page"));
                feedLinks.Add(new FeedLink(pageController.GetLinkLast(), LinkType.Last, MediaType.Atom, "Last Page"));

                string linkUrl;
                if (pageController.GetLinkNext(out linkUrl))
                    feedLinks.Add(new FeedLink(linkUrl, LinkType.Next, MediaType.Atom, "Next Page"));
                if (pageController.GetLinkPrevious(out linkUrl))
                    feedLinks.Add(new FeedLink(linkUrl, LinkType.Previous, MediaType.Atom, "Previous Page"));

                feed.Links = feedLinks;


                /* OPENSEARCH */
                feed.Opensearch_ItemsPerPageElement = pageController.GetOpensearch_ItemsPerPageElement();
                feed.Opensearch_StartIndexElement = pageController.GetOpensearch_StartIndexElement();
                feed.Opensearch_TotalResultsElement = pageController.GetOpensearch_TotalResultsElement();

                #endregion

                

                //feed.Id = url;
                //if (startIndex + count < correlatedResSyncInfos.Count)
                //{
                //    FeedLink linkNext = new FeedLink(string.Format("{0}?startIndex={1}&count=10", url, startIndex + count), LinkType.Next);
                //    feed.Links.Add(linkNext);
                //}

                //FeedLink linkFirst = new FeedLink(String.Format("{0}?startIndex=0&count=10", url), LinkType.First);
                //feed.Links.Add(linkFirst);

                //FeedLink linkSelf = new FeedLink(String.Format("{0}?startIndex={1}&count=10", url, startIndex), LinkType.Self);
                //feed.Links.Add(linkSelf);

                return feed;
            }

            #region Private Helpers

            // Asynchronous called method
            private void Execute(NorthwindConfig config, SyncFeedDigest syncDigestInfo)
            {
                #region Declaration

                SdataContext sdataContext;
                SupportedResourceKinds resource;
                IAppBookmarkInfoStore appBookmarkInfoStore;
                ICorrelatedResSyncInfoStore correlatedResSyncInfoStore;
                ISyncSyncDigestInfoStore syncDigestStore;
                ISyncTickProvider tickProvider;
                string resourceKind;
                string endpoint;
                int nextTick = 0;
                Token lastToken;
                Token nextToken;
                Identity[] changedIdentites;
                IEntityWrapper wrapper; 

                #endregion

                #region init

                sdataContext = _parentPerformer._requestContext.SdataContext;
                resource = _parentPerformer._requestContext.ResourceKind;
                resourceKind = resource.ToString();
                endpoint = _parentPerformer._requestContext.DatasetLink + resourceKind;
                appBookmarkInfoStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetAppBookmarkStore(sdataContext);
                correlatedResSyncInfoStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(sdataContext);
                syncDigestStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetSyncDigestStore(sdataContext);
                tickProvider = RequestReceiver.NorthwindAdapter.StoreLocator.GetTickProvider(sdataContext);

                wrapper = EntityWrapperFactory.Create(resource, _parentPerformer._requestContext);

                #endregion

                #region get last token or create a new one

                if (!appBookmarkInfoStore.Get<Token>(resourceKind, out lastToken))
                {
                    lastToken = new Token();
                    lastToken.InitRequest = true;
                }

                #endregion


                #region Get local identities of changed entries since last synchronisation

                changedIdentites = wrapper.Entity.GetLastChanges(lastToken, config, out nextToken);

                #endregion

                if (resource == SupportedResourceKinds.phoneNumbers)
                {
                    #region workaround for phones
                    for (int index = 0; index < changedIdentites.Length; index++)
                    {
                        string phoneid = changedIdentites[index].Id + Sage.Integration.Northwind.Application.API.Constants.PhoneIdPostfix;
                        string faxId = changedIdentites[index].Id + Sage.Integration.Northwind.Application.API.Constants.FaxIdPostfix;


                        // receive the feed entry for local identity
                        SyncFeedEntry phoneentry = wrapper.GetFeedEntry(phoneid);
                        SyncFeedEntry faxentry = wrapper.GetFeedEntry(faxId);
                        if (phoneentry == null && faxentry == null)
                            continue;
                        // receive the correlation for the local identity


                        if (phoneentry != null)
                        {
                            CorrelatedResSyncInfo[] correlatedResSyncInfos = correlatedResSyncInfoStore.GetByLocalId(resourceKind, new string[] { phoneid });



                            string etag = EtagServices.ComputeEtag(phoneentry.Payload, true);   // new etag
                            if (correlatedResSyncInfos.Length == 0)
                            {
                                nextTick = tickProvider.CreateNextTick(resourceKind); // create next tick

                                ResSyncInfo resyncInfo = new ResSyncInfo(Guid.NewGuid(), endpoint, nextTick, etag, DateTime.Now);
                                CorrelatedResSyncInfo info = new CorrelatedResSyncInfo(phoneid, resyncInfo);

                                correlatedResSyncInfoStore.Put(resourceKind, info);

                                syncDigestStore.PersistNewer(resourceKind, info.ResSyncInfo);



                            }
                            else if (!correlatedResSyncInfos[0].ResSyncInfo.Etag.Equals(etag))
                            {
                                nextTick = tickProvider.CreateNextTick(resourceKind);
                                correlatedResSyncInfos[0].ResSyncInfo.Etag = etag;
                                correlatedResSyncInfos[0].ResSyncInfo.Tick = nextTick;
                                correlatedResSyncInfos[0].ResSyncInfo.Endpoint = endpoint;
                                correlatedResSyncInfos[0].ResSyncInfo.ModifiedStamp = DateTime.Now;
                                correlatedResSyncInfoStore.Put(resourceKind, correlatedResSyncInfos[0]);

                                syncDigestStore.PersistNewer(resourceKind, correlatedResSyncInfos[0].ResSyncInfo);
                            }
                        }
                        if (faxentry != null)
                        {
                            CorrelatedResSyncInfo[] correlatedResSyncInfos = correlatedResSyncInfoStore.GetByLocalId(resourceKind, new string[] { faxId });

                            string etag = EtagServices.ComputeEtag(faxentry.Payload, true);   // new etag
                            if (correlatedResSyncInfos.Length == 0)
                            {
                                nextTick = tickProvider.CreateNextTick(resourceKind); // create next tick

                                ResSyncInfo resyncInfo = new ResSyncInfo(Guid.NewGuid(), endpoint, nextTick, etag, DateTime.Now);
                                CorrelatedResSyncInfo info = new CorrelatedResSyncInfo(faxId, resyncInfo);

                                correlatedResSyncInfoStore.Put(resourceKind, info);

                                syncDigestStore.PersistNewer(resourceKind, info.ResSyncInfo);



                            }
                            else if (!correlatedResSyncInfos[0].ResSyncInfo.Etag.Equals(etag))
                            {
                                nextTick = tickProvider.CreateNextTick(resourceKind);
                                correlatedResSyncInfos[0].ResSyncInfo.Etag = etag;
                                correlatedResSyncInfos[0].ResSyncInfo.Tick = nextTick;
                                correlatedResSyncInfos[0].ResSyncInfo.Endpoint = endpoint;
                                correlatedResSyncInfos[0].ResSyncInfo.ModifiedStamp = DateTime.Now;
                                correlatedResSyncInfoStore.Put(resourceKind, correlatedResSyncInfos[0]);

                                syncDigestStore.PersistNewer(resourceKind, correlatedResSyncInfos[0].ResSyncInfo);
                            }
                        }


                    }
                    #endregion
                }
                else
                {
                    for (int index = 0; index < changedIdentites.Length; index++)
                    {
                        string id = changedIdentites[index].Id;
                        // receive the feed entry for local identity
                        SyncFeedEntry entry = wrapper.GetFeedEntry(id);
                        if (entry == null)
                            continue;
                        // receive the correlation for the local identity



                        CorrelatedResSyncInfo[] correlatedResSyncInfos = correlatedResSyncInfoStore.GetByLocalId(resourceKind, new string[] { id });



                        string etag = EtagServices.ComputeEtag(entry.Payload, true);   // new etag
                        if (correlatedResSyncInfos.Length == 0)
                        {
                            nextTick = tickProvider.CreateNextTick(resourceKind); // create next tick

                            ResSyncInfo resyncInfo = new ResSyncInfo(Guid.NewGuid(), endpoint, nextTick, etag, DateTime.Now);
                            CorrelatedResSyncInfo info = new CorrelatedResSyncInfo(id, resyncInfo);

                            correlatedResSyncInfoStore.Put(resourceKind, info);

                            syncDigestStore.PersistNewer(resourceKind, info.ResSyncInfo);



                        }
                        else if (!correlatedResSyncInfos[0].ResSyncInfo.Etag.Equals(etag))
                        {
                            nextTick = tickProvider.CreateNextTick(resourceKind);
                            correlatedResSyncInfos[0].ResSyncInfo.Etag = etag;
                            correlatedResSyncInfos[0].ResSyncInfo.Tick = nextTick;
                            correlatedResSyncInfos[0].ResSyncInfo.Endpoint = endpoint;
                            correlatedResSyncInfos[0].ResSyncInfo.ModifiedStamp = DateTime.Now;
                            correlatedResSyncInfoStore.Put(resourceKind, correlatedResSyncInfos[0]);

                            syncDigestStore.PersistNewer(resourceKind, correlatedResSyncInfos[0].ResSyncInfo);
                        }
                    }



                }

                #region store next token

                appBookmarkInfoStore.Put(resourceKind, nextToken);

                #endregion

                // set tracking phase
                lock (_parentPerformer._asyncStateObj)
                {
                    _parentPerformer._asyncStateObj.Tracking.Phase = TrackingPhase.GETCHANGESBYTICK;
                }


                // Receive syncDigestInfo
                if (null != syncDigestInfo)
                {
                    ICorrelatedResSyncInfoEnumerator enumerator;
                    List<string> endpoints = new List<string>();

                    foreach (SyncFeedDigestEntry digestEntry in syncDigestInfo.Entries)
                    {
                        endpoints.Add(digestEntry.Endpoint);
                        enumerator = correlatedResSyncInfoStore.GetSinceTick(resourceKind, digestEntry.Endpoint, digestEntry.Tick-2);
                        while (enumerator.MoveNext())
                        {
                            // No lock needed, as we expect that CorrelatedResSyncInfos list is
                            // only acceeded anywhere else when Tracking phase is 'finish'.
                            //lock(_parentPerformer._asyncStateObj)
                            //{
                            _parentPerformer._asyncStateObj.CorrelatedResSyncInfos.Add(enumerator.Current);
                            //}
                        }
                    }

                    SyncDigestInfo sourceSyncDigestInfo = syncDigestStore.Get(resourceKind);
                    foreach (SyncDigestEntryInfo digestEntry in sourceSyncDigestInfo)
                    {
                        if (endpoints.Contains(digestEntry.Endpoint))
                            continue;
                        endpoints.Add(digestEntry.Endpoint);
                        enumerator = correlatedResSyncInfoStore.GetSinceTick(resourceKind, digestEntry.Endpoint, -1);
                        while (enumerator.MoveNext())
                        {
                            // No lock needed, as we expect that CorrelatedResSyncInfos list is
                            // only acceeded anywhere else when Tracking phase is 'finish'.
                            //lock(_parentPerformer._asyncStateObj)
                            //{
                            _parentPerformer._asyncStateObj.CorrelatedResSyncInfos.Add(enumerator.Current);
                            //}
                        }
                    }
                    if (!endpoints.Contains(endpoint))
                    {
                        enumerator = correlatedResSyncInfoStore.GetSinceTick(resourceKind, endpoint, -1);
                        while (enumerator.MoveNext())
                        {
                            // No lock needed, as we expect that CorrelatedResSyncInfos list is
                            // only acceeded anywhere else when Tracking phase is 'finish'.
                            //lock(_parentPerformer._asyncStateObj)
                            //{
                            _parentPerformer._asyncStateObj.CorrelatedResSyncInfos.Add(enumerator.Current);
                            //}
                        }
                    }
                }




                // Set tracking phase
                lock (_parentPerformer._asyncStateObj.Tracking)
                {
                    _parentPerformer._asyncStateObj.Tracking.Phase = TrackingPhase.FINISH;
                }

            }

            // Called when asynchronous method call finished.
            private void ExecuteCompletedCallback(IAsyncResult ar)
            {
                // Retrieve the delegate.
                ExecuteDelegate caller = (ExecuteDelegate)((AsyncResult)ar).AsyncDelegate;

                try
                {
                    // Finish the invocation. (Here we could receive out values if out parameters had been defined in the delegate)
                    caller.EndInvoke(ar);
                }
                catch (Exception exception)
                {
                    // Any unhandled exception of the asynchronous method call can be handled here.
                    // Note: Handle all exceptions here. Otherwise the SData Server would shut down.
                    lock (_parentPerformer._asyncStateObj)
                    {
                        _parentPerformer._asyncStateObj.Tracking.Phase = TrackingPhase.ERROR;
                        _parentPerformer._asyncStateObj.Tracking.PhaseDetail = exception.Message + " ***** Stack Trace ***** " + exception.StackTrace;
                    }
                }
            }

            #endregion

        }


        #endregion

        #region CLASS: AsyncState

        private class AsyncState
        {
            // Holds the Tracking information while synchronising
            private SyncTracking _tracking;
            // List that holds the resource synchronisation items that have been received while synchronising.
            private List<CorrelatedResSyncInfo> _correlatedResSyncInfos;

            #region Ctor.

            public AsyncState()
            {
                _correlatedResSyncInfos = new List<CorrelatedResSyncInfo>();
            }

            #endregion

            #region Properties

            public SyncTracking Tracking { get { return _tracking; } set { _tracking = value; } }
            public List<CorrelatedResSyncInfo> CorrelatedResSyncInfos { get { return _correlatedResSyncInfos; } }

            #endregion
        }

        #endregion
    }
}
