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
    public class CommodityGroupTransformation : TransformationBase, ITransformation<ProductFamilyDocument, CommodityGroupPayload>
    {
        #region Class Variables

        #endregion

        #region Ctor.

        public CommodityGroupTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.commodityGroups)
        {

        }

        #endregion


        #region ITransformation<ProductFamilyDocument,CommodityGroupPayload> Members

        public ProductFamilyDocument GetTransformedDocument(CommodityGroupPayload payload, List<SyncFeedEntryLink> links)
        {
            ProductFamilyDocument document = new ProductFamilyDocument();
            if (payload.CommodityGrouptype.active)
                document.active.Value = Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active;

            if (String.IsNullOrEmpty(payload.LocalID))
            {
                document.CrmId = payload.SyncUuid.ToString();//
                document.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                document.Id = payload.LocalID;
            }
            document.name.Value = payload.CommodityGrouptype.name;
            document.description.Value = payload.CommodityGrouptype.description;
            
 

            return document;
        }

        public CommodityGroupPayload GetTransformedPayload(ProductFamilyDocument document, out List<SyncFeedEntryLink> links)
        {
            CommodityGroupPayload payload = new CommodityGroupPayload();
            links = new List<SyncFeedEntryLink>();
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink,  SupportedResourceKinds.commodityGroups, document.Id));
            links.Add(selfLink);
            payload.CommodityGrouptype.active = false;
            
            if ((!document.active.IsNull) && 
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                   payload.CommodityGrouptype.active = true;
            
            //payload.CommodityGrouptype.uuid = GetUuid(document.Id, document.CrmId).ToString();
            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;
            payload.CommodityGrouptype.applicationID  = document.Id;

            payload.CommodityGrouptype.name = (document.name.IsNull) ? null : document.name.Value.ToString();
            payload.CommodityGrouptype.label = payload.CommodityGrouptype.name;
            payload.CommodityGrouptype.description = (document.description.IsNull) ? null : document.description.Value.ToString();

            return payload;
        }

        #endregion
    }
}
