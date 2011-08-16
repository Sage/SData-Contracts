using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Messaging.Model;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Application.Entities.Account;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.ComponentModel;
using Sage.Integration.Northwind.Adapter.Transform;
using Sage.Integration.Northwind.Adapter.Data;
using System.Data.OleDb;

namespace Sage.Integration.Northwind.Adapter.Data
{
    class ContactFeedEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        ContactTransform _transformer;
        public ContactFeedEntryWrapper(RequestContext request)
            : base(request, SupportedResourceKinds.contacts)
        {
            this._entity = new Account();
            this._entity.DelimiterClause = @"ContactName IS NOT NULL AND ContactName LIKE '_%'"; //Is not null or empty
            _transformer = new ContactTransform(request);
        }

        public override Document GetTransformedDocument(FeedEntry payload)
        {
            return _transformer.GetTransformedDocument(payload as ContactFeedEntry);
        }

        public override FeedEntry GetTransformedPayload(Document document)
        {
            _transformer.TradingAccountId = ((AccountDocument)document).Id;
            if(((AccountDocument)document).people.documents.Count > 0)
                return _transformer.GetTransformedPayload((PersonDocument)((AccountDocument)document).people.documents[0]);
            return null;
        }

        public override SdataTransactionResult Add(FeedEntry entry)
        {
            string accountUuid = string.Empty;
            SdataTransactionResult sdTrResult;
            ContactFeedEntry contactEntry = entry as ContactFeedEntry;

            if (contactEntry.primacyIndicator)
            {
                // is primary
            }
            else
            {
                sdTrResult = new SdataTransactionResult();
                sdTrResult.HttpMessage = "Only primary contacts supported";
                sdTrResult.HttpMethod = HttpMethod.POST;
                sdTrResult.HttpStatus = System.Net.HttpStatusCode.Forbidden;
                sdTrResult.ResourceKind = SupportedResourceKinds.contacts;
                sdTrResult.Uuid = contactEntry.UUID.ToString();

                AttachDiagnosis(sdTrResult);
                return sdTrResult;

            }

            if(contactEntry.tradingAccount != null)
                accountUuid = contactEntry.tradingAccount.UUID.ToString();


            if (String.IsNullOrEmpty(accountUuid) || Guid.Empty.ToString() == accountUuid)
            {
                sdTrResult = new SdataTransactionResult();
                sdTrResult.HttpMessage = "Trading Account UUID was missing";
                sdTrResult.HttpMethod = HttpMethod.POST;
                sdTrResult.HttpStatus = System.Net.HttpStatusCode.Forbidden;
                sdTrResult.ResourceKind = _resourceKind;
                sdTrResult.Uuid = contactEntry.UUID.ToString();

                AttachDiagnosis(sdTrResult);
                return sdTrResult;
            }


            string accountId = GetTradingAccountLocalId(accountUuid);

            if (String.IsNullOrEmpty(accountId))
            {
                sdTrResult = new SdataTransactionResult();
                sdTrResult.HttpMessage = String.Format("Trading Account UUID {0} was not linked", accountUuid);
                sdTrResult.HttpMethod = HttpMethod.POST;
                sdTrResult.HttpStatus = System.Net.HttpStatusCode.Forbidden;
                sdTrResult.ResourceKind = _resourceKind;
                sdTrResult.Uuid = contactEntry.UUID.ToString();

                AttachDiagnosis(sdTrResult);
                return sdTrResult;
            }


            Account account = new Account();
            Identity accIdentity = new Identity(account.EntityName, accountId);
            AccountDocument accountDocument = account.GetDocument(
                accIdentity, _emptyToken, _context.Config) as AccountDocument;
            accountDocument.CrmId = accountUuid;

            Document document = null;
            bool doAdd = false;

            document = GetTransformedDocument(entry);
            if (accountDocument.people.documents.Count == 0)
            {
                accountDocument.people.documents.Add(document);
                doAdd = true;
            }
            else
            {
                PersonDocument personDocument = accountDocument.people.documents[0] as PersonDocument;
                if (personDocument.firstname.IsNull &&
                    personDocument.lastname.IsNull)
                {
                    accountDocument.people.documents[0] = document;
                    doAdd = true;
                }

            }
            if (!doAdd)
            {
                sdTrResult = new SdataTransactionResult();
                sdTrResult.HttpMessage = "Trading Account has already a primary contact";
                sdTrResult.HttpMethod = HttpMethod.POST;
                sdTrResult.HttpStatus = System.Net.HttpStatusCode.Forbidden;
                sdTrResult.ResourceKind = _resourceKind;
                sdTrResult.Uuid = contactEntry.UUID.ToString();

                AttachDiagnosis(sdTrResult);
                return sdTrResult;
            }

            List<TransactionResult> transactionResults = new List<TransactionResult>();
            account.Update(accountDocument, _context.Config, ref transactionResults);
            sdTrResult = GetSdataTransactionResult(transactionResults,
                _context.OriginEndPoint, SupportedResourceKinds.tradingAccounts);
            if (sdTrResult != null)
            {
                sdTrResult.ResourceKind = _resourceKind;
                sdTrResult.HttpMessage = "POST";
            }
            return sdTrResult;
        }

