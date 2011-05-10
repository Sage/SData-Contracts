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
using Sage.Sis.Sdata.Sync.Storage;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class GetResourceRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestProcess Members

        public void DoWork(IRequest request)
        {
           IEntityWrapper wrapper = EntityWrapperFactory.Create(_requestContext.ResourceKind, _requestContext);
           SyncFeed syncFeed = wrapper.GetFeed();
            
            request.Response.Serializer = new SyncFeedSerializer();

            if (String.IsNullOrEmpty(_requestContext.ResourceKey))
            {
                syncFeed.FeedType = FeedType.Resource;
                request.Response.ContentType = Sage.Common.Syndication.MediaType.Atom;
            }
            else
            {
                syncFeed.FeedType = FeedType.ResourceEntry;
                request.Response.ContentType = Sage.Common.Syndication.MediaType.AtomEntry;
            }
            request.Response.Feed = syncFeed;
        }

        void IRequestPerformer.Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion
    }
}
