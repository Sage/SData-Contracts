#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Xml;
using Sage.Common.Syndication;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Common;
using Sage.Integration.Northwind.Etag;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Sync;
using Sage.Integration.Northwind.Sync.Syndication;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Adapter.Common.Paging;
using Sage.Integration.Northwind.Adapter.Requests;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class PostSyncTargetRequestPerformer : ITrackingPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;
        private AsyncState _asyncStateObj;      // holds the state of the asynchronous call.
        private InternalAsyncPerformer _asyncPerformer;


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
            // - Read Feed from request stream.
            // - Read trackingId from request URL

            // convert tracking ID from request to type Guid
            string strTrackingId = request.Uri.TrackingID;
            if (String.IsNullOrEmpty(strTrackingId))
                throw new RequestException("TrackingId is missing");

            GuidConverter converter = new GuidConverter();
            this.TrackingId = (Guid)converter.ConvertFrom(strTrackingId);


            //read feed
            SyncFeed feed = new SyncFeed();
            XmlReader reader = XmlReader.Create(request.Stream);
            feed.ReadXml(reader, ResourceKindHelpers.GetPayloadType(_requestContext.ResourceKind));


            // *** Do work asynchronously ***
            _asyncPerformer = new InternalAsyncPerformer(this);
            _asyncPerformer.DoWork(_requestContext.Config, feed);


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
                    request.Response.Serializer = new SyncFeedSerializer();
                    SyncFeed syncFeed = _asyncPerformer.GetFeed(_requestContext.Config, startindex, count);
                    syncFeed.FeedType = FeedType.SyncTarget;
                    request.Response.Feed = syncFeed;
                }
                else if (_asyncStateObj.Tracking.Phase == TrackingPhase.ERROR)
                {
                    request.Response.Xml = XmlSerializationHelpers.SerializeObjectToXml(_asyncStateObj.Tracking);
                    request.Response.ContentType = MediaType.Xml;
                    request.Response.StatusCode = HttpStatusCode.InternalServerError;
                    request.Response.Serializer = new XmlSerializer();
                    request.Response.Protocol.SendUnknownResponseHeader("location", String.Format("{0}{1}/$syncTarget('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString(), _requestContext.TrackingId));
                }
                else
                {
                    request.Response.Xml = XmlSerializationHelpers.SerializeObjectToXml(_asyncStateObj.Tracking);
                    request.Response.ContentType = MediaType.Xml;
                    request.Response.StatusCode = HttpStatusCode.Accepted;
                    request.Response.Serializer = new XmlSerializer();
                    request.Response.Protocol.SendUnknownResponseHeader("location", String.Format("{0}{1}/$syncTarget('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString(), _requestContext.TrackingId));
                }
            }
        }

        #endregion

        #region CLASS: InternalAsyncPerformer

        private class InternalAsyncPerformer
        {
            #region Delegates

            private delegate void ExecuteDelegate(NorthwindConfig config, SyncFeed feed);

            #endregion

            #region Class Variables

            private readonly PostSyncTargetRequestPerformer _parentPerformer;

            #endregion

            #region Ctor.

            public InternalAsyncPerformer(PostSyncTargetRequestPerformer parentPerformer)
            {
                _parentPerformer = parentPerformer;
            }

            #endregion

            public void DoWork(NorthwindConfig config, SyncFeed feed)
            {
                ExecuteDelegate worker = new ExecuteDelegate(Execute);
                AsyncCallback completedCallback = new AsyncCallback(ExecuteCompletedCallback);

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);

                // Begin asynchronous method call
                worker.BeginInvoke(config, feed, completedCallback, async);
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

                List<SdataTransactionResult> transactinResults;

                sdataContext = _parentPerformer._requestContext.SdataContext;
                resource = _parentPerformer._requestContext.ResourceKind;
                resourceKind = resource.ToString();
                transactinResults = _parentPerformer._asyncStateObj.TransactionResults;
                endpoint = _parentPerformer._requestContext.DatasetLink + resourceKind; ;
                trackingId = _parentPerformer._requestContext.TrackingId;

                if (count == 0)
                    count = 10;

                feed = new SyncFeed();
                Token emptyToken = new Token();



                IEntityWrapper wrapper = EntityWrapperFactory.Create(resource, _parentPerformer._requestContext);




                for (int index = startIndex;
                index < ((startIndex + count > transactinResults.Count) ? transactinResults.Count : startIndex + count);
                index++)
                {
                    SdataTransactionResult transactionResult = (SdataTransactionResult)transactinResults[index];

                    SyncFeedEntry entry = wrapper.GetFeedEntry(transactionResult);

                    if (entry != null)
                        feed.Entries.Add(entry);
                    else
                    {
                        entry = new SyncFeedEntry();
                        entry.Uuid = transactionResult.Uuid;
                        entry.HttpStatusCode = transactionResult.HttpStatus;
                        entry.HttpMessage = transactionResult.HttpMessage; ;
                        entry.HttpMethod = transactionResult.HttpMethod;
                        entry.HttpLocation = transactionResult.Location;
                        entry.HttpETag = transactionResult.Etag;
                        feed.Entries.Add(entry);
                    }
                }

                // initialize the feed
                string url = string.Format("{0}/$syncTarget('{1}')", endpoint, trackingId);
                
                feed.Title = resourceKind;
                feed.Id = url;

                #region PAGING & OPENSEARCH

                int totalResults = transactinResults.Count;

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

                //if (startIndex + count < transactinResults.Count)
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
            private void Execute(NorthwindConfig config, SyncFeed feed)
            {
                #region Declarations

                SdataContext sdataContext;
                SupportedResourceKinds resource;
                IAppBookmarkInfoStore appBookmarkInfoStore;
                ICorrelatedResSyncInfoStore correlatedResSyncInfoStore;
                ISyncSyncDigestInfoStore syncDigestStore;

                GuidConverter guidConverter = new GuidConverter();
                string resourceKind;
                string endpoint;
                SyncFeedDigest sourceDigest;
                SyncDigestInfo targetDigest;
                
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
                sourceDigest = feed.Digest;
                targetDigest = syncDigestStore.Get(resourceKind);
                wrapper = EntityWrapperFactory.Create(resource, _parentPerformer._requestContext);

                #endregion 

                #region process entries

                bool sourceIsDeleteMode;
                SdataTransactionResult sdTrResult;
                SyncState sourceState;
                SyncState targetState;

                foreach (SyncFeedEntry entry in feed.Entries)
                {
                    sdTrResult = null;
                    
                    try
                    {
                        // Check whether the source entry had been deleted.
                        // if not we expect a payload!
                        // The variable 'sourceIsDeleteMode' holds the result of this check for later calls.
                        if (entry.HttpMethod.Equals("DELETE", StringComparison.InvariantCultureIgnoreCase))
                            sourceIsDeleteMode = true;
                        else if (null == entry.Payload)
                            throw new Exception("Payload missing.");
                        else
                            sourceIsDeleteMode = false;
    
                        
                        // set the uuid to the payload.SyncUuid property
                        if (!sourceIsDeleteMode) { entry.Payload.SyncUuid = entry.Uuid; }

                        // get the source syncstate
                        sourceState = entry.SyncState;

                        // Look into the store to get all correlations of the uuid received from source
                        CorrelatedResSyncInfo[] corelations = correlatedResSyncInfoStore.GetByUuid(resourceKind, new Guid[] { entry.Uuid });

                        if (corelations.Length == 0)
                        {
                            if (sourceIsDeleteMode)
                            {
                                // the source entry had been deleted and no correlation exists for this
                                // entry in the target.
                                // So we do nothing here.
                                sdTrResult = new SdataTransactionResult();
                                sdTrResult.HttpStatus = HttpStatusCode.OK;
                                sdTrResult.HttpMessage = "OK";
                                sdTrResult.HttpMethod = "DELETE";
                                sdTrResult.ResourceKind = resource;
                                sdTrResult.Uuid = entry.Uuid;
                            }
                            else
                            {
                                // no target entry exists for the updated or added source entry.
                                // So we add the source entry to the application and add a new correlation to the sync store.
                                // If this operation fails we will have to set the result state to conflict.
                                try
                                {
                                    // add the entry to application
                                    sdTrResult = wrapper.Add(entry.Payload, entry.SyncLinks);

                                    if ((sdTrResult != null) && ((sdTrResult.HttpStatus == HttpStatusCode.OK) || (sdTrResult.HttpStatus == HttpStatusCode.Created)))
                                    {
                                        string etag = EtagServices.ComputeEtag(entry.Payload, true);

                                        ResSyncInfo resSyncInfo =
                                            new ResSyncInfo(entry.Uuid, entry.SyncState.Endpoint,
                                                entry.SyncState.Tick, etag, entry.SyncState.Stamp);

                                        CorrelatedResSyncInfo correlatedResSyncInfo = new CorrelatedResSyncInfo(sdTrResult.LocalId, resSyncInfo);

                                        // store the new correlation to the sync store
                                        correlatedResSyncInfoStore.Put(resourceKind, correlatedResSyncInfo);
                                        // update the sync digest entry if the tick of the source entry is newer than the actual tick of the target.
                                        syncDigestStore.PersistNewer(resourceKind, correlatedResSyncInfo.ResSyncInfo);
                                    }
                                }
                                catch (Exception e)
                                {
                                    // in case of an unexpected error while adding a new entry to target
                                    // we create a transaction result with the state 'Conflict'
                                    sdTrResult = new SdataTransactionResult();
                                    sdTrResult.HttpStatus = HttpStatusCode.Conflict;
                                    sdTrResult.HttpMethod = "POST";
                                    sdTrResult.HttpMessage = e.ToString();
                                    sdTrResult.ResourceKind = resource;
                                    sdTrResult.Uuid = entry.Uuid;
                                }
                            }
                        }
                        else
                        {
                            // a correlation was found for the source entry.

                            #region update or delete
                            
                            try
                            {
                                bool doUpdate = false;
                                //bool doDelete = false;
                                bool isConflict = true;
                                
                                // set the LocalID from correlation to the entry.Payload.LocalID property
                                // only if source had not been deleted.
                                if (!sourceIsDeleteMode) { entry.Payload.LocalID = corelations[0].LocalId; }
                                
                                targetState = Helper.GetSyncState( corelations[0]);


                                //If sourceState.endpoint = targetState.endpoint, 
                                //there is no conflict and the update must be applied 
                                //if sourceState.tick > targetState.tick.
                                if (targetState.Endpoint.Equals(sourceState.Endpoint, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    isConflict = false;
                                    if (sourceState.Tick > targetState.Tick)
                                    {
                                        //if (!sourceIsDeleteMode)
                                            doUpdate = true;
                                        //else
                                        //    doDelete = true;
                                    }
                                }
                                else
                                {
                                    SyncState sourceDigestSyncState = Helper.GetSyncState(sourceDigest, targetState.Endpoint); ;
                                    SyncState targetDigestSyncState = Helper.GetSyncState(targetDigest, sourceState.Endpoint);

                                    //If targetState is contained in sourceDigest, 
                                    //i.e. if sourceDigest has a digest entry E such that E.endpoint = targetState.endpoint 
                                    //and E.tick >= targetState.tick, there is no conflict and the update must be applied. 
                                    if (sourceDigestSyncState != null)
                                    {
                                        if (sourceDigestSyncState.Tick > targetState.Tick)
                                        {
                                            doUpdate = true;
                                            isConflict = false;
                                        }
                                    }


                                    //If sourceState is contained in targetDigest, 
                                    //i.e. if targetDigest has a digest entry E such that E.endpoint = sourceState.endpoint 
                                    //and E.tick >= sourceState.tick, there is no conflict and the update must be ignored 
                                    //(target has the most recent version). 
                                    if (targetDigestSyncState != null)
                                    {
                                        if (targetDigestSyncState.Tick > sourceState.Tick)
                                        {
                                            doUpdate = false;
                                            isConflict = false;
                                        }
                                    }



                                    //Otherwise (targetState not contained in sourceDigest, sourceState not contained in targetDigest), 
                                    //there is a conflict. 
                                    if ((sourceDigestSyncState == null) && (targetDigestSyncState == null))
                                        isConflict = true;
                                }
                                  
                                //****************** Conflict ****************
                                //In case of conflict, the target endpoint uses the following algorithm to resolve the conflict:
                               //Let sourceEntry be the sourceDigest digest entry such that sourceEntry.endpoint = sourceState.endpoint. 
                                //Let targetEntry be the targetDigest digest entry such that targetEntry.endpoint = targetState.endpoint. 
                                //If sourceEntry .conflictPriority <> targetEntry .conflictPriority, the side with lowest priority wins. 
                                if(isConflict)
                                {
                                    int sourceConflictPriority = Helper.GetConflictPriority(sourceDigest, sourceState.Endpoint);
                                    int targetConflictPriority = Helper.GetConflictPriority(targetDigest, targetState.Endpoint);
                                    
                                    if (sourceConflictPriority > targetConflictPriority)
                                    {
                                        doUpdate = true;
                                    }
                                    else if (sourceConflictPriority < targetConflictPriority)
                                    {
                                        doUpdate = false;
                                    }
                                    else
                                    {
                                        //Otherwise (sourceEntry .conflictPriority = targetEntry .conflictPriority), 
                                        //then sourceState.stamp and targetState.stamp are compared 
                                        //and the entry with the most recent timestamp wins. 
                                        if (sourceState.Stamp > targetState.Stamp)
                                        {
                                            doUpdate = true;
                                        }
                                    }

                                }
                                ResSyncInfo resSyncInfo =
                                            new ResSyncInfo(entry.Uuid, entry.SyncState.Endpoint,
                                                entry.SyncState.Tick, "", entry.SyncState.Stamp);


                                if (doUpdate && !sourceIsDeleteMode)
                                {
                                    // update the entry in the application and update the sync store
                                    sdTrResult = wrapper.Update(entry.Payload, entry.SyncLinks);

                                    if ((sdTrResult != null) && (sdTrResult.HttpStatus == HttpStatusCode.OK))
                                    {
                                        string etag = EtagServices.ComputeEtag(entry.Payload, true);

                                        resSyncInfo =
                                            new ResSyncInfo(entry.Uuid, entry.SyncState.Endpoint,
                                                entry.SyncState.Tick, etag, entry.SyncState.Stamp);

                                        CorrelatedResSyncInfo correlatedResSyncInfo = new CorrelatedResSyncInfo(sdTrResult.LocalId, resSyncInfo);

                                        correlatedResSyncInfoStore.Put(resourceKind, correlatedResSyncInfo);
                                    }
                                }
                                else if (!doUpdate && !sourceIsDeleteMode)
                                {
                                    sdTrResult = new SdataTransactionResult();
                                    sdTrResult.HttpStatus = HttpStatusCode.Conflict;
                                    sdTrResult.HttpMethod = "PUT";
                                    sdTrResult.HttpMessage = "";
                                    sdTrResult.ResourceKind = resource;
                                    sdTrResult.Uuid = entry.Uuid;
                                }
                                else if (doUpdate && sourceIsDeleteMode)
                                {
                                    // delete the entry in the application and update the sync store
                                    // we do not support nested deletion. So only TradingAccounts and SalesOrders can be deleted.
                                    if (resource == SupportedResourceKinds.tradingAccounts
                                        || resource == SupportedResourceKinds.salesOrders)
                                    {
                                        sdTrResult = wrapper.Delete(corelations[0].LocalId);

                                        if ((sdTrResult != null) && (sdTrResult.HttpStatus == HttpStatusCode.OK))
                                        {
                                            correlatedResSyncInfoStore.Delete(resource.ToString(), corelations[0].ResSyncInfo.Uuid);
                                        }
                                    }
                                    else
                                    {
                                        // this  does not correspond to real fact, what we did at target side!
                                        sdTrResult = new SdataTransactionResult();
                                        sdTrResult.HttpStatus = HttpStatusCode.OK;
                                        sdTrResult.HttpMessage = "OK";
                                        sdTrResult.HttpMethod = "DELETE";
                                        sdTrResult.ResourceKind = resource;
                                        sdTrResult.Uuid = entry.Uuid;
                                    }
                                }
                                else
                                {
                                    sdTrResult = new SdataTransactionResult();
                                    sdTrResult.HttpStatus = HttpStatusCode.Conflict;
                                    sdTrResult.HttpMessage = "";
                                    sdTrResult.HttpMethod = "DELETE";
                                    sdTrResult.ResourceKind = resource;
                                    sdTrResult.Uuid = entry.Uuid;
                                }
                                
                                syncDigestStore.PersistNewer(resourceKind, resSyncInfo);

                            }
                            catch (Exception e)
                            {
                                sdTrResult = new SdataTransactionResult();
                                sdTrResult.HttpStatus = HttpStatusCode.Conflict;
                                sdTrResult.HttpMethod = "PUT";
                                sdTrResult.HttpMessage = e.ToString();
                                sdTrResult.ResourceKind = resource;
                                sdTrResult.Uuid = entry.Uuid;
                            }
                            #endregion
                        }
                    }
                    catch (Exception e)
                    {
                        sdTrResult = new SdataTransactionResult();
                        sdTrResult.HttpStatus = HttpStatusCode.Conflict;
                        sdTrResult.HttpMessage = e.ToString();
                        sdTrResult.ResourceKind = resource;
                        sdTrResult.Uuid = entry.Uuid;
                    }

                    #region store transaction result
                    
                    if (sdTrResult != null)
                    {
                        lock (_parentPerformer._asyncStateObj)
                        {
                            this._parentPerformer._asyncStateObj.TransactionResults.Add(sdTrResult);
                        }
                    }

                    #endregion

                }
                #endregion
                
                
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



        private class AsyncState
        {
            // Holds the Tracking information while synchronising
            private SyncTracking _tracking;
            private List<SdataTransactionResult> _transactionResults;
            private SyncFeed feed;



            #region Ctor.

            public AsyncState()
            {
                _transactionResults = new List<SdataTransactionResult>();
            }

            #endregion

            #region Properties

            public SyncTracking Tracking { get { return _tracking; } set { _tracking = value; } }
            public List<SdataTransactionResult> TransactionResults { get { return _transactionResults; } }
            public SyncFeed Feed
            {
                get { return feed; }
                set { feed = value; }
            }

            #endregion
        }


    }
}
