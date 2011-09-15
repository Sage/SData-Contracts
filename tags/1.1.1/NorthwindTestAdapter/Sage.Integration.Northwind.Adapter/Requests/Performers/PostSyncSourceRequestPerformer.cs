#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.Remoting.Messaging;
using Sage.Common.Syndication;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Common.Paging;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Integration.Northwind.Adapter.Sync;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.API;

using Sage.Integration.Northwind.Sync;
using Sage.Integration.Northwind.Sync.Syndication;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.Reflection;
using System.IO;
using Sage.Integration.Adapter.Model;
using Sage.Integration.Northwind.Adapter.Data.FeedEntries;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Etag;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{

    public class PostSyncSourceRequestPerformer : ITrackingPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;
        private AsyncState _asyncStateObj;      // holds the state of the asynchronous call.
        private InternalAsyncPerformer _asyncPerformer;
       
        #endregion

        #region IRequestProcess Members
        public void DoWork(IRequest request)
        { }

        public void DoWork(IRequest request, DigestFeedEntry entry)
        {

            //ReturnSample(request);
            //return;
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

            

            
            if (null == entry.Digest)
                throw new RequestException("Digest payload missing in payload element.");

            Digest targetDigest = entry.Digest;

            
            // *** Do work asynchronously ***
            _asyncPerformer = new InternalAsyncPerformer(this);
            _asyncPerformer.DoWork(_requestContext.Config, targetDigest);


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
                    PageInfo normalizedPageInfo = PagingHelpers.Normalize(request.Uri.StartIndex, request.Uri.Count, _asyncStateObj.CorrelatedResSyncInfos.Count);
                    SdataContext sdataContext = _requestContext.SdataContext;
                    request.Response.ContentType = MediaType.Atom;
                    //request.Response.Serializer  = FeedComponentFactory.GetSerializer<ISyncSourceFeed>();
                    request.Response.Feed = _asyncPerformer.GetFeed(_requestContext.Config, normalizedPageInfo);
                }
                else if (_asyncStateObj.Tracking.Phase == TrackingPhase.ERROR)
                {
                    //request.Response.Xml = NorthwindFeedSerializer.GetXml(_asyncStateObj.Tracking);
                    //request.Response.ContentType = MediaType.Xml;
                    //request.Response.Serializer  = new Sage.Common.Syndication.XmlSerializer();
                    request.Response.Tracking = _asyncStateObj.Tracking;
                    request.Response.StatusCode = HttpStatusCode.InternalServerError;
                    
                    request.Response.Protocol.SendUnknownResponseHeader("location", String.Format("{0}{1}/$syncSource('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString(), _requestContext.TrackingId));
                }
                else
                {
                    //request.Response.Xml = NorthwindFeedSerializer.GetXml(_asyncStateObj.Tracking);
                    //request.Response.ContentType = MediaType.Xml;
                    //request.Response.Serializer  = new Sage.Common.Syndication.XmlSerializer();
                    request.Response.Tracking = _asyncStateObj.Tracking;
                    request.Response.StatusCode = HttpStatusCode.Accepted;
                    request.Response.Protocol.SendUnknownResponseHeader("location", String.Format("{0}{1}/$syncSource('{2}')", _requestContext.DatasetLink, _requestContext.ResourceKind.ToString(), _requestContext.TrackingId));
                }
            }
        }
        
        #endregion

        private void ReturnSample(IRequest request)
        {

            Assembly assembly = Assembly.GetExecutingAssembly();
            string tracking;

            //System.Xml.Serialization.XmlSerializer xmlSerializerEntry = new System.Xml.Serialization.XmlSerializer(typeof(Tracking));
            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream("Sage.Integration.Northwind.Adapter.Requests.Performers.SamplePayloads.PostSyncSource.xml")))
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

        #region CLASS: InternalAsyncPerformer

        private class InternalAsyncPerformer
        {
            #region Delegates

            private delegate void ExecuteDelegate(NorthwindConfig config, Digest targetDigest);

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

            public void DoWork(NorthwindConfig config, Digest targetDigest)
            {
                ExecuteDelegate worker = new ExecuteDelegate(Execute);
                AsyncCallback completedCallback = new AsyncCallback(ExecuteCompletedCallback);

                AsyncOperation async = AsyncOperationManager.CreateOperation(null);

                // Begin asynchronous method call
                worker.BeginInvoke(config, targetDigest, completedCallback, async);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="config"></param>
            /// <returns></returns>
            /// <remarks>This method is not threadsafe as the performer must be finished when calling this method.</remarks>
            public Feed<FeedEntry> GetFeed(NorthwindConfig config, PageInfo normalizedPageInfo)
            {
                Feed<FeedEntry> syncSourceFeed;
                SdataContext sdataContext;
                SupportedResourceKinds resource;
                string resourceKind;
                string EndPoint;
                Guid trackingId;

                List<CorrelatedResSyncInfo> correlatedResSyncInfos;

                sdataContext = _parentPerformer._requestContext.SdataContext;
                resource = _parentPerformer._requestContext.ResourceKind;
                resourceKind = resource.ToString();
                correlatedResSyncInfos = _parentPerformer._asyncStateObj.CorrelatedResSyncInfos;
                EndPoint = _parentPerformer._requestContext.DatasetLink + resourceKind;
                trackingId = _parentPerformer._requestContext.TrackingId;


                //ISyncSyncDigestInfoStore syncDigestStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetSyncDigestStore(sdataContext);
                ISyncSyncDigestInfoStore syncDigestStore = NorthwindAdapter.StoreLocator.GetSyncDigestStore(sdataContext);
                SyncDigestInfo syncDigestInfo = syncDigestStore.Get(resourceKind);

                // Create a new Feed instance
                syncSourceFeed = new Feed<FeedEntry>();//FeedComponentFactory.Create<ISyncSourceFeed>();  
                syncSourceFeed.Author = new FeedAuthor();
                syncSourceFeed.Author.Name = "Northwind Adapter";
                syncSourceFeed.Category = new FeedCategory("http://schemas.sage.com/sdata/categories", "collection", "Resource Collection");

               

                #region Digest

                syncSourceFeed.Digest = new Digest();
                syncSourceFeed.Digest.Entries = (syncDigestInfo == null) ? new DigestEntry[0] : new DigestEntry[syncDigestInfo.Count];

                // set digest origin
                syncSourceFeed.Digest.Origin = _parentPerformer._requestContext.OriginEndPoint;
                
                if (syncDigestInfo != null)
                {
                    // convert and set digest entries from synch store object to feed object
                    for (int i = 0; i < syncDigestInfo.Count; i++)
                    {
                        syncSourceFeed.Digest.Entries[i] = new DigestEntry();

                        syncSourceFeed.Digest.Entries[i].ConflictPriority = syncDigestInfo[i].ConflictPriority;
                        syncSourceFeed.Digest.Entries[i].EndPoint = syncDigestInfo[i].EndPoint;
                        syncSourceFeed.Digest.Entries[i].Tick = (int)syncDigestInfo[i].Tick;
                        syncSourceFeed.Digest.Entries[i].Stamp = DateTime.Now;
                    }
                }

                #endregion

                #region Entries

                // retrieve the data connection wrapper
                IFeedEntryEntityWrapper wrapper = FeedEntryWrapperFactory.Create(resource, _parentPerformer._requestContext);

                IEnumerator<CorrelatedResSyncInfo> correlationEnumerator = PagingHelpers.GetPagedEnumerator<CorrelatedResSyncInfo>(normalizedPageInfo, correlatedResSyncInfos.ToArray());
                while (correlationEnumerator.MoveNext())
                {
                    syncSourceFeed.Entries.Add(this.BuildFeedEntry(correlationEnumerator.Current, wrapper));
                }

                #endregion

                // initialize the feed
                string feedUrl = string.Format("{0}/$syncSource('{1}')", EndPoint, trackingId);
                string feedUrlWithoutQuery = (new Uri(feedUrl)).GetLeftPart(UriPartial.Path);   // the url without query
                // set id tag
                syncSourceFeed.Id = feedUrl;
                // set title tag
                syncSourceFeed.Title = string.Format("{0} synchronization source feed {1}", resourceKind.ToString(), trackingId);
                // set update
               // syncSourceFeed.Updated = DateTime.Now;
                // set syncMode
                syncSourceFeed.SyncMode = SyncMode.catchUp;// syncModeenum.catchUp;

                // add links (general)
                syncSourceFeed.Links.AddRange(LinkFactory.CreateFeedLinks(_parentPerformer._requestContext, feedUrl));

                #region PAGING & OPENSEARCH

                // add links (paging)
                syncSourceFeed.Links.AddRange(LinkFactory.CreatePagingLinks(normalizedPageInfo, correlatedResSyncInfos.Count, feedUrlWithoutQuery));

                /* OPENSEARCH */
                syncSourceFeed.ItemsPerPage = normalizedPageInfo.Count;
                syncSourceFeed.StartIndex = normalizedPageInfo.StartIndex;
                syncSourceFeed.TotalResults = correlatedResSyncInfos.Count;

                #endregion

                return syncSourceFeed;
            }

            private FeedEntry BuildFeedEntry(CorrelatedResSyncInfo correlation, IFeedEntryEntityWrapper wrapper)
            {
                // create a new empty Feed Entry
                //ISyncSourceResourceFeedEntry feedEntry = FeedComponentFactory.Create<ISyncSourceResourceFeedEntry>();
                // get resource payload container from data store
                FeedEntry feedEntry = wrapper.GetSyncSourceFeedEntry(correlation);
 

                // create and set SyncState
                feedEntry.SyncState = new SyncState();
                feedEntry.SyncState.EndPoint = correlation.ResSyncInfo.EndPoint;
                feedEntry.SyncState.Tick = (correlation.ResSyncInfo.Tick > 0) ? correlation.ResSyncInfo.Tick : 1;
                feedEntry.SyncState.Stamp = correlation.ResSyncInfo.ModifiedStamp;

                // set the id tag
                feedEntry.Id = feedEntry.Uri;
                // set the title tag
                feedEntry.Title = String.Format("{0}: {1}", _parentPerformer._requestContext.ResourceKind.ToString(), feedEntry.Key);
                // set the updated tag
                feedEntry.Updated = correlation.ResSyncInfo.ModifiedStamp.ToLocalTime();

                // set resource dependent  links (self, edit, schema, template, post, service)
                feedEntry.Links.AddRange(LinkFactory.CreateEntryLinks(_parentPerformer._requestContext, feedEntry));


                return feedEntry;
            }

            #region Private Helpers

            // Asynchronous called method
            private void Execute(NorthwindConfig config, Digest targetDigest)
            {
                #region Declaration

                SdataContext sdataContext;
                SupportedResourceKinds resource;
                IAppBookmarkInfoStore appBookmarkInfoStore;
                ICorrelatedResSyncInfoStore correlatedResSyncInfoStore;
                ISyncSyncDigestInfoStore syncDigestStore;
                ISynctickProvider tickProvider;
                string resourceKind;
                string EndPoint;
                int nexttick = 1;
                Token lastToken;
                Token nextToken;
                Identity[] changedIdentites;
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
                tickProvider = NorthwindAdapter.StoreLocator.GettickProvider(sdataContext);

                wrapper = FeedEntryWrapperFactory.Create(resource, _parentPerformer._requestContext);

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


                        // receive the resource payloads for the phone id and for the fax id
                        // so that we can calculate their current etags.
                        FeedEntry phoneResourcePayloadContainer = wrapper.GetFeedEntry(phoneid);
                        FeedEntry faxResourcePayloadConatainer = wrapper.GetFeedEntry(faxId);

                        string etag;
                        CorrelatedResSyncInfo[] correlatedResSyncInfos;

                        #region Phone
                        if (phoneResourcePayloadContainer != null)
                        {
                            // calculate etag of the resource
                            etag = EtagServices.ComputeEtag(phoneResourcePayloadContainer, true);   // new etag

                            // retrieve correlations for the current identity from synch storage
                            correlatedResSyncInfos = correlatedResSyncInfoStore.GetByLocalId(resourceKind, new string[] { phoneid });

                            // if no correlation exists AND resource has been deleted:
                            // -> continue with next id
                            // else if no correlation exists:
                            // -> get next tick, create new correlation and add correlation to synch store
                            // otherwise and if the etag stored in synch store is different than etag previously calculated:
                            // -> get next tick, modify existing correlation and update it in synch store.
                            if (correlatedResSyncInfos.Length == 0 && phoneResourcePayloadContainer.IsDeleted)
                            {
                                continue;
                            }
                            else if (correlatedResSyncInfos.Length == 0)
                            {
                                nexttick = tickProvider.CreateNexttick(resourceKind); // create next tick

                                ResSyncInfo resyncInfo = new ResSyncInfo(Guid.NewGuid(), EndPoint, nexttick, etag, DateTime.Now);
                                CorrelatedResSyncInfo info = new CorrelatedResSyncInfo(phoneid, resyncInfo);

                                correlatedResSyncInfoStore.Put(resourceKind, info);

                                syncDigestStore.PersistNewer(resourceKind, info.ResSyncInfo);

                            }
                            else if (!correlatedResSyncInfos[0].ResSyncInfo.Etag.Equals(etag))
                            {
                                nexttick = tickProvider.CreateNexttick(resourceKind);
                                correlatedResSyncInfos[0].ResSyncInfo.Etag = etag;
                                correlatedResSyncInfos[0].ResSyncInfo.Tick = nexttick;
                                correlatedResSyncInfos[0].ResSyncInfo.EndPoint = EndPoint;
                                correlatedResSyncInfos[0].ResSyncInfo.ModifiedStamp = DateTime.Now;
                                correlatedResSyncInfoStore.Put(resourceKind, correlatedResSyncInfos[0]);

                                syncDigestStore.PersistNewer(resourceKind, correlatedResSyncInfos[0].ResSyncInfo);
                            }
                        }

                        #endregion

                        #region Fax
                        if (faxResourcePayloadConatainer != null)
                        {
                            // calculate etag of the resource
                            etag = EtagServices.ComputeEtag(faxResourcePayloadConatainer, true);   // new etag

                            // retrieve correlations for the current identity from synch storage
                            correlatedResSyncInfos = correlatedResSyncInfoStore.GetByLocalId(resourceKind, new string[] { faxId });

                            // if no correlation exists AND resource has been deleted:
                            // -> continue with next id
                            // else if no correlation exists:
                            // -> get next tick, create new correlation and add correlation to synch store
                            // otherwise and if the etag stored in synch store is different than etag previously calculated:
                            // -> get next tick, modify existing correlation and update it in synch store.
                            if (correlatedResSyncInfos.Length == 0 && faxResourcePayloadConatainer.IsDeleted)
                            {
                                continue;
                            }
                            else if (correlatedResSyncInfos.Length == 0)
                            {
                                nexttick = tickProvider.CreateNexttick(resourceKind); // create next tick

                                ResSyncInfo resyncInfo = new ResSyncInfo(Guid.NewGuid(), EndPoint, nexttick, etag, DateTime.Now);
                                CorrelatedResSyncInfo info = new CorrelatedResSyncInfo(faxId, resyncInfo);

                                correlatedResSyncInfoStore.Put(resourceKind, info);

                                syncDigestStore.PersistNewer(resourceKind, info.ResSyncInfo);
                            }
                            else if (!correlatedResSyncInfos[0].ResSyncInfo.Etag.Equals(etag))
                            {
                                nexttick = tickProvider.CreateNexttick(resourceKind);
                                correlatedResSyncInfos[0].ResSyncInfo.Etag = etag;
                                correlatedResSyncInfos[0].ResSyncInfo.Tick = nexttick;
                                correlatedResSyncInfos[0].ResSyncInfo.EndPoint = EndPoint;
                                correlatedResSyncInfos[0].ResSyncInfo.ModifiedStamp = DateTime.Now;
                                correlatedResSyncInfoStore.Put(resourceKind, correlatedResSyncInfos[0]);

                                syncDigestStore.PersistNewer(resourceKind, correlatedResSyncInfos[0].ResSyncInfo);
                            }
                        }

                        #endregion

                    }
                    #endregion
                }
                else
                {
                    string id;
                    FeedEntry resourcePayloadContainer;
                    CorrelatedResSyncInfo[] correlatedResSyncInfos;
                    string etag;

                    // iterate through the collection of ids of all changed resources.
                    for (int index = 0; index < changedIdentites.Length; index++)
                    {
                        // current id from iterated collection
                        id = changedIdentites[index].Id;
                        
                        // get resource payload container for the current identity so that we can calculated the
                        // etag.
                        // continue with next identity if no resource was found.
                        resourcePayloadContainer = wrapper.GetFeedEntry(id);

                        // calculate etag of the current resource payload
                        etag = EtagServices.ComputeEtag(resourcePayloadContainer, true);

                        
                        // retrieve correlations for the current identity from synch storage
                        correlatedResSyncInfos = correlatedResSyncInfoStore.GetByLocalId(resourceKind, new string[] { id });


                        // if no correlation exists AND resource has been deleted:
                        // -> continue with next id
                        // else if no correlation exists:
                        // -> get next tick, create new correlation and add correlation to synch store
                        // otherwise and if the etag stored in synch store is different than etag previously calculated:
                        // -> get next tick, modify existing correlation and update it in synch store.
                        if (resourcePayloadContainer == null || (correlatedResSyncInfos.Length == 0 && resourcePayloadContainer.IsDeleted))
                        {
                            continue;
                        }
                        else if (correlatedResSyncInfos.Length == 0)
                        {
                            nexttick = tickProvider.CreateNexttick(resourceKind); // create next tick

                            ResSyncInfo resyncInfo = new ResSyncInfo(Guid.NewGuid(), EndPoint, nexttick, etag, DateTime.Now);
                            CorrelatedResSyncInfo info = new CorrelatedResSyncInfo(id, resyncInfo);

                            correlatedResSyncInfoStore.Put(resourceKind, info);

                            syncDigestStore.PersistNewer(resourceKind, info.ResSyncInfo);
                        }
                        else if (!correlatedResSyncInfos[0].ResSyncInfo.Etag.Equals(etag))
                        {
                            nexttick = tickProvider.CreateNexttick(resourceKind);
                            correlatedResSyncInfos[0].ResSyncInfo.Etag = etag;
                            correlatedResSyncInfos[0].ResSyncInfo.Tick = nexttick;
                            correlatedResSyncInfos[0].ResSyncInfo.EndPoint = EndPoint;
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
                    _parentPerformer._asyncStateObj.Tracking.Phase = TrackingPhase.GETCHANGESBYtick;
                }


                
                if (null != targetDigest)
                {
                    ICorrelatedResSyncInfoEnumerator enumerator;
                    List<string> EndPoints = new List<string>();
#warning remove this workaround
                    if (targetDigest.Entries != null)
                    {
                        foreach (DigestEntry targetDigestEntry in targetDigest.Entries)
                        {
                            EndPoints.Add(targetDigestEntry.EndPoint);
                            enumerator = correlatedResSyncInfoStore.GetSincetick(resourceKind, targetDigestEntry.EndPoint, ((int)targetDigestEntry.Tick) - 1);
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

                    SyncDigestInfo sourceSyncDigestInfo = syncDigestStore.Get(resourceKind);
                    foreach (SyncDigestEntryInfo digestEntry in sourceSyncDigestInfo)
                    {
                        if (EndPoints.Contains(digestEntry.EndPoint))
                            continue;
                        EndPoints.Add(digestEntry.EndPoint);
                        enumerator = correlatedResSyncInfoStore.GetSincetick(resourceKind, digestEntry.EndPoint, -1);
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
                    if (!EndPoints.Contains(EndPoint))
                    {
                        enumerator = correlatedResSyncInfoStore.GetSincetick(resourceKind, EndPoint, -1);
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

                // Set tracking phase to FINISH
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
            private ITracking _tracking;
            // List that holds the resource synchronisation items that have been received while synchronising.
            private List<CorrelatedResSyncInfo> _correlatedResSyncInfos;

            #region Ctor.

            public AsyncState()
            {
                _correlatedResSyncInfos = new List<CorrelatedResSyncInfo>();
            }

            #endregion

            #region Properties

            public ITracking Tracking { get { return _tracking; } set { _tracking = value; } }
            public List<CorrelatedResSyncInfo> CorrelatedResSyncInfos { get { return _correlatedResSyncInfos; } }

            #endregion
        }

        #endregion
    }
}
