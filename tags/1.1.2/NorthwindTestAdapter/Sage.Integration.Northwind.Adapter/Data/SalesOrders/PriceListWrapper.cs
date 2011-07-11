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
    public class PriceListWrapper : EntityWrapperBase ,IEntityWrapper, IEntityQueryWrapper
    {
        #region Class Variables

        private ITransformation<PricingListsDocument, PriceListPayload> _transformation;

        #endregion

        #region Ctor.

        public PriceListWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.priceLists)
        {
            _entity = new PricingList();
            _transformation = TransformationFactory.GetTransformation
                <ITransformation<PricingListsDocument, PriceListPayload>>
                (SupportedResourceKinds.priceLists, context);
        }

        #endregion

        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document result = _transformation.GetTransformedDocument(payload as PriceListPayload, Helper.ReducePayloadPath(links));
            return result;
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            PayloadBase result = _transformation.GetTransformedPayload(document as PricingListsDocument, out links);
            links = Helper.ExtendPayloadPath(links, "priceList");
            return result;
        }


        #endregion


        #region IEntityQueryWrapper Members

        public string GetDbFieldName(string propertyName)
        {
            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        #endregion
    }
}