        public String[] GetFeed()
        {

            string whereClause = string.Empty;
            OleDbParameter[] oleDbParameters = null;

            if (this is IEntityQueryWrapper) //What's dat?
            {
                QueryFilterBuilder queryFilterBuilder = new QueryFilterBuilder((IEntityQueryWrapper)this);

                queryFilterBuilder.BuildSqlStatement(_context, out whereClause, out oleDbParameters);
            }

            Token emptyToken = new Token();


            List<Identity> identities = new List<Identity>();


            identities = this._entity.GetAll(_context.Config, whereClause, oleDbParameters);


            string[] result = new string[identities.Count];
            for (int i = 0; i < identities.Count; i++)
                result[i] = identities[i].Id;

            return result;
        }

        public override SdataTransactionResult Update(FeedEntry payload)
        {
            //Transform account
            Document document = GetTransformedDocument(payload);
            AccountDocument accountDocument = (AccountDocument)_entity.GetDocumentTemplate();
            accountDocument.Id = document.Id;
            accountDocument.CrmId = GetTradingAccountUuid(document.Id);
            accountDocument.people.documents.Add(document);
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

        public override FeedEntry GetFeedEntry(string resource)
        {
            Identity id = new Identity(_entity.EntityName, resource);

            AccountDocument accountDocument = (AccountDocument)_entity.GetDocument(id, _emptyToken, _context.Config);
            if (accountDocument.LogState == LogState.Deleted)
            {
                FeedEntry e = new FeedEntry();
                e.IsDeleted = true;
                return e;
            }
            return GetTransformedPayload(accountDocument);
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


        /// <summary>
        /// Sets the SdataTransactionResult.Diagnosis Object based on the Information in sdTrResult so the consumer gets a decent Error Message
        /// </summary>
        /// <param name="sdTrResult"></param>
        private static void AttachDiagnosis(SdataTransactionResult sdTrResult)
        {
            Diagnosis diag = new Diagnosis();
            diag.Message = sdTrResult.HttpMessage;
            diag.Severity = Severity.Error;
            diag.ApplicationCode = sdTrResult.HttpStatus.ToString();
            diag.SDataCode = DiagnosisCode.ApplicationDiagnosis;
            sdTrResult.Diagnosis = diag;
        }

        string IEntityQueryWrapper.GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "id";
            if (propertyName.Equals("fullname", StringComparison.InvariantCultureIgnoreCase))
                return "ContactName";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        public override SdataTransactionResult Delete(string id)
        {
            throw new NotImplementedException();
        }

    }
}
