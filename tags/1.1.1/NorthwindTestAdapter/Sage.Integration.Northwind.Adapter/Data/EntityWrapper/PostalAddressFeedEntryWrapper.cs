using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Transform;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Entities.Account;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class PostalAddressFeedEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        PostalAdressTransform _transform;

        public PostalAddressFeedEntryWrapper(RequestContext context) : base(context, SupportedResourceKinds.postalAddresses)
        {
            this._entity = new Account();
            this._transform = new PostalAdressTransform(context);
        }


        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            return _transform.GetTransformedDocument(payload as PostalAddressFeedEntry);
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            return _transform.GetTransformedPayload(document as AddressDocument);
        }

        public SdataTransactionResult Update(Sage.Common.Syndication.FeedEntry payload)
        {
            //Transform account
            Document document = GetTransformedDocument(payload);
            AccountDocument accountDocument = (AccountDocument)_entity.GetDocumentTemplate();
            accountDocument.Id = document.Id;
            accountDocument.CrmId = GetTradingAccountUuid(document.Id);
            accountDocument.addresses.documents.Add(document);
            // Update Document

            List<TransactionResult> transactionResults = new List<TransactionResult>();
            _entity.Update(accountDocument, _context.Config, ref transactionResults);
            SdataTransactionResult sdTrResult = GetSdataTransactionResult(transactionResults,
                _context.OriginEndPoint, SupportedResourceKinds.tradingAccounts);
            if (sdTrResult != null)
            {
                sdTrResult.ResourceKind = _resourceKind;

                sdTrResult.HttpMessage = "PUT";
            }
            return sdTrResult;
        }

        public SdataTransactionResult Add(Sage.Common.Syndication.FeedEntry payload)
        {
            // until there is no reference inside the payload, no add will be possible
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        public new Sage.Common.Syndication.FeedEntry GetFeedEntry(string id)
        {
            PostalAddressFeedEntry result;

            Account account = new Account();
            Identity identity = new Identity(account.EntityName, id);
            AccountDocument accountDocument = (AccountDocument)_entity.GetDocument(identity, _emptyToken, _context.Config);

            if ((accountDocument.LogState == LogState.Deleted)
                    || (accountDocument.addresses.documents.Count == 0))
            {
                PostalAddressFeedEntry deletedPayload = new PostalAddressFeedEntry();
                deletedPayload.UUID = GetUuid(id);
                deletedPayload.IsDeleted = true;
                return deletedPayload;
            }

            Document document = accountDocument.addresses.documents[0];

            result = (PostalAddressFeedEntry)GetTransformedPayload(document);

            string taUuid = GetTradingAccountUuid(accountDocument.Id);

#warning no reference for trading accounts exists in the contract

            return result;
        }

        private Guid GetUuid(string localId)
        {
            if (String.IsNullOrEmpty(localId))
            {
                return Guid.Empty;
            }
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByLocalId(
                SupportedResourceKinds.postalAddresses.ToString(), new string[] { localId });
            if (results.Length > 0)
                return results[0].ResSyncInfo.Uuid;

            return Guid.Empty;
        }

        private string GetTradingAccountUuid(string localId)
        {
            if (String.IsNullOrEmpty(localId))
            {
                return null;
            }
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByLocalId(
                SupportedResourceKinds.tradingAccounts.ToString(), new string[] { localId });
            if (results.Length > 0)
                return results[0].ResSyncInfo.Uuid.ToString();

            return null;
        }

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "id";
            if (propertyName.Equals("townCity", StringComparison.InvariantCultureIgnoreCase))
                return "City";
            if (propertyName.Equals("zipPostCode", StringComparison.InvariantCultureIgnoreCase))
                return "PostalCode";
            if (propertyName.Equals("country", StringComparison.InvariantCultureIgnoreCase))
                return "Country";
            if (propertyName.Equals("stateregion", StringComparison.InvariantCultureIgnoreCase))
                return "Region";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }
    }
}
