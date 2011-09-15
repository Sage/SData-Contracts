using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Application.Entities.Order;
using Sage.Integration.Northwind.Adapter.Common;
using System.Data.OleDb;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using System.ComponentModel;
using System.Data;
using System.Collections;
using Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters;
using Sage.Integration.Northwind.Application.Entities.Product;
using Sage.Integration.Northwind.Adapter.Transform;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;

namespace Sage.Integration.Northwind.Adapter.Data
{
    class SalesInvoiceFeedEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        PriceTransform _transform;

        public SalesInvoiceFeedEntryWrapper(RequestContext context)
            : base(context, Adapter.Common.SupportedResourceKinds.salesInvoices)
        {
            _entity = new Order();      
        }


        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            throw new NotImplementedException();
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Add(Sage.Common.Syndication.FeedEntry payload)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Update(Sage.Common.Syndication.FeedEntry payload)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Delete(string id)
        {
            throw new NotImplementedException();
        }

        public override Sage.Common.Syndication.FeedEntry GetFeedEntry(string idString)
        {
            #region declarations
            int recordCount;
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order order = new Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order();
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
                recordCount = tableAdapter.FillBy(order.CalculatedOrders, id, id);
                if (recordCount == 0)
                    return null;

                detailTableAdapter.Connection = connection;
                detailTableAdapter.FillBy(order.CalculatedOrderDetails, id);

                //deletedDetailTableAdapter.Connection = connection;
                //deletedDetailTableAdapter.Fill(order.DeletedOrderDetails, id.ToString(), lastToken.SequenceNumber, config.CrmUser);
            }
            #endregion


            SalesInvoiceFeedEntry entry;

            entry = GetPayload((Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrdersRow)order.CalculatedOrders[0],
                order.CalculatedOrderDetails,
                //order.DeletedOrderDetails,
                 _context.Config);

