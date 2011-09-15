using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Sage.Integration.Northwind.Common;
using Sage.Common.Metadata;

namespace Sage.Integration.Northwind.Adapter.Feeds
{

    [XmlType(TypeName = "computePrice", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class ComputePriceFeedEntry : FeedEntry
    {
        private ComputePriceRequestFeedEntry requestField;

        private ComputePriceResponseFeedEntry responseField;

        [SMERelationshipProperty(MinOccurs = 0, IncludeByDefault = true)]
        public ComputePriceRequestFeedEntry request
        {
            get { return requestField; }
            set 
            { 
                requestField = value;
                SetPropertyChanged("request", true);
            }
        }

        [SMERelationshipProperty(MinOccurs = 0, IncludeByDefault = true)]
        public ComputePriceResponseFeedEntry response
        {
            get { return responseField; }
            set 
            {
                responseField = value;
                SetPropertyChanged("response", true);
            }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "computePrice", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    [XmlType(TypeName = "request", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class ComputePriceRequestFeedEntry : FeedEntry
    {
        private PricingDocumentLineFeed pricingDocumentLinesField;

        private TradingAccountFeedEntry tradingAccountField;

        private string currencyField;

        private decimal discountPercentField;

        private decimal netTotalField;

        private decimal discountTotalField;

        private decimal taxTotalField;

        private decimal grossTotalField;

        private string additionalTextField;

        /// <remarks/>
        public TradingAccountFeedEntry tradingAccount
        {
            get
            {
                return this.tradingAccountField;
            }
            set
            {
                this.tradingAccountField = value;
                SetPropertyChanged("tradingAccount", true);
            }
        }


        /// <remarks/>
        public string pricingDocumentCurrency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
                SetPropertyChanged("pricingDocumentCurrency", true);
            }
        }

        
        
        /// <remarks/>
        public decimal discountPercent
        {
            get
            {
                return this.discountPercentField;
            }
            set
            {
                this.discountPercentField = value;
                SetPropertyChanged("discountPercent", true);
            }
        }

        /// <remarks/>
        public decimal netTotal
        {
            get
            {
                return this.netTotalField;
            }
            set
            {
                this.netTotalField = value;
                SetPropertyChanged("netTotal", true);
            }
        }

        /// <remarks/>
        public decimal discountTotal
        {
            get
            {
                return this.discountTotalField;
            }
            set
            {
                this.discountTotalField = value;
                SetPropertyChanged("discountTotal", true);
            }
        }

        /// <remarks/>
        public decimal taxTotal
        {
            get
            {
                return this.taxTotalField;
            }
            set
            {
                this.taxTotalField = value;
                SetPropertyChanged("taxTotal", true);
            }
        }

        /// <remarks/>
        public decimal grossTotal
        {
            get
            {
                return this.grossTotalField;
            }
            set
            {
                this.grossTotalField = value;
                SetPropertyChanged("grossTotal", true);
            }
        }

        /// <remarks/>
        public string additionalText
        {
            get
            {
                return this.additionalTextField;
            }
            set
            {
                this.additionalTextField = value;
                SetPropertyChanged("additionalText", true);
            }
        }

        [SMERelationshipProperty(MinOccurs = 0, IsCollection = true, IncludeByDefault = true)]
        public PricingDocumentLineFeed pricingDocumentLines
        {
            get { return pricingDocumentLinesField; }
            set 
            { 
                pricingDocumentLinesField = value;
                SetPropertyChanged("pricingDocumentLines", true);
            }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "request", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    [XmlType(TypeName = "response", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class ComputePriceResponseFeedEntry : FeedEntry
    {
        private PricingDocumentLineFeed pricingDocumentLinesField;

        private TradingAccountFeedEntry tradingAccountField;

        private string currencyField;

        private decimal discountPercentField;

        private decimal netTotalField;

        private decimal discountTotalField;

        private decimal taxTotalField;

        private decimal grossTotalField;

        private string additionalTextField;

        /// <remarks/>
        public TradingAccountFeedEntry tradingAccount
        {
            get
            {
                return this.tradingAccountField;
            }
            set
            {
                this.tradingAccountField = value;
                SetPropertyChanged("tradingAccount", true);
            }
        }


        /// <remarks/>
        public string pricingDocumentCurrency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
                SetPropertyChanged("pricingDocumentCurrency", true);
            }
        }



        /// <remarks/>
        public decimal discountPercent
        {
            get
            {
                return this.discountPercentField;
            }
            set
            {
                this.discountPercentField = value;
                SetPropertyChanged("discountPercent", true);
            }
        }

        /// <remarks/>
        public decimal netTotal
        {
            get
            {
                return this.netTotalField;
            }
            set
            {
                this.netTotalField = value;
                SetPropertyChanged("netTotal", true);
            }
        }

        /// <remarks/>
        public decimal discountTotal
        {
            get
            {
                return this.discountTotalField;
            }
            set
            {
                this.discountTotalField = value;
                SetPropertyChanged("discountTotal", true);
            }
        }

        /// <remarks/>
        public decimal taxTotal
        {
            get
            {
                return this.taxTotalField;
            }
            set
            {
                this.taxTotalField = value;
                SetPropertyChanged("taxTotal", true);
            }
        }

        /// <remarks/>
        public decimal grossTotal
        {
            get
            {
                return this.grossTotalField;
            }
            set
            {
                this.grossTotalField = value;
                SetPropertyChanged("grossTotal", true);
            }
        }

        /// <remarks/>
        public string additionalText
        {
            get
            {
                return this.additionalTextField;
            }
            set
            {
                this.additionalTextField = value;
                SetPropertyChanged("additionalText", true);
            }
        }

        [SMERelationshipProperty(MinOccurs = 0, IsCollection = true, IncludeByDefault = true)]
        public PricingDocumentLineFeed pricingDocumentLines
        {
            get { return pricingDocumentLinesField; }
            set
            {
                pricingDocumentLinesField = value;
                SetPropertyChanged("pricingDocumentLines", true);
            }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "response", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    [XmlType(TypeName = "pricingDocumentLine", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class PricingDocumentLineFeedEntry : FeedEntry
    {
        private decimal quantityField;

        private decimal enteredPriceField;

        private CommodityFeedEntry commodityField;

        private lineTypeenum pricingDocumentLineTypeField;

        private decimal initialPriceField;

        private decimal actualPriceField;

        private discountTypeenum discountTypeField;

        private decimal discountAmountField;

        private decimal discountPercentField;

        private decimal subtotalDiscountAmountField;

        private decimal subtotalDiscountPercentField;

        private decimal netTotalField;
        
        private decimal chargesTotalField;

        private decimal discountTotalField;

        private decimal priceTaxField;

        private decimal taxTotalField;

        private decimal grossTotalField;

        private priceChangeStatusTypeenum priceChangeStatusField;

        private string shortTextField;

        private string longTextField;

        public decimal quantity
        {
            get { return quantityField; }
            set 
            { 
                quantityField = value;
                SetPropertyChanged("quantity", true);
            }
        }

        public decimal enteredPrice
        {
            get { return enteredPriceField; }
            set 
            { 
                enteredPriceField = value;
                SetPropertyChanged("enteredPrice", true);
            }
        }

        public CommodityFeedEntry commodity
        {
            get { return commodityField; }
            set 
            { 
                commodityField = value;
                SetPropertyChanged("commodity", true);
            }
        }

        public lineTypeenum pricingDocumentLineType
        {
            get { return pricingDocumentLineTypeField; }
            set 
            { 
                pricingDocumentLineTypeField = value;
                SetPropertyChanged("pricingDocumentLineType", true);
            }
        }

        public decimal initialPrice
        {
            get { return initialPriceField; }
            set 
            { 
                initialPriceField = value;
                SetPropertyChanged("initialPrice", true);
            }
        }

        public decimal actualPrice
        {
            get { return actualPriceField; }
            set 
            { 
                actualPriceField = value;
                SetPropertyChanged("actualPrice", true);
            }
        }

        public discountTypeenum discountType
        {
            get { return discountTypeField; }
            set 
            { 
                discountTypeField = value; 
                SetPropertyChanged("discountType", true);
            }
        }

        public decimal discountAmount
        {
            get { return discountAmountField; }
            set 
            {
                discountAmountField = value;
                SetPropertyChanged("discountAmount", true);
            }
        }

        public decimal discountPercent
        {
            get { return discountPercentField; }
            set 
            { 
                discountPercentField = value;
                SetPropertyChanged("discountPercent", true);
            }
        }

        public decimal subtotalDiscountAmount
        {
            get { return subtotalDiscountAmountField; }
            set 
            { 
                subtotalDiscountAmountField = value;
                SetPropertyChanged("subtotalDiscountAmount", true);
            }
        }

        public decimal subtotalDiscountPercent
        {
            get { return subtotalDiscountPercentField; }
            set 
            { 
                subtotalDiscountPercentField = value;
                SetPropertyChanged("subtotalDiscountPercent", true);
            }
        }

        public decimal netTotal
        {
            get { return netTotalField; }
            set 
            { 
                netTotalField = value;
                SetPropertyChanged("netTotal", true);
            }
        }

        public decimal chargesTotal
        {
            get { return chargesTotalField; }
            set
            {
                chargesTotalField = value;
                SetPropertyChanged("chargesTotal", true);
            }
        }

        public decimal discountTotal
        {
            get { return discountTotalField; }
            set
            {
                discountTotalField = value;
                SetPropertyChanged("discountTotal", true);
            }
        }

        public decimal priceTax
        {
            get { return priceTaxField; }
            set
            {
                priceTaxField = value;
                SetPropertyChanged("priceTax", true);
            }
        }

        public decimal taxTotal
        {
            get { return taxTotalField; }
            set
            {
                taxTotalField = value;
                SetPropertyChanged("taxTotal", true);
            }
        }

        public decimal grossTotal
        {
            get { return grossTotalField; }
            set
            {
                grossTotalField = value;
                SetPropertyChanged("grossTotal", true);
            }
        }

        public priceChangeStatusTypeenum priceChangeStatus
        {
            get { return priceChangeStatusField; }
            set
            {
                priceChangeStatusField = value;
                SetPropertyChanged("priceChangeStatus", true);
            }
        }

        public string shortText
        {
            get { return shortTextField; }
            set
            {
                shortTextField = value;
                SetPropertyChanged("shortText", true);
            }
        }

        public string longText
        {
            get { return longTextField; }
            set
            {
                longTextField = value;
                SetPropertyChanged("longText", true);
            }
        }
        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "pricingDocumentLine", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    public class PricingDocumentLineFeed : Feed<PricingDocumentLineFeedEntry> { }


    [XmlType(TypeName = "computePrice", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")]
    public class ComputePriceFeedEntryTemplate : FeedEntry
    {
        public ComputePriceFeedEntryTemplate()
        {
            requestField = new ComputePriceRequestFeedEntry();
            requestField.pricingDocumentLines = new PricingDocumentLineFeed();
            /*PricingDocumentLineFeedEntry entry = new PricingDocumentLineFeedEntry();
            requestField.pricingDocumentLines.Entries.Add(entry);*/
        }

        private ComputePriceRequestFeedEntry requestField;

        [SMERelationshipProperty(MinOccurs = 0, IncludeByDefault = true)]
        public ComputePriceRequestFeedEntry request
        {
            get
            {
                return requestField;
            }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "computePrice", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }
}
