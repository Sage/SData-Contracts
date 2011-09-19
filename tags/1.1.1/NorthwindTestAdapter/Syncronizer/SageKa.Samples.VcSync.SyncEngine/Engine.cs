#region Usings

using System;
using System.Net;
using System.ServiceModel.Syndication;
using Microsoft.Http;
using SageKa.Samples.VcSync.SyncEngine.Loggers;
using SageKa.Samples.VcSync.SyncEngine.Syndications;
using SageKa.Samples.VCSync.SyncEngine.Helpers;
using SageKa.Samples.VcSync.SyncEngine.Extensions;
using System.Xml;
using SageKa.Samples.VcSync.SyncEngine.Model;

#endregion

namespace SageKa.Samples.VcSync.SyncEngine
{
    public class Engine
    {
        #region Class Variables

        private ILogger _logger;
        private const string ATOMFEED = "application/atom+xml";
        private const string ATOMFEEDENTRY = "application/atom+xml;type=entry";
        //private const string ATOMFEEDWITHTYPE = "application/atom+xml; type=feed";
        private bool disableValidation = false;

        #endregion

        #region Ctor.

        private Engine()
        {
            _logger = new EmptyLogger();
        }

        public Engine(string runName, DateTime runStamp, SynchronisationDirection direction, SynchronisationInfo syncInfo)
            : this()
        {
            this.SyncInfo = syncInfo;
            this.Direction = direction;
            this.RunName = runName;
            this.RunStamp = runStamp;
        }

        #endregion

        #region Properties

        public ILogger Logger
        {
            get { return _logger; }
            set
            {
                if (null == value)
                    _logger = new EmptyLogger();
                else
                    _logger = value;
            }
        }

        public SynchronisationInfo SyncInfo { get; private set; }
        public SynchronisationDirection Direction { get; private set; }
        public string RunName { get; private set; }
        public DateTime RunStamp { get; private set; }

        #endregion

        public void Run()
        {
            if (!CheckSyncDigestAvailability())
                return;

            foreach (string resource in SyncInfo.Resources)
            {
                if (!RunPerResource(resource))
                    break;
            }
        }

        private bool CheckSyncDigestAvailability()
        {
            try
            {
                foreach (string resource in SyncInfo.Resources)
                {
                    HttpClient httpSourceClient = null;

                    HttpClient httpTargetClient = null;
                    HttpClient httpLoggingClient = null;
                    httpSourceClient = GetHttpClient(SyncInfo.Source, SyncInfo.Proxy, resource);
                    httpTargetClient = GetHttpClient(SyncInfo.Target, SyncInfo.Proxy, resource);
                    if (SyncInfo.Logging != null && SyncInfo.Logging.Url != null)
                        httpLoggingClient = GetHttpClient(SyncInfo.Logging, SyncInfo.Proxy, resource);



                    if (!CheckSyncDigest(httpSourceClient, resource, SyncInfo.Source))
                        return false;
                    if (!CheckSyncDigest(httpTargetClient, resource, SyncInfo.Target))
                        return false;

                    if ((httpLoggingClient != null) && (!CheckSyncDigest(httpLoggingClient, resource, SyncInfo.Logging)))
                        return false;
                }

                return true;
            }
            catch (Exception e)
            {
                this.Logger.Write(string.Format("Following error occured: {0}", e.Message), Severity.Error);
                return false;
            }

        }

