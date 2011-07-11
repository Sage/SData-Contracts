#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Account;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.TradingAccounts;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Integration.Northwind.Adapter.Common.Paging;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data
{

    public class ContactWrapper : EntityWrapperBase, IEntityWrapper, IEntityQueryWrapper
    {
        private ITransformation<PersonDocument, ContactPayload> _transformation;
        private string _tradingAccountUuidPayloadPath;

        public ContactWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.contacts)
        {
            _entity = new Account();
            _transformation = TransformationFactory.GetTransformation<ITransformation<PersonDocument, ContactPayload>>
                (SupportedResourceKinds.contacts, context);
            _tradingAccountUuidPayloadPath = _resourceKind.ToString() + "/" + SupportedResourceKinds.tradingAccounts.ToString();
        }


        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document result = _transformation.GetTransformedDocument(payload as ContactPayload, links);
            return result;
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            PayloadBase result = _transformation.GetTransformedPayload(document as PersonDocument,out links);
            links = Helper.ExtendPayloadPath(links, "contact");
            return result;
        }


        public override SyncFeedEntry GetFeedEntry(string id)
        {
            SyncFeedEntry result = new SyncFeedEntry();
            result.Payload = PayloadFactory.CreatePayload(_resourceKind);

            Identity identity;
            AccountDocument accountDocument;
            Account account = new Account();
            identity = GetIdentity(id);
            accountDocument = (AccountDocument)_entity.GetDocument(identity, _emptyToken, _context.Config);
            if (accountDocument.LogState == LogState.Deleted)
                return null;
            if (accountDocument.people.documents.Count == 0)
                return null;

            Document document = accountDocument.people.documents[0];
            List<SyncFeedEntryLink> links;
            result.Payload = GetTransformedPayload(document, out links);

            string taUuid = GetTradingAccountUuid(accountDocument.Id);
            if (!String.IsNullOrEmpty(taUuid))
            {
                SyncFeedEntryLink tradingAccountLink = SyncFeedEntryLink.CreateRelatedLink(
                    String.Format("{0}{1}('{2}')", _context.DatasetLink, SupportedResourceKinds.tradingAccounts.ToString(), accountDocument.Id),
                    SupportedResourceKinds.tradingAccounts.ToString(),
                     _tradingAccountUuidPayloadPath, taUuid);
                links.Add(tradingAccountLink);

            }
            result.SyncLinks = links;
            result.Id = String.Format("{0}{1}('{2}')", _context.DatasetLink, _resourceKind.ToString(), id);
            result.Title = String.Format("{0}: {1}", _resourceKind.ToString(), id);
            result.Updated = DateTime.Now;
            return result;

        }

        public override SyncFeed GetFeed()
        {
            bool includeUuid;
            string whereClause = string.Empty;
            OleDbParameter[] oleDbParameters = null;

            if (this is IEntityQueryWrapper)
            {
                QueryFilterBuilder queryFilterBuilder = new QueryFilterBuilder((IEntityQueryWrapper)this);

                queryFilterBuilder.BuildSqlStatement(_context, out whereClause, out oleDbParameters);
            }

            SyncFeed feed = new SyncFeed();

            feed.Title = _resourceKind.ToString() + ": " + DateTime.Now.ToString();

            Token emptyToken = new Token();


            List<Identity> identities = new List<Identity>();

            if (String.IsNullOrEmpty(_context.ResourceKey))
            {
                identities = _entity.GetAll(_context.Config, whereClause, oleDbParameters);
            }
            else
            {
                identities.Add(GetIdentity(_context.ResourceKey));
            }

            int totalResult = identities.Count;

            #region PAGING & OPENSEARCH

            /* PAGING */
            feed.Links = FeedMetadataHelpers.CreatePageFeedLinks(_context, totalResult, FeedMetadataHelpers.RequestKeywordType.none);

            /* OPENSEARCH */
            PageController pageController = FeedMetadataHelpers.GetPageLinkBuilder(_context, totalResult, FeedMetadataHelpers.RequestKeywordType.none);

            feed.Opensearch_ItemsPerPageElement = pageController.GetOpensearch_ItemsPerPageElement();
            feed.Opensearch_StartIndexElement = pageController.GetOpensearch_StartIndexElement();
            feed.Opensearch_TotalResultsElement = pageController.GetOpensearch_TotalResultsElement();

            #endregion


            feed.Id = _context.SdataUri.ToString();

            string tmpValue;
            // ?includeUuid
            includeUuid = false;    // default value, but check for settings now
            if (_context.SdataUri.QueryArgs.TryGetValue("includeUuid", out tmpValue))
                includeUuid = System.Xml.XmlConvert.ToBoolean(tmpValue);

            ICorrelatedResSyncInfoStore correlatedResSyncStore = null;
            if (includeUuid)
                // get store to request the correlations
                correlatedResSyncStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_context.SdataContext);

            for (int pageIndex = pageController.StartIndex; pageIndex <= pageController.LastIndex; pageIndex++)
            //for (int index = startIndex; index < startIndex + count; index++)
            {
                int zeroBasedIndex = pageIndex - 1;
                Identity identity = identities[zeroBasedIndex];
                AccountDocument accountDocument = (AccountDocument)_entity.GetDocument(identity, emptyToken, _context.Config);
                if (accountDocument.LogState == LogState.Deleted)
                    continue;


                SyncFeedEntry entry = new SyncFeedEntry();
                if (accountDocument.people.documents.Count == 0)
                    return null;

                
                entry.Id = String.Format("{0}{1}('{2}')", _context.DatasetLink, _resourceKind.ToString(), identity.Id);

                entry.Title = String.Format("{0}: {1}", _resourceKind.ToString(), identity.Id);
                entry.Updated = DateTime.Now;

                if (_context.SdataUri.Precedence == null)
                {
                    List<SyncFeedEntryLink> links;
                    Document document = accountDocument.people.documents[0];
                    entry.Payload = GetTransformedPayload(document, out links);
                    string taUuid = GetTradingAccountUuid(accountDocument.Id);
                    if (!String.IsNullOrEmpty(taUuid))
                    {
                        SyncFeedEntryLink tradingAccountLink = SyncFeedEntryLink.CreateRelatedLink(
                            String.Format("{0}{1}('{2}')", _context.DatasetLink, SupportedResourceKinds.tradingAccounts.ToString(), accountDocument.Id),
                            SupportedResourceKinds.tradingAccounts.ToString(),
                             _tradingAccountUuidPayloadPath, taUuid);
                        links.Add(tradingAccountLink);

                    }

                    entry.SyncLinks = links;
                }
                
                if (includeUuid)
                {
                    CorrelatedResSyncInfo[] infos = correlatedResSyncStore.GetByLocalId(_context.ResourceKind.ToString(), new string[] { identity.Id });
                    entry.Uuid = (infos.Length > 0) ? infos[0].ResSyncInfo.Uuid : Guid.Empty;
                }

                if(entry!=null)
                    feed.Entries.Add(entry);
            }

            return feed;
        }

        public override SdataTransactionResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Add(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            string accountUuid = "";
            SdataTransactionResult sdTrResult;
            ContactPayload contactPayload = payload as ContactPayload;

            if ((contactPayload.Contacttype.primacyIndicatorSpecified) && 
                contactPayload.Contacttype.primacyIndicator)
            {
                // is primary
            }
            else
            {
                sdTrResult = new SdataTransactionResult();
                sdTrResult.HttpMessage = "Only primary contacts suported";
                sdTrResult.HttpMethod = "POST";
                sdTrResult.HttpStatus = System.Net.HttpStatusCode.Forbidden;
                sdTrResult.ResourceKind = SupportedResourceKinds.contacts;
                sdTrResult.Uuid = StringToGuid(contactPayload.Contacttype.uuid);
                return sdTrResult;
            
            }
            foreach (SyncFeedEntryLink link in links)
            {
                if ((!String.IsNullOrEmpty(link.PayloadPath)) &&
                    link.PayloadPath.Equals(_tradingAccountUuidPayloadPath, 
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    accountUuid = link.Uuid;
                    break;
                }
            }



            if (String.IsNullOrEmpty(accountUuid))
            {
                sdTrResult = new SdataTransactionResult();
                sdTrResult.HttpMessage = "Trading Account UUID was missing";
                sdTrResult.HttpMethod = "POST";
                sdTrResult.HttpStatus = System.Net.HttpStatusCode.Forbidden;
                sdTrResult.ResourceKind = _resourceKind;
                sdTrResult.Uuid = StringToGuid(contactPayload.Contacttype.uuid);
                return sdTrResult;
            }

            
            string accountId = GetTradingAccountLocalId(accountUuid);
            
            if (String.IsNullOrEmpty(accountId))
            {
                sdTrResult = new SdataTransactionResult();
                sdTrResult.HttpMessage = String.Format("Trading Account UUID {0} was not linked", accountUuid);
                sdTrResult.HttpMethod = "POST";
                sdTrResult.HttpStatus = System.Net.HttpStatusCode.Forbidden;
                sdTrResult.ResourceKind = _resourceKind;
                sdTrResult.Uuid = StringToGuid(contactPayload.Contacttype.uuid);
                return sdTrResult;
            }


            Account account = new Account();
            Identity accIdentity = new Identity(account.EntityName, accountId);
            AccountDocument accountDocument = account.GetDocument(
                accIdentity, _emptyToken, _context.Config) as AccountDocument;

            Document document = null;
            bool doAdd = false;
            if (accountDocument.people.documents.Count == 0)
            {
                document = GetTransformedDocument(payload, links);
            accountDocument.CrmId = GetTradingAccountUuid(document.Id);
            accountDocument.addresses.documents.Add(document);
                doAdd = true;
            }
            else 
            {
                PersonDocument personDocument = accountDocument.people.documents[0] as PersonDocument;
                if (personDocument.firstname.IsNull &&
                    personDocument.lastname.IsNull)
                {
                    document = GetTransformedDocument(payload, links);
            accountDocument.CrmId = GetTradingAccountUuid(document.Id);
            accountDocument.addresses.documents[0] = document;
                doAdd = true;
                }

            }
            if (!doAdd)
            {
                sdTrResult = new SdataTransactionResult();
                sdTrResult.HttpMessage = "Trading Account has already a primary contact";
                sdTrResult.HttpMethod = "POST";
                sdTrResult.HttpStatus = System.Net.HttpStatusCode.Forbidden;
                sdTrResult.ResourceKind = _resourceKind;
                sdTrResult.Uuid = StringToGuid(contactPayload.Contacttype.uuid);
                return sdTrResult;
            }

            List<TransactionResult> transactionResults = new List<TransactionResult>();
            account.Update(accountDocument, _context.Config, ref transactionResults);
            sdTrResult = Helper.GetSdataTransactionResult(transactionResults,
                _context.OriginEndPoint, SupportedResourceKinds.tradingAccounts);
            if (sdTrResult != null)
            {
                sdTrResult.ResourceKind = _resourceKind;
                sdTrResult.HttpMessage = "POST";
            }
            return sdTrResult;
        }

        public override SdataTransactionResult Update(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            //Transform account
            Document document = GetTransformedDocument(payload, links);
            AccountDocument accountDocument = (AccountDocument)_entity.GetDocumentTemplate();
            accountDocument.Id = document.Id;
            accountDocument.CrmId = GetTradingAccountUuid(document.Id);
            accountDocument.people.documents.Add(document);
            // Update Document

            List<TransactionResult> transactionResults = new List<TransactionResult>();
            _entity.Update(accountDocument, _context.Config, ref transactionResults);
            SdataTransactionResult sdTrResult = Helper.GetSdataTransactionResult(transactionResults,
               _context.OriginEndPoint, SupportedResourceKinds.tradingAccounts);
            if (sdTrResult != null)
            {
                sdTrResult.ResourceKind = _resourceKind;

                sdTrResult.HttpMessage = "PUT";
            }
            return sdTrResult;

        }


        private Guid StringToGuid(string guid)
        {
            try
            {
                GuidConverter converter = new GuidConverter();

                Guid result = (Guid)converter.ConvertFromString(guid);
                return result;
            }
            catch
            {
                return Guid.Empty;
            }
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
        #endregion

        #region IEntityQueryWrapper Members

        string IEntityQueryWrapper.GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "id";
            if (propertyName.Equals("fullname", StringComparison.InvariantCultureIgnoreCase))
                return "ContactName";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        #endregion
    }
}
