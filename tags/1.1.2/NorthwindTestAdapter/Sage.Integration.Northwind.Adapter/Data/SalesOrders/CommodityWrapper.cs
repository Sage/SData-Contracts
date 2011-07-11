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
    public class CommodityWrapper : EntityWrapperBase ,IEntityWrapper, IEntityQueryWrapper
    {
        #region Class Variables

        private ITransformation<ProductDocument, CommodityPayload> _transformation;

        #endregion

        #region Ctor.

        public CommodityWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.commodityGroups)
        {
            _entity = new Product();
            _transformation = TransformationFactory.GetTransformation
                <ITransformation<ProductDocument, CommodityPayload>>
                (SupportedResourceKinds.commodities, context);
        }

        #endregion

        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document result = _transformation.GetTransformedDocument(payload as CommodityPayload, Helper.ReducePayloadPath(links));
            return result;
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            PayloadBase result = _transformation.GetTransformedPayload(document as ProductDocument, out links);
            links = Helper.ExtendPayloadPath(links, "commodity");
            return result;
        }


        #endregion


        #region IEntityQueryWrapper Members

        string IEntityQueryWrapper.GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "ProductID";
            else if (propertyName.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                return "ProductName";
            
            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        #endregion
    }
}