        private bool CheckSyncDigest(HttpClient httpClient, string resource, EndPointInfo endpoint)
        {
            string tmpUrl = endpoint.GetResourceUri(resource).ToString();
            tmpUrl = string.Format("{0}/{1}?runName={2}&runStamp={3}", tmpUrl, "$syncDigest", "testDigest", this.RunStamp.ToUniversalTime());
            HttpResponseMessage respMsg = httpClient.Get(tmpUrl);
            
            respMsg.Content.LoadIntoBuffer();
            
            if (respMsg.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            
            this.Logger.Write(string.Format("The Resource {0} is not accessable", resource), Severity.Error);
            this.Logger.Write(string.Format("on following root url {0}", endpoint.UrlString), Severity.Error);
            return false;
        }

        private bool RunPerResource(string resource)
        {
            bool logTargetResult = false;
            this.Logger.Write(string.Format("Starting synchronization for Resource {0} ", resource), Severity.Info);
            this.Logger.Write(string.Format("Source: {0}", this.SyncInfo.Source.GetResourceUri(resource)), Severity.Trace);
            this.Logger.Write(string.Format("Target: {0}", this.SyncInfo.Target.GetResourceUri(resource)), Severity.Trace);
            
            if ((this.SyncInfo.Logging != null) && (this.SyncInfo.Logging.Url != null))
            {
                this.Logger.Write(string.Format("Logging: {0}", this.SyncInfo.Logging.GetResourceUri(resource)), Severity.Trace);
                logTargetResult = true;
            }
            
            this.Logger.Write(string.Format("Direction: {0}", this.Direction.ToString()), Severity.Trace);
            

            HttpClient httpSourceClient = null;
            Guid sourceClientTrackingId = Guid.Empty;

            HttpClient httpTargetClient = null;
            Guid targetClientTrackingId = Guid.Empty;

            HttpClient httpLoggingClient = null;


            string tmpUrl;
            HttpResponseMessage respMsg;
            SyndicationFeed tmpSourceFeed;
            SyndicationFeed tmpTargetFeed;
            SyndicationLink tmpSourceNextLink = null;
            SyndicationLink tmpTargetNextLink = null;


            // Create application client objects
            if (Direction == SynchronisationDirection.forward)
            {
                httpSourceClient = GetHttpClient(SyncInfo.Source, SyncInfo.Proxy, resource);
                httpTargetClient = GetHttpClient(SyncInfo.Target, SyncInfo.Proxy, resource);
            }else
            {
                httpSourceClient = GetHttpClient(SyncInfo.Target, SyncInfo.Proxy, resource);
                httpTargetClient = GetHttpClient(SyncInfo.Source, SyncInfo.Proxy, resource);
            }

            if (logTargetResult)
            {
                httpLoggingClient = GetHttpClient(SyncInfo.Logging, SyncInfo.Proxy, resource);
                httpLoggingClient.TransportSettings.ConnectionTimeout = new TimeSpan(0, 5, 0);
            }
            

            // set timeout
            httpSourceClient.TransportSettings.ConnectionTimeout = new TimeSpan(0, 5, 0);
            httpTargetClient.TransportSettings.ConnectionTimeout = new TimeSpan(0, 5, 0);
            


            try
            {
                // *********************************************************
                // GET the sync digest from target applictaion ($syncDigest)
                // *********************************************************
                this.Logger.Write("Requesting the Sync Digest from target application.", Severity.Info);
                tmpUrl = GetSyncDigestGetUrl(resource);
                
                this.Logger.Write(string.Format("HTTP GET: {0}", tmpUrl), Severity.Trace);
                respMsg = httpTargetClient.Get(tmpUrl);
                this.Log(respMsg);

                // receive the content of the response
                respMsg.Content.LoadIntoBuffer();
                // handle errors
                this.ThrowIfNotStatusCode(HttpStatusCode.OK, respMsg);

                // check for correct content type of the result
                ThrowIfNoFeedEntry(respMsg);
                

                this.Logger.Write("Sync Digest received.", Severity.Info);

                // As we do not need to read the SyncDigest, the response content will be posted directly to the source application.
                // At the same time no reading means no validation. So eventually wrong data could be sent to the source application
                // with the next request. The source application must handle eventually wrong input data.


                // *************************************************************************
                // POST the retrieved Sync Digest to the source application ($syncSource)
                // *************************************************************************
                this.Logger.Write("Posting the Sync Digest from target application to the source application.", Severity.Info);
                sourceClientTrackingId = Guid.NewGuid();        // create the tracking ID for the asynchronous request
                tmpUrl = GetSyncSourcePostUrl(sourceClientTrackingId, resource);

                this.Logger.Write(string.Format("HTTP POST: {0}", tmpUrl), Severity.Trace);
                this.Logger.Write(string.Format("Content Type: {0}", ATOMFEEDENTRY), Severity.Trace);
                respMsg = httpSourceClient.Post(tmpUrl, ATOMFEEDENTRY, respMsg.Content);
                this.Log(respMsg);

                // receive the content of the response
                respMsg.Content.LoadIntoBuffer();
                // handle errors
                this.ThrowIfNotStatusCode(HttpStatusCode.Accepted, respMsg);

                // check for correct content type of the result
                ThrowIfNoXml(respMsg);
                
                // *************************************************************************
                // GET the Sync Feed from source application ($syncSource)
                // *************************************************************************
                // The helper method used here looks at the header of the response for the Location parameter
                // and requests the location url until a Status Code 200(OK) and an sdata feed is returned.
                // If an error occurred (No 200 or 202 status) an exception is thown.
                this.Logger.Write("Polling source application until retrieving sync feed.", Severity.Info);
                respMsg = this.RequestHeaderLocationUntilStatusIs200(respMsg, httpSourceClient);
                this.Log(respMsg);

                // check for correct content type of the result
                ThrowIfNoFeed(respMsg);

                // receive the content of the response
                respMsg.Content.LoadIntoBuffer();

                int sourceFeedPage = 1;
                do
                {
                    // parse source result as a feed
                    // needed to read a 'next' link, so that we can retrieve and submit the next page.
                    tmpSourceFeed = respMsg.Content.ReadAsSyndicationFeed();
               
                    Atom10FeedFormatter ff = tmpSourceFeed.GetAtom10Formatter();

                    // Handle error feed
                    this.ThrowIfDiagnosisFeed(tmpSourceFeed);

                    // *************************************************************************
                    // POST the retrieved Sync Feed to the target application ($syncTarget)
                    // *************************************************************************
                    this.Logger.Write(string.Format("Posting page {0} of the Sync Feed from source application to the target application.", sourceFeedPage), Severity.Info);
                    targetClientTrackingId = Guid.NewGuid();        // create the tracking ID for the asynchronous request
                    tmpUrl = GetSyncTargetPostUrl(targetClientTrackingId, resource);


                    this.Logger.Write(string.Format("HTTP POST: {0}", tmpUrl), Severity.Trace);
                    this.Logger.Write(string.Format("Content Type: {0}", ATOMFEED), Severity.Trace);
                    //XmlWriter writer;
                    //tmpSourceFeed.SaveAsAtom10(writer);


                    //HttpContent httpContent = HttpContent.Create(
                    respMsg = httpTargetClient.Post(tmpUrl, ATOMFEED, respMsg.Content);
                    this.Log(respMsg);

                    // receive the content of the response
                    respMsg.Content.LoadIntoBuffer();
                    // handle errors
                    this.ThrowIfNotStatusCode(HttpStatusCode.Accepted, respMsg);

                    // check for correct content type of the result
                    ThrowIfNoXml(respMsg);
                    

                    // *************************************************************************
                    // GET the Sync Feed from target application ($syncTarget)
                    // *************************************************************************
                    // The helper method used here looks at the header of the response for the Location parameter
                    // and requests the location url until a Status Code 200(OK) and an sdata feed is returned.
                    // If an error occurred (No 200 or 202 status) an exception is thown.
                    this.Logger.Write("Polling target application until retrieving sync feed.", Severity.Info);
                    respMsg = this.RequestHeaderLocationUntilStatusIs200(respMsg, httpTargetClient);
                    this.Log(respMsg);

                    int targetFeedPage = 1;
                    do
                    {
                        // parse target result as a feed
                        // needed to read a 'next' link, so that we can retrieve and submit the next page.
                        tmpTargetFeed = respMsg.Content.ReadAsSyndicationFeed();

                        // Handle error feed
                        this.ThrowIfDiagnosisFeed(tmpTargetFeed);

                        // *************************************************************************
                        // POST the retrieved Sync Feed to the logging application ($syncResults)
                        // *************************************************************************
                        if (!logTargetResult)
                            this.Logger.Write("No logging url specified", Severity.Trace);
                        else
                        {
                            this.Logger.Write(string.Format("Posting page {0} of the Sync Feed from target application to the logging application.", targetFeedPage), Severity.Info);
                            tmpUrl = string.Format("{0}/{1}?runName={2}&runStamp={3}", SyncInfo.Logging.GetResourceUri(resource), "$syncResults", this.RunName, this.RunStamp.ToUniversalTime());

                            this.Logger.Write(string.Format("HTTP POST: {0}", tmpUrl), Severity.Trace);
                            this.Logger.Write(string.Format("Content Type: {0}", ATOMFEED), Severity.Trace);
                            respMsg = httpLoggingClient.Post(tmpUrl, ATOMFEED, respMsg.Content);
                            this.Log(respMsg);
                        }

                        // receive the content of the response
                        respMsg.Content.LoadIntoBuffer();
                        // handle errors
                        this.ThrowIfNotStatusCode(HttpStatusCode.OK, respMsg);

                        // *************************************************************************
                        // GET the next page of the Sync Feed of the target application($syncTarget)
                        // *************************************************************************
                        tmpTargetNextLink = tmpTargetFeed.GetNextPageLink();

                        if (null != tmpTargetNextLink)
                        {
                            // Todo: LOG
                            respMsg = httpTargetClient.Get(tmpTargetNextLink.Uri.ToString());
                            this.Log(respMsg);

                            // validate
                            respMsg.Content.LoadIntoBuffer();

                            targetFeedPage++;
                        }



                    } while (null != tmpTargetNextLink);


                    // *************************************************************************
                    // DELETE the batching context of the source application($syncSource)
                    // *************************************************************************
                    this.Logger.Write("Deleting batching context from target application.", Severity.Info);
                    tmpUrl = GetSyncTargetDeleteUrl(targetClientTrackingId, resource);

                    this.Logger.Write(string.Format("HTTP DELETE: {0}", tmpUrl), Severity.Trace);
                    respMsg = httpTargetClient.Delete(tmpUrl);
                    this.Log(respMsg);

                    // receive the content of the response
                    respMsg.Content.LoadIntoBuffer();
                    // handle errors
                    //this.ThrowIfNotStatusCode(HttpStatusCode.OK, respMsg);


                    // *************************************************************************
                    // GET the next page of the Sync Feed of the source application($syncSource)
                    // *************************************************************************
                    tmpSourceNextLink = tmpSourceFeed.GetNextPageLink();

                    if (null != tmpSourceNextLink)
                    {
                        // Todo: LOG
                        respMsg = httpSourceClient.Get(tmpSourceNextLink.Uri);
                        this.Log(respMsg);

                        // validate
                        respMsg.Content.LoadIntoBuffer();

                        sourceFeedPage++;
                    }

                } while (null != tmpSourceNextLink);


                // *************************************************************************
                // DELETE the batching context of the source application($syncSource)
                // *************************************************************************
                this.Logger.Write("Deleting batching context from source application.", Severity.Info);
                tmpUrl = GetSyncSourceDeleteUrl(sourceClientTrackingId, resource);

                this.Logger.Write(string.Format("HTTP DELETE: {0}", tmpUrl), Severity.Trace);
                respMsg = httpSourceClient.Delete(tmpUrl);
                this.Log(respMsg);

                // receive the content of the response
                respMsg.Content.LoadIntoBuffer();
                // handle errors
                //this.ThrowIfNotStatusCode(HttpStatusCode.OK, respMsg);

                this.Logger.Write("Synchronization succeeded.", Severity.Info);
            }
            catch (SdataException sdataException)
            {
                this.Logger.Write(sdataException.Diagnosis);
                return false;
            }
            catch (Exception exception)
            {
                this.Logger.Write(exception);
                return false;
            }
            return true;
        }

        private string GetSyncTargetPostUrl(Guid targetClientTrackingId, string resource)
        {
            string tmpUrl;
            if (Direction == SynchronisationDirection.forward)
                tmpUrl = SyncInfo.Target.GetResourceUri(resource).ToString();
            else
                tmpUrl = SyncInfo.Source.GetResourceUri(resource).ToString();

            return string.Format("{0}/{1}?trackingID={2}&runName={3}&runStamp={4}", tmpUrl, "$syncTarget", targetClientTrackingId, this.RunName, this.RunStamp.ToUniversalTime());
        }

        private string GetSyncTargetDeleteUrl(Guid targetClientTrackingId, string resource)
        {
            string tmpUrl;
            if (Direction == SynchronisationDirection.forward)
                tmpUrl = SyncInfo.Target.GetResourceUri(resource).ToString();
            else
                tmpUrl = SyncInfo.Source.GetResourceUri(resource).ToString();

            return string.Format("{0}/{1}('{2}')", tmpUrl, "$syncTarget", targetClientTrackingId);
        }

        private string GetSyncSourcePostUrl(Guid sourceClientTrackingId, string resource)
        {
            string tmpUrl;
            if (Direction == SynchronisationDirection.forward)
                tmpUrl = SyncInfo.Source.GetResourceUri(resource).ToString();
            else
                tmpUrl = SyncInfo.Target.GetResourceUri(resource).ToString();
            return string.Format("{0}/{1}?trackingID={2}&runName={3}&runStamp={4}", tmpUrl, "$syncSource", sourceClientTrackingId, this.RunName, this.RunStamp.ToUniversalTime());
        }

        private string GetSyncSourceDeleteUrl(Guid sourceClientTrackingId, string resource)
        {
            string tmpUrl;
            if (Direction == SynchronisationDirection.forward)
                tmpUrl = SyncInfo.Source.GetResourceUri(resource).ToString();
            else
                tmpUrl = SyncInfo.Target.GetResourceUri(resource).ToString();
            return string.Format("{0}/{1}('{2}')", tmpUrl, "$syncSource", sourceClientTrackingId);
        }

    

        private string GetSyncDigestGetUrl(string resource)
        {
            string tmpUrl;
            if (Direction == SynchronisationDirection.forward)
                tmpUrl = SyncInfo.Target.GetResourceUri(resource).ToString();
            else
                tmpUrl = SyncInfo.Source.GetResourceUri(resource).ToString();
            return string.Format("{0}/{1}?runName={2}&runStamp={3}", tmpUrl, "$syncDigest", this.RunName, this.RunStamp.ToUniversalTime());
        }

        private HttpClient GetHttpClient(EndPointInfo endpoint, ProxyInfo proxy, string resource)
        {
            HttpClient result = new HttpClient();
            result.BaseAddress = endpoint.GetResourceUri(resource);
            if ((endpoint.Credentials != null) && (!String.IsNullOrEmpty(endpoint.Credentials.User)))
            {
                result.TransportSettings.Credentials = new NetworkCredential(endpoint.Credentials.User, endpoint.Credentials.Password);
            }
            if ((proxy != null) && (!String.IsNullOrEmpty(proxy.Host)))
            {
                WebProxy webProxy = new WebProxy(proxy.Host, proxy.Port);
                if ((proxy.Credentials != null) && (!String.IsNullOrEmpty(proxy.Credentials.User)))
                {
                    webProxy.Credentials = new NetworkCredential(proxy.Credentials.User, proxy.Credentials.Password);
                }
                result.TransportSettings.Proxy = webProxy;
            }
            result.TransportSettings.ConnectionTimeout = new TimeSpan(0, 5, 0);
            return result;
        }

        private HttpResponseMessage RequestHeaderLocationUntilStatusIs200(HttpResponseMessage respMsg, HttpClient httpClient)
        {
           // if (this.disableValidation)
             //   return respMsg;

            int pollingMillis;
            string tmpUrl;
            HttpResponseMessage tmpRespMsg = respMsg;
            Tracking tmpTracking;
           
            while (tmpRespMsg.StatusCode != HttpStatusCode.OK)
            {
                // handle error
                this.ThrowIfNotStatusCode(HttpStatusCode.Accepted, tmpRespMsg);

                // read content as tracking
                tmpTracking = respMsg.Content.SdataReadAsTracking();

                // get the time to wait
                pollingMillis = (tmpTracking.PollingMillis > 10000 || tmpTracking.PollingMillis < 100) ? 500 : tmpTracking.PollingMillis;

                // check for location in header
                Validator.ThrowIf(null == respMsg.Headers.Location, () => new ApplicationException("Location header missing."));

                tmpUrl = respMsg.Headers.Location.ToString();

                // wait a little time period 
                System.Threading.Thread.Sleep(pollingMillis);

                // GET request
                tmpRespMsg = httpClient.Get(tmpUrl);

                tmpRespMsg.Content.LoadIntoBuffer();
            }

            return tmpRespMsg;
        }

        #region Logging Helpers

        private void Log(HttpResponseMessage respMsg)
        {
            // TODO:
            // detailed logging of response
        }

        #endregion

        private void ThrowIfNoFeed(HttpResponseMessage respMsg)
        {
            if (this.disableValidation)
                return;
            Validator.ThrowIf(!IsFeed(respMsg.Content.ContentType),
                () => new ApplicationException("Invalid content type (no feed) of result."));
        }

        private void ThrowIfNoFeedEntry(HttpResponseMessage respMsg)
        {
            if (this.disableValidation)
                return;
            Validator.ThrowIf(!IsFeedEntry(respMsg.Content.ContentType),
                        () => new ApplicationException("Invalid content type (no feedentry) of result."));
        }

        private void ThrowIfNoXml(HttpResponseMessage respMsg)
        {
            if (this.disableValidation)
                return;

            Validator.ThrowIf(!respMsg.Content.ContentType.StartsWith("application/xml"),
                           () => new ApplicationException("Invalid content type (no xml) of result."));
        }

        private bool IsFeedEntry(string contenttype)
        {
            if (this.disableValidation)
                return true;
            return ((contenttype.StartsWith("application/atom+xml; type=entry", StringComparison.InvariantCultureIgnoreCase)) ||
                       (contenttype.StartsWith("application/atom+xml;type=entry", StringComparison.InvariantCultureIgnoreCase)));
        }

        private bool IsFeed(string contenttype)
        {
            if (this.disableValidation)
                return true;
            return ((contenttype.StartsWith("application/atom+xml; type=feed", StringComparison.InvariantCultureIgnoreCase)) ||
                      (contenttype.StartsWith("application/atom+xml;type=feed", StringComparison.InvariantCultureIgnoreCase)) ||
                      (contenttype.Equals("application/atom+xml", StringComparison.InvariantCultureIgnoreCase)));
        }

        private void ThrowIfDiagnosisFeed(SyndicationFeed feed)
        {
            if (this.disableValidation)
                return;
            Diagnosis diagnosis;

            try
            {
                diagnosis = feed.ReadSdataDiagnosis();
            }
            catch
            {
                throw new ApplicationException("Failed to check for diagnosis in sdata feed.");
            }

            if (null != diagnosis)
                throw new SdataException(diagnosis);
        }
        private void ThrowIfNotStatusCode(HttpStatusCode expectedHttpStatusCode, HttpResponseMessage respMsg)
        {
            if (this.disableValidation)
                return;
            if (respMsg.StatusCode != expectedHttpStatusCode)
            {
                if (IsFeed(respMsg.Content.ContentType))
                {
                    this.ThrowIfDiagnosisFeed(respMsg.Content.ReadAsSyndicationFeed());
                }

                throw new ApplicationException("Unexpected StatusCode: " + respMsg.StatusCode.ToString());
            }
        }
    }
}
