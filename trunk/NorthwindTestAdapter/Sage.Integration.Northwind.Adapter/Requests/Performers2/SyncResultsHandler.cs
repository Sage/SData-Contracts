using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Messaging.Model;
using Sage.Sis.Sdata.Sync.Results.Syndication;
using System.Net;
using Sage.Common.Syndication;
using Sage.Sis.Sdata.Sync.Results;
using System.IO;
using System.Xml;

namespace Sage.Integration.Northwind.Adapter.Common.Handler
{
    class SyncResultsHandler
    {
        private RequestContext _requestContext;
        private IRequest _request;

        public SyncResultsHandler(IRequest request)
        {
            _requestContext = new RequestContext(request.Uri);
            _request = request;
        }

        public void LogResults(Feed<FeedEntry> targetFeed)
        {
                // input stream MUST be of type ATOM
            if (_request.ContentType != Sage.Common.Syndication.MediaType.Atom)
                throw new RequestException("Atom content type expected");

            string resourceKindName;
            ISyncResultInfoStore syncResultInfoStore;
            string EndPoint;
            int noOfEntries;    // the number of entries retrived ffrom input feed and that will be stored
            string runName;
            string runStamp;
            
            // Retrieve the resourceKindName
            resourceKindName = _requestContext.ResourceKind.ToString();

            // Retrieve a reference to the synchronisation result store
            syncResultInfoStore = NorthwindAdapter.StoreLocator.GetSyncResultStore(_requestContext.SdataContext);

            // Read query parameters
                if (!_requestContext.SdataUri.QueryArgs.TryGetValue("runName", out runName))
                    runName = null;
                if (!_requestContext.SdataUri.QueryArgs.TryGetValue("runStamp", out runStamp))
                    runName = null;


            syncResultInfoStore.SetRunName(runName);
            syncResultInfoStore.SetRunStamp(runStamp);

            // Retrieve the EndPoint that will be stored into the result store
            // The EndPoint is the base url of the requst that returned the sync target feed.
            // Here: the EndPoint is parsed from ID tag of the feed
            EndPoint = string.Empty;
            try
            {
                EndPoint = new RequestContext(new Sage.Common.Syndication.SDataUri(targetFeed.Id)).OriginEndPoint;
            }
            catch { /* MS: Why do we not expect an EndPoint here ??? */ }

            // Create a list of result entries and add them to the result storage
            noOfEntries = targetFeed.Entries.Count;
            SyncResultEntryInfo[] syncResultEntries = new SyncResultEntryInfo[noOfEntries];
            FeedSerializer serializer = (FeedSerializer)_request.Serializer;

            FeedEntry entry;
            string httpMethod = null;
            int httpStatus = 0;
            string httpLocation = null;
            string httpMessage = null;
            string diagnosisXml = null;
            string payloadXml = null;
            DateTime stamp = DateTime.Now;
            for (int i = 0; i < noOfEntries; i++)
            {
                entry = targetFeed.Entries[i];
                if (entry != null)
                {
                    httpMethod = entry.HttpMethod;
                    httpStatus = -1;
                    if (Enum.IsDefined(typeof(HttpStatusCode), entry.HttpStatusCode.ToString()))
                        httpStatus = (int)Enum.Parse(typeof(HttpStatusCode), entry.HttpStatusCode.ToString(), true);
                    httpLocation = entry.HttpLocation;
                    httpMessage = entry.HttpMessage;
                    payloadXml = this.SerializeFeedEntryToXml(entry, serializer);
                    stamp = entry.Updated.ToLocalTime();
                }
                if (entry.Diagnoses != null && entry.Diagnoses.Count > 0)
                {
                    diagnosisXml = this.SerializeObjectToXml(entry.Diagnoses[0]);
                }
                syncResultEntries[i] = new SyncResultEntryInfo(httpMethod, httpStatus, httpLocation, httpMessage, diagnosisXml, payloadXml, stamp, EndPoint);
            }

            // add entries to result storage
            syncResultInfoStore.Add(resourceKindName, syncResultEntries);
            _request.Response.StatusCode = HttpStatusCode.OK;
        }

        private string SerializeFeedEntryToXml(FeedEntry obj, FeedSerializer serializer)
        {
            if (null == obj)
                return null;

            try
            {
                String xml = null;
                MemoryStream memoryStream = new MemoryStream();

                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                serializer.SaveToStream(obj, memoryStream, null);

                xml = UTF8ByteArrayToString(memoryStream.ToArray());
                return xml;

            }
            catch
            {
                //Debug.WriteLine(e);
                return null;
            }
        }

        private string SerializeObjectToXml(object obj)
        {
            if (null == obj)
                return null;

            try
            {
                String xml = null;
                MemoryStream memoryStream = new MemoryStream();

                System.Xml.Serialization.XmlSerializer xs;
                xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());

                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, obj);

                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;

                xml = UTF8ByteArrayToString(memoryStream.ToArray());
                return xml;

            }
            catch
            {
                return null;
            }
        }


        private string UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return constructedString;
        }

    }
}
