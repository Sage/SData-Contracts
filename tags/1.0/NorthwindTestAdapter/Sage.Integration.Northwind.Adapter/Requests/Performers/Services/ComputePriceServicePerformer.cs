#region Usings

using System.Xml;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Feeds;
using System.Collections.Generic;
using Sage.Integration.Northwind.Feeds.Services;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Adapter.Services;
using System;
using Sage.Common.Syndication;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Integration.Northwind.Sync;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.ComponentModel;

#endregion

namespace Sage.Integration.Northwind.Adapter.Requests.Performers.Services
{
    internal class ComputePriceServicePerformer : IServicePerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            //// check for resource kind existence and supported types
            //if (_requestContext.ResourceKind == SupportedResourceKinds.None)
            //    throw new RequestException("Invalid request: The service {0} is a resource based service.");
            
            // if request was done in the context of a resource kind -> check for supported resourceKinds
            if (_requestContext.ResourceKind != SupportedResourceKinds.None)
            {
                if (!(_requestContext.ResourceKind == SupportedResourceKinds.salesOrders ||
                    _requestContext.ResourceKind == SupportedResourceKinds.salesOrderLines))
                    throw new RequestException("Invalid request: The service {0} is only supported by salesOrders and salesOrderLines.");
            }

            // read entry from stream
            SyncFeedEntry entry = new SyncFeedEntry();
            XmlReader reader = XmlReader.Create(request.Stream);
            reader.MoveToContent();
            entry.ReadXml(reader, typeof(ComputePriceRequestPayload));

            // underlying resources for price computing
            computePriceRequesttype computePriceRequest;
            CommodityIdentity[] productIds;     // will contain local ids of the commodity referenced by line items.
                                                // Could contain empty items if commodity id could not be found.

            // compute price
            computePriceResponsetype computePriceResponse;
            if (null == entry.Payload)
                throw new RequestException("Invalid Xml input stream. Could not parse the payload.");

            computePriceRequest = ((ComputePriceRequestPayload)entry.Payload).ComputePriceRequest;

            #region Trading Account (discounted)

            // get the undelying trading account
            // is null if request is not in the context of an account.
            // if no account found means that the account exists in CRM only 
            // and has not yet been linked with ERP. ERP could treat the account as a 
            // prospect account and generate a price for the account, or if this is not
            // possible it could return an error.

            #endregion

            #region PricingDocument (only salesOrder suppported)

            // document associated with this pricing request (e.g. the Sales Order, Quotation, Purchase Order number).
            // could be of any type of enumeration "pricingDocumenttype"
            // if no document exists the request is not in the context of a document like order, quote, etc.

            // TODO: Get the associated document to find out the context of the request.
            
            // Validate document type
            // here: only salesOrder supported
            // removed to avoid possible errors (OK as this is only an example project)
            //if (computePriceRequest.pricingDocumentType != pricingDocumenttype.salesOrder)
            //    throw new RequestException("Invalid document type in element 'pricingDocumentType' defined. Only salesOrder supported.");

            #endregion

            #region Commodities (for each document lines)

            // Get Commodity local ids used on the associated document lines.
            // (the array returned is always of the same size as the number of document lines in the request payload.
            // If a commodity local id could not be found in feed entry the array item will be null.
            productIds = this.GetCommodityIds(entry);   // (never returns null, but could return an empty array)

            #endregion

            // compute price
            PricingServices pricingServices = new PricingServices(_requestContext.Config);

            if (_requestContext.ResourceKind == SupportedResourceKinds.salesOrders ||
                _requestContext.ResourceKind == SupportedResourceKinds.None)
                computePriceResponse = pricingServices.ComputePrice(computePriceRequest, productIds, false);
            else
                computePriceResponse = pricingServices.ComputePrice(computePriceRequest, productIds, true);


            // create response feed
            this.BuildResponseFeed(ref request, computePriceResponse);
            //this.BuildResponseFeed(ref request, cpr);
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion

        #region Helpers

