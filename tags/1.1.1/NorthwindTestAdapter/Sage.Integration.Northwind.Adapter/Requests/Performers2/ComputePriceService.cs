using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Adapter.Services;
using Sage.Integration.Northwind.Application.Entities.Product;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Adapter.Services.Data;
using Sage.Common.Syndication;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

namespace Sage.Integration.Northwind.Adapter.Common.Handler
{
    class ComputePriceService
    {
        #region CONSTANTS

        decimal TAX_IN_PERCENT = 7;         // always add tax

        #endregion

        private Messaging.Model.IRequest _request;
        private Adapter.Feeds.ComputePriceFeedEntry _entry;
        private RequestContext _context;

        public ComputePriceService(Messaging.Model.IRequest request, ComputePriceFeedEntry entry)
        {
            this._context = new RequestContext(request.Uri);
            this._request = request;
            this._entry = entry;
        }

        public ComputePriceResponseFeedEntry ComputePrice()
        {
            ComputePriceRequestFeedEntry requestEntry = _entry.request;
            CommodityIdentity[] commodityIds = this.GetCommodityIds(requestEntry);

            /* Create a new instance of computePriceResponsetype and initialize it. */
            ComputePriceResponseFeedEntry responsePrice = new ComputePriceResponseFeedEntry();
            responsePrice.pricingDocumentLines = new PricingDocumentLineFeed();//new PricingDocumentLineFeed(commodityIds.Length);

            // Get the prices for commodities (can include null entries)
            ICommodityPrice[] commodityPrices = this.GetCommodityPriceData(commodityIds);

            // Get the currency converter
            EuroCurrencyConverter currencyConverter = CurrencyConverterFactory.Create(requestEntry);
            
            // document total prices
            decimal documentNetTotal = 0;
            decimal documentTaxTotal = 0;
            decimal documentDiscountTotal = 0;


            // iterate through all product identities. Take care that eventually some items of array 'productIds'
            // could be empty (no product id found in feed entry). While iterating find the corresponding database item in
            // array 'productPrices'.
            for (int i = 0; i < commodityPrices.Length; i++)
            {
                // The quantity of the Commodity ordered on the associated document Line.
                // Hint for ERP developers: All CRM applications will send this value.
                decimal lineQuantity = requestEntry.pricingDocumentLines.Entries[i].quantity;
                if (lineQuantity < 0)
                    throw new ArgumentException(string.Format("Computing price for line item[{0}] failed: Property quantity is less than 0.", i));

                // The entered price of the Commodity on the document Line (e.g. the requested price).
                // Hint for ERP developers: All CRM applications will send this value.
                decimal enteredPrice = requestEntry.pricingDocumentLines.Entries[i].enteredPrice;
                if (enteredPrice < 0)
                    throw new ArgumentException(string.Format("Computing price for line item[{0}] failed: Property enteredPrice is less than 0.", i));

                #region Pricing Docoument Line Global ID

                // Pricing Document Line unique identifier.  The associated document Line unique identifier.
                // Hint for ERP developers: This value may be blank, or it may be filled in and then can be used
                // for auditing/logging on the ERP side.
                responsePrice.pricingDocumentLines.Entries.Add(new PricingDocumentLineFeedEntry());
                responsePrice.pricingDocumentLines.Entries[i].UUID = requestEntry.pricingDocumentLines.Entries[i].UUID;

                #endregion

                #region Commodity

                // The unique identifier of the Commodity used on the associated document Line. 
                // Hint for ERP developers: All CRM products will send this value.
                responsePrice.pricingDocumentLines.Entries[i].commodity = requestEntry.pricingDocumentLines.Entries[i].commodity;

                #endregion

                #region Commodity Variant (not supported)

                // The Variant of the Commodity (e.g. colour or size) on the Sales Order Line.
                //responsePrice.pricingDocumentLines.Entries[i].commodityVariant = requestEntry.pricingDocumentLines.Entries[i].commodityVariant;

                #endregion

                #region Commodity Dimension (not supported)

                // The Dimension of the Commodity on the Sales Order Line.
                //responsePrice.pricingDocumentLines.Entries[i].commodityDimension = requestEntry.pricingDocumentLines.Entries[i].commodityDimension;

                #endregion

                #region Quantity

                // The quantity of the Commodity ordered on the associated document Line.
                // Hint for ERP developers: All CRM applications will send this value.
                responsePrice.pricingDocumentLines.Entries[i].quantity = lineQuantity;

                #endregion

                #region Unit of Measure (not supported)

                // The unique identifier of the Unit of Measure for the Commodity on the associated document Line.
                // Hint for ERP developers: If this is blank use the default uom for the commodity.
                //responsePrice.pricingDocumentLines.Entries[i].unitOfMeasure = requestEntry.pricingDocumentLines.Entries[i].unitOfMeasure;

                #endregion

                #region Document Line Type

                // Enumerator indicating the type of Line on the associated document.
                // Allowable values:
                //      L = Standard Line;
                //      F = Free text (with price);
                //      C = Commentary (without price, e.g. “thank you for your business!”);
                //
                // Hint for ERP developers: All CRM applications will send this value.
                responsePrice.pricingDocumentLines.Entries[i].pricingDocumentLineType = requestEntry.pricingDocumentLines.Entries[i].pricingDocumentLineType;

                #endregion

                #region Fulfillment Location (not supported)

                // The Location to/from which the document Line will be fulfilled (e.g. the warehouse receiving/holding the Commodity).
                // Hint for ERP developers: If this is blank use the default location for the commodity.
                //responsePrice.pricingDocumentLines.Entries[i].fulfillmentLocation = requestEntry.pricingDocumentLines.Entries[i].fulfillmentLocation;

                #endregion

                #region Description (not supported)

                // Textual description of the associated document Line (e.g. the Commodity description, free text or commentary).
                //responsePrice.pricingDocumentLines.Entries[i].description = requestEntry.pricingDocumentLines.Entries[i].description;

                #endregion

                if (commodityPrices[i].IsEmpty())
                    // Currently we throw an exception so that the whole price check fails.
                    // In future perhaps the contract provides an own line based error handling.
                    throw new ApplicationException(string.Format("Computing price for lineItem[{0}] failed: No commodity found in Erp.", i));


                #region Entered Price

                // The entered price of the Commodity on the document Line (e.g. the requested price).
                // Hint for ERP developers: All CRM applications will send this value.
                responsePrice.pricingDocumentLines.Entries[i].enteredPrice = enteredPrice;

                #endregion

                #region Intial Price

                // The initial price of the Commodity on the document Line (e.g. list price, before discounts).
                // Hint for ERP developers: All CRM applications will send this value.
                // TODO: Don't understand hint message.
                responsePrice.pricingDocumentLines.Entries[i].initialPrice = currencyConverter.Convert(commodityPrices[i].UnitPrice);

                #endregion

                // the discount (not currency converted!)
                // Discount
                Discount discount = DiscountFactory.GetDiscount(commodityPrices[i].UnitPrice, lineQuantity);

                // (not currency converted)
                decimal taxAmount = commodityPrices[i].UnitPrice - (commodityPrices[i].UnitPrice * (new decimal(1) - (TAX_IN_PERCENT / new decimal(100))));

                #region Actual Price

                // The actual price of the Commodity on the document Line (e.g. after discounts or adjustments).
                // Hint for ERP developers: Use your ERP pricing rules to fill in this value.
                responsePrice.pricingDocumentLines.Entries[i].actualPrice = currencyConverter.Convert(commodityPrices[i].UnitPrice - discount.Amount);

                #endregion

                #region Discount Type

                // Flag to indicate the type of discount applicable to this Document Line (percent or amount)
                // Hint for ERP developers: Use your ERP pricing rules to fill in this value.
                responsePrice.pricingDocumentLines.Entries[i].discountType = discountTypeenum.Percent;

                #endregion

                #region Discount Amount

                // Discount applicable to the document Line price e.g. the per unit discount.
                // Hint for ERP developers Use your ERP pricing rules to fill in this value.
                responsePrice.pricingDocumentLines.Entries[i].discountAmount = currencyConverter.Convert(discount.Amount);

                #endregion

                #region Discount Percent

                // Discount percent applicable to the document Line e.g. the per unit discount percent.
                // Hint for ERP developers: Use your ERP pricing rules to fill in this value. 
                responsePrice.pricingDocumentLines.Entries[i].discountPercent = discount.Percent;

                #endregion

                #region Subtotal Discount Type (not supported)

                // Flag to indicate the type of discount applicable to a subtotal of Pricing Document lines (percent or amount)
                // of which this Line is part.
                //responsePrice.pricingDocumentLines.Entries[i].subtotalDiscountType = discountTypeenum.Percent;

                #endregion

                #region Subtotal Discount Amount

                // Discount applicable to the Pricing Document Line, expressed as an amount.
                // Hint for ERP developers: Use your ERP pricing rules to fill in this value.
                responsePrice.pricingDocumentLines.Entries[i].subtotalDiscountAmount = currencyConverter.Convert(lineQuantity * discount.Amount);
                
                #endregion

                documentDiscountTotal += responsePrice.pricingDocumentLines.Entries[i].subtotalDiscountAmount;

                #region Subtotal Discount Percent

                // Discount applicable to the Pricing Document  Line, expressed as a percentage.
                responsePrice.pricingDocumentLines.Entries[i].subtotalDiscountPercent = discount.Percent;

                #endregion

                #region Net Total

                // Pricing Document Line Net Total	Total value of the document Line, including discounts, excluding tax (e.g. quantity * actual price)
                responsePrice.pricingDocumentLines.Entries[i].netTotal = currencyConverter.Convert(lineQuantity * (commodityPrices[i].UnitPrice - discount.Amount));
                
                #endregion

                documentNetTotal += responsePrice.pricingDocumentLines.Entries[i].netTotal;

                #region Charges Total (not supported)

                // Total value of any (sur)charges applied to the associated document
                //responsePrice.pricingDocumentLines.Entries[i].chargesTotal = 0;

                #endregion

                #region Discount Total

                // Total value of all discount applied to the document Line (e.g. quantity x Line discount amount).
                // TODO: Don't really understand different to 'Subtotal Discount Amount'
                responsePrice.pricingDocumentLines.Entries[i].discountTotal = currencyConverter.Convert(lineQuantity * discount.Amount);

                #endregion

                #region Tax Code (not supported)

                // ID of the tax code governing the tax regime applicable to this pricing request line.

                #endregion

                #region Price Tax

                //Tax applicable to the document Line price e.g. the per unit tax.
                responsePrice.pricingDocumentLines.Entries[i].priceTax = currencyConverter.Convert(taxAmount);

                #endregion

                #region Tax Total

                // Pricing Document Line Tax Total	Tax applicable to the document Line.
                responsePrice.pricingDocumentLines.Entries[i].taxTotal = currencyConverter.Convert(lineQuantity * taxAmount);

                #endregion

                documentTaxTotal += responsePrice.pricingDocumentLines.Entries[i].taxTotal;

                #region Gross Total

                // Total value of the document Line including tax, discounts and charges.
                responsePrice.pricingDocumentLines.Entries[i].grossTotal = responsePrice.pricingDocumentLines.Entries[i].netTotal + responsePrice.pricingDocumentLines.Entries[i].taxTotal;

                #endregion

                #region Cost Total (not supported)

                // Total cost of the Pricing Document Line.
                //responsePrice.pricingDocumentLines.Entries[i].costTotal = 0;

                #endregion

                #region Profit Total (not supported)

                // Total profit on the Pricing Document Line (e.g. net total – cost total).
                //responsePrice.pricingDocumentLines.Entries[i].profitTotal = 0;

                #endregion

                #region reference (not supported)

                // Any reference returned by the pricing request.
                //responsePrice.pricingDocumentLines.Entries[i].reference = string.Empty;

                #endregion

                #region Price Change Status

                // Pricing Price Change Status	If the pricing service changes the quoted price, the repricing status is used to signify if the price has gone up or down.
                // OK = No Change (default);
                // UP = Price went up;
                // DN = Price went down.

                if (requestEntry.pricingDocumentLines.Entries[i].enteredPrice == responsePrice.pricingDocumentLines.Entries[i].grossTotal)
                    responsePrice.pricingDocumentLines.Entries[i].priceChangeStatus = priceChangeStatusTypeenum.noChange;
                else if (requestEntry.pricingDocumentLines.Entries[i].enteredPrice < responsePrice.pricingDocumentLines.Entries[i].grossTotal)
                    responsePrice.pricingDocumentLines.Entries[i].priceChangeStatus = priceChangeStatusTypeenum.priceUp;
                else
                    responsePrice.pricingDocumentLines.Entries[i].priceChangeStatus = priceChangeStatusTypeenum.priceDown;

                #endregion

                #region Short Text

                // Additional text returned by the pricing request.
                // This is intended to be a short message for display on the same screen as the line item
                if (discount.Amount > 0)
                    responsePrice.pricingDocumentLines.Entries[i].shortText = "Quantity discount applied.";
                else
                    responsePrice.pricingDocumentLines.Entries[i].shortText = string.Empty;

                #endregion

                #region Long Text

                // Additional text returned by the pricing request supported by SLX only.
                // This is intended for display in a pop up dialog in CRM, and may display tables and other formatted information.  It is envisaged that the amount of information returned will be large and so it need to be displayed on a separate screen to the line item.
                // Definition:
                // The Additional Price Information field provides the following information to the CRM system:
                // •	Price Breaks: Price break information that alerts the order-taker whether additional quantities purchased would result in a lower price for the customer
                // •	Additional Discounts: the discounts that the customer could qualify for if specific changes were made to the order
                // •	Any other information related to price that  the ERP system would like to report
                if (lineQuantity < DiscountFactory.DISCOUNT_QUANTITY)
                    responsePrice.pricingDocumentLines.Entries[i].longText = string.Format("A quantity of {0} would result to a lower price of {1}%.", DiscountFactory.DISCOUNT_QUANTITY, DiscountFactory.DISOUNT_IN_PERCENT);
                else
                    responsePrice.pricingDocumentLines.Entries[i].longText = string.Empty;

                #endregion

            }
            #region Trading Account

            responsePrice.tradingAccount = requestEntry.tradingAccount;

            #endregion


            #region Pricing List (not supported)

            // Reference to the Pricing List
            //responsePrice.pricelist = requestEntry.pricelist;

            #endregion

            #region Pricing Shipping Addresses (not supported)

            // Postal (shipping) Address(es) associated with the document.
            //responsePrice.pricingShippingAddress = requestEntry.pricingShippingAddress;

            #endregion

            #region carrierTradingAccount (not supported)

            // Supplier Trading Account that will be the carrier / freight forwarder / deliverer of the document.
            //responsePrice.carrierTradingAccount = requestEntry.carrierTradingAccount;

            #endregion

            #region Carrier Net Price (not supported)

            // The price of Delivery/carriage.
            //responsePrice.carrierNetPrice = requestEntry.carrierNetPrice;

            #endregion

            #region Carrier Tax Price (not supported)

            // The tax on Delivery/carriage.
            //responsePrice.carrierTaxPrice = requestEntry.carrierTaxPrice;

            #endregion

            #region Carrier Gross Price (not supported)

            // The total price of Delivery/carriage (net + tax).
            //responsePrice.carrierGrossPrice = requestEntry.carrierGrossPrice;

            #endregion

            #region Carrier Reference Number (not supported)

            // number as used by the carrier during Delivery e.g. for Delivery tracking.
            //responsePrice.carrierReference = requestEntry.carrierReference;

            #endregion

            #region SalesPerson (not supported)

            // Sales person on the associated document (Sales Order or Quotation)
            //responsePrice.salesPerson = requestEntry.salesPerson;

            #endregion

            #region Buyer Contact (not supported)

            // ID of the contact who raised the associated purchase document (Purchase Order or Requisition).
            //responsePrice.buyer = requestEntry.buyer;

            #endregion

            #region Tax Code (not supported)

            // ID of the tax code governing the tax regime applicable to this pricing request.
            //responsePrice.taxCode = requestEntry.taxCode;

            #endregion

            #region Do Not Change Price (not supported)

            // Flag to indicate if the pricing request should change the price.
            // True means the user is requesting that the pricing request does not change the price.
            // False means the user wants the pricing request to change the price if necessary.
            // Default = False (price service can change Price).
            //responsePrice.doNotChangePrice = false;

            #endregion

            #region Discount Type (always percent! no input supported!)

            // Flag to indicate the type of discount applicable to this document (percent or amount).
            // Hint for ERP developers: Apply standard discounting rules for your ERP system.
            // The type, amount and percent field are linked. If the type of discount is a percentage fill 
            // in percent for the type value, and the percentage in the percent field. If the type of the 
            // discount is an amount fill in amount in the type value and the amount in the amount field.

            //responsePrice.discountType = discountTypeenum.Percent;

            #endregion

            #region Discount Amount (not supported. see Discount Type)

            // Discount applicable to this document expressed as an amount.
            //responsePrice.discountAmount = -1;

            #endregion

            #region Discount Percent

            // Discount applicable to this document expressed as a percent.
            if ((documentNetTotal - documentDiscountTotal) != 0)
            {

                responsePrice.discountPercent = (documentNetTotal / (documentNetTotal - documentDiscountTotal) - 1) * 100;
                responsePrice.discountPercent = Math.Round(responsePrice.discountPercent, 2);
            }
            else
            {
                responsePrice.discountPercent = 0;
            }

            #endregion

            #region Settlement Discount Type (not supported)

            // Flag to indicate the type of settlement discount applicable to the Trading Account (percent or amount)
            //responsePrice.settlementDiscountType = requestEntry.settlementDiscountType;

            #endregion

            #region Settlement Discount Percent (not supported)

            // Settlement discount applicable to the Trading Account expressed as a percent.
            //responsePrice.settlementDiscountPercent = requestEntry.settlementDiscountPercent;

            #endregion

            #region Order Additional Discount1 Type (not supported)

            // Flag to indicate the type of additional discount 1 applicable to this Pricing Document  (percent or amount)
            //responsePrice.additionalDiscount1Type = requestEntry.additionalDiscount1Type;

            #endregion

            #region Order Additional Discount1 Amount (not supported)

            // Additional discount 1 applicable to the Pricing Document expressed as an amount.
            //responsePrice.additionalDiscount1Amount = requestEntry.additionalDiscount1Amount;

            #endregion

            #region Order Additional Discount1 Percent (not supported)

            // Additional discount 1 applicable to the Pricing Document expressed as a percent.
            //responsePrice.additionalDiscount1Percent = requestEntry.additionalDiscount1Percent;

            #endregion

            #region Order Additional Discount2 Type (not supported)

            // Flag to indicate the type of additional discount 2 applicable to this Pricing Document  (percent or amount)
            //responsePrice.additionalDiscount2Type = requestEntry.additionalDiscount2Type;

            #endregion

            #region Order Additional Discount2 Amount (not supported)

            // Additional discount 2 applicable to the Pricing Document expressed as an amount.
            //responsePrice.additionalDiscount2Amount = requestEntry.additionalDiscount2Amount;

            #endregion

            #region Order Additional Discount2 Percent (not supported)

            // Additional discount 2 applicable to the Pricing Document expressed as a percent.
            //responsePrice.additionalDiscount2Percent = requestEntry.additionalDiscount2Percent;

            #endregion

            #region Net Total

            // Total value of the associated document, including discounts, excluding tax.
            responsePrice.netTotal = documentNetTotal;

            #endregion

            #region Discount Total

            // Total value of discounts applied to the associated document (all lines).
            responsePrice.discountTotal = documentDiscountTotal;

            #endregion

            #region Tax Total

            // Total tax applicable to the associated document (all lines).
            responsePrice.taxTotal = documentTaxTotal;

            #endregion

            #region Gross Total

            // Total value of the associated document (all lines) including tax and discounts and charges.
            responsePrice.grossTotal = responsePrice.netTotal + responsePrice.taxTotal;

            #endregion

            #region Cost Total (not supported)

            // Total cost of the Document.
            //responsePrice.costTotal = -1;

            #endregion

            #region Profit Total (not supported)

            // Total profit on the Document(e.g. net total – cost total).
            //responsePrice.profitTotal = -1;

            #endregion

            #region Additional Text

            //  Additional text returned by the pricing request.
            // This is intended to be a short message for display on the same screen as the document. This could display for example
            // textual information on the discount applied.
            if (responsePrice.discountPercent > 0)
                responsePrice.additionalText = "Quantity discount applied on some lines.";
            else
                responsePrice.additionalText = string.Empty;

            #endregion

            return responsePrice;
        }

