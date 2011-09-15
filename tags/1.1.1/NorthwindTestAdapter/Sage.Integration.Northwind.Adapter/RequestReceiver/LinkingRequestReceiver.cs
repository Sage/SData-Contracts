using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Common.Handler;
using Sage.Common.Syndication;

namespace Sage.Integration.Northwind.Adapter.RequestReceiver
{
    public class LinkingRequestReceiver
    {
        [GetRequestTarget("*/$linked")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedContentType(MediaType.Atom)]
        public void GetLinked(IRequest request)
        {
            LinkingCRUD performer = new LinkingCRUD(request);
            performer.Read();
        }

        [DeleteRequestTarget("*/$linked")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedContentType(MediaType.Atom)]
        public void DeleteLinked(IRequest request)
        {
            LinkingCRUD performer = new LinkingCRUD(request);
            performer.Delete();
        }

        [PutRequestTarget("*/$linked")]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PutLinked(IRequest request, FeedEntry entry)
        {
            LinkingCRUD performer = new LinkingCRUD(request);
            performer.Update(entry);
        }

        [PostRequestTarget("*/$linked")]
        [SupportedAccept(MediaType.AtomEntry, true)]
        [SupportedContentType(MediaType.AtomEntry)]
        public void PostLinked(IRequest request, FeedEntry entry)
        {
            LinkingCRUD performer = new LinkingCRUD(request);
            performer.Create(entry);
        }
    }
}
