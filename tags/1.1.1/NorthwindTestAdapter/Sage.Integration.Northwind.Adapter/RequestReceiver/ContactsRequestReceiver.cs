using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Messaging.Model;
using Sage.Common.Syndication;
using System.ComponentModel;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Account;
using Sage.Integration.Northwind.Adapter.Common;
using System.Data.OleDb;
using Sage.Integration.Northwind.Application.Entities.Account.DataSets;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.Entities.Account.DataSets.AccountDatasetTableAdapters;
using Sage.Integration.Northwind.Adapter.Feeds;
using System.Xml.Schema;
using System.IO;
using Sage.Integration.Northwind.Adapter.Transform;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Sync;
using Sage.Integration.Northwind.Adapter.Common.Paging;
using Sage.Integration.Northwind.Adapter.Common.Performers;
using Sage.Integration.Northwind.Adapter.Data.FeedEntries;
using Sage.Integration.Northwind.Adapter.Common.Handler;
using Sage.Integration.Adapter.Model;
using Sage.Integration.Northwind.Adapter.Data;

namespace Sage.Integration.Northwind.Adapter.Receiver
{
    [RequestPath("contacts", "contacts(<resource>)")]
    public class ContactsRequestReceiver
    {
        public static NorthwindAdapter NorthwindAdapter;

        #region Ctor.

        /// <summary>
        ///
        /// </summary>
        /// <param name="adapter"></param>
        public ContactsRequestReceiver(NorthwindAdapter adapter)
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
        public void GetContacts(IRequest request, string resource)
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
            request.Response.FeedEntry = TemplateFactory.GetTemplate<ContactFeedEntry>(request);
        }


        [PostRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PostContact(IRequest request, ContactFeedEntry entry)
        {
            CRUD crud = new CRUD(request);
            crud.Create(entry);
        }

        [PutRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PutContact(IRequest request, ContactFeedEntry entry, string resource)
        {
            CRUD crud = new CRUD(request);
            crud.Update(entry, resource);
        }

        [DeleteRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void DeleteContact(IRequest request, string resource)
        {
            CRUD crud = new CRUD(request);
            crud.Delete(resource);
        }

        [GetRequestTarget("$schema")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedContentType(MediaType.Atom)]
        public void GetSchema(IRequest request)
        {
            RequestContext context = new RequestContext(request.Uri);
            string redirect = String.Format("{0}{1}#{2}", context.DatasetLink, Sage.Integration.Northwind.Adapter.Common.Constants.schema, context.ResourceKind.ToString());
            request.Response.StatusCode = System.Net.HttpStatusCode.Found;
            request.Response.Protocol.SendUnknownResponseHeader("Location", redirect);
        }

       [GetRequestTarget("$syncDigest")]
        //[SupportedAccept(MediaType.Atom, true)]
        //[SupportedContentType(MediaType.AtomEntry)]
        public void GetDigest(IRequest request)
        {
            RequestContext context = new RequestContext(request.Uri);
            //Old method (Several Attributes have been switched off/overridden attributes in the XmlSerializer)
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
           RequestContext context = new RequestContext(request.Uri);
            GetSyncSourceRequestPerformer performer = NorthwindAdapter.RequestPerformerLocator.Resolve<GetSyncSourceRequestPerformer>(context);
            performer.DoWork(request);
        }

        [PostRequestTarget("$syncTarget")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PostSyncTarget(IRequest request, Feed<ContactFeedEntry> feed)
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

        /*[DeleteRequestTarget("$syncTarget")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void DeleteSyncTarget(IRequest request)
        {
        }*/
        
       
    }
}