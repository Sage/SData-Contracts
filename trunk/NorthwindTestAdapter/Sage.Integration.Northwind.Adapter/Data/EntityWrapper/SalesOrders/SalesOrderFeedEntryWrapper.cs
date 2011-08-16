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
using Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters;
using System.Collections;
using Sage.Common.Syndication;

namespace Sage.Integration.Northwind.Adapter.Data
{
    class SalesOrderFeedEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        IFeedEntryEntityWrapper _tradingAccountsFeedEntryWrapper;
        IFeedEntryEntityWrapper _commoditiesFeedEntryWrapper;
        IFeedEntryEntityWrapper _unitsOfMeasureFeedEntryWrapper;

        public SalesOrderFeedEntryWrapper(RequestContext context)
            : base(context, Adapter.Common.SupportedResourceKinds.salesOrders)
        {
            _entity = new Order();
            _tradingAccountsFeedEntryWrapper = FeedEntryWrapperFactory.Create(SupportedResourceKinds.tradingAccounts, context);
            _commoditiesFeedEntryWrapper = FeedEntryWrapperFactory.Create(SupportedResourceKinds.commodities, context);
            _unitsOfMeasureFeedEntryWrapper = FeedEntryWrapperFactory.Create(SupportedResourceKinds.unitsOfMeasure, context);
           
            
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

            SalesOrderFeedEntry entry;

            entry = GetPayload((Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrdersRow)order.CalculatedOrders[0],
                order.CalculatedOrderDetails,
                //order.DeletedOrderDetails,
                 _context.Config);
            SetCommonProperties(idString, entry, SupportedResourceKinds.salesOrders);
            return entry;
        }

        private SalesOrderFeedEntry GetPayload(Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrdersRow row,
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrderDetailsDataTable detailDataTable,
            NorthwindConfig config)
        {
            #region Declarations
            SalesOrderFeedEntry payload;
            string id;
            CountryCodes countryCodes = new CountryCodes();
            #endregion

            id = row.OrderID.ToString();

            payload = new SalesOrderFeedEntry();
            payload.UUID = GetUuid(id, "", SupportedResourceKinds.salesOrders);
            payload.active = true;


            payload.currency = config.CurrencyCode;

            payload.pricelist = new PriceListFeedEntry();
            payload.pricelist.UUID = GetUuid(id, "", SupportedResourceKinds.priceLists);



            if (!row.IsCustomerIDNull())
            {
                /*payload.tradingAccount = new TradingAccountFeedEntry();
                payload.tradingAccount.Key = Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix + row.CustomerID;
                payload.tradingAccount.UUID = GetUuid(payload.tradingAccount.Key, "", SupportedResourceKinds.tradingAccounts);
                payload.tradingAccount.Id = GetSDataId(payload.tradingAccount.Key, SupportedResourceKinds.tradingAccounts);
                payload.tradingAccount.Uri = payload.tradingAccount.Id;*/

                payload.tradingAccount = (TradingAccountFeedEntry)_tradingAccountsFeedEntryWrapper.GetFeedEntry(Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix + row.CustomerID);

            }

            if (!row.IsOrderDateNull())
            {
                payload.date = row.OrderDate;
            }

            //payload.lineCount = detailDataTable.Rows.Count;

            payload.discountTotal = row.IsDiscountAmountNull() ? new decimal(0) : Convert.ToDecimal(row.DiscountAmount);

            payload.netTotal = row.IsTotalNetPriceNull() ? new decimal(0) : Convert.ToDecimal(row.TotalNetPrice);

            payload.carrierTotalPrice = row.IsFreightNull() ? new decimal(0) : row.Freight;

            payload.grossTotal = payload.netTotal;

            if (!row.IsRequiredDateNull())
            {
                payload.dueDate = row.RequiredDate;
            }



            if (!row.IsShipViaNull())
            {
                payload.deliveryMethod = row.ShipVia.ToString(); ;
            }

            PostalAddressFeedEntry address = new PostalAddressFeedEntry();
            address.active = true;
            address.address1 = row.IsShipAddressNull() ? null : row.ShipAddress;
            address.country = row.IsShipCountryNull() ? null : row.ShipCountry;
            address.townCity = row.IsShipCityNull() ? null : row.ShipCity;
            address.zipPostCode = row.IsShipPostalCodeNull() ? null : row.ShipPostalCode;
            address.type = postalAddressTypeenum.Shipping;

            payload.postalAddresses = new PostalAddressFeed();
            //TODO: check if valid Address?
            payload.postalAddresses.Entries.Add(address);


            payload.salesOrderLines = new SalesOrderLineFeed();
            foreach (Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrderDetailsRow detailRow in detailDataTable.Rows)
            {
                SalesOrderLineFeedEntry soPayload = GetLineItem(detailRow, config);
                payload.salesOrderLines.Entries.Add(soPayload);
            }

            return payload;
        }

