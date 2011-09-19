using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Adapter.Common;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class UnitOfMeasureTransform : TransformationBase, IFeedEntryTransformation<UnitOfMeasureDocument, UnitOfMeasureFeedEntry>
    {
        public UnitOfMeasureTransform(RequestContext context) : base(context, Adapter.Common.SupportedResourceKinds.unitsOfMeasure) { }

        public UnitOfMeasureDocument GetTransformedDocument(UnitOfMeasureFeedEntry feedEntry)
        {
            UnitOfMeasureDocument document = new UnitOfMeasureDocument();
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


            document.defaultvalue.Value = true;
            document.name.Value = feedEntry.name;

            return document;
        }

        public UnitOfMeasureFeedEntry GetTransformedPayload(UnitOfMeasureDocument document)
        {
            UnitOfMeasureFeedEntry payload = new UnitOfMeasureFeedEntry();

            if ((!document.active.IsNull) &&
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                payload.active = true;

            payload.name = (document.name.IsNull) ? null : document.name.Value.ToString();
            payload.description = payload.name;

            SetCommonProperties(document, payload.name, payload);
            return payload;
        }
    }
}
