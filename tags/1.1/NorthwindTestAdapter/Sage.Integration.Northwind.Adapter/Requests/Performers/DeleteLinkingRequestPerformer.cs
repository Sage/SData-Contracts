#region Usings

using System;
using System.ComponentModel;
using Sage.Integration.Messaging.Model;
using Sage.Sis.Sdata.Sync.Storage;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class DeleteLinkingRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            if (String.IsNullOrEmpty(_requestContext.ResourceKey))
                throw new RequestException("Please use a uuid predicate.");
            // TODO: Check resourceKind for value None???

            Guid uuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(_requestContext.ResourceKey);
            string resourceKind = _requestContext.ResourceKind.ToString();

            ICorrelatedResSyncInfoStore correlatedResSyncInfoStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_requestContext.SdataContext);

            correlatedResSyncInfoStore.Delete(resourceKind, uuid);
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion
    }
}
