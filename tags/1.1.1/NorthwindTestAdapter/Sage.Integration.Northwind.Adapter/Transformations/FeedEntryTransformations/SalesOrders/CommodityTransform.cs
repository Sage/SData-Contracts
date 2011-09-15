using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.ComponentModel;
using Sage.Integration.Northwind.Adapter.Data;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class CommodityTransform : TransformationBase, IFeedEntryTransformation<ProductDocument, CommodityFeedEntry>
    {
        public CommodityTransform(RequestContext context) : base(context, Adapter.Common.SupportedResourceKinds.commodities) { }

        public ProductDocument GetTransformedDocument(CommodityFeedEntry feedEntry)
        {
            ProductDocument document = new ProductDocument();

            if (GuidIsNullOrEmpty(feedEntry.UUID))
            {
                document.Id = feedEntry.Key;
            }
            else
            {
                document.CrmId = feedEntry.UUID.ToString();
                document.Id = GetLocalId(feedEntry.UUID);
            }

            document.code.Value = document.Id;

            if(feedEntry.IsPropertyChanged("active"))
                document.active.Value = feedEntry.active ?
                    Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active : Sage.Integration.Northwind.Application.API.Constants.DefaultValues.NotActive;

            if(feedEntry.IsPropertyChanged("name"))
                document.name.Value = feedEntry.name;

            if (feedEntry.IsPropertyChanged("commodityGroup"))
                if(feedEntry.commodityGroup != null)
                {
                    string commodityGroupGuid = feedEntry.commodityGroup.UUID.ToString();
                    document.productfamilyid.Value = Getproductfamilyid(commodityGroupGuid);
                }
            //TODO: What do if commodityGroup = null?

            if(feedEntry.IsPropertyChanged("unitOfMeasure"))
                if (feedEntry.unitOfMeasure != null)
                {
                    string uomGroupGuid = feedEntry.unitOfMeasure.UUID.ToString();
                    //document.uomcategory.Value = GetUomfamilyid(uomGroupGuid);
                }
            //TODO: What do if unitOfMeasure == null?

            return document;
        }

        public CommodityFeedEntry GetTransformedPayload(ProductDocument document)
        {
            CommodityFeedEntry payload = new CommodityFeedEntry();

            if (!document.productfamilyid.IsNull)
            {
                string commodityGroupId = document.productfamilyid.Value.ToString();
                CommodityGroupFeedEntryWrapper cgWrapper = new CommodityGroupFeedEntryWrapper(_context);
                payload.commodityGroup = cgWrapper.GetFeedEntry(commodityGroupId) as CommodityGroupFeedEntry;
            }
            if (!document.uomcategory.IsNull)
            {
                string uomGroupId = document.uomcategory.Value.ToString();
                UnitOfMeasureFeedEntryWrapper uomWrapper = new UnitOfMeasureFeedEntryWrapper(_context);
                payload.unitOfMeasure = uomWrapper.GetFeedEntry(uomGroupId) as UnitOfMeasureFeedEntry;
            }

            if ((!document.active.IsNull) &&
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                payload.active = true;

            payload.type = commodityTypeenum.Unknown;

            payload.name = (document.name.IsNull) ? null : document.name.Value.ToString();

            SetCommonProperties(document, payload.name, payload);
            return payload;
        }

        #region helper

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
    }
}
