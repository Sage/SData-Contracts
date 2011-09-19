using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Adapter.Common;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class PriceListTransform : TransformationBase, IFeedEntryTransformation<PricingListsDocument, PriceListFeedEntry>
    {
        public PriceListTransform(RequestContext context) : base(context, Adapter.Common.SupportedResourceKinds.priceLists) { }

        public PricingListsDocument GetTransformedDocument(PriceListFeedEntry feedEntry)
        {
            PricingListsDocument document = new PricingListsDocument();
            if (GuidIsNullOrEmpty(feedEntry.UUID))
            {
                document.Id = feedEntry.Key;
            }
            else
            {
                document.CrmId = feedEntry.UUID.ToString();
                document.Id = GetLocalId(feedEntry.UUID);
            }
            if (feedEntry.active)
                document.active.Value = Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active;

            /*document.defaultvalue.Value = feedEntry.primacyIndicator;
            if (feedEntry.primacyIndicator)
                document.erpdefaultvalue.Value = "Y";*/

            document.name.Value = feedEntry.name;
            //document.description.Value = feedEntry.description;
            return document;
        }

        public PriceListFeedEntry GetTransformedPayload(PricingListsDocument document)
        {
            PriceListFeedEntry payload = new PriceListFeedEntry();
            

            if ((!document.active.IsNull) &&
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                payload.active = true;

            // (document.name.IsNull) ? null : document.name.Value.ToString();
            SetCommonProperties(document, payload.name, payload);
            return payload;
        }
    }
}
