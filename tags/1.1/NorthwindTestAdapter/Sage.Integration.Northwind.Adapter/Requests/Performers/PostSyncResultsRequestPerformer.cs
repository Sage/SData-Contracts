#region Usings

using System;
using System.Xml;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Common;
using Sage.Integration.Northwind.Feeds;
using Sage.Sis.Sdata.Sync.Results;
using Sage.Sis.Sdata.Sync.Results.Syndication;
using System.Net;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class PostSyncResultsRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            if (request.ContentType != Sage.Common.Syndication.MediaType.Atom)
                throw new RequestException("Atom content type expected");

            //read feed
            string resourceKindName = _requestContext.ResourceKind.ToString();
            SyncFeed feed = new SyncFeed();
            XmlReader reader = XmlReader.Create(request.Stream);
            feed.ReadXml(reader, ResourceKindHelpers.GetPayloadType(_requestContext.ResourceKind));

            /* iterate through all entries and store result information */
            ISyncResultInfoStore syncResultInfoStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetSyncResultStore(_requestContext.SdataContext);
            
            int noOfEntries = feed.Entries.Count;
            string endpoint = string.Empty;
            try
            {
                endpoint = new RequestContext(new Sage.Common.Syndication.SDataUri(feed.Id)).OriginEndPoint;
            }
            catch { }

            SyncResultEntryInfo[] syncResultEntries = new SyncResultEntryInfo[noOfEntries];
            for (int i = 0; i < noOfEntries; i++)
            {
                SyncFeedEntry entry = (SyncFeedEntry)feed.Entries[i];
                string httpMethod = entry.HttpMethod;
                int httpStatus = -1;
                if (Enum.IsDefined(typeof(HttpStatusCode), entry.HttpStatusCode.ToString()))
                    httpStatus = (int)Enum.Parse(typeof(HttpStatusCode), entry.HttpStatusCode.ToString(), true);
                string httpLocation = entry.HttpLocation;
                string httpMessage = entry.HttpMessage;
                string diagnosisXml = XmlSerializationHelpers.SerializeObjectToXml(entry.Diagnoses);
                string payloadXml = XmlSerializationHelpers.SerializeObjectToXml(entry.Payload);
                
                syncResultEntries[i] = new SyncResultEntryInfo(httpMethod, httpStatus, httpLocation, httpMessage, diagnosisXml, payloadXml, DateTime.Now, endpoint);
            }

            syncResultInfoStore.Add(resourceKindName, syncResultEntries);
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion
    }
}
