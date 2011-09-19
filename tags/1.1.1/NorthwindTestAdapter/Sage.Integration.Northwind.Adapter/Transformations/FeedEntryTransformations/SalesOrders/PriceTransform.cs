using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Data;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class PriceTransform : TransformationBase, IFeedEntryTransformation<PriceDocument, PriceFeedEntry>
    {
        public PriceTransform(RequestContext context) : base(context, Adapter.Common.SupportedResourceKinds.prices) { }

        public PriceDocument GetTransformedDocument(PriceFeedEntry feedEntry)
        {
            PriceDocument document = new PriceDocument();
            if (GuidIsNullOrEmpty(feedEntry.UUID))
            {
                document.Id = feedEntry.Key;
            }
            else
            {
                document.CrmId = feedEntry.UUID.ToString();
                document.Id = GetLocalId(feedEntry.UUID);
            }
            
            if (feedEntry.IsPropertyChanged("active"))
                document.active.Value = feedEntry.active ?
                    Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active : Sage.Integration.Northwind.Application.API.Constants.DefaultValues.NotActive;

            if (feedEntry.IsPropertyChanged("price"))
                document.price.Value = feedEntry.price;

            return document;
        }

        public PriceFeedEntry GetTransformedPayload(PriceDocument document)
        {
            PriceFeedEntry payload = new PriceFeedEntry();

            if (!document.active.NotSet) 
                payload.active = Convert.ToBoolean(document.active.Value);
            else
                payload.active = true;

            if (!(document.price.IsNull))
                payload.price = (decimal)document.price.Value;

            SetCommonProperties(document, payload.price.ToString(), payload);
            return payload;
        }
    }
}