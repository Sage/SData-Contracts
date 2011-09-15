using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Transformations;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class CommodityGroupTransform : TransformationBase, IFeedEntryTransformation<ProductFamilyDocument, CommodityGroupFeedEntry>
    {
        public CommodityGroupTransform(RequestContext context) : base(context, SupportedResourceKinds.commodityGroups) { }

        public ProductFamilyDocument GetTransformedDocument(CommodityGroupFeedEntry feedEntry)
        {
            ProductFamilyDocument document = new ProductFamilyDocument();
            
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

            if(feedEntry.IsPropertyChanged("name"))
                document.name.Value = feedEntry.name;

            if(feedEntry.IsPropertyChanged("description"))
                document.description.Value = feedEntry.description;

            return document;
        }

        public CommodityGroupFeedEntry GetTransformedPayload(ProductFamilyDocument document)
        {
            CommodityGroupFeedEntry feedEntry = new CommodityGroupFeedEntry();

            feedEntry.active = false;

            if ((!document.active.IsNull) &&
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                feedEntry.active = true;

            feedEntry.name = (document.name.IsNull) ? null : document.name.Value.ToString();
            feedEntry.label = feedEntry.name;
            feedEntry.Descriptor = feedEntry.name;
            feedEntry.description = (document.description.IsNull) ? null : document.description.Value.ToString();
            feedEntry.type = commodityGroupTypeenum.Unknown;

            SetCommonProperties(document, feedEntry.name, feedEntry);
            return feedEntry;
        }
    }
}
