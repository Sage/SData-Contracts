#region Usings

using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Requests.Performers.Services;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class PostServiceRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            // Content type atom entry expected
            if (request.ContentType != Sage.Common.Syndication.MediaType.AtomEntry)
                throw new RequestException("Atom entry content type expected");

            // Get the specific request performer using a dispatcher and start it.
            // The performer could handle the request synchronously or asynchronously.
            // TODO: in case of an asynchronous call set HTTP status code 202 (Accepted)
            this.CreatePerformerAndDoWork(request);
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion

        private void CreatePerformerAndDoWork(IRequest request)
        {
            IServicePerformer performer = null;
            RequestPerformerLocator requestPerformerLocator = RequestReceiver.NorthwindAdapter.RequestPerformerLocator;

            switch (_requestContext.SdataUri.ServiceMethod.ToLowerInvariant())
            {
                case "computeprice":
                    performer = requestPerformerLocator.Resolve<ComputePriceServicePerformer>(_requestContext);
                    break;
            }
            if (null != performer)
                performer.DoWork(request);
            else
                throw new RequestException(string.Format("Invalid request: No service named {0} supported.", _requestContext.SdataUri.ServiceMethod));
        }
    }
}
