#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Application;
using System.IO;
using Sage.Common.Syndication;
using Sage.Integration.Messaging.Model;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class GetDatasetRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            NorthwindConfig config = new NorthwindConfig();
            config.CurrencyCode = "EUR";
            config.CrmUser = "Sdata";
            config.Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Northwind");

            string fileName = Path.Combine(config.Path, "Northwind.mdb");
            FileInfo fileInfo = new FileInfo(fileName);
            string link = _requestContext.ContractLink + "-";

            DatasetFeedEntry entry = new DatasetFeedEntry();
            entry.Id = link;
            entry.Title = fileInfo.Name;

            entry.Published = fileInfo.CreationTime;
            entry.Updated = fileInfo.LastAccessTime;
            entry.Link = link;

            Feed<DatasetFeedEntry> feed = new Feed<DatasetFeedEntry>();
            feed.Id = "Available Datasets";
            feed.Title = "Available Datasets";
            feed.Entries.Add(entry);
            request.Response.Feed = feed;
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion
    }
}
