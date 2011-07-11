﻿#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Feeds;
using System.IO;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    /// <summary>
    /// Performer that redirects the request to the general contract schema link.
    /// </summary>
    internal class GetResourceSchemaRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion


        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            string redirect = String.Format("{0}{1}#{2}", _requestContext.DatasetLink, Constants.schema, _requestContext.ResourceKind.ToString());
            request.Response.StatusCode = System.Net.HttpStatusCode.Found;
            request.Response.Protocol.SendUnknownResponseHeader("Location", redirect);
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion
    }
}