        private CommodityIdentity[] GetCommodityIds(ComputePriceRequestFeedEntry payloadContainer)
        {
            List<CommodityIdentity> identities = new List<CommodityIdentity>();

            // The commodities are situated under the pricing document lines.
            // The commodities are linked resources. That means that the schema provides a sme:relationship="reference"
            // on the commodity schema node.
            // Example: (<xs:element sme:label="Commodity" name="commodity" type="commodity--type" sme:relationship="reference" minOccurs="0" maxOccurs="1" sme:mandatory="true" >)
            //
            // The commodity payload is an empty payload providing all or some sdata attributes as follows:
            // url, key, uuid, lookup, descriptor.
            //
            // To be able to receive commodity data we need the local commodity ids.
            // So we iterate through all line items of the given entry and read the commodity payload attributes.
            // Dependent on the existence of the attributes an id can be requested in 3 ways:
            // 1. Using attribute key (MS: SHOULD or MUST exist??? cannot find compliance info)
            // 2. Parsing attribute url (MS: SHOULD or MUST exist??? cannot find compliance info)
            // 3. Using the uuid attribute and using the correlation repository. 

            int noOfLines;                      // The number of line items
            CommodityFeedEntry tmpCommodityPayload;

            noOfLines = payloadContainer.pricingDocumentLines.Entries.Count;

            for (int i = 0; i < noOfLines; i++)
            {
                tmpCommodityPayload = payloadContainer.pricingDocumentLines.Entries[i].commodity;

                if (null == tmpCommodityPayload)
                    throw new RequestException(string.Format("Failed to parse feed. Commodity payload missing in pricing document lines[{0}].", i));

                if (!string.IsNullOrEmpty(tmpCommodityPayload.Key))
                {
                    identities.Add(new CommodityIdentity(tmpCommodityPayload.Key));
                    continue;
                }

                if (!string.IsNullOrEmpty(tmpCommodityPayload.Uri))
                {
                    if (tmpCommodityPayload.Uri.StartsWith(_context.DatasetLink + SupportedResourceKinds.commodities.ToString()))
                    {
                        RequestContext tmpRequestContext = new RequestContext(new SDataUri(tmpCommodityPayload.Uri));
                        if (tmpRequestContext.RequestType == RequestType.Resource)
                        {
                            identities.Add(new CommodityIdentity(tmpRequestContext.ResourceKey));
                            continue;   // continue iteration (parse next line item)
                        }
                    }
                }

                if (tmpCommodityPayload.UUID != null && Guid.Empty != tmpCommodityPayload.UUID)
                {
                    Guid uuid = tmpCommodityPayload.UUID;

                    // get the local id using synch correlation (linking)
                    ICorrelatedResSyncInfoStore correlationStore = NorthwindAdapter.StoreLocator.GetCorrelatedResSyncStore(_context.SdataContext);
                    CorrelatedResSyncInfo[] correlations = correlationStore.GetByUuid(SupportedResourceKinds.commodities.ToString(), new Guid[] { uuid });

                    if (correlations.Length == 1)
                    {
                        identities.Add(new CommodityIdentity(correlations[0].LocalId));
                        continue;   // continue iteration (parse next line item)
                    }
                }

                // Add an empty CommodityIdentity if everything failed.
                identities.Add(CommodityIdentity.Empty);
            }
            return identities.ToArray();
        }

