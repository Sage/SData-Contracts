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
using Sage.Integration.Northwind.Sync;
using Sage.Integration.Northwind.Sync.Syndication;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Adapter.Common.Paging;
using Sage.Integration.Northwind.Adapter.Requests;
using System.Reflection;
using System.IO;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Etag;

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
        { }

        public void DoWork(IRequest request, IFeed feed, Digest digest)
        {
          //  ReturnSample(request);
           // return;
            // If an asyncState object already exists, an exception is thrown as the performer only accepts
            // one call after each other. The Request receiver has to manage a queued execution.

            // Before calling the performers execution implementation method a new AsyncState object is created
            // and set to an initial tracking state.

            lock (_asyncStateObj)
            {
                if (null != _asyncStateObj.Tracking)
                    throw new InvalidOperationException("The performer cannot be executed because it is already running.");

                ITracking tracking = new Tracking();
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


            // *** Do work asynchronously ***
            _asyncPerformer = new InternalAsyncPerformer(this);
            _asyncPerformer.DoWork(_requestContext.Config, feed, digest);


            // *** set the tracking to the request response ***
            this.GetTrackingState(request);
        }

        void IRequestPerformer.Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
            _asyncStateObj = new AsyncState();
        }

        #endregion

        private void ReturnSample(IRequest request)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            string tracking = null;

            System.Xml.Serialization.XmlSerializer xmlSerializerEntry = new System.Xml.Serialization.XmlSerializer(typeof(Tracking));
            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream("Sage.Integration.Northwind.Adapter.Requests.Performers.SamplePayloads.PostSyncTarget.xml")))
            {
                tracking = sr.ReadToEnd();
                //tracking = (Tracking)xmlSerializerEntry.Deserialize(sr);
            }
            request.Response.Xml = tracking;//NorthwindFeedSerializer.GetXml(tracking);

            request.Response.ContentType = MediaType.Xml;
            //request.Response.Serializer  = new Sage.Common.Syndication.XmlSerializer();
            request.Response.StatusCode = HttpStatusCode.Accepted;
            request.Response.Protocol.SendUnknownResponseHeader("location", String.Format("{0}{1}/$syncSource('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString(), _requestContext.TrackingId));
        }

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
                    PageInfo normalizedPageInfo = PagingHelpers.Normalize(request.Uri.StartIndex, request.Uri.Count, _asyncStateObj.TransactionResults.Count);
                    SdataContext sdataContext = _requestContext.SdataContext;
                    request.Response.ContentType = MediaType.Atom;
                    request.Response.Feed = _asyncPerformer.GetFeed(_requestContext.Config, normalizedPageInfo);

                }
                else if (_asyncStateObj.Tracking.Phase == TrackingPhase.ERROR)
                {
                    //request.Response.Xml = NorthwindFeedSerializer.GetXml(_asyncStateObj.Tracking);
                    //request.Response.ContentType = MediaType.Xml;
                    //request.Response.Serializer  = new Sage.Common.Syndication.XmlSerializer();
                    request.Response.Tracking = _asyncStateObj.Tracking;
                    request.Response.StatusCode = HttpStatusCode.InternalServerError;
                    request.Response.Protocol.SendUnknownResponseHeader("location", String.Format("{0}{1}/$syncTarget('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString(), _requestContext.TrackingId));
                }
                else
                {
                    //request.Response.Xml = NorthwindFeedSerializer.GetXml(_asyncStateObj.Tracking);
                    //request.Response.ContentType = MediaType.Xml;
                    //request.Response.Serializer  = new Sage.Common.Syndication.XmlSerializer();
                    request.Response.Tracking = _asyncStateObj.Tracking;
                    request.Response.StatusCode = HttpStatusCode.Accepted;
                    request.Response.Protocol.SendUnknownResponseHeader("location", String.Format("{0}{1}/$syncTarget('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString(), _requestContext.TrackingId));
                }
            }
        }

        #endregion

        #region CLASS: InternalAsyncPerformer

        private class InternalAsyncPerformer
        {
            #region Delegates

            private delegate void ExecuteDelegate(NorthwindConfig config, IFeed feed, Digest digest);

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

            public void DoWork(NorthwindConfig config, IFeed feed, Digest digest)
            {
                ExecuteDelegate worker = new ExecuteDelegate(Execute);
                AsyncCallback completedCallback = new AsyncCallback(ExecuteCompletedCallback);

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);

                // Begin asynchronous method call
                worker.BeginInvoke(config, feed, digest, completedCallback, async);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config"></param>
            /// <returns></returns>
            /// <remarks>This method is not threadsafe as the performer must be finished when calling this method.</remarks>
            public Feed<FeedEntry> GetFeed(NorthwindConfig config, PageInfo normalizedPageInfo)
            {
                Feed<FeedEntry> syncTargetFeed = new Feed<FeedEntry>();

                syncTargetFeed.Author = new FeedAuthor();
                syncTargetFeed.Author.Name = "Northwind Adapter";
                syncTargetFeed.Category = new FeedCategory("http://schemas.sage.com/sdata/categories", "collection", "Resource Collection");

                SdataContext sdataContext;
                SupportedResourceKinds resource;
                string resourceKind;
                string EndPoint;
                Guid trackingId;

                List<SdataTransactionResult> transactinResults;

                sdataContext = _parentPerformer._requestContext.SdataContext;
                resource = _parentPerformer._requestContext.ResourceKind;
                resourceKind = resource.ToString();
                transactinResults = _parentPerformer._asyncStateObj.TransactionResults;
                EndPoint = _parentPerformer._requestContext.DatasetLink + resourceKind; ;
                trackingId = _parentPerformer._requestContext.TrackingId;


                // Create a new Feed instance
    

                // retrieve the data connection wrapper
                IFeedEntryEntityWrapper wrapper = FeedEntryWrapperFactory.Create(resource, _parentPerformer._requestContext);

                IEnumerator<SdataTransactionResult> transactionResultEnumerator = PagingHelpers.GetPagedEnumerator<SdataTransactionResult>(normalizedPageInfo, transactinResults.ToArray());
                while (transactionResultEnumerator.MoveNext())
                {
                    syncTargetFeed.Entries.Add(this.BuildFeedEntry(transactionResultEnumerator.Current, wrapper));
                }

                // initialize the feed
                string feedUrl = string.Format("{0}/$syncTarget('{1}')", EndPoint, trackingId);
                string feedUrlWithoutQuery = (new Uri(feedUrl)).GetLeftPart(UriPartial.Path);   // the url without query
                // set id tag
                syncTargetFeed.Id = feedUrl;
                // set title tag
                syncTargetFeed.Title = string.Format("{0} synchronization target feed {1}", resourceKind.ToString(), trackingId);
                // set update
#warning  implement this
                //syncTargetFeed.Updated = DateTime.Now;
                // set syncMode
                syncTargetFeed.SyncMode = SyncMode.catchUp;

                // add links (general)
                syncTargetFeed.Links.AddRange(LinkFactory.CreateFeedLinks(_parentPerformer._requestContext, feedUrl));


                #region PAGING & OPENSEARCH

                // add links (paging)
                syncTargetFeed.Links.AddRange(LinkFactory.CreatePagingLinks(normalizedPageInfo, transactinResults.Count, feedUrlWithoutQuery));

                /* OPENSEARCH */
                syncTargetFeed.ItemsPerPage = normalizedPageInfo.Count;
                syncTargetFeed.StartIndex = normalizedPageInfo.StartIndex;
                syncTargetFeed.TotalResults = transactinResults.Count;

                #endregion


                return syncTargetFeed;
            }

            private FeedEntry BuildFeedEntry(SdataTransactionResult transactionResult, IFeedEntryEntityWrapper wrapper)
            {
                // Create result feed entry
                FeedEntry feedEntry;

                if (null != transactionResult.Diagnosis)
                {
                    /* set diagnosis */
                    feedEntry = new FeedEntry();
                    feedEntry.Diagnoses = new Diagnoses();
                    feedEntry.Diagnoses.Add(transactionResult.Diagnosis);
                }
                else
                {
                    /* get and the resource payload */

                    // Get resource data
                    feedEntry = wrapper.GetSyncTargetFeedEntry(transactionResult);

                    // set id tag
                    feedEntry.Id = feedEntry.Uri;

                    // set title tag
                    feedEntry.Title = String.Format("{0}: {1}", _parentPerformer._requestContext.ResourceKind.ToString(), feedEntry.Key);

                    // set resource dependent  links (self, edit, schema, template, post, service)
                    feedEntry.Links.AddRange(LinkFactory.CreateEntryLinks(_parentPerformer._requestContext, feedEntry));
                }

                
                // set updated
                feedEntry.Updated = DateTime.Now.ToLocalTime();


                feedEntry.HttpStatusCode = transactionResult.HttpStatus;
                feedEntry.HttpMessage = transactionResult.HttpMessage; ;
                if (transactionResult.HttpMethod == HttpMethod.PUT)
                    feedEntry.HttpMethod = "PUT";
                else if (transactionResult.HttpMethod == HttpMethod.POST)
                    feedEntry.HttpMethod = "POST";
                else
                    feedEntry.HttpMethod = "DELETE";

                feedEntry.HttpLocation = transactionResult.Location;


                return feedEntry;
            }
            #region Private Helpers

            // Asynchronous called method
            private void Execute(NorthwindConfig config, IFeed feed, Digest sourceDigest)
            {
                #region Declarations

                SdataContext sdataContext;
                SupportedResourceKinds resource;
                IAppBookmarkInfoStore appBookmarkInfoStore;
                ICorrelatedResSyncInfoStore correlatedResSyncInfoStore;
                ISyncSyncDigestInfoStore syncDigestStore;

                GuidConverter guidConverter = new GuidConverter();
                string resourceKind;
                string EndPoint;
                //Digest sourceDigest;
                SyncDigestInfo targetDigest;
                
                IFeedEntryEntityWrapper wrapper;

                #endregion

                #region init

                sdataContext = _parentPerformer._requestContext.SdataContext;
                resource = _parentPerformer._requestContext.ResourceKind;
                resourceKind = resource.ToString();
                EndPoint = _parentPerformer._requestContext.DatasetLink + resourceKind;
                appBookmarkInfoStore = NorthwindAdapter.StoreLocator.GetAppBookmarkStore(sdataContext);
                correlatedResSyncInfoStore = NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(sdataContext);
                syncDigestStore = NorthwindAdapter.StoreLocator.GetSyncDigestStore(sdataContext);
//#warning implement this
                //sourceDigest = new Digest();
                //sourceDigest = ((Feed<FeedEntry>)feed).Digest;
                targetDigest = syncDigestStore.Get(resourceKind);
                wrapper = FeedEntryWrapperFactory.Create(resource, _parentPerformer._requestContext);

                #endregion 

                #region process entries

                bool sourceIsDeleteMode;
                SdataTransactionResult sdTrResult;
                SyncState sourceState;
                SyncState targetState;
                foreach (FeedEntry entry in feed.Entries)
                {
                    sdTrResult = null;

                    try
                    {
                        // Check whether the source entry had been deleted.
                        // if not we expect a payload!
                        // The variable 'sourceIsDeleteMode' holds the result of this check for later calls.
                        if (entry.IsDeleted)
                            sourceIsDeleteMode = true;
                        /*else if (null == entry.Payload)
                            throw new Exception("Payload missing.");*/
                        else
                            sourceIsDeleteMode = false;

                        // get the source syncstate
                        sourceState = entry.SyncState;
                        string uuidString = entry.UUID.ToString();
                        Guid uuid = entry.UUID;

                        // Look into the store to get all correlations of the uuid received from source
                        CorrelatedResSyncInfo[] corelations = correlatedResSyncInfoStore.GetByUuid(resourceKind, new Guid[] { uuid });

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
                                sdTrResult.HttpMethod = HttpMethod.DELETE;
                                sdTrResult.ResourceKind = resource;
                                sdTrResult.Uuid = uuidString;
                            }
                            else
                            {
                                // no target entry exists for the updated or added source entry.
                                // So we add the source entry to the application and add a new correlation to the sync store.
                                // If this operation fails we will have to set the result state to conflict.
                                try
                                {
                                    // add the entry to application
                                    sdTrResult = wrapper.Add(entry);

                                    if ((sdTrResult != null) && ((sdTrResult.HttpStatus == HttpStatusCode.OK) || (sdTrResult.HttpStatus == HttpStatusCode.Created)))
                                    {
                                        string etag =  EtagServices.ComputeEtag(entry, true);

                                        ResSyncInfo resSyncInfo =
                                            new ResSyncInfo(uuid, entry.SyncState.EndPoint,
                                                (entry.SyncState.Tick > 0) ? entry.SyncState.Tick : 1, etag, entry.SyncState.Stamp);

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
                                    sdTrResult.HttpMethod = HttpMethod.POST;
                                    sdTrResult.HttpMessage = e.ToString();
                                    sdTrResult.ResourceKind = resource;
                                    sdTrResult.Uuid = uuidString;
                                }
                            }
                        }
                        else
                        {
                            string key = corelations[0].LocalId; 
                            // a correlation was found for the source entry.

                            #region update or delete

                            try
                            {
                                bool doUpdate = false;
                                //bool doDelete = false;
                                bool isConflict = true;

                                // set the Key from correlation to the entry.Payload.Key property
                                // only if source had not been deleted.
                                if (!sourceIsDeleteMode) { entry.Key = corelations[0].LocalId; }

                                targetState = Helper.GetSyncState(corelations[0]);


                                //If sourceState.EndPoint = targetState.EndPoint, 
                                //there is no conflict and the update must be applied 
                                //if sourceState.Tick > targetState.tick.
                                if (targetState.EndPoint.Equals(sourceState.EndPoint, StringComparison.InvariantCultureIgnoreCase))
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
                                    SyncState sourceDigestSyncState = Helper.GetSyncState(sourceDigest, targetState.EndPoint); ;
                                    SyncState targetDigestSyncState = Helper.GetSyncState(targetDigest, sourceState.EndPoint);

                                    //If targetState is contained in sourceDigest, 
                                    //i.e. if sourceDigest has a digest entry E such that E.EndPoint = targetState.EndPoint 
                                    //and E.Tick >= targetState.Tick, there is no conflict and the update must be applied. 
                                    if (sourceDigestSyncState != null)
                                    {
                                        if (sourceDigestSyncState.Tick > targetState.Tick)
                                        {
                                            doUpdate = true;
                                            isConflict = false;
                                        }
                                    }


                                    //If sourceState is contained in targetDigest, 
                                    //i.e. if targetDigest has a digest entry E such that E.EndPoint = sourceState.EndPoint 
                                    //and E.Tick >= sourceState.Tick, there is no conflict and the update must be ignored 
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
                                //In case of conflict, the target EndPoint uses the following algorithm to resolve the conflict:
                                //Let sourceEntry be the sourceDigest digest entry such that sourceEntry.EndPoint = sourceState.EndPoint. 
                                //Let targetEntry be the targetDigest digest entry such that targetEntry.EndPoint = targetState.EndPoint. 
                                //If sourceEntry .conflictPriority <> targetEntry .conflictPriority, the side with lowest priority wins. 
                                if (isConflict)
                                {
                                    int sourceConflictPriority = Helper.GetConflictPriority(sourceDigest, sourceState.EndPoint);
                                    int targetConflictPriority = Helper.GetConflictPriority(targetDigest, targetState.EndPoint);

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
                                            new ResSyncInfo(uuid, entry.SyncState.EndPoint,
                                                (entry.SyncState.Tick>0)?entry.SyncState.Tick:1, "", entry.SyncState.Stamp);


                                if (doUpdate && !sourceIsDeleteMode)
                                {
                                    // update the entry in the application and update the sync store
                                    sdTrResult = wrapper.Update(entry);

                                    if ((sdTrResult != null) && (sdTrResult.HttpStatus == HttpStatusCode.OK))
                                    {
                                        string etag = EtagServices.ComputeEtag(entry, true);

                                        resSyncInfo =
                                            new ResSyncInfo(uuid, entry.SyncState.EndPoint,
                                                (entry.SyncState.Tick > 0) ? entry.SyncState.Tick : 1, etag, entry.SyncState.Stamp);

                                        CorrelatedResSyncInfo correlatedResSyncInfo = new CorrelatedResSyncInfo(sdTrResult.LocalId, resSyncInfo);

                                        correlatedResSyncInfoStore.Put(resourceKind, correlatedResSyncInfo);
                                    }
                                }
                                else if (!doUpdate && !sourceIsDeleteMode)
                                {
                                    sdTrResult = new SdataTransactionResult();
                                    sdTrResult.HttpStatus = HttpStatusCode.Conflict;
                                    sdTrResult.HttpMethod = HttpMethod.PUT;
                                    sdTrResult.HttpMessage = "";
                                    sdTrResult.ResourceKind = resource;
                                    sdTrResult.Uuid = uuidString;
                                    sdTrResult.LocalId = key;

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
                                        sdTrResult.HttpMethod = HttpMethod.DELETE;
                                        sdTrResult.ResourceKind = resource;
                                        sdTrResult.Uuid = uuidString;
                                        sdTrResult.LocalId = key;
                                    }
                                }
                                else
                                {
                                    sdTrResult = new SdataTransactionResult();
                                    sdTrResult.HttpStatus = HttpStatusCode.Conflict;
                                    sdTrResult.HttpMessage = "";
                                    sdTrResult.HttpMethod = HttpMethod.DELETE;
                                    sdTrResult.ResourceKind = resource;
                                    sdTrResult.Uuid = uuidString;
                                    sdTrResult.LocalId = key;
                                }

                                syncDigestStore.PersistNewer(resourceKind, resSyncInfo);

                            }
                            catch (Exception e)
                            {
                                sdTrResult = new SdataTransactionResult();
                                sdTrResult.HttpStatus = HttpStatusCode.Conflict;
                                sdTrResult.HttpMethod = HttpMethod.PUT;
                                sdTrResult.HttpMessage = e.ToString();
                                sdTrResult.ResourceKind = resource;
                                sdTrResult.Uuid = uuidString;
                                sdTrResult.LocalId = key;
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
                        //sdTrResult.Uuid = entry.Payload.PayloadContainer.Uuid;
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
            private ITracking _tracking;
            private List<SdataTransactionResult> _transactionResults;
            private Feed<FeedEntry> feed;



            #region Ctor.

            public AsyncState()
            {
                _transactionResults = new List<SdataTransactionResult>();
            }

            #endregion

            #region Properties

            public ITracking Tracking { get { return _tracking; } set { _tracking = value; } }
            public List<SdataTransactionResult> TransactionResults { get { return _transactionResults; } }
            public Feed<FeedEntry> Feed
            {
                get { return feed; }
                set { feed = value; }
            }

            #endregion
        }


    }
}
