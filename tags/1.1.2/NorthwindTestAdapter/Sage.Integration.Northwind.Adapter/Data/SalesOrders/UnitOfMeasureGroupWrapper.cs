#region Usings

using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Product;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using System;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class UnitOfMeasureGroupWrapper : EntityWrapperBase ,IEntityWrapper, IEntityQueryWrapper
    {
        #region Class Variables

        private ITransformation<UnitOfMeasureFamilyDocument, UnitOfMeasureGroupPayload> _transformation;

        #endregion

        #region Ctor.

        public UnitOfMeasureGroupWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.unitsOfMeasureGroup)
        {
            _entity = new UnitOfMeasureFamily();
            _transformation = TransformationFactory.GetTransformation
                <ITransformation<UnitOfMeasureFamilyDocument, UnitOfMeasureGroupPayload>>
                (SupportedResourceKinds.unitsOfMeasureGroup, context);
        }

        #endregion

        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document result = _transformation.GetTransformedDocument(payload as UnitOfMeasureGroupPayload, Helper.ReducePayloadPath(links));
            return result;
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            PayloadBase result = _transformation.GetTransformedPayload(document as UnitOfMeasureFamilyDocument, out links);
            links = Helper.ExtendPayloadPath(links, "unitOfMeasureGroup");
            return result;
        }


        #endregion


        #region IEntityQueryWrapper Members

        string IEntityQueryWrapper.GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "ProductID";
            else if (propertyName.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                return "QuantityPerUnit";
            else if (propertyName.Equals("Description", StringComparison.InvariantCultureIgnoreCase))
                return "QuantityPerUnit";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        #endregion
    }
}
