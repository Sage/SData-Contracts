#region Usings

using System;
using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using Sage.Sis.Sdata.Sync.Storage;

#endregion

namespace Sage.Integration.Northwind.Adapter.Transformations.SalesOrders
{
    public class PriceListTransformation : TransformationBase, ITransformation<PricingListsDocument, PriceListPayload>
    {
        #region Class Variables

        #endregion

        #region Ctor.

        public PriceListTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.priceLists)
        {

        }

        #endregion


        #region ITransformation<PricingListsDocument,PriceListPayload> Members

        public PricingListsDocument GetTransformedDocument(PriceListPayload payload, List<SyncFeedEntryLink> links)
        {
            PricingListsDocument document = new PricingListsDocument();
            if (String.IsNullOrEmpty(payload.LocalID))
            {
                document.CrmId = payload.SyncUuid.ToString();//
                document.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                document.Id = payload.LocalID;
            }
            if (payload.PriceListtype.active)
                document.active.Value = Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active;

            document.defaultvalue.Value = payload.PriceListtype.primacyIndicator;
            if (payload.PriceListtype.primacyIndicator)
                document.erpdefaultvalue.Value = "Y";

            document.name.Value =  payload.PriceListtype.name;
            document.description.Value = payload.PriceListtype.description;
            return document;
        }

        public PriceListPayload GetTransformedPayload(PricingListsDocument document, out List<SyncFeedEntryLink> links)
        {
            PriceListPayload payload = new PriceListPayload();
            links = new List<SyncFeedEntryLink>();
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.priceLists, document.Id));
            links.Add(selfLink);

            //payload.PriceListtype.uuid = GetUuid(document.Id, document.CrmId).ToString();
            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;
            payload.PriceListtype.applicationID = document.Id;

            if ((!document.active.IsNull) &&
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                payload.PriceListtype.active = true;

            // (document.name.IsNull) ? null : document.name.Value.ToString();
            return payload;
        }

        #endregion
    }
}
