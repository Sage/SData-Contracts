using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Messaging.Model;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common.Handler;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Adapter.Feeds;

namespace Sage.Integration.Northwind.Adapter.RequestReceiver
{
    [RequestPath("postalAddresses", "postalAddresses(<resource>)")]
    public class PostalAddressesRequestReceiver
    {
        
        public static NorthwindAdapter NorthwindAdapter;

        #region Ctor.

        /// <summary>
        ///
        /// </summary>
        /// <param name="adapter"></param>
        public PostalAddressesRequestReceiver(NorthwindAdapter adapter)
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
        public void GetPostalAddresses(IRequest request, string resource)
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
            request.Response.FeedEntry = TemplateFactory.GetTemplate<PostalAddressFeedEntry>(request);
        }


        [PostRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PostPostalAddresses(IRequest request, PostalAddressFeedEntry entry)
        {
            CRUD crud = new CRUD(request);
            crud.Create(entry);
        }

        [PutRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PutPostalAddresses(IRequest request, PostalAddressFeedEntry entry, string resource)
        {
            CRUD crud = new CRUD(request);
            crud.Update(entry, resource);
        }

        [DeleteRequestTarget()]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.Atom)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void DeletePostalAddresses(IRequest request, string resource)
        {
            CRUD crud = new CRUD(request);
            crud.Delete(resource);
        }
    }
}