        private SalesOrderLineFeedEntry GetLineItem(Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.CalculatedOrderDetailsRow row, NorthwindConfig config)
        {
            #region Declarations
            SalesOrderLineFeedEntry payload;
            string id;
            decimal discountPercentage;
            #endregion

            id = row.OrderID.ToString() + "-" + row.ProductID.ToString();

            payload = new SalesOrderLineFeedEntry();
            payload.UUID = GetUuid(id, "", SupportedResourceKinds.salesOrderLines);

            payload.commodity = (CommodityFeedEntry)_commoditiesFeedEntryWrapper.GetFeedEntry(row.ProductID.ToString());

           /* payload.commodity = new CommodityFeedEntry();
            payload.commodity.UUID = GetUuid(row.ProductID.ToString(), "", SupportedResourceKinds.commodities);*/

            payload.salesOrder = new SalesOrderFeedEntry();
            payload.salesOrder.UUID = GetUuid(row.OrderID.ToString(), "", SupportedResourceKinds.salesOrders);


            payload.unitOfMeasure = (UnitOfMeasureFeedEntry)_unitsOfMeasureFeedEntryWrapper.GetFeedEntry(row.ProductID.ToString());
            /*payload.unitOfMeasure = new UnitOfMeasureFeedEntry();
            payload.unitOfMeasure.UUID = GetUuid(row.ProductID.ToString(), "", SupportedResourceKinds.unitsOfMeasure);*/

            payload.quantity = row.IsQuantityNull() ? Convert.ToInt16(0) : row.Quantity;

            payload.initialPrice = row.IsUnitPriceNull() ? new decimal(0) : row.UnitPrice;

            payload.orderLineDiscountPercent = row.IsDiscountNull() ? (decimal)0 : Convert.ToDecimal(row.Discount);

            payload.discountTotal = payload.initialPrice * (decimal)payload.orderLineDiscountPercent;

            payload.costTotal = (decimal)payload.initialPrice * (1 - payload.orderLineDiscountPercent);

            payload.netTotal = Convert.ToDecimal(payload.quantity) * Convert.ToDecimal(payload.costTotal);

            SetCommonProperties(id, payload, SupportedResourceKinds.salesOrderLines);

            return payload;
        }

        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            throw new NotImplementedException();
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Delete(string localID)
        {
            SdataTransactionResult tmpTransactionResult;
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.OrdersTableAdapter tableAdapter = new OrdersTableAdapter();
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter = new Order_DetailsTableAdapter();
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order order = new Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order();

            int id;
            if (!(Int32.TryParse(localID, out id)))
                id = 0;
            int recordCount;

            using (OleDbConnection connection = new OleDbConnection(_context.Config.ConnectionString))
            {
                try
                {
                    tableAdapter.Connection = connection;
                    recordCount = tableAdapter.FillBy(order.Orders, id);
                    if (recordCount == 0)
                    {
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.LocalId = localID;
                        tmpTransactionResult.HttpMethod = HttpMethod.DELETE;
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                        tmpTransactionResult.HttpMessage = ("salesorder not found");
                        return tmpTransactionResult;
                    }

                    detailsTableAdapter.Connection = connection;
                    detailsTableAdapter.FillBy(order.Order_Details, id);
                    foreach (DataRow row in order.Order_Details.Rows)
                    {
                        row.Delete();
                    }
                    order.Orders[0].Delete();
                    detailsTableAdapter.Update(order.Order_Details);
                    tableAdapter.Update(order.Orders);
                    tmpTransactionResult = new SdataTransactionResult();
                    tmpTransactionResult.LocalId = localID;
                    tmpTransactionResult.HttpMethod = HttpMethod.DELETE;
                    tmpTransactionResult.ResourceKind = _resourceKind;
                    tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.OK;
                    return tmpTransactionResult;
                }
                catch (Exception e)
                {
                    tmpTransactionResult = new SdataTransactionResult();
                    tmpTransactionResult.LocalId = localID;
                    tmpTransactionResult.HttpMethod = HttpMethod.DELETE;
                    tmpTransactionResult.ResourceKind = _resourceKind;
                    tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                    tmpTransactionResult.HttpMessage = e.Message;
                    return tmpTransactionResult;
                }
            }
        }

