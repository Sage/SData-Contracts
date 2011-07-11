// $Author:$

#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Common.Paging;
using Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.API;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Order;
using Sage.Integration.Northwind.Application.Entities.Order.Documents;
using Sage.Integration.Northwind.Application.Toolkit;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
#endregion

namespace Sage.Integration.Northwind.Adapter.Data.SalesOrders
{
    /// <summary>
    /// This implementation of the wrapper does not use the original application opject.
    /// </summary>
    public class SalesOrderWrapper : EntityWrapperBase, IEntityWrapper, IEntityQueryWrapper
    {
        #region Class Variables


        #endregion

        #region Ctor.

        public SalesOrderWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.salesOrders)
        {
            _entity = new Order();
        }
        #endregion


        #region get
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

        

        private SalesOrderPayload GetPayload(DataSets.Order.CalculatedOrdersRow row,
            DataSets.Order.CalculatedOrderDetailsDataTable detailDataTable,
            //DataSets.Order.DeletedOrderDetailsDataTable deletedOrderDetailsDataTable,
            NorthwindConfig config)
        {
            #region Declarations
            SalesOrderPayload payload;
            string id;
            CountryCodes countryCodes = new CountryCodes();
            #endregion

            id = row.OrderID.ToString();

            payload = new SalesOrderPayload();
            payload.LocalID = id;
            payload.SyncUuid = GetUuid(id, "", SupportedResourceKinds.salesOrders);
            payload.SalesOrdertype.active = true;
            payload.SalesOrdertype.applicationID = id;

            payload.SalesOrdertype.currency = config.CurrencyCode;

            payload.ForeignIds.Add("pricelist", Sage.Integration.Northwind.Application.API.Constants.DefaultValues.PriceList.ID);


            if (!row.IsCustomerIDNull())
                payload.ForeignIds.Add("tradingAccount", Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix + row.CustomerID);


            if (!row.IsOrderDateNull())
            {
                payload.SalesOrdertype.date = row.OrderDate;
                payload.SalesOrdertype.dateSpecified = true;
            }

            payload.SalesOrdertype.lineCountSpecified = true;
            payload.SalesOrdertype.lineCount = detailDataTable.Rows.Count;

            payload.SalesOrdertype.discountTotalSpecified = true;
            payload.SalesOrdertype.discountTotal = row.IsDiscountAmountNull() ? new decimal(0) : Convert.ToDecimal(row.DiscountAmount);

            payload.SalesOrdertype.netTotalSpecified = true;
            payload.SalesOrdertype.netTotal = row.IsTotalNetPriceNull() ? new decimal(0) : Convert.ToDecimal(row.TotalNetPrice);

            payload.SalesOrdertype.carrierTotalPriceSpecified = true;
            payload.SalesOrdertype.carrierTotalPrice = row.IsFreightNull() ? new decimal(0) : row.Freight;

            payload.SalesOrdertype.grossTotalSpecified = true;
            payload.SalesOrdertype.grossTotal = payload.SalesOrdertype.netTotal;



            if (!row.IsRequiredDateNull())
            {
                payload.SalesOrdertype.dueDateSpecified = true;
                payload.SalesOrdertype.dueDate = row.RequiredDate;
            }



            if (!row.IsShipViaNull())
            {
                payload.SalesOrdertype.deliveryMethod = row.ShipVia.ToString(); ;
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

            payload.SalesOrdertype.postalAddresses = new postalAddresstype[1];
            payload.SalesOrdertype.postalAddresses[0] = address;


            payload.SalesOrdertype.salesOrderLines = new salesOrderLinetype[detailDataTable.Rows.Count];
            int index = 0;
            foreach (DataSets.Order.CalculatedOrderDetailsRow detailRow in detailDataTable.Rows)
            {
                SalesOrderLinePayload soPayload = GetLineItem(detailRow, config);
                payload.ForeignIds.Add(
                            String.Format("salesOrderLines[{0}]",
                            index.ToString()),
                            soPayload.LocalID);
                foreach (string key in soPayload.ForeignIds.Keys)//  (int foreignIdIndex = 0; foreignIdIndex <= soPayload.ForeignIds.Count; foreignIdIndex++)
                {
                    string value;
                    if (soPayload.ForeignIds.TryGetValue(key, out value))
                    {
                        payload.ForeignIds.Add(
                            String.Format("salesOrderLines[{0}]/{1}",
                            index.ToString(),
                            key),
                            value);
                    }
                }
                payload.SalesOrdertype.salesOrderLines[index] = soPayload.SalesOrderLinetype;
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

        private SalesOrderLinePayload GetLineItem(DataSets.Order.CalculatedOrderDetailsRow row, NorthwindConfig config)
        {
            #region Declarations
            SalesOrderLinePayload payload;
            string id;
            decimal discountPercentage;
            #endregion



            id = row.OrderID.ToString() + "-" + row.ProductID.ToString();

            payload = new SalesOrderLinePayload();
            payload.LocalID = id;
            //payload.SyncUuid = GetUuid(id, "", SupportedResourceKinds.salesOrderLines);
            payload.SalesOrderLinetype.applicationID = id;

            payload.ForeignIds.Add("commodity", row.ProductID.ToString());
            payload.ForeignIds.Add("salesOrder", row.OrderID.ToString());
            payload.ForeignIds.Add("unitOfMeasure", row.ProductID.ToString());

            payload.SalesOrderLinetype.quantitySpecified = true;
            payload.SalesOrderLinetype.quantity = row.IsQuantityNull() ? Convert.ToInt16(0) : row.Quantity;

            payload.SalesOrderLinetype.initialPriceSpecified = true;
            payload.SalesOrderLinetype.initialPrice = row.IsUnitPriceNull() ? new decimal(0) : row.UnitPrice;

            payload.SalesOrderLinetype.orderLineDiscountPercentSpecified = true;
            payload.SalesOrderLinetype.orderLineDiscountPercent = row.IsDiscountNull() ? (decimal)0 : Convert.ToDecimal(row.Discount);

            payload.SalesOrderLinetype.discountTotalSpecified = true;
            payload.SalesOrderLinetype.discountTotal = payload.SalesOrderLinetype.initialPrice * (decimal)payload.SalesOrderLinetype.orderLineDiscountPercent;

            payload.SalesOrderLinetype.costTotalSpecified = true;
            payload.SalesOrderLinetype.costTotal = (decimal)payload.SalesOrderLinetype.initialPrice * (1 - payload.SalesOrderLinetype.orderLineDiscountPercent);



            payload.SalesOrderLinetype.netTotalSpecified = true;
            payload.SalesOrderLinetype.netTotal = Convert.ToDecimal(payload.SalesOrderLinetype.quantity) * Convert.ToDecimal(payload.SalesOrderLinetype.costTotal);

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


            //for (int index = startIndex; index < startIndex + count; index++)
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


        #region transformations
        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            return new OrderDocument();
        }

        public override Sage.Integration.Northwind.Feeds.PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            throw new NotImplementedException();
        }
        #endregion


        public override SdataTransactionResult Delete(string localID)
        {
            SdataTransactionResult tmpTransactionResult;
            DataSets.OrderTableAdapters.OrdersTableAdapter tableAdapter = new OrdersTableAdapter();
            DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter = new Order_DetailsTableAdapter();
            DataSets.Order order = new DataSets.Order();

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
                        tmpTransactionResult.HttpMethod = "DELETE";
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
                    tmpTransactionResult.HttpMethod = "DELETE";
                    tmpTransactionResult.ResourceKind = _resourceKind;
                    tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.OK;
                    return tmpTransactionResult;
                }
                catch (Exception e)
                {
                    tmpTransactionResult = new SdataTransactionResult();
                    tmpTransactionResult.LocalId = localID;
                    tmpTransactionResult.HttpMethod = "DELETE";
                    tmpTransactionResult.ResourceKind = _resourceKind;
                    tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                    tmpTransactionResult.HttpMessage = e.Message;
                    return tmpTransactionResult;
                }

            }

        }


