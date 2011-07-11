#region Usings

using System;
using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Product;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class CommodityGroupWrapper : EntityWrapperBase ,IEntityWrapper, IEntityQueryWrapper
    {
        #region Class Variables

        private ITransformation<ProductFamilyDocument, CommodityGroupPayload> _transformation;

        #endregion

        #region Ctor.

        public CommodityGroupWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.commodityGroups)
        {
            _entity = new ProductFamily();
            _transformation = TransformationFactory.GetTransformation
                <ITransformation<ProductFamilyDocument, CommodityGroupPayload>>
                (SupportedResourceKinds.commodityGroups, context);
        }

        #endregion

        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document result = _transformation.GetTransformedDocument(payload as CommodityGroupPayload, Helper.ReducePayloadPath(links));
            return result;
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            PayloadBase result = _transformation.GetTransformedPayload(document as ProductFamilyDocument, out links);
            links = Helper.ExtendPayloadPath(links, "commodityGroup");
            return result;
        }


        #endregion

        #region IEntityQueryWrapper Members

        string IEntityQueryWrapper.GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "CategoryID";
            else if (propertyName.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                return "CategoryName";
            else if (propertyName.Equals("label", StringComparison.InvariantCultureIgnoreCase))
                return "CategoryName";
            else if (propertyName.Equals("description", StringComparison.InvariantCultureIgnoreCase))
                return "Description";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        #endregion
    }
}
