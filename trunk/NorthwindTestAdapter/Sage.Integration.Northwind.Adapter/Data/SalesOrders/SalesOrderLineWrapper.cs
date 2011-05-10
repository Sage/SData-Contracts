#region Usings

using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.Entities.Order;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using System;
using System.Data.OleDb;
using System.Data;
using Sage.Integration.Northwind.Adapter.Data.SalesOrders.DataSets.OrderTableAdapters;
using System.ComponentModel;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data.SalesOrders
{
    public class SalesOrderLineWrapper : EntityWrapperBase, IEntityWrapper, IEntityQueryWrapper
    {
                #region Ctor.

        public SalesOrderLineWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.salesOrderLines)
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
            SdataTransactionResult tmpTransactionResult;

            if (!(payload is SalesOrderLinePayload))
            {
                tmpTransactionResult = new SdataTransactionResult();
                tmpTransactionResult.HttpMethod = "PUT";
                tmpTransactionResult.ResourceKind = _resourceKind;
                tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                tmpTransactionResult.HttpMessage = ("salesorder payload missing");
                return tmpTransactionResult;
            }
            salesOrderLinetype soLine = (payload as SalesOrderLinePayload).SalesOrderLinetype;
            DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter = new Order_DetailsTableAdapter();
            DataSets.Order order = new DataSets.Order();

            int productId;
            int orderId;
            if (GetLocalIds(payload.LocalID, out orderId, out productId))
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
                            tmpTransactionResult.LocalId = payload.LocalID;
                            tmpTransactionResult.HttpMethod = "PUT";
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

                                break;
                            }
                        }

                        detailsTableAdapter.Update(order.Order_Details);
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.LocalId = payload.LocalID;
                        tmpTransactionResult.HttpMethod = "PUT";
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.OK;
                        return tmpTransactionResult;
                    }
                    catch (Exception e)
                    {
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.LocalId = payload.LocalID;
                        tmpTransactionResult.HttpMethod = "PUT";
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                        tmpTransactionResult.HttpMessage = e.Message;
                        return tmpTransactionResult;
                    }

                }

            }
            tmpTransactionResult = new SdataTransactionResult();
            tmpTransactionResult.LocalId = payload.LocalID;
            tmpTransactionResult.HttpMethod = "PUT";
            tmpTransactionResult.ResourceKind = _resourceKind;
            tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
            tmpTransactionResult.HttpMessage = "Not found";
            return tmpTransactionResult;
        }

        public override SdataTransactionResult Delete(string id)
        {

            SdataTransactionResult tmpTransactionResult;
            DataSets.OrderTableAdapters.Order_DetailsTableAdapter detailsTableAdapter = new Order_DetailsTableAdapter();
            DataSets.Order order = new DataSets.Order();

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
                            tmpTransactionResult.HttpMethod = "DELETE";
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
                        tmpTransactionResult.HttpMethod = "DELETE";
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.OK;
                        return tmpTransactionResult;
                    }
                    catch (Exception e)
                    {
                        tmpTransactionResult = new SdataTransactionResult();
                        tmpTransactionResult.LocalId = id;
                        tmpTransactionResult.HttpMethod = "DELETE";
                        tmpTransactionResult.ResourceKind = _resourceKind;
                        tmpTransactionResult.HttpStatus = System.Net.HttpStatusCode.BadRequest;
                        tmpTransactionResult.HttpMessage = e.Message;
                        return tmpTransactionResult;
                    }

                }

            }
            tmpTransactionResult = new SdataTransactionResult();
            tmpTransactionResult.LocalId = id;
            tmpTransactionResult.HttpMethod = "DELETE";
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
            string productIdString = localId.Substring(localId.IndexOf("-")+1);
            string orderIdString = localId.Substring(0, localId.IndexOf("-"));
            if (!(Int32.TryParse(productIdString, out productId)))
                productId = 0;

            if (!(Int32.TryParse(orderIdString, out orderId)))
                orderId = 0;

            if (orderId == 0 || productId == 0)
                return false;

            return true;
        }

        public override SyncFeed  GetFeed()
        {
            throw new NotImplementedException();
        }

        public override SyncFeedEntry GetFeedEntry(string id)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region IEntityQueryWrapper Members

        public string GetDbFieldName(string propertyName)
        {
            throw new NotImplementedException();
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
