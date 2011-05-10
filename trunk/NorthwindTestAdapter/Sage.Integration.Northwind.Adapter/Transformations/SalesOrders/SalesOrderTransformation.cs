#region Usings

using System;
using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Application.Entities.Order.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.SalesOrders;
using Sage.Sis.Sdata.Sync.Storage;


#endregion

namespace Sage.Integration.Northwind.Adapter.Transformations.SalesOrders
{
    public class SalesOrderTransformation : TransformationBase, ITransformation<OrderDocument, SalesOrderPayload>
    {
        #region Class Variables

        private readonly SalesOrderLineTransformation _salesOrderLineTransformation;

        #endregion

        #region Ctor.

        public SalesOrderTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.salesOrders)
        {
            _salesOrderLineTransformation =
                TransformationFactory.GetTransformation<SalesOrderLineTransformation>(
                SupportedResourceKinds.salesOrderLines, context);
        }

        #endregion

        #region ITransformation<OrderDocument,SalesOrderPayload> Members

        public OrderDocument GetTransformedDocument(SalesOrderPayload payload, List<SyncFeedEntryLink> links)
        {
            OrderDocument document = new OrderDocument();
            salesOrdertype salesOrder = payload.SalesOrdertype;

#warning TODO:
            
            return document;
        }

        public SalesOrderPayload GetTransformedPayload(OrderDocument document, out List<SyncFeedEntryLink> links)
        {
            SalesOrderPayload payload = new SalesOrderPayload();
            salesOrdertype salesOrder = new salesOrdertype();

            #region initial values

            //salesOrder.active = true;
            //salesOrder.activeSpecified = true;
            //salesOrder.allocationStatus = null;
            //salesOrder.allocationStatusSpecified = false;
            //salesOrder.applicationID = null;
            //salesOrder.buyerContact = new contacttype();
            //salesOrder.carrierCompany = new operatingCompanylist();
            //salesOrder.carrierNetPrice = null;
            //salesOrder.carrierNetPriceSpecified = false;
            //salesOrder.carrierPurchaseInvoice = new purchaseInvoicelist();
            //salesOrder.carrierReference = null;
            //salesOrder.carrierSalesInvoice = new salesInvoicelist();
            //salesOrder.carrierTaxCodes = new taxCodelist();
            //salesOrder.carrierTaxPrice = null;
            //salesOrder.carrierTaxPriceSpecified = false;
            //salesOrder.carrierTotalPrice = null;
            //salesOrder.carrierTotalPriceSpecified = false;
            //salesOrder.carrierTradingAccount = new tradingAccountlist();
            //salesOrder.cases = new caselist();
            //salesOrder.chargesTotal = null;
            //salesOrder.chargesTotalSpecified = false;
            //salesOrder.contract = null;
            //salesOrder.copyFlag = false;
            //salesOrder.copyFlagSpecified = false;
            //salesOrder.costTotal = null;
            //salesOrder.costTotalSpecified = false;
            //salesOrder.

            //salesOrder.accountingType = tradingAccountAccountingTypeenum.Unknown;
            //salesOrder.customerSupplierFlag = (document.type.IsNull) ? null : document.type.Value.ToString();
            //salesOrder.active = true;
            //salesOrder.postalAddresses = new postalAddresslist();
            //salesOrder.contacts = new contactlist();
            //salesOrder.phones = new phoneNumberlist();
            //salesOrder.deleted = false;
            //salesOrder.deliveryContact = null;
            //salesOrder.deliveryMethod = null;
            //salesOrder.deliveryRule = false;
            //salesOrder.emails = new emaillist();
            //salesOrder.applicationID = document.Id;
            //salesOrder.uuid = GetUuid(document.Id, document.CrmId).ToString();
            //salesOrder.label = SupportedResourceKinds.TradingAccounts.ToString();
            //salesOrder.name = (document.name.IsNull) ? null : document.name.Value.ToString();

            //Many more things should set to default values

            #endregion

            #region salesorder lines

            int salesOrderLinesCount = document.orderitems.documents.Count;
            salesOrder.salesOrderLines = new salesOrderLinetype[salesOrderLinesCount];
            for (int index = 0; index < salesOrderLinesCount; index++)
            {
                List<SyncFeedEntryLink> dummyLinks;
                LineItemDocument lineItem = document.orderitems.documents[index] as LineItemDocument;
                SalesOrderLinePayload salesOrderLinePayload;
                salesOrderLinePayload = _salesOrderLineTransformation.GetTransformedPayload(lineItem, out dummyLinks);
                salesOrder.salesOrderLines[index] = salesOrderLinePayload.SalesOrderLinetype;
            }

            #endregion

            payload.SalesOrdertype = salesOrder;
            links = new List<SyncFeedEntryLink>();
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.salesOrders, document.Id));
            links.Add(selfLink);

            return payload;
        }

        #endregion
    }
}
