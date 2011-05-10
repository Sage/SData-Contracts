#region Usings

using System;
using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Account;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.TradingAccounts;



#endregion

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class TradingAccountWrapper : EntityWrapperBase, IEntityWrapper, IEntityQueryWrapper
    {
        #region Class Variables

        private ITransformation<AccountDocument, TradingAccountPayload> _transformation;

        #endregion

        #region Ctor.

        public TradingAccountWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.tradingAccounts)
        {
            _entity = new Account();
            _transformation = TransformationFactory.GetTransformation<ITransformation<AccountDocument, TradingAccountPayload>>(SupportedResourceKinds.tradingAccounts, context);
        }

        #endregion

        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document result = _transformation.GetTransformedDocument(payload as TradingAccountPayload, links);
            return result;
        }

        public override PayloadBase GetTransformedPayload(Document document,out List<SyncFeedEntryLink> links)
        {
            PayloadBase result = _transformation.GetTransformedPayload(document as AccountDocument, out links);
            links = Helper.ExtendPayloadPath(links, "tradingAccount");
            return result;
        }


        #endregion

        #region IEntityQueryWrapper Members

        string IEntityQueryWrapper.GetDbFieldName(string propertyName)
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
