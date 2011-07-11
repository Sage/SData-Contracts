#region Usings

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Common.Paging;
using Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Order;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;


#endregion

namespace Sage.Integration.Northwind.Adapter.Data.SalesOrders
{
    public class SalesInvoicesWrapper : EntityWrapperBase, IEntityWrapper, IEntityQueryWrapper
    {
                #region Ctor.

        public SalesInvoicesWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.salesInvoices)
        {
            _entity = new Order();
            
        }

        #endregion
        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            throw new NotImplementedException();
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Add(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Update(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override SyncFeedEntry GetFeedEntry(string idString)
        {
            #region declarations
            int recordCount;
            DataSets.Order order = new DataSets.Order();
            CalculatedOrdersTableAdapter tableAdapter;
            tableAdapter = new CalculatedOrdersTableAdapter();
            CalculatedOrderDetailsTableAdapter detailTableAdapter;
            detailTableAdapter = new CalculatedOrderDetailsTableAdapter();
            //DeletedOrderDetailsTableAdapter deletedDetailTableAdapter;
            //deletedDetailTableAdapter = new DeletedOrderDetailsTableAdapter();


            int id;
            if (!(Int32.TryParse(idString, out id)))
                id = 0;
            #endregion


            #region fill dataset
            using (OleDbConnection connection = new OleDbConnection(_context.Config.ConnectionString))
            {
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(order.CalculatedOrders, id);
                if (recordCount == 0)
                    return null;

                detailTableAdapter.Connection = connection;
                detailTableAdapter.FillBy(order.CalculatedOrderDetails, id);

                //deletedDetailTableAdapter.Connection = connection;
                //deletedDetailTableAdapter.Fill(order.DeletedOrderDetails, id.ToString(), lastToken.SequenceNumber, config.CrmUser);
            }
            #endregion


            SyncFeedEntry entry = new SyncFeedEntry();
            entry.Id = String.Format("{0}{1}('{2}')", _context.DatasetLink, _resourceKind.ToString(), idString);

            entry.Title = String.Format("{0}: {1}", _resourceKind.ToString(), idString);
            entry.Updated = DateTime.Now;

            entry.Payload = GetPayload((DataSets.Order.CalculatedOrdersRow)order.CalculatedOrders[0],
                order.CalculatedOrderDetails,
                //order.DeletedOrderDetails,
                 _context.Config);

            entry.SyncLinks.AddRange(GetLinks(entry.Payload.ForeignIds));

            return entry;

        }



        private SalesInvoicePayload GetPayload(DataSets.Order.CalculatedOrdersRow row,
            DataSets.Order.CalculatedOrderDetailsDataTable detailDataTable,
            //DataSets.Order.DeletedOrderDetailsDataTable deletedOrderDetailsDataTable,
            NorthwindConfig config)
        {
            #region Declarations
            SalesInvoicePayload payload;
            string id;
            CountryCodes countryCodes = new CountryCodes();
            #endregion

            id = row.OrderID.ToString();

            payload = new SalesInvoicePayload();
            payload.LocalID = id;
            payload.SyncUuid = GetUuid(id, "", SupportedResourceKinds.salesOrders);
            payload.SalesInvoicetype.active = true;
            payload.SalesInvoicetype.applicationID = id;

            payload.SalesInvoicetype.currency = config.CurrencyCode;

            payload.ForeignIds.Add("pricelist", Sage.Integration.Northwind.Application.API.Constants.DefaultValues.PriceList.ID);


            if (!row.IsCustomerIDNull())
                payload.ForeignIds.Add("tradingAccount", Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix + row.CustomerID);


            if (!row.IsOrderDateNull())
            {
                payload.SalesInvoicetype.date = row.OrderDate;
                payload.SalesInvoicetype.dateSpecified = true;
            }

            payload.SalesInvoicetype.lineCountSpecified = true;
            payload.SalesInvoicetype.lineCount = detailDataTable.Rows.Count;

            payload.SalesInvoicetype.discountTotalSpecified = true;
            payload.SalesInvoicetype.discountTotal = row.IsDiscountAmountNull() ? new decimal(0) : Convert.ToDecimal(row.DiscountAmount);

            payload.SalesInvoicetype.netTotalSpecified = true;
            payload.SalesInvoicetype.netTotal = row.IsTotalNetPriceNull() ? new decimal(0) : Convert.ToDecimal(row.TotalNetPrice);

            payload.SalesInvoicetype.carrierTotalPriceSpecified = true;
            payload.SalesInvoicetype.carrierTotalPrice = row.IsFreightNull() ? new decimal(0) : row.Freight;

            payload.SalesInvoicetype.grossTotalSpecified = true;
            payload.SalesInvoicetype.grossTotal = payload.SalesInvoicetype.netTotal;



            if (!row.IsRequiredDateNull())
            {
                payload.SalesInvoicetype.deliveryDateSpecified = true;
                payload.SalesInvoicetype.deliveryDate = row.RequiredDate;
            }



            if (!row.IsShipViaNull())
            {
                payload.SalesInvoicetype.deliveryMethod = row.ShipVia.ToString(); ;
            }

            postalAddresstype address = new postalAddresstype();
            address.active = true;
            address.activeSpecified = true;
            address.address1 = row.IsShipAddressNull() ? "" : row.ShipAddress;
            address.applicationID = id;
            address.country = row.IsShipCountryNull() ? "" : row.ShipCountry;
            address.townCity = row.IsShipCityNull() ? "" : row.ShipCity;
            address.zipPostCode = row.IsShipPostalCodeNull() ? "" : row.ShipPostalCode;
            address.type = postalAddressTypeenum.Shipping;

            payload.SalesInvoicetype.postalAddresses = new postalAddresstype[1];
            payload.SalesInvoicetype.postalAddresses[0] = address;


            payload.SalesInvoicetype.salesInvoiceLines = new salesInvoiceLinetype[detailDataTable.Rows.Count];
            int index = 0;
            foreach (DataSets.Order.CalculatedOrderDetailsRow detailRow in detailDataTable.Rows)
            {
                SalesInvoiceLinePayload  soPayload = GetLineItem(detailRow, config);
                payload.ForeignIds.Add(
                            String.Format("salesInvoiceLines[{0}]",
                            index.ToString()),
                            soPayload.LocalID);
                foreach (string key in soPayload.ForeignIds.Keys)//  (int foreignIdIndex = 0; foreignIdIndex <= soPayload.ForeignIds.Count; foreignIdIndex++)
                {
                    string value;
                    if (soPayload.ForeignIds.TryGetValue(key, out value))
                    {
                        payload.ForeignIds.Add(
                            String.Format("salesInvoiceLines[{0}]/{1}",
                            index.ToString(),
                            key),
                            value);
                    }
                }
                payload.SalesInvoicetype.salesInvoiceLines[index] = soPayload.SalesInvoiceLinetype;
                index++;
            }

            //foreach (DataSets.Order.DeletedOrderDetailsRow deletedRow in deletedOrderDetailsDataTable.Rows)
            //{
            //    lineItemDoc = new LineItemDocument();
            //    lineItemDoc.Id = deletedRow[0].ToString();
            //    lineItemDoc.LogState = LogState.Deleted;
            //    doc.orderitems.Add(lineItemDoc);
            //}



            return payload;

        }

        private SalesInvoiceLinePayload GetLineItem(DataSets.Order.CalculatedOrderDetailsRow row, NorthwindConfig config)
        {
            #region Declarations
            SalesInvoiceLinePayload payload;
            string id;
            decimal discountPercentage;
            #endregion



            id = row.OrderID.ToString() + "-" + row.ProductID.ToString();

            payload = new SalesInvoiceLinePayload();
            payload.LocalID = id;
            //payload.SyncUuid = GetUuid(id, "", SupportedResourceKinds.salesOrderLines);
            payload.SalesInvoiceLinetype .applicationID = id;

            payload.ForeignIds.Add("commodity", row.ProductID.ToString());
            payload.ForeignIds.Add("salesOrder", row.OrderID.ToString());
            payload.ForeignIds.Add("unitOfMeasure", row.ProductID.ToString());

            payload.SalesInvoiceLinetype.quantitySpecified = true;
            payload.SalesInvoiceLinetype.quantity = row.IsQuantityNull() ? Convert.ToInt16(0) : row.Quantity;

            payload.SalesInvoiceLinetype.initialPriceSpecified = true;
            payload.SalesInvoiceLinetype.initialPrice = row.IsUnitPriceNull() ? new decimal(0) : row.UnitPrice;

            payload.SalesInvoiceLinetype.invoiceLineDiscountPercentSpecified = true;
            payload.SalesInvoiceLinetype.invoiceLineDiscountPercent = row.IsDiscountNull() ? (decimal)0 : Convert.ToDecimal(row.Discount);

            payload.SalesInvoiceLinetype.discountTotalSpecified = true;
            payload.SalesInvoiceLinetype.discountTotal = (decimal)payload.SalesInvoiceLinetype.initialPrice * (decimal)payload.SalesInvoiceLinetype.invoiceLineDiscountPercent;

            payload.SalesInvoiceLinetype.costTotalSpecified = true;
            payload.SalesInvoiceLinetype.costTotal = (decimal)payload.SalesInvoiceLinetype.initialPrice * (1 - payload.SalesInvoiceLinetype.invoiceLineDiscountPercent);



            payload.SalesInvoiceLinetype.netTotalSpecified = true;
            payload.SalesInvoiceLinetype.netTotal = Convert.ToDecimal(payload.SalesInvoiceLinetype.quantity) * Convert.ToDecimal(payload.SalesInvoiceLinetype.costTotal);

            return payload;

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
                SyncFeedEntry entry;
                if (_context.SdataUri.Precedence == null)
                {
                    entry = GetFeedEntry(identity.Id);
                }
                else
                {
                    entry = new SyncFeedEntry();
                }

                entry.Id = String.Format("{0}{1}('{2}')", _context.DatasetLink, _resourceKind.ToString(), identity.Id);

                entry.Title = String.Format("{0}: {1}", _resourceKind.ToString(), identity.Id);
                entry.Updated = DateTime.Now;

                // warning add links

                if (includeUuid)
                {
                    CorrelatedResSyncInfo[] infos = correlatedResSyncStore.GetByLocalId(_context.ResourceKind.ToString(), new string[] { identity.Id });
                    entry.Uuid = (infos.Length > 0) ? infos[0].ResSyncInfo.Uuid : Guid.Empty;
                }

                if (entry != null)
                    feed.Entries.Add(entry);
            }

            return feed;
        }


        #endregion

        #region IEntityQueryWrapper Members

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "OrderID";
            if (propertyName.Equals("tradingaccount", StringComparison.InvariantCultureIgnoreCase))
                return "CustomerID";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        #endregion


        #region helper

        private List<SyncFeedEntryLink> GetLinks(Dictionary<string, string> foreignIds)
        {
            List<SyncFeedEntryLink> result = new List<SyncFeedEntryLink>();
            foreach (string key in foreignIds.Keys)
            {
                string value;
                if (foreignIds.TryGetValue(key, out value))
                {
                    SupportedResourceKinds tmpResKind = ResourceKindHelpers.GetResourceKind(key);
                    Guid guid = GetUuid(value, "", tmpResKind);
                    SyncFeedEntryLink link = SyncFeedEntryLink.CreateRelatedLink(
                        Common.ResourceKindHelpers.GetSingleResourceUrl(_context.DatasetLink, key, value),
                        tmpResKind.ToString(),
                        key, guid.ToString());
                    result.Add(link);
                }
            }
            return result;
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

        private Guid GetUuid(string localId, string uuidString, SupportedResourceKinds resKind)
        {
            if (String.IsNullOrEmpty(localId))
            {
                return Guid.Empty;
            }

            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByLocalId(resKind.ToString(),
                new string[] { localId });
            if (results.Length > 0)
                return results[0].ResSyncInfo.Uuid;
            Guid result;
            if (string.IsNullOrEmpty(uuidString))
                result = Guid.NewGuid();
            else
                try
                {
                    GuidConverter converter = new GuidConverter();
                    result = (Guid)converter.ConvertFromString(uuidString);
                    if (Guid.Empty.Equals(result))
                        result = Guid.NewGuid();
                }
                catch (Exception)
                {
                    result = Guid.NewGuid();
                }

            ResSyncInfo newResSyncInfo = new ResSyncInfo(result, _context.DatasetLink + resKind.ToString(), 0, string.Empty, DateTime.Now);
            CorrelatedResSyncInfo newCorrelation = new CorrelatedResSyncInfo(localId, newResSyncInfo);
            _correlatedResSyncInfoStore.Put(resKind.ToString(), newCorrelation);
            return result;

        }

        private string GetLocalId(string uuidString, SupportedResourceKinds resKind)
        {
            GuidConverter converter = new GuidConverter();
            try
            {
                Guid uuid = (Guid)converter.ConvertFromString(uuidString);
                return GetLocalId(uuid, resKind);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string GetLocalId(Guid uuid, SupportedResourceKinds resKind)
        {
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByUuid(resKind.ToString(), new Guid[] { uuid });
            if (results.Length > 0)
                return results[0].LocalId;
            return string.Empty;
        }

        #endregion
    }
}
