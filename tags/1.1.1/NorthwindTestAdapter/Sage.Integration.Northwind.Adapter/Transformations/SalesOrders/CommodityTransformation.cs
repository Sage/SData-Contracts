#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Integration.Northwind.Adapter.Transformations.SalesOrders
{
    public class CommodityTransformation : TransformationBase, ITransformation<ProductDocument, CommodityPayload>
    {
        #region Class Variables
        private string _commodityGroupUuidPayloadPath;
        private string _uomGroupUuidPayloadPath;
        #endregion

        #region Ctor.

        public CommodityTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.commodities)
        {
            _commodityGroupUuidPayloadPath = SupportedResourceKinds.commodityGroups.ToString();
            _uomGroupUuidPayloadPath = SupportedResourceKinds.unitsOfMeasureGroup.ToString();
        }

        #endregion


        #region ITransformation<ProductDocument,CommodityPayload> Members

        public ProductDocument GetTransformedDocument(CommodityPayload payload, List<SyncFeedEntryLink> links)
        {
            ProductDocument document = new ProductDocument();
            if (String.IsNullOrEmpty(payload.LocalID))
            {
                document.CrmId = payload.SyncUuid.ToString();//
                document.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                document.Id = payload.LocalID;
            }
            if (payload.Commoditytype.active)
                document.active.Value = Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active;
            document.code.Value = document.Id;
            //document.instock.Value = payload.Commoditytype.
            document.name.Value = payload.Commoditytype.name;
            string commodityGroupGuid = null;
            foreach (SyncFeedEntryLink link in links)
            {
                if ((!String.IsNullOrEmpty(link.PayloadPath)) &&
                    link.PayloadPath.Equals(_commodityGroupUuidPayloadPath, 
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    commodityGroupGuid = link.Uuid;
                    break;
                }
            }
            document.productfamilyid.Value = Getproductfamilyid(commodityGroupGuid);
            string uomGroupGuid = null;
            foreach (SyncFeedEntryLink link in links)
            {
                if ((!String.IsNullOrEmpty(link.PayloadPath)) &&
                    link.PayloadPath.Equals(_uomGroupUuidPayloadPath,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    uomGroupGuid = link.Uuid;
                    break;
                }
            }
            document.uomcategory.Value = GetUomfamilyid(uomGroupGuid);

            return document;

        }

        #region helper

        private string GetUomfamilyid(string uuidString)
        {
            GuidConverter converter = new GuidConverter();
            try
            {
                Guid uuid = (Guid)converter.ConvertFromString(uuidString);
                return GetUomfamilyid(uuid);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string GetUomfamilyid(Guid uuid)
        {
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByUuid(
                SupportedResourceKinds.unitsOfMeasureGroup.ToString(), new Guid[] { uuid });
            if (results.Length > 0)
                return results[0].LocalId;
            return null;
        }
        private string GetUnitOfMeasureGroupUuid(string localId)
        {
            if (String.IsNullOrEmpty(localId))
            {
                return null;
            }
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByLocalId(
                SupportedResourceKinds.unitsOfMeasureGroup.ToString(), new string[] { localId });
            if (results.Length > 0)
                return results[0].ResSyncInfo.Uuid.ToString();

            return null;

        }



        private string Getproductfamilyid(string uuidString)
        {
            GuidConverter converter = new GuidConverter();
            try
            {
                Guid uuid = (Guid)converter.ConvertFromString(uuidString);
                return Getproductfamilyid(uuid);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string Getproductfamilyid(Guid uuid)
        {
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByUuid(
                SupportedResourceKinds.commodityGroups.ToString(), new Guid[] { uuid });
            if (results.Length > 0)
                return results[0].LocalId;
            return null;
        }
        private string GetCommodityGroupUuid(string localId)
        {
            if (String.IsNullOrEmpty(localId))
            {
                return null;
            }
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByLocalId(
                SupportedResourceKinds.commodityGroups.ToString(), new string[] { localId });
            if (results.Length > 0)
                return results[0].ResSyncInfo.Uuid.ToString();

            return null;

        }
        #endregion

        public CommodityPayload GetTransformedPayload(ProductDocument document, out List<SyncFeedEntryLink> links)
        {
            CommodityPayload payload = new CommodityPayload();
            links = new List<SyncFeedEntryLink>();
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.commodities, document.Id));
            links.Add(selfLink);
            if (!document.productfamilyid.IsNull)
            {
                string commodityGroupUuid = GetCommodityGroupUuid(document.productfamilyid.Value.ToString());
                if (!String.IsNullOrEmpty(commodityGroupUuid))
                {
                    SyncFeedEntryLink commodityGroupLink = SyncFeedEntryLink.CreateRelatedLink(selfLink.Href, SupportedResourceKinds.commodityGroups.ToString(), _commodityGroupUuidPayloadPath, commodityGroupUuid);
                    links.Add(commodityGroupLink);
                }
            }
            if (!document.uomcategory.IsNull)
            {
            string uomGroupUuid = GetUnitOfMeasureGroupUuid(document.uomcategory.Value.ToString());
            if (!String.IsNullOrEmpty(uomGroupUuid))
            {
                SyncFeedEntryLink uomGroupLink = SyncFeedEntryLink.CreateRelatedLink(selfLink.Href, SupportedResourceKinds.unitsOfMeasureGroup.ToString(), _commodityGroupUuidPayloadPath, uomGroupUuid);
                links.Add(uomGroupLink);
            }
        }
            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;

            payload.Commoditytype.uuid = payload.SyncUuid.ToString();
            payload.Commoditytype.applicationID = document.Id;

            if ((!document.active.IsNull) &&
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                payload.Commoditytype.active = true;

            payload.Commoditytype.name = (document.name.IsNull) ? null : document.name.Value.ToString();
           

            return payload;
        }

        #endregion
    }
}
