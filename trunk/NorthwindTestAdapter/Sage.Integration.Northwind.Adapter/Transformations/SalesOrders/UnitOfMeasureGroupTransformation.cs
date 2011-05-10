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
    public class UnitOfMeasureGroupTransformation : TransformationBase, ITransformation<UnitOfMeasureFamilyDocument, UnitOfMeasureGroupPayload>
    {
        #region Class Variables

        #endregion

        #region Ctor.

        public UnitOfMeasureGroupTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.unitsOfMeasureGroup)
        {

        }

        #endregion


        #region ITransformation<UnitOfMeasureFamilyDocument,UnitOfMeasureGroupPayload> Members

        public UnitOfMeasureFamilyDocument GetTransformedDocument(UnitOfMeasureGroupPayload payload, List<SyncFeedEntryLink> links)
        {
            UnitOfMeasureFamilyDocument document = new UnitOfMeasureFamilyDocument();
            if (String.IsNullOrEmpty(payload.LocalID))
            {
                document.CrmId = payload.SyncUuid.ToString();//
                document.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                document.Id = payload.LocalID;
            }
            if (payload.UnitOfMeasureGrouptype.active)
                document.active.Value = Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active;
            document.defaultvalue.Value = true;
            document.description.Value = payload.UnitOfMeasureGrouptype.description;
            document.name.Value = payload.UnitOfMeasureGrouptype.name;

            return document;
        }

        public UnitOfMeasureGroupPayload GetTransformedPayload(UnitOfMeasureFamilyDocument document, out List<SyncFeedEntryLink> links)
        {
            UnitOfMeasureGroupPayload payload = new UnitOfMeasureGroupPayload();
            links = new List<SyncFeedEntryLink>();
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.unitsOfMeasureGroup, document.Id));
            links.Add(selfLink);

            //payload.UnitOfMeasureGrouptype.uuid = GetUuid(document.Id, document.CrmId).ToString();
            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;
            payload.UnitOfMeasureGrouptype.applicationID = document.Id;

            if ((!document.active.IsNull) &&
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                payload.UnitOfMeasureGrouptype.active = true;

            payload.UnitOfMeasureGrouptype.name = (document.name.IsNull) ? null : document.name.Value.ToString();
            payload.UnitOfMeasureGrouptype.description = (document.description.IsNull) ? null : document.description.Value.ToString();


            return payload;
        }

        #endregion
    }
}
