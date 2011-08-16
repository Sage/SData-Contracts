#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Messaging.Model;
using System.Reflection;
using System.IO;
using Sage.Common.Syndication;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class GetSyncSourceRequestPerformer : IRequestPerformer, ITrackingConsumer
    {
        #region Class Variables

        private RequestContext _requestContext;
        private ITrackingPerformer _trackingPerformer;

        #endregion

        #region IRequestProcess Members

        public void DoWork(IRequest request)
        {
           // ReturnSample(request);
           // return;
            _trackingPerformer.GetTrackingState(request);
        }

        void IRequestPerformer.Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion

        #region ITrackingConsumer Members

        void ITrackingConsumer.Initialize(ITrackingPerformer trackingPerformer)
        {
            _trackingPerformer = trackingPerformer;
        }
        
        #endregion
    }
}
