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
    public class UnitOfMeasureTransformation : TransformationBase, ITransformation<UnitOfMeasureDocument, UnitOfMeasurePayload>
    {
        #region Class Variables

        #endregion

        #region Ctor.

        public UnitOfMeasureTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.unitsOfMeasure)
        {

        }

        #endregion


        #region ITransformation<UnitOfMeasureDocument,UnitOfMeasurePayload> Members

        public UnitOfMeasureDocument GetTransformedDocument(UnitOfMeasurePayload payload, List<SyncFeedEntryLink> links)
        {
            UnitOfMeasureDocument document = new UnitOfMeasureDocument();
            if (String.IsNullOrEmpty(payload.LocalID))
            {
                document.CrmId = payload.SyncUuid.ToString();//
                document.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                document.Id = payload.LocalID;
            }
            if (payload.UnitOfMeasuretype.active)
                document.active.Value = Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active;


            document.defaultvalue.Value = true;
            document.name.Value = payload.UnitOfMeasuretype.name;

            return document;
        }

        public UnitOfMeasurePayload GetTransformedPayload(UnitOfMeasureDocument document, out List<SyncFeedEntryLink> links)
        {
            UnitOfMeasurePayload payload = new UnitOfMeasurePayload();
            links = new List<SyncFeedEntryLink>();
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.unitsOfMeasure, document.Id));
            links.Add(selfLink);

            //payload.UnitOfMeasuretype.uuid = GetUuid(document.Id, document.CrmId).ToString();
            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;
            payload.UnitOfMeasuretype.applicationID = document.Id;

            if ((!document.active.IsNull) &&
                document.active.Value.ToString().Equals(Sage.Integration.Northwind.Application.API.Constants.DefaultValues.Active, StringComparison.InvariantCultureIgnoreCase))
                payload.UnitOfMeasuretype.active = true;

            payload.UnitOfMeasuretype.name = (document.name.IsNull) ? null : document.name.Value.ToString();
            payload.UnitOfMeasuretype.description = payload.UnitOfMeasuretype.name;

            return payload;
        }

        #endregion
    }
}
