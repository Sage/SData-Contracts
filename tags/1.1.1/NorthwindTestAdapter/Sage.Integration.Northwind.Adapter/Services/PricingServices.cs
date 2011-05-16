#region Usings

using System;
using System.Collections.Generic;
using Sage.Integration.Northwind.Application;
using Sage.Integration.Northwind.Application.Entities.Product;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Adapter.Services.Data;


#endregion

namespace Sage.Integration.Northwind.Adapter.Services
{
    internal partial class PricingServices
    {
        #region CONSTANTS

        decimal TAX_IN_PERCENT = 7;         // always add tax

        #endregion

        #region Fields

        private readonly NorthwindConfig _config;

        #endregion

        #region Ctor.

        public PricingServices(NorthwindConfig config)
        {
            _config = config;
        }

        #endregion

        public computePriceResponsetype ComputePrice(computePriceRequesttype computePriceRequest, CommodityIdentity[] commodityIds, bool isSingleItemCheck)
        {
            /* Create a new instance of computePriceResponsetype and initialize it. */
            computePriceResponsetype responsePrice = new computePriceResponsetype();
            responsePrice.pricingDocumentLines = new pricingDocumentLinetype[commodityIds.Length];

            // Get the prices for commodities (can include null entries)
            ICommodityPrice[] commodityPrices = this.GetCommodityPriceData(commodityIds);

            // Get the currency converter
            EuroCurrencyConverter currencyConverter = CurrencyConverterFactory.Create(computePriceRequest);
            
            // document total prices
            decimal documentNetTotal = 0;
            decimal documentTaxTotal = 0;
            decimal documentDiscountTotal = 0;


            // iterate through all product identities. Take care that eventually some items of array 'productIds'
            // could be empty (no product id found in feed entry). While iterating find the corresponding database item in
            // array 'productPrices'.
            for (int i = 0; i < commodityPrices.Length; i++)
            {
                // instantiate the document line
                responsePrice.pricingDocumentLines[i] = new pricingDocumentLinetype();

                // The quantity of the Commodity ordered on the associated document Line.
                // Hint for ERP developers: All CRM applications will send this value.
                decimal lineQuantity = computePriceRequest.pricingDocumentLines[i].quantity;
                if (lineQuantity < 0)
                    throw new ArgumentException(string.Format("Computing price for line item[{0}] failed: Property quantity is less than 0.", i));

                // The entered price of the Commodity on the document Line (e.g. the requested price).
                // Hint for ERP developers: All CRM applications will send this value.
                decimal enteredPrice = computePriceRequest.pricingDocumentLines[i].enteredPrice;
                if (enteredPrice < 0)
                    throw new ArgumentException(string.Format("Computing price for line item[{0}] failed: Property enteredPrice is less than 0.", i));


                #region Pricing Docoument Line Global ID

                // Pricing Document Line unique identifier.  The associated document Line unique identifier.
                // Hint for ERP developers: This value may be blank, or it may be filled in and then can be used
                // for auditing/logging on the ERP side.
                responsePrice.pricingDocumentLines[i].uuid = computePriceRequest.pricingDocumentLines[i].uuid;

                #endregion

                #region Commodity

                // The unique identifier of the Commodity used on the associated document Line. 
                // Hint for ERP developers: All CRM products will send this value.
                responsePrice.pricingDocumentLines[i].commodity = computePriceRequest.pricingDocumentLines[i].commodity;

                #endregion

                #region Commodity Variant (not supported)

                // The Variant of the Commodity (e.g. colour or size) on the Sales Order Line.
                responsePrice.pricingDocumentLines[i].commodityVariant = computePriceRequest.pricingDocumentLines[i].commodityVariant;

                #endregion

                #region Commodity Dimension (not supported)

                // The Dimension of the Commodity on the Sales Order Line.
                responsePrice.pricingDocumentLines[i].commodityDimension = computePriceRequest.pricingDocumentLines[i].commodityDimension;

                #endregion

                #region Quantity

                // The quantity of the Commodity ordered on the associated document Line.
                // Hint for ERP developers: All CRM applications will send this value.
                responsePrice.pricingDocumentLines[i].quantity = lineQuantity;

                #endregion

                #region Unit of Measure (not supported)

                // The unique identifier of the Unit of Measure for the Commodity on the associated document Line.
                // Hint for ERP developers: If this is blank use the default uom for the commodity.
                responsePrice.pricingDocumentLines[i].unitOfMeasure = computePriceRequest.pricingDocumentLines[i].unitOfMeasure;

                #endregion

                #region Document Line Type

                // Enumerator indicating the type of Line on the associated document.
                // Allowable values:
                //      L = Standard Line;
                //      F = Free text (with price);
                //      C = Commentary (without price, e.g. “thank you for your business!”);
                //
                // Hint for ERP developers: All CRM applications will send this value.
                responsePrice.pricingDocumentLines[i].pricingDocumentLineType = computePriceRequest.pricingDocumentLines[i].pricingDocumentLineType;

                #endregion

                #region Fulfillment Location (not supported)

                // The Location to/from which the document Line will be fulfilled (e.g. the warehouse receiving/holding the Commodity).
                // Hint for ERP developers: If this is blank use the default location for the commodity.
                responsePrice.pricingDocumentLines[i].fulfillmentLocation = computePriceRequest.pricingDocumentLines[i].fulfillmentLocation;

                #endregion

                #region Description (not supported)

                // Textual description of the associated document Line (e.g. the Commodity description, free text or commentary).
                responsePrice.pricingDocumentLines[i].description = computePriceRequest.pricingDocumentLines[i].description;

                #endregion

                if (commodityPrices[i].IsEmpty())
                    // Currently we throw an exception so that the whole price check fails.
                    // In future perhaps the contract provides an own line based error handling.
                    throw new ApplicationException(string.Format("Computing price for lineItem[{0}] failed: No commodity found in Erp.", i));


                #region Entered Price

                // The entered price of the Commodity on the document Line (e.g. the requested price).
                // Hint for ERP developers: All CRM applications will send this value.
                responsePrice.pricingDocumentLines[i].enteredPrice = enteredPrice;

                #endregion

                #region Intial Price

                // The initial price of the Commodity on the document Line (e.g. list price, before discounts).
                // Hint for ERP developers: All CRM applications will send this value.
                // TODO: Don't understand hint message.
                responsePrice.pricingDocumentLines[i].initialPrice = currencyConverter.Convert(commodityPrices[i].UnitPrice);

                #endregion

                // the discount (not currency converted!)
                // Discount
                Discount discount = DiscountFactory.GetDiscount(commodityPrices[i].UnitPrice, lineQuantity);

                // (not currency converted)
                decimal taxAmount = commodityPrices[i].UnitPrice - (commodityPrices[i].UnitPrice * (new decimal(1) - (TAX_IN_PERCENT / new decimal(100))));

                #region Actual Price

                // The actual price of the Commodity on the document Line (e.g. after discounts or adjustments).
                // Hint for ERP developers: Use your ERP pricing rules to fill in this value.
                responsePrice.pricingDocumentLines[i].actualPrice = currencyConverter.Convert(commodityPrices[i].UnitPrice - discount.Amount);

                #endregion

                #region Discount Type

                // Flag to indicate the type of discount applicable to this Document Line (percent or amount)
                // Hint for ERP developers: Use your ERP pricing rules to fill in this value.
                responsePrice.pricingDocumentLines[i].discountType = discountTypeenum.Percent;

                #endregion

                #region Discount Amount

                // Discount applicable to the document Line price e.g. the per unit discount.
                // Hint for ERP developers Use your ERP pricing rules to fill in this value.
                responsePrice.pricingDocumentLines[i].discountAmount = currencyConverter.Convert(discount.Amount);

                #endregion

                #region Discount Percent

                // Discount percent applicable to the document Line e.g. the per unit discount percent.
                // Hint for ERP developers: Use your ERP pricing rules to fill in this value. 
                responsePrice.pricingDocumentLines[i].discountPercent = discount.Percent;

                #endregion

                #region Subtotal Discount Type (not supported)

                // Flag to indicate the type of discount applicable to a subtotal of Pricing Document lines (percent or amount)
                // of which this Line is part.
                responsePrice.pricingDocumentLines[i].subtotalDiscountType = discountTypeenum.Percent;

                #endregion

                #region Subtotal Discount Amount

                // Discount applicable to the Pricing Document Line, expressed as an amount.
                // Hint for ERP developers: Use your ERP pricing rules to fill in this value.
                responsePrice.pricingDocumentLines[i].subtotalDiscountAmount = currencyConverter.Convert(lineQuantity * discount.Amount);

                #endregion

                documentDiscountTotal += responsePrice.pricingDocumentLines[i].subtotalDiscountAmount;

                #region Subtotal Discount Percent

                // Discount applicable to the Pricing Document  Line, expressed as a percentage.
                responsePrice.pricingDocumentLines[i].subtotalDiscountPercent = discount.Percent;

                #endregion

                #region Net Total

                // Pricing Document Line Net Total	Total value of the document Line, including discounts, excluding tax (e.g. quantity * actual price)
                responsePrice.pricingDocumentLines[i].netTotal = currencyConverter.Convert(lineQuantity * (commodityPrices[i].UnitPrice - discount.Amount));

                #endregion

                documentNetTotal += responsePrice.pricingDocumentLines[i].netTotal;

                #region Charges Total (not supported)

                // Total value of any (sur)charges applied to the associated document
                responsePrice.pricingDocumentLines[i].chargesTotal = 0;

                #endregion

                #region Discount Total

                // Total value of all discount applied to the document Line (e.g. quantity x Line discount amount).
                // TODO: Don't really understand different to 'Subtotal Discount Amount'
                responsePrice.pricingDocumentLines[i].discountTotal = currencyConverter.Convert(lineQuantity * discount.Amount);

                #endregion

                #region Tax Code (not supported)

                // ID of the tax code governing the tax regime applicable to this pricing request line.

                #endregion

                #region Price Tax

                //Tax applicable to the document Line price e.g. the per unit tax.
                responsePrice.pricingDocumentLines[i].priceTax = currencyConverter.Convert(taxAmount);

                #endregion

                #region Tax Total

                // Pricing Document Line Tax Total	Tax applicable to the document Line.
                responsePrice.pricingDocumentLines[i].taxTotal = currencyConverter.Convert(lineQuantity * taxAmount);

                #endregion

                documentTaxTotal += responsePrice.pricingDocumentLines[i].taxTotal;

                #region Gross Total

                // Total value of the document Line including tax, discounts and charges.
                responsePrice.pricingDocumentLines[i].grossTotal = responsePrice.pricingDocumentLines[i].netTotal + responsePrice.pricingDocumentLines[i].taxTotal;

                #endregion

                #region Cost Total (not supported)

                // Total cost of the Pricing Document Line.
                responsePrice.pricingDocumentLines[i].costTotal = 0;

                #endregion

                #region Profit Total (not supported)

                // Total profit on the Pricing Document Line (e.g. net total – cost total).
                responsePrice.pricingDocumentLines[i].profitTotal = 0;

                #endregion

                #region reference (not supported)

                // Any reference returned by the pricing request.
                responsePrice.pricingDocumentLines[i].reference = string.Empty;

                #endregion

                #region Price Change Status

                // Pricing Price Change Status	If the pricing service changes the quoted price, the repricing status is used to signify if the price has gone up or down.
                // OK = No Change (default);
                // UP = Price went up;
                // DN = Price went down.

                if (computePriceRequest.pricingDocumentLines[i].enteredPrice == responsePrice.pricingDocumentLines[i].grossTotal)
                    responsePrice.pricingDocumentLines[i].priceChangeStatus = priceChangeStatusTypeenum.noChange;
                else if (computePriceRequest.pricingDocumentLines[i].enteredPrice < responsePrice.pricingDocumentLines[i].grossTotal)
                    responsePrice.pricingDocumentLines[i].priceChangeStatus = priceChangeStatusTypeenum.priceUp;
                else
                    responsePrice.pricingDocumentLines[i].priceChangeStatus = priceChangeStatusTypeenum.priceDown;

                #endregion

                #region Short Text

                // Additional text returned by the pricing request.
                // This is intended to be a short message for display on the same screen as the line item
                if (discount.Amount > 0)
                    responsePrice.pricingDocumentLines[i].shortText = "Quantity discount applied.";
                else
                    responsePrice.pricingDocumentLines[i].shortText = string.Empty;

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
                    responsePrice.pricingDocumentLines[i].longText = string.Format("A quantity of {0} would result to a lower price of {1}%.", DiscountFactory.DISCOUNT_QUANTITY, DiscountFactory.DISOUNT_IN_PERCENT);
                else
                    responsePrice.pricingDocumentLines[i].longText = string.Empty;

                #endregion

            }

            #region Trading Account

            responsePrice.tradingAccount = computePriceRequest.tradingAccount;

            #endregion

            #region Pricing Document ID

            responsePrice.pricingDocumentID = computePriceRequest.pricingDocumentID;

            #endregion

            #region Pricing Document Type

            responsePrice.pricingDocumentType = computePriceRequest.pricingDocumentType;

            #endregion

            #region PricingDocumentDate
            
            // The date the document associated with this pricing request was opened.
            // Hint for ERP developers: This field will be sent by all CRM applications.
            responsePrice.pricingDocumentDate = computePriceRequest.pricingDocumentDate;

            #endregion

            #region Pricing Document Status (not supported)

            // Status of the document associated with this pricing request.
            // Hint for ERP developers: This field will be sent by all CRM applications.
            responsePrice.pricingDocumentStatus = string.Empty;

            #endregion

            #region Pricing Document Currency

            // The pricing request currency (e.g. GBP, USD, EUR).
            // ISO 4217
            responsePrice.pricingDocumentCurrency = currencyConverter.Code;

            #endregion

            #region Pricing List (not supported)

            // Reference to the Pricing List
            responsePrice.pricingList = computePriceRequest.pricingList;

            #endregion

            #region Pricing Shipping Addresses (not supported)

            // Postal (shipping) Address(es) associated with the document.
            responsePrice.pricingShippingAddress = computePriceRequest.pricingShippingAddress;

            #endregion

            #region carrierTradingAccount (not supported)

            // Supplier Trading Account that will be the carrier / freight forwarder / deliverer of the document.
            responsePrice.carrierTradingAccount = computePriceRequest.carrierTradingAccount;

            #endregion

            #region Carrier Net Price (not supported)

            // The price of Delivery/carriage.
            responsePrice.carrierNetPrice = computePriceRequest.carrierNetPrice;

            #endregion

            #region Carrier Tax Price (not supported)

            // The tax on Delivery/carriage.
            responsePrice.carrierTaxPrice = computePriceRequest.carrierTaxPrice;

            #endregion

            #region Carrier Gross Price (not supported)

            // The total price of Delivery/carriage (net + tax).
            responsePrice.carrierGrossPrice = computePriceRequest.carrierGrossPrice;
            
            #endregion

            #region Carrier Reference Number (not supported)

            // number as used by the carrier during Delivery e.g. for Delivery tracking.
            responsePrice.carrierReference = computePriceRequest.carrierReference;

            #endregion

            #region SalesPerson (not supported)

            // Sales person on the associated document (Sales Order or Quotation)
            responsePrice.salesPerson = computePriceRequest.salesPerson;

            #endregion

            #region Buyer Contact (not supported)

            // ID of the contact who raised the associated purchase document (Purchase Order or Requisition).
            responsePrice.buyer = computePriceRequest.buyer;

            #endregion

            #region Tax Code (not supported)

            // ID of the tax code governing the tax regime applicable to this pricing request.
            responsePrice.taxCode = computePriceRequest.taxCode;

            #endregion

            #region Do Not Change Price (not supported)

            // Flag to indicate if the pricing request should change the price.
            // True means the user is requesting that the pricing request does not change the price.
            // False means the user wants the pricing request to change the price if necessary.
            // Default = False (price service can change Price).
            responsePrice.doNotChangePrice = false;

            #endregion

            #region Discount Type (always percent! no input supported!)

            // Flag to indicate the type of discount applicable to this document (percent or amount).
            // Hint for ERP developers: Apply standard discounting rules for your ERP system.
            // The type, amount and percent field are linked. If the type of discount is a percentage fill 
            // in percent for the type value, and the percentage in the percent field. If the type of the 
            // discount is an amount fill in amount in the type value and the amount in the amount field.

            responsePrice.discountType = discountTypeenum.Percent;

            #endregion

            #region Discount Amount (not supported. see Discount Type)

            // Discount applicable to this document expressed as an amount.
            responsePrice.discountAmount = -1;

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
            responsePrice.settlementDiscountType = computePriceRequest.settlementDiscountType;

            #endregion

            #region Settlement Discount Percent (not supported)

            // Settlement discount applicable to the Trading Account expressed as a percent.
            responsePrice.settlementDiscountPercent = computePriceRequest.settlementDiscountPercent;

            #endregion

            #region Order Additional Discount1 Type (not supported)

            // Flag to indicate the type of additional discount 1 applicable to this Pricing Document  (percent or amount)
            responsePrice.additionalDiscount1Type = computePriceRequest.additionalDiscount1Type;

            #endregion

            #region Order Additional Discount1 Amount (not supported)

            // Additional discount 1 applicable to the Pricing Document expressed as an amount.
            responsePrice.additionalDiscount1Amount = computePriceRequest.additionalDiscount1Amount;

            #endregion

            #region Order Additional Discount1 Percent (not supported)

            // Additional discount 1 applicable to the Pricing Document expressed as a percent.
            responsePrice.additionalDiscount1Percent = computePriceRequest.additionalDiscount1Percent;

            #endregion

            #region Order Additional Discount2 Type (not supported)

            // Flag to indicate the type of additional discount 2 applicable to this Pricing Document  (percent or amount)
            responsePrice.additionalDiscount2Type = computePriceRequest.additionalDiscount2Type;

            #endregion

            #region Order Additional Discount2 Amount (not supported)

            // Additional discount 2 applicable to the Pricing Document expressed as an amount.
            responsePrice.additionalDiscount2Amount = computePriceRequest.additionalDiscount2Amount;

            #endregion

            #region Order Additional Discount2 Percent (not supported)

            // Additional discount 2 applicable to the Pricing Document expressed as a percent.
            responsePrice.additionalDiscount2Percent = computePriceRequest.additionalDiscount2Percent;

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
            responsePrice.costTotal = -1;

            #endregion

            #region Profit Total (not supported)

            // Total profit on the Document(e.g. net total – cost total).
            responsePrice.profitTotal = -1;

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
            ProductPriceDocument[] dataDocuments = product.GetProductPrices(nonEmptyLocalIds.ToArray(), _config);


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

        private ProductPriceDocument FindProductPriceById(ProductPriceDocument[] array, string productId)
        {
            foreach (ProductPriceDocument doc in array)
                if (doc.ProductId.ToString() == productId)
                    return doc;

            return null;
        }

        #endregion
    }
}