            return entry;
        }

        private SalesInvoiceFeedEntry GetPayload(Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrdersRow row,
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrderDetailsDataTable detailDataTable,
            //DataSets.Order.DeletedOrderDetailsDataTable deletedOrderDetailsDataTable,
            NorthwindConfig config)
        {
            #region Declarations
            SalesInvoiceFeedEntry payload;
            string id;
            CountryCodes countryCodes = new CountryCodes();
            #endregion

            id = row.OrderID.ToString();

            payload = new SalesInvoiceFeedEntry();
            payload.Key = id;
            payload.Id = GetSDataId(id);
            payload.UUID = GetUuid(id, "", SupportedResourceKinds.salesInvoices);
            payload.active = true;


            payload.currency = config.CurrencyCode;

            payload.pricelist = new PriceListFeedEntry();
            //TODO: Add Id?
            /*payload.pricelist = (PriceListPayload)PayloadFactory.CreateResourcePayload(
                SupportedResourceKinds.priceLists,
                Sage.Integration.Northwind.Application.API.Constants.DefaultValues.PriceList.ID, _context.DatasetLink, true);*/
            payload.pricelist.UUID = GetUuid(id, "", SupportedResourceKinds.priceLists);



            if (!row.IsCustomerIDNull())
            {
                /*payload.tradingAccount = (TradingAccountPayload)PayloadFactory.CreateResourcePayload(
                SupportedResourceKinds.tradingAccounts, Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix +
                row.CustomerID, _context.DatasetLink, true);*/
                payload.tradingAccount = new TradingAccountFeedEntry();
                payload.tradingAccount.Key = Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix + row.CustomerID;
                payload.tradingAccount.Id = (GetSDataId(payload.tradingAccount.Key, SupportedResourceKinds.tradingAccounts));
                payload.tradingAccount.UUID = GetUuid(payload.tradingAccount.Key, "", SupportedResourceKinds.tradingAccounts);
            }

            if (!row.IsOrderDateNull())
            {
                payload.date = row.OrderDate;
            }

            payload.discountTotal = row.IsDiscountAmountNull() ? new decimal(0) : Convert.ToDecimal(row.DiscountAmount);

            payload.netTotal = row.IsTotalNetPriceNull() ? new decimal(0) : Convert.ToDecimal(row.TotalNetPrice);

            payload.carrierTotalPrice = row.IsFreightNull() ? new decimal(0) : row.Freight;

            payload.grossTotal = payload.netTotal;



            if (!row.IsRequiredDateNull())
            {
                payload.deliveryDate = row.RequiredDate;
            }



            if (!row.IsShipViaNull())
            {
                payload.deliveryMethod = row.ShipVia.ToString(); ;
            }

            PostalAddressFeedEntry address = new PostalAddressFeedEntry();
            address.Id = GetSDataId(id, SupportedResourceKinds.postalAddresses);
            address.Key = id;
            address.active = true;
            address.address1 = row.IsShipAddressNull() ? "" : row.ShipAddress;
            address.country = row.IsShipCountryNull() ? "" : row.ShipCountry;
            address.townCity = row.IsShipCityNull() ? "" : row.ShipCity;
            address.zipPostCode = row.IsShipPostalCodeNull() ? "" : row.ShipPostalCode;
            address.type = postalAddressTypeenum.Shipping;

            payload.postalAddresses = new PostalAddressFeed();
            payload.postalAddresses.Entries.Add(address);


            payload.salesInvoiceLines = new SalesInvoiceLineFeed();
            foreach (Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrderDetailsRow detailRow in detailDataTable.Rows)
            {
                SalesInvoiceLineFeedEntry soPayload = GetLineItem(detailRow, config);
                soPayload.salesInvoice = payload;
                payload.salesInvoiceLines.Entries.Add(soPayload);
            }

            return payload;

        }

        private SalesInvoiceLineFeedEntry GetLineItem(Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrderDetailsRow row, NorthwindConfig config)
        {
            #region Declarations
            SalesInvoiceLineFeedEntry payload;
            string id;
            decimal discountPercentage;
            #endregion

            id = row.OrderID.ToString() + "-" + row.ProductID.ToString();

            payload = new SalesInvoiceLineFeedEntry();
            payload.Key = id;
            payload.Id = GetSDataId(payload.Key, SupportedResourceKinds.salesInvoiceLines);
            payload.UUID = GetUuid(id, "", SupportedResourceKinds.salesInvoiceLines);

            /*payload.commodity = (CommodityPayload)PayloadFactory.CreateResourcePayload(
                SupportedResourceKinds.commodities, row.ProductID.ToString(), _context.DatasetLink, true);*/
            payload.commodity = new CommodityFeedEntry();
            payload.commodity.Key = row.ProductID.ToString();
            payload.commodity.Id = GetSDataId(payload.commodity.Key, SupportedResourceKinds.commodities);
            
            payload.commodity.UUID = GetUuid(row.ProductID.ToString(), "", SupportedResourceKinds.commodities);

            //payload.SalesInvoiceLinetype.salesInvoice = (SalesInvoicePayload)PayloadFactory.CreateResourcePayload(
            //   SupportedResourceKinds.salesInvoices, row.OrderID.ToString(), _context.DatasetLink, true);
            //payload.SalesInvoiceLinetype.salesInvoice.Uuid = GetUuid(row.OrderID.ToString(), "", SupportedResourceKinds.salesInvoices);

            /*payload.unitOfMeasure = (UnitOfMeasurePayload)PayloadFactory.CreateResourcePayload(
               SupportedResourceKinds.unitsOfMeasure, row.ProductID.ToString(), _context.DatasetLink, true);*/
            payload.unitOfMeasure = new UnitOfMeasureFeedEntry();
            payload.unitOfMeasure.Key = row.ProductID.ToString();
            payload.unitOfMeasure.Id = GetSDataId(payload.unitOfMeasure.Key, SupportedResourceKinds.unitsOfMeasure);
            payload.unitOfMeasure.UUID = GetUuid(row.ProductID.ToString(), "", SupportedResourceKinds.unitsOfMeasure);

            payload.quantity = row.IsQuantityNull() ? Convert.ToInt16(0) : row.Quantity;

            payload.initialPrice = row.IsUnitPriceNull() ? new decimal(0) : row.UnitPrice;

            payload.invoiceLineDiscountPercent = row.IsDiscountNull() ? (decimal)0 : Convert.ToDecimal(row.Discount);

            payload.discountTotal = (decimal)payload.initialPrice * (decimal)payload.invoiceLineDiscountPercent;

            payload.costTotal = (decimal)payload.initialPrice * (1 - payload.invoiceLineDiscountPercent);

            payload.netTotal = Convert.ToDecimal(payload.quantity) * Convert.ToDecimal(payload.costTotal);

            return payload;

        }

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "OrderID";
            if (propertyName.Equals("tradingaccount", StringComparison.InvariantCultureIgnoreCase))
                return "CustomerID";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }


        #region helper
        protected string GetSDataId(string id)
        {
            return String.Format("{0}{1}('{2}')", _context.DatasetLink, _resourceKind.ToString(), id);
        }

        protected string GetSDataId(string id, SupportedResourceKinds resourceKind)
        {
            return String.Format("{0}{1}('{2}')", _context.DatasetLink, resourceKind.ToString(), id);
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