        private CommodityIdentity[] GetCommodityIds(SyncFeedEntry requestEntry)
        {
            List<CommodityIdentity> identities = new List<CommodityIdentity>();

            // To be able to receive commodity data we need the local commodity ids.
            // These ids can be requested in 2 ways:
            // 1a) Using reference link: If attribute 'href' is a valid url of this adapter and has a resource key we can use the given resourceKey.
            // 1b) Using reference link: Otherwise we use the attribute 'uuid' and use the correlation repository.
            // 2)  Using the value contained in line property 'uuid' and using the correlation repository.
            // If 1) and 2) do not succeed we add an empty object.
            
            ComputePriceRequestPayload payload = (ComputePriceRequestPayload)requestEntry.Payload;

            int noOflines = payload.ComputePriceRequest.pricingDocumentLines.Length;

            for(int i=0; i< noOflines; i++)
            {
                string payloadPath = string.Format("computePrice/pricingDocumentLines[{0}]/commodity", i);

                SyncFeedEntryLink feedLink = Helper.FindLinkByPayloadPath(requestEntry.SyncLinks.ToArray(), payloadPath);

                //if (null == feedLink)
                //    throw new RequestException(string.Format("Link for payloadPath '{0}' missing", payloadPath));

                if (null != feedLink)
                {
                    // validate
                    string strRel = SyncFeedEntryLink.GetRelString(Sage.Integration.Northwind.Feeds.RelEnum.related);
                    if (feedLink.LinkRel != strRel)
                        throw new RequestException(string.Format("Parsing link with payloadPath '{0}' failed: Attribute 'rel' must contain value '{1}'.", payloadPath, strRel));

                    // TODO: excluded because condition linktype could have valid space character and we cannot check this in a simple way.
                    //string strType = SyncFeedEntryLink.GetTypeString(Sage.Integration.Northwind.Feeds.LinkTypeEnum.entry);
                    //if (feedLink.LinkType != strType)
                    //    throw new RequestException(string.Format("Parsing link with payloadPath '{0}' failed: Invalid media type defined in attribute 'type'. Value '{1}' expected.", payloadPath, strType));

                    string url = feedLink.Href;

                    // 1a) Try to parse href
                    if (url.StartsWith(_requestContext.DatasetLink + SupportedResourceKinds.commodities.ToString()))
                    {
                        RequestContext tmpRequestContext = new RequestContext(new SDataUri(url));
                        if (tmpRequestContext.RequestType == RequestType.Resource)
                        {
                            identities.Add(new CommodityIdentity(tmpRequestContext.ResourceKey));
                            continue;   // continue iteration (parse next line item)
                        }
                    }

                    // 1b) Try to get uuid from link and to get the local id using synch correlation (linking)
                    if (!string.IsNullOrEmpty(feedLink.Uuid))
                    {
                        Guid uuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(feedLink.Uuid);
                        ICorrelatedResSyncInfoStore correlationStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_requestContext.SdataContext);
                        CorrelatedResSyncInfo[] correlations = correlationStore.GetByUuid(SupportedResourceKinds.commodities.ToString(), new Guid[] { uuid });

                        if (correlations.Length == 1)
                        {
                            identities.Add(new CommodityIdentity(correlations[0].LocalId));
                            continue;   // continue iteration (parse next line item)
                        }
                    }
                }
                else
                {
                    // 2) Try to get uuid from property named 'uuid'
                    string strLineUuid = payload.ComputePriceRequest.pricingDocumentLines[i].uuid;
                    if (!string.IsNullOrEmpty(strLineUuid))
                    {
                        Guid uuid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFrom(strLineUuid);

                        // get the local id using synch correlation (linking)
                        ICorrelatedResSyncInfoStore correlationStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_requestContext.SdataContext);
                        CorrelatedResSyncInfo[] correlations = correlationStore.GetByUuid(SupportedResourceKinds.commodities.ToString(), new Guid[] { uuid });

                        if (correlations.Length == 1)
                        {
                            identities.Add(new CommodityIdentity(correlations[0].LocalId));
                            continue;   // continue iteration (parse next line item)
                        }
                    }
                }

                // If 1) and 2) failed add an empty CommodityIdentity
                identities.Add(CommodityIdentity.Empty);
            }

            return identities.ToArray();
        }


        private void BuildResponseFeed(ref IRequest request, computePriceResponsetype computePriceResponse)
        {
            ComputePriceResponsePayload computePriceResponsePayload = new ComputePriceResponsePayload();

            computePriceResponsePayload.ComputePriceResponse = computePriceResponse;


            SyncFeedEntry entry = new SyncFeedEntry();
            entry.Payload = computePriceResponsePayload;

            entry.Id = _requestContext.SdataUri.Uri.ToString();
            entry.Title = "computePrice";
            entry.Updated = DateTime.Now;

            //entry.Links
            //entry.Categories


            SyncFeed feed = new SyncFeed();
            feed.FeedType = FeedType.ResourceEntry;
            feed.Entries.Add(entry);
            request.Response.Serializer = new SyncFeedSerializer();
            request.Response.Feed = feed;
            request.Response.ContentType = Sage.Common.Syndication.MediaType.AtomEntry;
        }

        #endregion
    }
}