        #region update
        public override SdataTransactionResult Update(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            SdataTransactionResult tmpTransactionResult;
            salesOrdertype salesorder = null;

            #region check input values
            if (!(payload is SalesOrderPayload))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = "PUT";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;
            }
            salesorder = (payload as SalesOrderPayload).SalesOrdertype;

            if (salesorder == null)
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = "PUT";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;

            }
            foreach (SyncFeedEntryLink link in links)
            {
                if (link.LinkRel != RelEnum.related.ToString())
                    continue;
                string localID = string.Empty;
                if (link.Href.StartsWith(_context.DatasetLink, StringComparison.InvariantCultureIgnoreCase))
                    if (link.Href.LastIndexOf("('") < link.Href.LastIndexOf("')"))
                        localID = link.Href.Substring(link.Href.LastIndexOf("('") + 2,
                            link.Href.LastIndexOf("')") - link.Href.LastIndexOf("('") - 2);

                SupportedResourceKinds reskind = ResourceKindHelpers.GetResourceKind(link.PayloadPath);

                if (localID == string.Empty)
                    localID = GetLocalId(link.Uuid, reskind);

                payload.ForeignIds.Add(link.PayloadPath, localID);
            }


            
            if (payload == null)
                return null;

            string customerID;

            if (!payload.ForeignIds.TryGetValue("tradingAccount", out customerID))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = "PUT";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("Trading Acount Id missing");
                return tmpTransactionResult;
            }

            if (!customerID.StartsWith(Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = "PUT";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("Salesorder submission is only supported by customers");
                return tmpTransactionResult;
            }
            #endregion

            DataSets.OrderTableAdapters.OrdersTableAdapter tableAdapter = new OrdersTableAdapter();
            DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter=new Order_DetailsTableAdapter();

            DataSets.Order order = new DataSets.Order();

            int id;
            if (!(Int32.TryParse(payload.LocalID, out id)))
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


            DataSets.Order.OrdersRow row = (DataSets.Order.OrdersRow)order.Orders[0];


            #region fill dataset from document
            try
            {
                if (!salesorder.dateSpecified)
                    row.SetOrderDateNull();
                else
                    row.OrderDate = salesorder.date;

                if (!salesorder.dueDateSpecified)
                    row.SetRequiredDateNull();
                else
                    row.RequiredDate = (DateTime)salesorder.dueDate;

                //if (orderDoc.shippedvia.IsNull)
                //    newOrder.SetShipViaNull();
                //else
                //    newOrder.ShipVia = (int)orderDoc.shippedvia.Value;


                if (salesorder.postalAddresses == null || salesorder.postalAddresses.Length == 0)
                {
                    row.SetShipAddressNull();
                    row.SetShipCityNull();
                    row.SetShipCountryNull();
                    row.SetShipPostalCodeNull();
                }
                else
                {
                    postalAddresstype postadress = salesorder.postalAddresses[0];
                    row.ShipAddress = postadress.address1;
                    row.ShipCity = postadress.townCity;
                    row.ShipPostalCode = postadress.zipPostCode;
                    row.ShipCountry = postadress.country;

                }


                if (!salesorder.carrierTotalPriceSpecified)
                    row.Freight = (decimal)0;
                else
                    row.Freight = (decimal)salesorder.carrierTotalPrice;

                //row.CreateUser = _context.Config.CrmUser;
                row.ModifyUser = _context.Config.CrmUser;
                //row.CreateID = _context.Config.SequenceNumber;
                row.ModifyID = _context.Config.SequenceNumber;

                Dictionary<Guid, SyncFeedEntryLink> itemLinks = new Dictionary<Guid, SyncFeedEntryLink>();
                GuidConverter converter = new GuidConverter();
                 Guid itemUuid;

                foreach (SyncFeedEntryLink link in links)
                {
                    SupportedResourceKinds resKind = ResourceKindHelpers.GetResourceKind(link.PayloadPath);
                    if (resKind != SupportedResourceKinds.salesOrderLines)
                        continue;
                    try
                    {

                        itemUuid = (Guid)converter.ConvertFromString(link.Uuid);
                        if (!Guid.Empty.Equals(itemUuid))
                            itemLinks.Add(itemUuid, link);
                    }
                    catch (Exception)
                    {
                    }
                }
                List<DataSets.Order.Order_DetailsRow> rowsToDelete = new List<DataSets.Order.Order_DetailsRow>();

                //foreach (DataSets.Order.Order_DetailsRow detailsRow in rowsToDelete)
                //{
                //    detailsRow.Delete();
                //}

                foreach (DataSets.Order.Order_DetailsRow detailsRow in order.Order_Details)
                {
                    string itemId = detailsRow.OrderID.ToString() + "-" + detailsRow.ProductID.ToString();
                    itemUuid = GetUuid(itemId, "", SupportedResourceKinds.salesOrderLines);
                    SyncFeedEntryLink itemlink;
                    if (itemLinks.TryGetValue(itemUuid, out itemlink))
                    {
                        string lineNumberstring = itemlink.PayloadPath.Substring(itemlink.PayloadPath.IndexOf("[") + 1, itemlink.PayloadPath.IndexOf("]") - itemlink.PayloadPath.IndexOf("[") - 1);
                        int lineNumber;
                        if (Int32.TryParse(lineNumberstring, out lineNumber))
                        {
                            if (lineNumber < salesorder.salesOrderLines.Length)
                            {
                                salesOrderLinetype soLine = salesorder.salesOrderLines[lineNumber];
                                detailsRow.ModifyUser = _context.Config.CrmUser;
                                detailsRow.ModifyID = _context.Config.SequenceNumber;
                                if (soLine.quantitySpecified)
                                    detailsRow.Quantity = Convert.ToInt16(soLine.quantity);
                                else
                                    detailsRow.Quantity = 0;

                                if (soLine.initialPriceSpecified)
                                    detailsRow.UnitPrice = (Decimal)soLine.initialPrice;
                                else
                                    detailsRow.UnitPrice = 0;

                                if ((!soLine.discountTotalSpecified) || (detailsRow.Quantity == 0) || (detailsRow.UnitPrice == 0))
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

                            }
                        }
                        itemLinks.Remove(itemUuid);
                    }
                    else
                    {
                        //delete item
                        rowsToDelete.Add(detailsRow);
                    }

                }
               


                foreach (SyncFeedEntryLink itemlink in itemLinks.Values)
                {
                    {
                        try
                        {
                            int productID;
                            string productIdString;
                            string productIdPayload = String.Format("{0}/{1}", itemlink.PayloadPath, "commodity");

                            if (payload.ForeignIds.TryGetValue(productIdPayload, out productIdString))
                            {
                                try
                                {
                                    productID = Convert.ToInt32(productIdString);
                                }
                                catch (Exception)
                                {
                                    continue;
                                }
                            }
                            else
                                continue;

                            string lineNumberstring = itemlink.PayloadPath.Substring(itemlink.PayloadPath.IndexOf("[") + 1, itemlink.PayloadPath.IndexOf("]") - itemlink.PayloadPath.IndexOf("[") - 1);
                            int lineNumber;
                            if (Int32.TryParse(lineNumberstring, out lineNumber))
                            {
                                if (lineNumber < salesorder.salesOrderLines.Length)
                                {
                                    salesOrderLinetype soLine = salesorder.salesOrderLines[lineNumber];


                                    DataSets.Order.Order_DetailsRow detailRow = order.Order_Details.NewOrder_DetailsRow();
                                    soLine.applicationID = payload.LocalID + "-" + productID.ToString();
                                    detailRow.OrderID = Convert.ToInt32(payload.LocalID);
                                    detailRow.ProductID = productID;
                                    if (soLine.quantitySpecified)
                                        detailRow.Quantity = Convert.ToInt16(soLine.quantity);
                                    else
                                        detailRow.Quantity = 0;

                                    if (soLine.initialPriceSpecified)
                                        detailRow.UnitPrice = (Decimal)soLine.initialPrice;
                                    else
                                        detailRow.UnitPrice = 0;

                                    if ((!soLine.discountTotalSpecified) || (detailRow.Quantity == 0) || (detailRow.UnitPrice == 0))
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
                            }
                        }
                        // this error occours in case of invalid data types
                        catch (Exception e)
                        {

                            tmpTransactionResult = new SdataTransactionResult();
                            tmpTransactionResult.HttpMethod = "POST";
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

                        foreach (DataSets.Order.Order_DetailsRow detailsRow in rowsToDelete)
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
                        tmpTransactionResult.HttpMethod = "PUT";
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.OK;
                        tmpTransactionResult.LocalId = payload.LocalID;
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
                tmpTransactionResult.HttpMethod = "PUT";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = e.ToString();
                return tmpTransactionResult;
            }

            #endregion


        }

        #endregion


        #region add
        public override SdataTransactionResult Add(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            SdataTransactionResult tmpTransactionResult;
            salesOrdertype salesorder = null;
            if (!(payload is SalesOrderPayload))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = "POST";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;
            }
            salesorder = (payload as SalesOrderPayload).SalesOrdertype;

            if (salesorder == null)
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = "POST";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;
            
            }
            foreach (SyncFeedEntryLink link in links)
            {
                if (link.LinkRel != RelEnum.related.ToString())
                    continue;
                string localID = string.Empty;
                if(link.Href.StartsWith(_context.DatasetLink, StringComparison.InvariantCultureIgnoreCase))
                    if (link.Href.LastIndexOf("('") < link.Href.LastIndexOf("')"))
                        localID = link.Href.Substring(link.Href.LastIndexOf("('")+2, 
                            link.Href.LastIndexOf("')") - link.Href.LastIndexOf("('") -2);
 
                SupportedResourceKinds reskind = ResourceKindHelpers.GetResourceKind(link.PayloadPath);

                if (localID == string.Empty)
                    localID = GetLocalId(link.Uuid, reskind);

                payload.ForeignIds.Add(link.PayloadPath, localID);
            }


            #region check input values
            if (payload == null)
                return null;

            string customerID;

            if (!payload.ForeignIds.TryGetValue("tradingAccount", out customerID))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = "POST";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("Trading Acount Id missing");
                return tmpTransactionResult;
            }

            if (!customerID.StartsWith(Sage.Integration.Northwind.Application.API.Constants.CustomerIdPrefix))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = "POST";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("Salesorder submission is only supported by customers");
                return tmpTransactionResult;
            }
            #endregion

            DataSets.OrderTableAdapters.OrdersTableAdapter tableAdapter;
            DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter;

            DataSets.Order order = new DataSets.Order();

            DataSets.Order.OrdersRow newOrder = order.Orders.NewOrdersRow();


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
                        tmpTransactionResult.HttpMethod = "POST";
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


            #region get Sels rep
            //if (orderDoc.salesrepr.IsNull)
            //    newOrder.SetEmployeeIDNull();
            //else
            //{
            //    try
            //    {
            //        newOrder.EmployeeID = int.Parse((string)orderDoc.salesrepr.Value);
            //    }
            //    catch (Exception)
            //    {
            //        newOrder.SetEmployeeIDNull();
            //    }
            //    if (newOrder.IsEmployeeIDNull())
            //    {
            //        try
            //        {
            //            dataSet = new DataSet();
            //            sqlQuery = "SELECT Employees.EmployeeID FROM Employees where Employees.FirstName + ' ' + Employees.LastName = ? ";
            //            using (OleDbConnection connection = new OleDbConnection(config.ConnectionString))
            //            {
            //                dataAdapter = new OleDbDataAdapter(sqlQuery, connection);
            //                OleDbParameter parameter = new OleDbParameter("Name", (string)orderDoc.salesrepr.Value);
            //                dataAdapter.SelectCommand.Parameters.Add(parameter);
            //                if (dataAdapter.Fill(dataSet, "Employees") > 0)
            //                    newOrder.EmployeeID = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);
            //                else
            //                    newOrder.EmployeeID = 1;
            //            }
            //        }
            //        catch (Exception e)
            //        {
            //            orderDoc.Id = "";
            //            throw;
            //        }

            //    }
            //}
            #endregion

            #region fill dataset from document
            try
            {
                if(!salesorder.dateSpecified)
                    newOrder.SetOrderDateNull();
                else
                    newOrder.OrderDate = salesorder.date;

                if (!salesorder.dueDateSpecified)
                    newOrder.SetRequiredDateNull();
                else
                    newOrder.RequiredDate = (DateTime)salesorder.dueDate;

                //if (orderDoc.shippedvia.IsNull)
                //    newOrder.SetShipViaNull();
                //else
                //    newOrder.ShipVia = (int)orderDoc.shippedvia.Value;


                if (salesorder.postalAddresses == null || salesorder.postalAddresses.Length == 0)
                {
                    newOrder.SetShipAddressNull();
                    newOrder.SetShipCityNull();
                    newOrder.SetShipCountryNull();
                    newOrder.SetShipPostalCodeNull();
                }
                else
                {
                    postalAddresstype postadress = salesorder.postalAddresses[0];
                    newOrder.ShipAddress = postadress.address1;
                    newOrder.ShipCity = postadress.townCity;
                    newOrder.ShipPostalCode = postadress.zipPostCode;
                    newOrder.ShipCountry = postadress.country;

                }


                if (!salesorder.carrierTotalPriceSpecified)
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
                tmpTransactionResult.HttpMethod = "POST";
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

                    tableAdapter = new DataSets.OrderTableAdapters.OrdersTableAdapter();
                    tableAdapter.Connection = connection;
                    detailsTableAdapter = new DataSets.OrderTableAdapters.Order_DetailsTableAdapter();
                    detailsTableAdapter.Connection = connection;

                    tableAdapter.SetTransaction(transaction);
                    detailsTableAdapter.SetTransaction(transaction);
                    order.Orders.AddOrdersRow(newOrder);
                    tableAdapter.Update(order.Orders);
                    OleDbCommand Cmd = new OleDbCommand("SELECT @@IDENTITY", connection);
                    Cmd.Transaction = transaction;
                    object lastid = Cmd.ExecuteScalar();
                    payload.LocalID = ((int)lastid).ToString();
                    // add line Items

                    DataSets.Order.Order_DetailsRow detailRow;

                    Hashtable addedProductsProducts;
                    addedProductsProducts = new Hashtable();
                    int productID;

                    int productIndex= 0;
                    foreach (salesOrderLinetype salesOrderLine in salesorder.salesOrderLines)
                    {
                        try
                        {
                            string productIdString;
                            string productIdPayload = String.Format("salesOrderLines[{0}]/{1}",
                            productIndex.ToString(),
                            "commodity");
                            if (payload.ForeignIds.TryGetValue(productIdPayload, out productIdString))
                            {
                                try
                                {
                                    productID = Convert.ToInt32( productIdString);
                                }
                                catch (Exception)
                                {
#warning only to test unsupported products
                                    productID = 0;
                                }
                            }
                            else
                                productID = 0;

                            if (addedProductsProducts.Contains(productID))
                            {
                                transaction.Rollback();
                                tmpTransactionResult = new SdataTransactionResult();
                                tmpTransactionResult.HttpMethod = "POST";
                                tmpTransactionResult.ResourceKind = _resourceKind;
                                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                                tmpTransactionResult.HttpMessage = "Order contains a product twice";
                                return tmpTransactionResult;
                                
                            }
                            addedProductsProducts.Add(productID, productID);
                            
                            detailRow = order.Order_Details.NewOrder_DetailsRow();
                            salesOrderLine.applicationID = payload.LocalID + "-" + productID.ToString();
                            detailRow.OrderID = Convert.ToInt32(payload.LocalID);
                            detailRow.ProductID = productID;
                            if (salesOrderLine.quantitySpecified)
                                detailRow.Quantity = Convert.ToInt16(salesOrderLine.quantity);
                            else
                                detailRow.Quantity = 0;

                            if (salesOrderLine.initialPriceSpecified)
                                detailRow.UnitPrice = (Decimal)salesOrderLine.initialPrice;
                            else
                                detailRow.UnitPrice = 0;




                            if ((!salesOrderLine.discountTotalSpecified) || (detailRow.Quantity == 0) || (detailRow.UnitPrice == 0))
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
                            tmpTransactionResult.HttpMethod = "POST";
                            tmpTransactionResult.ResourceKind = _resourceKind;
                            tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                            tmpTransactionResult.HttpMessage = e.Message;
                            return tmpTransactionResult;

                        }
                        order.Order_Details.AddOrder_DetailsRow(detailRow);
                        productIndex++;
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
                        tmpTransactionResult.HttpMethod = "POST";
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                        tmpTransactionResult.HttpMessage = e.Message;
                        return tmpTransactionResult;
                    }
                    transaction.Commit();

                    
                    tmpTransactionResult = new SdataTransactionResult();
                    tmpTransactionResult.HttpMethod = "POST";
                    tmpTransactionResult.ResourceKind = _resourceKind;
                    tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.Created;
                    tmpTransactionResult.LocalId = payload.LocalID;
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
