#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Feeds.TradingAccounts;
using Sage.Integration.Northwind.Application.API;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Application.Base;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using Sage.Integration.Northwind.Application.Entities.Product;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class UnitOfMeasureWrapper : EntityWrapperBase ,IEntityWrapper, IEntityQueryWrapper
    {
        #region Class Variables

        private ITransformation<UnitOfMeasureDocument, UnitOfMeasurePayload> _transformation;

        #endregion

        #region Ctor.

        public UnitOfMeasureWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.unitsOfMeasure)
        {
            _entity = new UnitOfMeasure();
            _transformation = TransformationFactory.GetTransformation
                <ITransformation<UnitOfMeasureDocument, UnitOfMeasurePayload>>
                (SupportedResourceKinds.unitsOfMeasure, context);
        }

        #endregion

        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document result = _transformation.GetTransformedDocument(payload as UnitOfMeasurePayload, Helper.ReducePayloadPath(links));
            return result;
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            PayloadBase result = _transformation.GetTransformedPayload(document as UnitOfMeasureDocument, out links);
            links = Helper.ExtendPayloadPath(links, "unitOfMeasure");
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
