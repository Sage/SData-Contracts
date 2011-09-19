using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common.Handler;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Common.Performers;
using Sage.Integration.Northwind.Adapter.Data.FeedEntries;
using Sage.Integration.Messaging.Model;

namespace Sage.Integration.Northwind.Adapter.RequestReceiver
{
    [RequestPath("tradingAccounts", "tradingAccounts(<resource>)")]
    public class TradingAccountsRequestReceiver
    {
        
        public static NorthwindAdapter NorthwindAdapter;

        #region Ctor.

        /// <summary>
        ///
        /// </summary>
        /// <param name="adapter"></param>
        public TradingAccountsRequestReceiver(NorthwindAdapter adapter)
        {
            NorthwindAdapter = adapter;
        }

        #endregion

        /// <summary>
        /// Method does not have a return parameter because you have to differenciate between single result and Feed. Framework does NOT do this!
        /// </summary>
        /// <param name="request"></param>
        /// <param name="resource"></param>
        [GetRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void GetTradingAccounts(IRequest request, string resource)
        {
            CRUD crud = new CRUD(request);
            crud.Read(resource);
        }

        [GetRequestTarget("$template")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void GetTemplate(IRequest request)
        {
            request.Response.ContentType = MediaType.AtomEntry;
            request.Response.FeedEntry = TemplateFactory.GetTemplate<TradingAccountFeedEntry>(request);
        }


        [PostRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PostTradingAccounts(IRequest request, TradingAccountFeedEntry entry)
        {
            CRUD crud = new CRUD(request);
            crud.Create(entry);
        }

        [PutRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PutTradingAccounts(IRequest request, TradingAccountFeedEntry entry, string resource)
        {
            CRUD crud = new CRUD(request);
            crud.Update(entry, resource);
        }

        [DeleteRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void DeleteTradingAccounts(IRequest request, string resource)
        {
            CRUD crud = new CRUD(request);
            crud.Delete(resource);
        }
        [GetRequestTarget("$syncDigest")]
        public void GetDigest(IRequest request)
        {
            RequestContext context = new RequestContext(request.Uri);
            GetSyncDigestRequestPerformer performer = new GetSyncDigestRequestPerformer();
            performer.Initialize(context);
            performer.DoWork(request);
        }

        [PostRequestTarget("$syncSource")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PostSyncSource(IRequest request, DigestFeedEntry entry)
        {
            //The FeedEntry Parameter on this method is intentionally omitted 1. because the framework fails in parsing the Digest and 2. the request.Stream is already read which will cause an error in subsequent calls
            PostSyncSourceRequestPerformer performer = NorthwindAdapter.RequestPerformerLocator.Resolve<PostSyncSourceRequestPerformer>(new RequestContext(request.Uri));
            performer.DoWork(request, entry);
        }

        [GetRequestTarget("$syncSource")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void GetSyncSource(IRequest request)
        {
            RequestContext context = new RequestContext(request.Uri);;
            GetSyncSourceRequestPerformer performer = NorthwindAdapter.RequestPerformerLocator.Resolve<GetSyncSourceRequestPerformer>(context);
            performer.DoWork(request);
        }

        [PostRequestTarget("$syncTarget")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PostSyncTarget(IRequest request, Feed<TradingAccountFeedEntry> feed)
        {
            RequestContext context = new RequestContext(request.Uri);
            PostSyncTargetRequestPerformer performer = NorthwindAdapter.RequestPerformerLocator.Resolve<PostSyncTargetRequestPerformer>(context);
            performer.DoWork(request, feed, feed.Digest);
        }

        [GetRequestTarget("$syncTarget")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void GetSyncTarget(IRequest request)
        {
            RequestContext context = new RequestContext(request.Uri);
            GetSyncTargetRequestPerformer performer = NorthwindAdapter.RequestPerformerLocator.Resolve<GetSyncTargetRequestPerformer>(context);
            performer.DoWork(request);
        }

    }
}
