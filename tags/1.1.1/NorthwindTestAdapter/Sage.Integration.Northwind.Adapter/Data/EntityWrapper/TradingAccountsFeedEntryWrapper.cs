using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Application.Entities.Account;
using Sage.Integration.Northwind.Adapter.Transform;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class TradingAccountsFeedEntryWrapper : EntityWrapperBase, Sage.Integration.Northwind.Adapter.Data.IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        private TradingAccountTransform _transform;

        public TradingAccountsFeedEntryWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.tradingAccounts)
        {
            this._entity = new Account();
            _transform = new TradingAccountTransform(context);
        }

        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            Application.Base.Document document = _transform.GetTransformedDocument(payload as TradingAccountFeedEntry);
            if (document.CrmId == null)
            {
                document.CrmId = payload.Key;
            }
            return document;
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            return _transform.GetTransformedPayload(document as AccountDocument);
        }

        public override SdataTransactionResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        #region IEntityQueryWrapper Members

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "id";
            else if (propertyName.Equals("Name", StringComparison.InvariantCultureIgnoreCase))
                return "CompanyName";
            else if (propertyName.Equals("CustomerSupplierFlag", StringComparison.InvariantCultureIgnoreCase))
                return "CustomerSupplierFlag";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        #endregion
    }
}
