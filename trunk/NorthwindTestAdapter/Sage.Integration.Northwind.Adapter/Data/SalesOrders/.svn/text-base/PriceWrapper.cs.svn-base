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
    public class PriceWrapper : EntityWrapperBase ,IEntityWrapper, IEntityQueryWrapper
    {
        #region Class Variables

        private ITransformation<PriceDocument, PricePayload> _transformation;

        #endregion

        #region Ctor.

        public PriceWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.prices)
        {
            _entity = new Price();
            _transformation = TransformationFactory.GetTransformation
                <ITransformation<PriceDocument, PricePayload>>
                (SupportedResourceKinds.prices, context);
        }

        #endregion

        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document result = _transformation.GetTransformedDocument(payload as PricePayload, Helper.ReducePayloadPath(links));
            return result;
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            PayloadBase result = _transformation.GetTransformedPayload(document as PriceDocument, out links);
            links = Helper.ExtendPayloadPath(links, "price");
            return result;
        }


        #endregion


        #region IEntityQueryWrapper Members

        string IEntityQueryWrapper.GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "ProductID";
            if (propertyName.Equals("price", StringComparison.InvariantCultureIgnoreCase))
                return "UnitPrice";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        #endregion
    }
}
