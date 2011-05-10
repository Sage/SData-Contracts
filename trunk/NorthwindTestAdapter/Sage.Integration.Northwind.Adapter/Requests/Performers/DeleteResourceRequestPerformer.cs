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
#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class DeleteResourceRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {

            if (String.IsNullOrEmpty(_requestContext.ResourceKey))
                throw new RequestException("Please use single resource url");


            IEntityWrapper wrapper; 
            
            wrapper = EntityWrapperFactory.Create(_requestContext.ResourceKind, _requestContext);
            
            SdataTransactionResult sdTrResult = wrapper.Delete(_requestContext.ResourceKey);

            if (sdTrResult == null)
                throw new RequestException("Entity does not exists");

            if (sdTrResult.HttpStatus == System.Net.HttpStatusCode.OK)
            {
                request.Response.StatusCode = System.Net.HttpStatusCode.OK;
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
