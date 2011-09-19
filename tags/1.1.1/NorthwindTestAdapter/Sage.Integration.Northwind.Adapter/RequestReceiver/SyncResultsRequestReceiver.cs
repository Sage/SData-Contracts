using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Messaging.Model;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common.Handler;

namespace Sage.Integration.Northwind.Adapter.RequestReceiver
{
    public class SyncResultsRequestReceiver
    {
        [PostRequestTarget("*/$syncResults")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedContentType(MediaType.Atom)]
        public void PostSyncResults(IRequest request, Feed<FeedEntry> feed)
        {
            SyncResultsHandler handler = new SyncResultsHandler(request);
            handler.LogResults(feed);
        }
    }
}