        public override SdataTransactionResult Update(Sage.Common.Syndication.FeedEntry payload)
        {
            SdataTransactionResult tmpTransactionResult;
            SalesOrderFeedEntry salesorder = null;

            #region check input values
            if (!(payload is SalesOrderFeedEntry))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;
            }
            salesorder = (payload as SalesOrderFeedEntry);

            if (salesorder == null)
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;
            }

            string customerID = "";

            if (salesorder.tradingAccount != null)
                customerID = GetLocalId(salesorder.tradingAccount.UUID, SupportedResourceKinds.tradingAccounts);


            if (String.IsNullOrEmpty(customerID))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("Trading Acount Id missing");
                return tmpTransactionResult;
            }

            if (!customerID.StartsWith(Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("Salesorder submission is only supported by customers");
                return tmpTransactionResult;
            }
            #endregion

            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.OrdersTableAdapter tableAdapter = new OrdersTableAdapter();
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter = new Order_DetailsTableAdapter();

            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order order = new Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order();

            int id;
            if (!(Int32.TryParse(payload.Key, out id)))
                id = 0;
            int recordCount;

            using (OleDbConnection connection = new OleDbConnection(_context.Config.ConnectionString))
            {
                tableAdapter.Connection = connection;
                recordCount = tableAdapter.FillBy(order.Orders, id);
                if (recordCount == 0)
                    return null;

                detailsTableAdapter.Connection = connection;
                detailsTableAdapter.FillBy(order.Order_Details, id);

            }


            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.OrdersRow row = (Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.OrdersRow)order.Orders[0];


            #region fill dataset from document
            try
            {
                if (!salesorder.IsPropertyChanged("date"))
                    row.SetOrderDateNull();
                else
                    row.OrderDate = salesorder.date;

                if (!salesorder.IsPropertyChanged("dueDate"))
                    row.SetRequiredDateNull();
                else
                    row.RequiredDate = (DateTime)salesorder.dueDate;

                //if (orderDoc.shippedvia.IsNull)
                //    newOrder.SetShipViaNull();
                //else
                //    newOrder.ShipVia = (int)orderDoc.shippedvia.Value;


                if (salesorder.postalAddresses == null || salesorder.postalAddresses.Entries.Count == 0)
                {
                    row.SetShipAddressNull();
                    row.SetShipCityNull();
                    row.SetShipCountryNull();
                    row.SetShipPostalCodeNull();
                }
                else
                {
                    PostalAddressFeedEntry postadress = salesorder.postalAddresses.Entries[0];
                    row.ShipAddress = postadress.address1;
                    row.ShipCity = postadress.townCity;
                    row.ShipPostalCode = postadress.zipPostCode;
                    row.ShipCountry = postadress.country;

                }


                if (!salesorder.IsPropertyChanged("carrierTotalPrice"))
                    row.Freight = (decimal)0;
                else
                    row.Freight = (decimal)salesorder.carrierTotalPrice;

                //row.CreateUser = _context.Config.CrmUser;
                row.ModifyUser = _context.Config.CrmUser;
                //row.CreateID = _context.Config.SequenceNumber;
                row.ModifyID = _context.Config.SequenceNumber;

                Guid itemUuid;


                List<Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.Order_DetailsRow> rowsToDelete = new List<Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.Order_DetailsRow>();
                List<Guid> itemUuids = new List<Guid>();
                if (salesorder.salesOrderLines != null)
                {
                    foreach (SalesOrderLineFeedEntry soLine in salesorder.salesOrderLines.Entries)
                    {
                        if ((soLine.UUID != null && soLine.UUID != Guid.Empty) && (!itemUuids.Contains(soLine.UUID)))
                            itemUuids.Add(soLine.UUID);
                    }
                }

                foreach (Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.Order_DetailsRow detailsRow in order.Order_Details)
                {
                    string itemId = detailsRow.OrderID.ToString() + "-" + detailsRow.ProductID.ToString();
                    itemUuid = GetUuid(itemId, "", SupportedResourceKinds.salesOrderLines);
                    if (itemUuids.Contains(itemUuid))
                    {
                        foreach (SalesOrderLineFeedEntry soLine in salesorder.salesOrderLines.Entries)
                        {
                            if (soLine.UUID.Equals(itemUuid))
                            {
                                if (soLine.IsDeleted)
                                {
                                    rowsToDelete.Add(detailsRow);
                                    break;
                                }
                                /*if (soLine.IsEmpty)
                                {
                                    break;
                                }*/
                                detailsRow.ModifyUser = _context.Config.CrmUser;
                                detailsRow.ModifyID = _context.Config.SequenceNumber;
                                if (soLine.IsPropertyChanged("quantity"))
                                    detailsRow.Quantity = Convert.ToInt16(soLine.quantity);
                                else
                                    detailsRow.Quantity = 0;

                                if (soLine.IsPropertyChanged("initialPrice"))
                                    detailsRow.UnitPrice = (Decimal)soLine.initialPrice;
                                else
                                    detailsRow.UnitPrice = 0;

                                if ((!soLine.IsPropertyChanged("discountTotal")) || (detailsRow.Quantity == 0) || (detailsRow.UnitPrice == 0))
                                {
                                    detailsRow.Discount = (float)0;
                                }
                                else
                                {
                                    // discountPC = discountsum / qunatity * listprice
                                    //detailRow.Discount = Convert.ToSingle((decimal)lineItemDoc.discountsum.Value / ((decimal)detailRow.Quantity * detailRow.UnitPrice));
                                    float discount = Convert.ToSingle((decimal)soLine.discountTotal / (detailsRow.UnitPrice));
                                    if (discount > 1)
                                        discount = 0;
                                    detailsRow.Discount = discount;
                                }
                                break;
                            }
                        }
                        itemUuids.Remove(itemUuid);
                    }
                    else
                    {
                        //delete item
                        rowsToDelete.Add(detailsRow);
                    }

                }

                if (salesorder.salesOrderLines != null)
                {
                    foreach (SalesOrderLineFeedEntry soLine in salesorder.salesOrderLines.Entries)
                    {
                        Guid soUuid = soLine.UUID;
                        if (itemUuids.Contains(soUuid))
                            itemUuids.Remove(soUuid);
                        else
                            continue;

                        try
                        {

                            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.Order_DetailsRow detailRow = order.Order_Details.NewOrder_DetailsRow();
                            Guid productUuid = soLine.commodity.UUID;
                            string productIdString = GetLocalId(productUuid, SupportedResourceKinds.commodities);

                            int productID;
                            if (!int.TryParse(productIdString, out productID))
                                continue;

                            string sorderID = payload.Key + "-" + productID.ToString();
                            detailRow.OrderID = Convert.ToInt32(payload.Key);
                            detailRow.ProductID = productID;
                            if (soLine.IsPropertyChanged("quantity"))
                                detailRow.Quantity = Convert.ToInt16(soLine.quantity);
                            else
                                detailRow.Quantity = 0;

                            if (soLine.IsPropertyChanged("initialPrice"))
                                detailRow.UnitPrice = (Decimal)soLine.initialPrice;
                            else
                                detailRow.UnitPrice = 0;

                            if ((!soLine.IsPropertyChanged("discountTotal")) || (detailRow.Quantity == 0) || (detailRow.UnitPrice == 0))
                            {
                                detailRow.Discount = (float)0;
                            }
                            else
                            {
                                // discountPC = discountsum / qunatity * listprice
                                //detailRow.Discount = Convert.ToSingle((decimal)lineItemDoc.discountsum.Value / ((decimal)detailRow.Quantity * detailRow.UnitPrice));
                                float discount = Convert.ToSingle((decimal)soLine.discountTotal / (detailRow.UnitPrice));
                                if (discount > 1)
                                    discount = 0;
                                detailRow.Discount = discount;
                            }

                            detailRow.CreateUser = _context.Config.CrmUser;
                            detailRow.ModifyUser = _context.Config.CrmUser;
                            detailRow.CreateID = _context.Config.SequenceNumber;
                            detailRow.ModifyID = _context.Config.SequenceNumber;
                            order.Order_Details.AddOrder_DetailsRow(detailRow);

                        }
                        // this error occours in case of invalid data types
                        catch (Exception e)
                        {

                            tmpTransactionResult = new SdataTransactionResult();
                            tmpTransactionResult.HttpMethod = HttpMethod.POST;
                            tmpTransactionResult.ResourceKind = _resourceKind;
                            tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                            tmpTransactionResult.HttpMessage = e.Message;
                            return tmpTransactionResult;

                        }

                    }
                }



                using (OleDbConnection connection = new OleDbConnection(_context.Config.ConnectionString))
                {
                    OleDbTransaction transaction = null;
                    try
                    {
                        connection.Open();
                        transaction = connection.BeginTransaction();

                        tableAdapter.Connection = connection;
                        detailsTableAdapter.Connection = connection;

                        tableAdapter.SetTransaction(transaction);
                        detailsTableAdapter.SetTransaction(transaction);

                        foreach (Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.Order_DetailsRow detailsRow in rowsToDelete)
                        {
                            detailsTableAdapter.Delete(detailsRow.OrderID,
                                detailsRow.ProductID,
                                detailsRow.UnitPrice, detailsRow.Quantity,
                                detailsRow.Discount, detailsRow.CreateID, detailsRow.CreateUser, detailsRow.ModifyID, detailsRow.ModifyUser);
                        }

                        tableAdapter.Update(order.Orders);
                        detailsTableAdapter.Update(order.Order_Details);

                        transaction.Commit();


                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.OK;
                        tmpTransactionResult.LocalId = payload.Key;
                        return tmpTransactionResult;

                    }
                    catch (Exception transactionException)
                    {
                        if (transaction != null)
                            transaction.Rollback();
                        throw;
                    }

                }
            }
            catch (Exception e)
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = e.ToString();
                return tmpTransactionResult;
            }

            #endregion
        }

        public override SdataTransactionResult Add(Sage.Common.Syndication.FeedEntry payload)
        {
            SdataTransactionResult tmpTransactionResult;
            SalesOrderFeedEntry salesorder = null;
            if (!(payload is SalesOrderFeedEntry))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.POST;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;
            }
            salesorder = (payload as SalesOrderFeedEntry);

            if (salesorder == null)
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.POST;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;

            }



            #region check input values
            if (payload == null)
                return null;

            string customerID = "";

            if (salesorder.tradingAccount != null)
                customerID = GetLocalId(salesorder.tradingAccount.UUID, SupportedResourceKinds.tradingAccounts);


            if (String.IsNullOrEmpty(customerID))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("Trading Acount Id missing");
                return tmpTransactionResult;
            }

            if (!customerID.StartsWith(Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("Salesorder submission is only supported by customers");
                return tmpTransactionResult;
            }
            #endregion

            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.OrdersTableAdapter tableAdapter;
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter;

            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order order = new Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order();

            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.OrdersRow newOrder = order.Orders.NewOrdersRow();


            customerID = customerID.Substring(Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix.Length);
            newOrder.CustomerID = customerID;

            #region get Company Name
            DataSet dataSet = new DataSet();
            OleDbDataAdapter dataAdapter;
            string sqlQuery = "Select CompanyName from Customers where CustomerID = '" + customerID + "'";

            try
            {
                using (OleDbConnection connection = new OleDbConnection(_context.Config.ConnectionString))
                {
                    dataAdapter = new OleDbDataAdapter(sqlQuery, connection);
                    if (dataAdapter.Fill(dataSet, "Customers") == 0)
                    {
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.HttpMethod = HttpMethod.POST;
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                        tmpTransactionResult.HttpMessage = ("trading account not found");
                        return tmpTransactionResult;
                    }
                    newOrder.ShipName = dataSet.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception e)
            {
                throw;
            }

            #endregion


            #region fill dataset from document
            try
            {
                if (!salesorder.IsPropertyChanged("date") || salesorder.date==null)
                    newOrder.SetOrderDateNull();
                else
                    newOrder.OrderDate = salesorder.date;

                if (!salesorder.IsPropertyChanged("dueDate") || salesorder.dueDate==null)
                    newOrder.SetRequiredDateNull();
                else
                    newOrder.RequiredDate = (DateTime)salesorder.dueDate;

                //if (orderDoc.shippedvia.IsNull)
                //    newOrder.SetShipViaNull();
                //else
                //    newOrder.ShipVia = (int)orderDoc.shippedvia.Value;


                if (salesorder.postalAddresses == null || salesorder.postalAddresses.Entries.Count == 0)
                {
                    newOrder.SetShipAddressNull();
                    newOrder.SetShipCityNull();
                    newOrder.SetShipCountryNull();
                    newOrder.SetShipPostalCodeNull();
                }
                else
                {
                    PostalAddressFeedEntry postadress = salesorder.postalAddresses.Entries[0];
                    newOrder.ShipAddress = postadress.address1;
                    newOrder.ShipCity = postadress.townCity;
                    newOrder.ShipPostalCode = postadress.zipPostCode;
                    newOrder.ShipCountry = postadress.country;

                }


                if (!salesorder.IsPropertyChanged("carrierTotalPrice"))
                    newOrder.Freight = (decimal)0;
                else
                    newOrder.Freight = (decimal)salesorder.carrierTotalPrice;

                newOrder.CreateUser = _context.Config.CrmUser;
                newOrder.ModifyUser = _context.Config.CrmUser;
                newOrder.CreateID = _context.Config.SequenceNumber;
                newOrder.ModifyID = _context.Config.SequenceNumber;
            }
            catch (Exception e)
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.POST;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = e.ToString();
                return tmpTransactionResult;
            }

            #endregion

            using (OleDbConnection connection = new OleDbConnection(_context.Config.ConnectionString))
            {
                OleDbTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    tableAdapter = new Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.OrdersTableAdapter();
                    tableAdapter.Connection = connection;
                    detailsTableAdapter = new Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.Order_DetailsTableAdapter();
                    detailsTableAdapter.Connection = connection;

                    tableAdapter.SetTransaction(transaction);
                    detailsTableAdapter.SetTransaction(transaction);
                    order.Orders.AddOrdersRow(newOrder);
                    tableAdapter.Update(order.Orders);
                    OleDbCommand Cmd = new OleDbCommand("SELECT @@IDENTITY", connection);
                    Cmd.Transaction = transaction;
                    object lastid = Cmd.ExecuteScalar();
                    payload.Key = ((int)lastid).ToString();
                    // add line Items

                    Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.Order_DetailsRow detailRow;

                    Hashtable addedProductsProducts;
                    addedProductsProducts = new Hashtable();
                    int productID;

                    int productIndex = 0;
                    if (salesorder.salesOrderLines != null)
                    {
                        foreach (SalesOrderLineFeedEntry salesOrderLine in salesorder.salesOrderLines.Entries)
                        {
                            try
                            {

                                string productIdString = "";
                                productID = 0;
                                if (salesOrderLine.commodity != null && salesOrderLine.commodity.UUID != null && salesOrderLine.commodity.UUID != Guid.Empty)
                                {
                                    productIdString = GetLocalId(salesOrderLine.commodity.UUID, SupportedResourceKinds.commodities);
                                    if (!int.TryParse(productIdString, out productID))
                                        productID = 0;
                                }

                                if (addedProductsProducts.Contains(productID))
                                {
                                    transaction.Rollback();
                                    tmpTransactionResult = new SdataTransactionResult();
                                    tmpTransactionResult.HttpMethod = HttpMethod.POST;
                                    tmpTransactionResult.ResourceKind = _resourceKind;
                                    tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                                    tmpTransactionResult.HttpMessage = "Order contains a product twice";
                                    return tmpTransactionResult;

                                }

                                addedProductsProducts.Add(productID, productID);

                                detailRow = order.Order_Details.NewOrder_DetailsRow();
                                salesOrderLine.Key = payload.Key + "-" + productID.ToString();
                                detailRow.OrderID = Convert.ToInt32(payload.Key);
                                detailRow.ProductID = productID;
                                if (salesOrderLine.IsPropertyChanged("quantity"))
                                    detailRow.Quantity = Convert.ToInt16(salesOrderLine.quantity);
                                else
                                    detailRow.Quantity = 0;

                                if (salesOrderLine.IsPropertyChanged("initialPrice"))
                                    detailRow.UnitPrice = (Decimal)salesOrderLine.initialPrice;
                                else
                                    detailRow.UnitPrice = 0;




                                if ((!salesOrderLine.IsPropertyChanged("discountTotal")) || (detailRow.Quantity == 0) || (detailRow.UnitPrice == 0))
                                {
                                    detailRow.Discount = (float)0;
                                }
                                else
                                {
                                    // discountPC = discountsum / qunatity * listprice
                                    //detailRow.Discount = Convert.ToSingle((decimal)lineItemDoc.discountsum.Value / ((decimal)detailRow.Quantity * detailRow.UnitPrice));
                                    float discount = Convert.ToSingle((decimal)salesOrderLine.discountTotal / (detailRow.UnitPrice));
                                    if (discount > 1)
                                        discount = 0;
                                    detailRow.Discount = discount;
                                }

                                detailRow.CreateUser = _context.Config.CrmUser;
                                detailRow.ModifyUser = _context.Config.CrmUser;
                                detailRow.CreateID = _context.Config.SequenceNumber;
                                detailRow.ModifyID = _context.Config.SequenceNumber;
                            }
                            // this error occours in case of invalid data types
                            catch (Exception e)
                            {
                                transaction.Rollback();

                                tmpTransactionResult = new SdataTransactionResult();
                                tmpTransactionResult.HttpMethod = HttpMethod.POST;
                                tmpTransactionResult.ResourceKind = _resourceKind;
                                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                                tmpTransactionResult.HttpMessage = e.Message;
                                return tmpTransactionResult;

                            }
                            order.Order_Details.AddOrder_DetailsRow(detailRow);
                            productIndex++;
                        }
                    }

                    // here could an error ouucour in case on broken database connection 
                    // or of same invalid constraints which are unhandled before
                    try
                    {
                        detailsTableAdapter.Update(order.Order_Details);
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.HttpMethod = HttpMethod.POST;
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                        tmpTransactionResult.HttpMessage = e.Message;
                        return tmpTransactionResult;
                    }
                    transaction.Commit();


                    tmpTransactionResult = new SdataTransactionResult();
                    tmpTransactionResult.HttpMethod = HttpMethod.POST;
                    tmpTransactionResult.ResourceKind = _resourceKind;
                    tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.Created;
                    tmpTransactionResult.LocalId = payload.Key;
                    return tmpTransactionResult;

                }
                catch (Exception transactionException)
                {
                    if (transaction != null)
                        transaction.Rollback();
                    throw;

                }

            }
        }

        public string GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "OrderID";
            if (propertyName.Equals("tradingaccount", StringComparison.InvariantCultureIgnoreCase))
                return "CustomerID";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        private void SetCommonProperties(string idString, FeedEntry entry, SupportedResourceKinds resKind)
        {
            entry.Id = GetSDataId(idString, resKind);
            entry.Key = idString;
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

        protected string GetSDataId(string id, SupportedResourceKinds resourceKind)
        {
            return String.Format("{0}{1}('{2}')", _context.DatasetLink, resourceKind.ToString(), id);
        }
    }
}
