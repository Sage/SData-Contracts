#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Feeds;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Integration.Northwind.Adapter.Common.Paging;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data
{
    public abstract class EntityWrapperBase
    {
        protected RequestContext _context;
        protected SupportedResourceKinds _resourceKind;
        protected Token _emptyToken;
        protected EntityBase _entity;
        protected ICorrelatedResSyncInfoStore _correlatedResSyncInfoStore;


        #region Ctor.

        public EntityWrapperBase(RequestContext context, SupportedResourceKinds resourceKind)
        {
            _context = context;
            _resourceKind = resourceKind;
            _emptyToken = new Token();
            _correlatedResSyncInfoStore = RequestReceiver.NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_context.SdataContext);
        }
        
        #endregion


        public EntityBase Entity
        {
            get { return _entity; }
        }

        public abstract Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links);
        public abstract PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links);

        public virtual SyncFeedEntry GetFeedEntry(CorrelatedResSyncInfo resSyncInfo)
        {
            SyncFeedEntry result = GetFeedEntry(resSyncInfo.LocalId);
            if (result == null)
                return null;


            result.Uuid = resSyncInfo.ResSyncInfo.Uuid;
            result.HttpETag = resSyncInfo.ResSyncInfo.Etag;
            result.HttpMethod = "PUT";
            result.SyncState.Endpoint = resSyncInfo.ResSyncInfo.Endpoint;
            result.SyncState.Tick = resSyncInfo.ResSyncInfo.Tick;
            result.SyncState.Stamp = resSyncInfo.ResSyncInfo.ModifiedStamp;

            return result;
 

        }

        public virtual SyncFeedEntry GetFeedEntry(SdataTransactionResult transactionResult)
        {
            SyncFeedEntry result;
            if (!String.IsNullOrEmpty(transactionResult.LocalId))
            {
                result = GetFeedEntry(transactionResult.LocalId);
            }
            else
            {
                result = new SyncFeedEntry();
                result.Uuid = transactionResult.Uuid;
            }
            if (result == null)
                return null;

            //entry.Uuid = transactionResult.Uuid;

            result.HttpStatusCode = transactionResult.HttpStatus;
            result.HttpMessage = transactionResult.HttpMessage; ;
            result.HttpMethod = transactionResult.HttpMethod;
            result.HttpLocation = transactionResult.Location;
            result.HttpETag = transactionResult.Etag;
            return result;

        }

        public virtual Identity GetIdentity(string Id)
        {
            return new Identity(_entity.EntityName, Id);
        }

        public virtual SyncFeedEntry GetFeedEntry(string id)
        {
            SyncFeedEntry result = new SyncFeedEntry();
            result.Payload = PayloadFactory.CreatePayload(_resourceKind);

            Identity identity;
            Document document;

            identity = GetIdentity(id);
            document = _entity.GetDocument(identity, _emptyToken, _context.Config);
            if (document.LogState == LogState.Deleted)
                return null;
            List<SyncFeedEntryLink> links;
            result.Payload = GetTransformedPayload(document, out links);
            result.Id = String.Format("{0}{1}('{2}')", _context.DatasetLink, _resourceKind.ToString(), id);
            result.Title = String.Format("{0}: {1}", _resourceKind.ToString(), id);
            result.Updated = DateTime.Now;
            result.SyncLinks = links;
            return result;

        }

        public virtual SyncFeed GetFeed()
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
                identities = _entity.GetAll(_context.Config, whereClause, oleDbParameters);
            else
                identities.Add(GetIdentity(_context.ResourceKey));


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
            {
                int zeroBasedIndex = pageIndex - 1;
                Identity identity = identities[zeroBasedIndex];
                Document document = _entity.GetDocument(identity, emptyToken, _context.Config);
                if (document.LogState == LogState.Deleted)
                    continue;
                SyncFeedEntry entry = new SyncFeedEntry();

                entry.Id = String.Format("{0}{1}('{2}')", _context.DatasetLink, _resourceKind.ToString(), identity.Id);

                entry.Title = String.Format("{0}: {1}", _resourceKind.ToString(), identity.Id);
                entry.Updated = DateTime.Now;

                if (_context.SdataUri.Precedence == null)
                {
                    List<SyncFeedEntryLink> links;
                    entry.Payload = GetTransformedPayload(document, out links);
                    entry.SyncLinks = links;
                }

                if (includeUuid)
                {
                    CorrelatedResSyncInfo[] infos = correlatedResSyncStore.GetByLocalId(_context.ResourceKind.ToString(), new string[] { identity.Id });
                    entry.Uuid = (infos.Length > 0) ? infos[0].ResSyncInfo.Uuid : Guid.Empty;
                }
                feed.Entries.Add(entry);
            }

            return feed;
        }

        public virtual SdataTransactionResult Delete(string id)
        {
            Identity identity = GetIdentity(id);
            
            List<TransactionResult> transactionResults = new List<TransactionResult>();
            Document document = _entity.GetDocumentTemplate();
            document.Id = id;
            document.LogState = LogState.Deleted;

            _entity.Delete(document, _context.Config, ref transactionResults);

            SdataTransactionResult sdTrResult = Helper.GetSdataTransactionResult(transactionResults, _context.OriginEndPoint, _resourceKind);

            return sdTrResult;
        }

        public virtual SdataTransactionResult Add(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document document = GetTransformedDocument(payload, links);
            List<TransactionResult> transactionResults = new List<TransactionResult>();

            _entity.Add(document, _context.Config, ref transactionResults);

            SdataTransactionResult sdTrResult = Helper.GetSdataTransactionResult(transactionResults,
                _context.OriginEndPoint, _resourceKind);
            if (sdTrResult!= null)
            sdTrResult.HttpMessage = "POST";
            return sdTrResult;
        }

        public virtual SdataTransactionResult Update(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            //Transform account
            Document document = GetTransformedDocument(payload, links);

            // Update Document

            List<TransactionResult> transactionResults = new List<TransactionResult>();
            _entity.Update(document, _context.Config, ref transactionResults);
            SdataTransactionResult sdTrResult = Helper.GetSdataTransactionResult(transactionResults,
                _context.OriginEndPoint, _resourceKind);
            if (sdTrResult != null)
            sdTrResult.HttpMessage = "PUT";

            return sdTrResult;

        }
    }
}
