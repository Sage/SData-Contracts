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

namespace Sage.Integration.Northwind.Adapter.Data
{
    class SalesOrderFeedLineEntryWrapper : EntityWrapperBase, IEntityQueryWrapper, IFeedEntryEntityWrapper
    {
        public SalesOrderFeedLineEntryWrapper(RequestContext context)
            : base(context, Adapter.Common.SupportedResourceKinds.salesOrders)
        {
            _entity = new Order();
        }


        public override SdataTransactionResult Update(Sage.Common.Syndication.FeedEntry payload)
        {
            SdataTransactionResult tmpTransactionResult;

            if (!(payload is SalesOrderLineFeedEntry))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;
            }

            SalesOrderLineFeedEntry soLine = payload as SalesOrderLineFeedEntry;
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter = new Order_DetailsTableAdapter();
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order order = new Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order();

            int productId;
            int orderId;
            if (GetLocalIds(payload.Key, out orderId, out productId))
            {
                using (OleDbConnection connection = new OleDbConnection(_context.Config.ConnectionString))
                {
                    try
                    {
                        detailsTableAdapter.Connection = connection;
                        int recordCount = detailsTableAdapter.FillBy(order.Order_Details, orderId);
                        if (recordCount == 0)
                        {
                            tmpTransactionResult = new SdataTransactionResult();
                            tmpTransactionResult.LocalId = payload.Key;
                            tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                            tmpTransactionResult.ResourceKind = _resourceKind;
                            tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                            tmpTransactionResult.HttpMessage = ("salesorder not found");
                            return tmpTransactionResult;
                        }

                        foreach (Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.Order_DetailsRow detailsRow in order.Order_Details.Rows)
                        {
                            if (detailsRow.ProductID == productId)
                            {
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

                        detailsTableAdapter.Update(order.Order_Details);
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.LocalId = payload.Key;
                        tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.OK;
                        return tmpTransactionResult;
                    }
                    catch (Exception e)
                    {
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.LocalId = payload.Key;
                        tmpTransactionResult.HttpMethod = HttpMethod.PUT;
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                        tmpTransactionResult.HttpMessage = e.Message;
                        return tmpTransactionResult;
                    }

                }

            }
            tmpTransactionResult = new SdataTransactionResult();
            tmpTransactionResult.LocalId = payload.Key;
            tmpTransactionResult.HttpMethod = HttpMethod.PUT;
            tmpTransactionResult.ResourceKind = _resourceKind;
            tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
            tmpTransactionResult.HttpMessage = "Not found";
            return tmpTransactionResult;
        }

        public override SdataTransactionResult Delete(string id)
        {
            SdataTransactionResult tmpTransactionResult;
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter = new Order_DetailsTableAdapter();
            Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order order = new Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order();

            int productId;
            int orderId;
            if (GetLocalIds(id, out orderId, out productId))
            {
                using (OleDbConnection connection = new OleDbConnection(_context.Config.ConnectionString))
                {
                    try
                    {
                        detailsTableAdapter.Connection = connection;
                        int recordCount = detailsTableAdapter.FillBy(order.Order_Details, orderId);
                        if (recordCount == 0)
                        {
                            tmpTransactionResult = new SdataTransactionResult();
                            tmpTransactionResult.LocalId = id;
                            tmpTransactionResult.HttpMethod = HttpMethod.DELETE;
                            tmpTransactionResult.ResourceKind = _resourceKind;
                            tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                            tmpTransactionResult.HttpMessage = ("salesorder not found");
                            return tmpTransactionResult;
                        }

                        foreach (Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.Order.Order_DetailsRow row in order.Order_Details.Rows)
                        {
                            if (row.ProductID == productId)
                            {
                                row.Delete();
                                break;
                            }
                        }

                        detailsTableAdapter.Update(order.Order_Details);
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.LocalId = id;
                        tmpTransactionResult.HttpMethod = HttpMethod.DELETE;
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.OK;
                        return tmpTransactionResult;
                    }
                    catch (Exception e)
                    {
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.LocalId = id;
                        tmpTransactionResult.HttpMethod = HttpMethod.DELETE;
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                        tmpTransactionResult.HttpMessage = e.Message;
                        return tmpTransactionResult;
                    }

                }

            }
            tmpTransactionResult = new SdataTransactionResult();
            tmpTransactionResult.LocalId = id;
            tmpTransactionResult.HttpMethod = HttpMethod.DELETE;
            tmpTransactionResult.ResourceKind = _resourceKind;
            tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
            tmpTransactionResult.HttpMessage = "Not found";
            return tmpTransactionResult;
        }

        private bool GetLocalIds(string localId, out int orderId, out int productId)
        {
            if ((String.IsNullOrEmpty(localId)) || (!localId.Contains("-")))
            {
                orderId = 0;
                productId = 0;
                return false;
            }
            string productIdString = localId.Substring(localId.IndexOf("-") + 1);
            string orderIdString = localId.Substring(0, localId.IndexOf("-"));
            if (!(Int32.TryParse(productIdString, out productId)))
                productId = 0;

            if (!(Int32.TryParse(orderIdString, out orderId)))
                orderId = 0;

            if (orderId == 0 || productId == 0)
                return false;

            return true;
        }

        public override Sage.Common.Syndication.FeedEntry GetFeedEntry(string id)
        {
            throw new NotImplementedException();
        }

        public override string[] GetFeed()
        {
            throw new NotImplementedException();
        }

        public override SdataTransactionResult Add(Sage.Common.Syndication.FeedEntry payload)
        {
            throw new NotImplementedException();
        }

        public override Application.Base.Document GetTransformedDocument(Sage.Common.Syndication.FeedEntry payload)
        {
            throw new NotImplementedException();
        }

        public override Sage.Common.Syndication.FeedEntry GetTransformedPayload(Application.Base.Document document)
        {
            throw new NotImplementedException();
        }

        public string GetDbFieldName(string propertyName)
        {
            throw new NotImplementedException();
        }


    }
}
