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
    internal class PutResourceRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            if (request.ContentType!= Sage.Common.Syndication.MediaType.AtomEntry)
                throw new RequestException("Atom entry content type expected");

            if (String.IsNullOrEmpty(_requestContext.ResourceKey))
                throw new RequestException("Please use single resource url");
            //SupportedResourceKinds resKind = _requestContext.ResourceKind;
            //switch (resKind)
            //{
            //    case SupportedResourceKinds.tradingAccounts:
            //    case SupportedResourceKinds.contacts:
            //    case SupportedResourceKinds.phoneNumbers:
            //    case SupportedResourceKinds.postalAddresses:
            //        break;
            //    default:
            //        throw new RequestException("Put is not Supported for requested resource");
            //}

            // read entry from stream
            SyncFeedEntry entry = new SyncFeedEntry();
            XmlReader reader = XmlReader.Create(request.Stream);
            reader.MoveToContent();

            entry.ReadXml(reader, ResourceKindHelpers.GetPayloadType(_requestContext.ResourceKind));

            IEntityWrapper wrapper = EntityWrapperFactory.Create(_requestContext.ResourceKind, _requestContext);
            
            Token emptyToken = new Token();
            Identity identity = wrapper.GetIdentity(_requestContext.ResourceKey);

            Document originalDocument = wrapper.Entity.GetDocument(identity, emptyToken, _requestContext.Config);

            if (originalDocument.LogState == LogState.Deleted)
                throw new RequestException("Entity does not exists");

            entry.Payload.LocalID = _requestContext.ResourceKey;
            SdataTransactionResult sdTrResult = wrapper.Update(entry.Payload, entry.SyncLinks);

            if (sdTrResult == null)
            {

                SyncFeedEntry responseEntry = wrapper.GetFeedEntry(_requestContext.ResourceKey);

                SyncFeed feed = new SyncFeed();
                feed.FeedType = FeedType.ResourceEntry;
                feed.Entries.Add(responseEntry);
                request.Response.Serializer = new SyncFeedSerializer();
                request.Response.Feed = feed;
                request.Response.ContentType = Sage.Common.Syndication.MediaType.AtomEntry;
              
            }
            else if (sdTrResult.HttpStatus == System.Net.HttpStatusCode.OK)
            {
                SyncFeedEntry responseEntry = wrapper.GetFeedEntry(_requestContext.ResourceKey);
                
                SyncFeed feed = new SyncFeed();
                feed.FeedType = FeedType.ResourceEntry;
                feed.Entries.Add(responseEntry);
                request.Response.Serializer = new SyncFeedSerializer();
                request.Response.Feed = feed;
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
