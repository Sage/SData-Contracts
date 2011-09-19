using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Common.Syndication;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.Data.OleDb;
using System.Net;
using System.ComponentModel;
using Sage.Integration.Northwind.Sync;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Adapter;
using System.Reflection;

namespace Sage.Integration.Northwind.Adapter.Data
{
    /// <summary>
    /// This class is based on Sage.Integration.Northwind.Adapter.Data.EntityWrapperBase, but uses FeedEntry instead of ResourcePayloadContainer
    /// </summary>
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
            _correlatedResSyncInfoStore = NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_context.SdataContext);
        }

        #endregion


        public EntityBase Entity
        {
            get { return _entity; }
        }

        public abstract Document GetTransformedDocument(FeedEntry payload);

        public abstract FeedEntry GetTransformedPayload(Document document);

        public virtual FeedEntry GetSyncSourceFeedEntry(CorrelatedResSyncInfo resSyncInfo)
        {
            FeedEntry result = GetFeedEntry(resSyncInfo.LocalId);
            if (result == null)
                return null;


            result.UUID = resSyncInfo.ResSyncInfo.Uuid;
            result.Key = resSyncInfo.LocalId;

            return result;


        }

        public virtual FeedEntry GetSyncTargetFeedEntry(SdataTransactionResult transactionResult)
        {
            FeedEntry result;

            if (!String.IsNullOrEmpty(transactionResult.LocalId))
                result = GetFeedEntry(transactionResult.LocalId);
            else
                //result = PayloadFactory.CreateResourcePayload(this._resourceKind);
#warning The Feed Entry should have a type...
                result = new FeedEntry();

            if (!String.IsNullOrEmpty(transactionResult.Uuid))
                result.UUID = new Guid(transactionResult.Uuid);

            if (transactionResult.HttpMethod == HttpMethod.DELETE)
                result.IsDeleted = true;

            return result;

        }

        public virtual Identity GetIdentity(string Id)
        {
            return new Identity(_entity.EntityName, Id);
        }

        public virtual FeedEntry GetFeedEntry(string id)
        {
            FeedEntry result;

            Identity identity;
            Document document;

            identity = GetIdentity(id);
            document = _entity.GetDocument(identity, _emptyToken, _context.Config);
            if (document.LogState == LogState.Deleted)
            {
#warning The empty Feed Entry should have a type...
                result = getNewFeedEntry();
                result.IsDeleted = true;
                result.Key= id;
                CorrelatedResSyncInfo[] corr = _correlatedResSyncInfoStore.GetByLocalId(_resourceKind.ToString(), new string[] { id });
                if ((corr != null) && (corr.Length > 0))
                {
                    result.UUID = corr[0].ResSyncInfo.Uuid;
                }
                return result;
            }
            result = GetTransformedPayload(document);

            return result;

        }

        private FeedEntry getNewFeedEntry()
        {
           
            switch (_resourceKind)
            {
                case SupportedResourceKinds.commodities:
                    return new Feeds.CommodityFeedEntry();
                case SupportedResourceKinds.commodityGroups:
                    return new Feeds.CommodityGroupFeedEntry();
                case SupportedResourceKinds.contacts:
                    return new Feeds.ContactFeedEntry();
                case SupportedResourceKinds.emails:
                    return new Feeds.EmailFeedEntry();
                case SupportedResourceKinds.phoneNumbers:
                    return new Feeds.PhoneNumberFeedEntry();
                case SupportedResourceKinds.postalAddresses:
                    return new Feeds.PostalAddressFeedEntry();
                case SupportedResourceKinds.priceLists:
                    return new Feeds.PriceListFeedEntry();
                case SupportedResourceKinds.prices:
                    return new Feeds.PriceFeedEntry();
                case SupportedResourceKinds.salesOrderLines:
                    return new Feeds.SalesOrderLineFeedEntry();
                case SupportedResourceKinds.salesOrders:
                    return new Feeds.SalesOrderFeedEntry();
                case SupportedResourceKinds.tradingAccounts:
                    return new Feeds.TradingAccountFeedEntry();
                case SupportedResourceKinds.unitsOfMeasure:
                    return new Feeds.UnitOfMeasureFeedEntry();
            }
            return new FeedEntry() ;
        }

        public virtual String[] GetFeed()
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

            string[] result = new string[identities.Count];
            for (int i = 0; i < identities.Count; i++)
                result[i] = identities[i].Id;

            return result;
        }

        public virtual SdataTransactionResult Delete(string id)
        {
            Identity identity = GetIdentity(id);

            List<TransactionResult> transactionResults = new List<TransactionResult>();

            Document document = _entity.GetDocumentTemplate();
            document.Id = id;
            document.LogState = LogState.Deleted;

            _entity.Delete(document, _context.Config, ref transactionResults);

            SdataTransactionResult sdTrResult = GetSdataTransactionResult(transactionResults, _context.OriginEndPoint, _resourceKind);

            return sdTrResult;
        }

        public virtual SdataTransactionResult Add(FeedEntry payload)
        {
            Document document = GetTransformedDocument(payload);
            List<TransactionResult> transactionResults = new List<TransactionResult>();

            _entity.Add(document, _context.Config, ref transactionResults);

            SdataTransactionResult sdTrResult = GetSdataTransactionResult(transactionResults,
                _context.OriginEndPoint, _resourceKind);
            if (sdTrResult != null)
                sdTrResult.HttpMethod = HttpMethod.POST;
            return sdTrResult;
        }

        public virtual FeedEntry Merge(FeedEntry sourceEntry)
        {
            FeedEntry targetEntry = GetFeedEntry(sourceEntry.Key);

            string[] changedProperties = sourceEntry.GetChangedProperties();
            foreach (string propName in changedProperties)
            {
                PropertyInfo propInfo = targetEntry.GetType().GetProperty(propName);
                if(propInfo != null && propInfo.CanWrite)
                {
                    object value = propInfo.GetValue(sourceEntry, null);
                    propInfo.SetValue(targetEntry, value, null);
                }
            }
            return targetEntry;
        }
        
        public virtual SdataTransactionResult Update(FeedEntry payload)
        {
            //Transform account
            Document document = GetTransformedDocument(payload);
            // Update Document

            List<TransactionResult> transactionResults = new List<TransactionResult>();
            _entity.Update(document, _context.Config, ref transactionResults);
            SdataTransactionResult sdTrResult = GetSdataTransactionResult(transactionResults,
                _context.OriginEndPoint, _resourceKind);
            if (sdTrResult != null)
                sdTrResult.HttpMethod = HttpMethod.PUT;

            return sdTrResult;

        }

        internal SdataTransactionResult GetSdataTransactionResult(
            List<TransactionResult> transactions, string EndPoint, SupportedResourceKinds resource)
        {
            SdataTransactionResult result;
            // create default result
            result = new SdataTransactionResult();
            result.ResourceKind = resource;
            result.HttpStatus = HttpStatusCode.BadRequest;


            if (transactions == null)
                return null;
            if (transactions.Count == 0)
                return null;

            PersistRelations(transactions);

            foreach (TransactionResult transaction in transactions)
            {
                SupportedResourceKinds res = GetSupportedResourceKind(transaction.EntityName);
                if (res == resource)
                {
                    result.ResourceKind = resource;
                    result.LocalId = transaction.ID;
                    result.Uuid = transaction.CRMID;
                    result.HttpMessage = transaction.Message;
                    result.Location = EndPoint + "('" + transaction.ID + "')";
                    result.HttpStatus = Helper.GetHttpStatus(transaction.Status, transaction.Action);
                    result.HttpMethod = Helper.GetHttpMethod(transaction.Action);

                }

            }

            foreach (TransactionResult transaction in transactions)
            {
                if (transaction.Status != TransactionStatus.Success)
                {
                    result = new SdataTransactionResult();
                    result.ResourceKind = resource;
                    result.HttpMessage = transaction.Message;
                    result.HttpStatus = Helper.GetHttpStatus(transaction.Status, transaction.Action);
                    return result;
                }
            }

            return result;

        }

        private void PersistRelations(List<TransactionResult> transactions)
        {
            GuidConverter guidConverter = new GuidConverter();
            foreach (TransactionResult transaction in transactions)
            {
                if (transaction.Status == TransactionStatus.Success)
                {
                    SupportedResourceKinds resourceKind = GetSupportedResourceKind(transaction.EntityName);
                    if (resourceKind == SupportedResourceKinds.None)
                        continue;

                    string uuidString = transaction.CRMID;
                    Guid uuid = Guid.Empty;
                    try
                    {
                        uuid = (Guid)guidConverter.ConvertFromString(uuidString);
                    }
                    catch
                    {
                        continue;
                    }
                    if (uuid == Guid.Empty)
                        continue;

                    string localId = transaction.ID;
                    ResSyncInfo resSyncInfo = new ResSyncInfo(uuid, _context.DatasetLink + resourceKind.ToString(), -1, "", DateTime.Now);

                    CorrelatedResSyncInfo correlatedResSyncInfo = new CorrelatedResSyncInfo(localId, resSyncInfo);

                    // store the new correlation to the sync store
                    _correlatedResSyncInfoStore.Put(resourceKind.ToString(), correlatedResSyncInfo);

                }
            }
        }


        private SupportedResourceKinds GetSupportedResourceKind(string entityName)
        {
            switch (entityName)
            {

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.Account:
                    return SupportedResourceKinds.tradingAccounts;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.Product:
                    return SupportedResourceKinds.commodities;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.ProductFamily:
                    return SupportedResourceKinds.commodityGroups;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.Person:
                    return SupportedResourceKinds.contacts;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.Email:
                    return SupportedResourceKinds.emails;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.Phone:
                    return SupportedResourceKinds.phoneNumbers;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.Address:
                    return SupportedResourceKinds.postalAddresses;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.PricingList:
                    return SupportedResourceKinds.priceLists;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.Price:
                    return SupportedResourceKinds.prices;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.Order:
                    return SupportedResourceKinds.salesOrders;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.LineItem:
                    return SupportedResourceKinds.salesOrderLines;

                case Sage.Integration.Northwind.Application.API.Constants.EntityNames.UnitOfMeasure:
                    return SupportedResourceKinds.unitsOfMeasure;

                default:
                    return SupportedResourceKinds.None;
            }
        }


    }
}
