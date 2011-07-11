#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Feeds;
using System.IO;
using System.Xml;
using Sage.Integration.Northwind.Application.Entities.Account;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Feeds.TradingAccounts;
using Sage.Integration.Northwind.Application.Base;
#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class PostResourceRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            if (request.ContentType != Sage.Common.Syndication.MediaType.AtomEntry)
                throw new RequestException("Atom entry content type expected");

            if (!String.IsNullOrEmpty(_requestContext.ResourceKey))
                throw new RequestException("Please use resource url");

            // read entry from stream
            SyncFeedEntry entry = new SyncFeedEntry();
            XmlReader reader = XmlReader.Create(request.Stream);
            reader.MoveToContent();

            entry.ReadXml(reader, ResourceKindHelpers.GetPayloadType(_requestContext.ResourceKind));

            IEntityWrapper wrapper = EntityWrapperFactory.Create(_requestContext.ResourceKind, _requestContext);
           

            Document document = wrapper.GetTransformedDocument(entry.Payload, entry.SyncLinks);
            if (!String.IsNullOrEmpty(document.Id))
                throw new RequestException("Entity alredy exists");

            // Add Document

            SdataTransactionResult sdTrResult = wrapper.Add(entry.Payload, entry.SyncLinks );

            if (sdTrResult == null)
                throw new RequestException("Entity does not exists"); //?????


            if ((sdTrResult.HttpStatus == System.Net.HttpStatusCode.OK) ||
                (sdTrResult.HttpStatus == System.Net.HttpStatusCode.Created))
            {

                // store correlation
                SyncFeedEntry responseEntry = wrapper.GetFeedEntry(sdTrResult.LocalId);
                
                SyncFeed feed = new SyncFeed();
                feed.FeedType = FeedType.ResourceEntry;
                feed.Entries.Add(responseEntry);
                request.Response.StatusCode = System.Net.HttpStatusCode.Created;
                request.Response.Serializer = new SyncFeedSerializer();
                request.Response.Feed = feed;
                request.Response.Protocol.SendUnknownResponseHeader("Location", responseEntry.Id);
                request.Response.ContentType = Sage.Common.Syndication.MediaType.AtomEntry;
            }
            else
            {

                throw new RequestException(sdTrResult.HttpMessage);
            }
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion
    }
}
