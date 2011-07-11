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
    public class PriceTransformation : TransformationBase, ITransformation<PriceDocument, PricePayload>
    {
        #region Class Variables

        #endregion

        #region Ctor.

        public PriceTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.prices)
        {

        }

        #endregion


        #region ITransformation<PriceDocument,PricePayload> Members

        public PriceDocument GetTransformedDocument(PricePayload payload, List<SyncFeedEntryLink> links)
        {
            PriceDocument document = new PriceDocument();
            if (String.IsNullOrEmpty(payload.LocalID))
            {
                document.CrmId = payload.SyncUuid.ToString();//
                document.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                document.Id = payload.LocalID;
            }
            if (payload.Pricetype.active)
                document.active.Value = Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active;

            if (payload.Pricetype.priceSpecified)
                document.price.Value = payload.Pricetype.price;
            else
                document.price.Value = null;

            return document;
        }

        public PricePayload GetTransformedPayload(PriceDocument document, out List<SyncFeedEntryLink> links)
        {
            PricePayload payload = new PricePayload();
            links = new List<SyncFeedEntryLink>();
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.prices, document.Id));
            links.Add(selfLink);

            //payload.Pricetype.uuid = GetUuid(document.Id, document.CrmId).ToString();
            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;
            payload.Pricetype.applicationID = document.Id;

            if ((!document.active.IsNull) &&
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                payload.Pricetype.active = true;

            payload.Pricetype.priceSpecified = !(document.price.IsNull);
            if (payload.Pricetype.priceSpecified)
                payload.Pricetype.price = (decimal)document.price.Value;

            return payload;
        }

        #endregion
    }
}
