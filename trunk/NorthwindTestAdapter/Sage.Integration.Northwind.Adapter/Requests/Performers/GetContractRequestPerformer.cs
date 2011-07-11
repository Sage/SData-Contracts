#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Feeds;
using Sage.Common.Syndication;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class GetContractRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            ContractFeedEntry entry = new ContractFeedEntry();
            entry.Id = _requestContext.ApplicationLink + @"gcrm/";
            entry.Title = "Global CRM Contract";
            entry.Updated = DateTime.Now;

            entry.Name = "GCRM";
            entry.Description = "The Global CRM contract is a contract between CRM products and ERP products in Sage.";
            //entry.ContractSpecificationLink = @"http://interop.sage.com/daisy/SGCRMContract/274-DSY.html";
            entry.Link = entry.Id;
            
            Feed<ContractFeedEntry> feed = new Feed<ContractFeedEntry>();
            feed.Title = "Available Contracts";
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