        #region Helpers

        private ICommodityPrice[] GetCommodityPriceData(CommodityIdentity[] commodityIdentities)
        {
            // Get pricing data for the products

            // adapter for database access
            Product product = new Product();

            List<string> nonEmptyLocalIds = new List<string>();
            foreach (CommodityIdentity prodId in commodityIdentities)
            {
                if (!prodId.IsEmpty())
                    nonEmptyLocalIds.Add(prodId.CommodityId);
            }

            // database request
            ProductPriceDocument[] dataDocuments = product.GetProductPrices(nonEmptyLocalIds.ToArray(), _context.Config);


            // create collection of commodity prices
            List<ICommodityPrice> commodityPrices = new List<ICommodityPrice>();

            foreach (CommodityIdentity identity in commodityIdentities)
            {
                if (identity.IsEmpty())
                    commodityPrices.Add(CommodityPriceData.Empty);
                else
                {
                    // get the corresponding db document and add a new CommodityPriceData to list
                    bool priceFound = false;
                    foreach (ProductPriceDocument data in dataDocuments)
                    {
                        if (data.ProductId.ToString() == identity.CommodityId)
                        {
                            commodityPrices.Add(new CommodityPriceData(identity, data));
                            priceFound = true;
                            break;
                        }
                    }

                    if (!priceFound)
                        commodityPrices.Add(CommodityPriceData.Empty);
                }
            }

            return commodityPrices.ToArray();
        }

        /*private ProductPriceDocument FindProductPriceById(ProductPriceDocument[] array, string productId)
        {
            foreach (ProductPriceDocument doc in array)
                if (doc.ProductId.ToString() == productId)
                    return doc;

            return null;
        }*/

        #endregion
    }
}
