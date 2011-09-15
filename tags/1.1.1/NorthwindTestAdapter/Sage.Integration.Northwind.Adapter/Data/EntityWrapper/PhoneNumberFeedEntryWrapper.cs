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
using System.Data.OleDb;
using System.ComponentModel;

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class PhoneNumberFeedEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        PhoneNumberTransform _transform;
        private string _tradingAccountUuidPayloadPath;


        public PhoneNumberFeedEntryWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.phoneNumbers)
        {
            this._entity = new Account();
            this._transform = new PhoneNumberTransform(context);
            _tradingAccountUuidPayloadPath = _resourceKind.ToString() + "/" + SupportedResourceKinds.tradingAccounts.ToString();
        }


        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            return _transform.GetTransformedDocument(payload as PhoneNumberFeedEntry);
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            return _transform.GetTransformedPayload(document as PhoneDocument);
        }

        public override SdataTransactionResult Update(Sage.Common.Syndication.FeedEntry payload)
        {
            //Transform account
            Document document = GetTransformedDocument(payload);
            string id;
            string accountId;
            id = document.Id;
            if (id.EndsWith(Sage.Integration.Northwind.Application.API.Constants.PhoneIdPostfix))
                accountId = id.Replace(Sage.Integration.Northwind.Application.API.Constants.PhoneIdPostfix, "");
            else if (id.EndsWith(Sage.Integration.Northwind.Application.API.Constants.FaxIdPostfix))
                accountId = id.Replace(Sage.Integration.Northwind.Application.API.Constants.FaxIdPostfix, "");
            else
                return null;

            AccountDocument accountDocument = (AccountDocument)_entity.GetDocumentTemplate();
            accountDocument.Id = accountId;
            accountDocument.CrmId = GetTradingAccountUuid(accountId);
            accountDocument.phones.documents.Add(document);
            // Update Document

            List<TransactionResult> transactionResults = new List<TransactionResult>();
            _entity.Update(accountDocument, _context.Config, ref transactionResults);
            SdataTransactionResult sdTrResult = GetSdataTransactionResult(transactionResults,
                _context.OriginEndPoint, SupportedResourceKinds.phoneNumbers);
            if (sdTrResult != null)
            {
                sdTrResult.ResourceKind = _resourceKind;

                sdTrResult.HttpMessage = "PUT";
            }
            return sdTrResult;
        }

        public override Identity GetIdentity(string id)
        {
            string accountId = id;
            if (id.EndsWith(Sage.Integration.Northwind.Application.API.Constants.PhoneIdPostfix))
                accountId = id.Substring(0, id.Length - Sage.Integration.Northwind.Application.API.Constants.PhoneIdPostfix.Length);
            else if (id.EndsWith(Sage.Integration.Northwind.Application.API.Constants.FaxIdPostfix))
                accountId = id.Substring(0, id.Length - Sage.Integration.Northwind.Application.API.Constants.FaxIdPostfix.Length);

            return new Identity(_entity.EntityName, accountId);
        }

        public override string[] GetFeed()
        {
            string whereClause = string.Empty;
            OleDbParameter[] oleDbParameters = null;

            if (this is IEntityQueryWrapper)
            {
                QueryFilterBuilder queryFilterBuilder = new QueryFilterBuilder((IEntityQueryWrapper)this);

                queryFilterBuilder.BuildSqlStatement(_context, out whereClause, out oleDbParameters);
            }

            Token emptyToken = new Token();


            List<Identity> identities = new List<Identity>();

            if (String.IsNullOrEmpty(_context.ResourceKey))
                identities = _entity.GetAll(_context.Config, whereClause, oleDbParameters);
            else
                identities.Add(GetIdentity(_context.ResourceKey));

            string[] result = new string[identities.Count * 2];
            for (int i = 0; i < identities.Count; i++)
            {
                result[i * 2] = identities[i].Id + Sage.Integration.Northwind.Application.API.Constants.PhoneIdPostfix;
            }
            for (int i = 0; i < identities.Count; i++)
            {
                result[i * 2 + 1] = identities[i].Id + Sage.Integration.Northwind.Application.API.Constants.FaxIdPostfix;
            }

            return result;
        }

        public override SdataTransactionResult Add(Sage.Common.Syndication.FeedEntry payload)
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
            Sage.Common.Syndication.FeedEntry result;

            Identity identity;
            AccountDocument accountDocument;
            Account account = new Account();
            string accountId;
            if (id.EndsWith(Sage.Integration.Northwind.Application.API.Constants.PhoneIdPostfix))
                accountId = id.Substring(0, id.Length - Sage.Integration.Northwind.Application.API.Constants.PhoneIdPostfix.Length);
            else if (id.EndsWith(Sage.Integration.Northwind.Application.API.Constants.FaxIdPostfix))
                accountId = id.Substring(0, id.Length - Sage.Integration.Northwind.Application.API.Constants.FaxIdPostfix.Length);
            else
                return null;

            identity = new Identity(account.EntityName, accountId);


            accountDocument = (AccountDocument)_entity.GetDocument(identity, _emptyToken, _context.Config);

            if (accountDocument.LogState == LogState.Deleted)
                return null;
            if (accountDocument.addresses.documents.Count == 0)
                return null;
            Document document = null;
            foreach (Document phoneDoc in accountDocument.phones.documents)
            {
                if (phoneDoc.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase))
                {
                    document = phoneDoc;
                    break;
                }
            }
            if (document == null)
                return null;

            result = GetTransformedPayload(document);
            return result;
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

        private string GetTradingAccountLocalId(string uuidString)
        {
            GuidConverter converter = new GuidConverter();
            try
            {
                Guid uuid = (Guid)converter.ConvertFromString(uuidString);
                return GetTradingAccountLocalId(uuid);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string GetTradingAccountLocalId(Guid uuid)
        {
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByUuid(
                SupportedResourceKinds.tradingAccounts.ToString(), new Guid[] { uuid });
            if (results.Length > 0)
                return results[0].LocalId;
            return null;
        } 

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "id";
            if (propertyName.Equals("text", StringComparison.InvariantCultureIgnoreCase))
                return "Phone";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }
    }
}
