using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    //UNUSED
    public class SalesQuotationFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool activeFieldSpecified;

        private bool deletedField;

        private bool deletedFieldSpecified;

        private string labelField;

        private string referenceField;

        private string reference2Field;

        private System.Nullable<salesQuotationStatusenum> statusField;

        private bool statusFieldSpecified;

        private System.Nullable<bool> statusFlagField;

        private bool statusFlagFieldSpecified;

        private string statusFlagTextField;

        private TradingAccountFeedEntry tradingAccountField;

        private string customerReferenceField;

        private OpportunityFeedEntry opportunityField;

        private PriceListFeedEntry pricelistField;

        private string typeField;

        private bool copyFlagField;

        private bool copyFlagFieldSpecified;

        private SalesQuotationFeedEntry originatorDocumentField;

        private Feed<PostalAddressFeedEntry> postalAddressesField;

        private string deliveryMethodField;

        private System.Nullable<bool> deliveryRuleField;

        private bool deliveryRuleFieldSpecified;

        private string deliveryTermsField;

        private System.Nullable<System.DateTime> deliveryDateField;

        private bool deliveryDateFieldSpecified;

        private System.Nullable<System.DateTime> dueDateField;

        private bool dueDateFieldSpecified;

        private string currencyField;

        private string operatingCompanyCurrencyField;

        private System.Nullable<decimal> operatingCompanyCurrencyExchangeRateField;

        private bool operatingCompanyCurrencyExchangeRateFieldSpecified;

        private System.Nullable<rateOperatorenum> operatingCompanyCurrencyExchangeRateOperatorField;

        private bool operatingCompanyCurrencyExchangeRateOperatorFieldSpecified;

        private System.Nullable<System.DateTime> operatingCompanyCurrencyExchangeRateDateField;

        private bool operatingCompanyCurrencyExchangeRateDateFieldSpecified;

        private string customerTradingAccountCurrencyField;

        private System.Nullable<decimal> customerTradingAccountCurrencyExchangeRateField;

        private bool customerTradingAccountCurrencyExchangeRateFieldSpecified;

        private System.Nullable<rateOperatorenum> customerTradingAccountCurrencyExchangeRateOperatorField;

        private bool customerTradingAccountCurrencyExchangeRateOperatorFieldSpecified;

        private System.Nullable<System.DateTime> customerTradingAccountCurrencyExchangeRateDateField;

        private bool customerTradingAccountCurrencyExchangeRateDateFieldSpecified;

        private System.DateTime dateField;

        private bool dateFieldSpecified;

        private System.Nullable<System.DateTime> timeField;

        private bool timeFieldSpecified;

        private System.Nullable<decimal> validityField;

        private bool validityFieldSpecified;

        private System.Nullable<System.DateTime> expirationDateField;

        private bool expirationDateFieldSpecified;

        //TaxCodeFeedEntry
        private Feed<FeedEntry> taxCodesField;

        private ContactFeedEntry buyerContactField;

        //SalesPersonFeedEntry
        private Feed<FeedEntry> salespersonsField;

        private string userField;

        private Feed<SalesQuotationFeedEntry> salesQuotationLinesField;

        private decimal lineCountField;

        private bool lineCountFieldSpecified;

        private System.Nullable<discountTypeenum> orderDiscountTypeField;

        private bool orderDiscountTypeFieldSpecified;

        private System.Nullable<decimal> orderDiscountAmountField;

        private bool orderDiscountAmountFieldSpecified;

        private System.Nullable<decimal> orderDiscountPercentField;

        private bool orderDiscountPercentFieldSpecified;

        private System.Nullable<discountTypeenum> orderAdditionalDiscount1TypeField;

        private bool orderAdditionalDiscount1TypeFieldSpecified;

        private System.Nullable<decimal> orderAdditionalDiscount1AmountField;

        private bool orderAdditionalDiscount1AmountFieldSpecified;

        private System.Nullable<decimal> orderAdditionalDiscount1PercentField;

        private bool orderAdditionalDiscount1PercentFieldSpecified;

        private System.Nullable<discountTypeenum> orderAdditionalDiscount2Field;

        private bool orderAdditionalDiscount2FieldSpecified;

        private System.Nullable<decimal> orderAdditionalDiscount2AmountField;

        private bool orderAdditionalDiscount2AmountFieldSpecified;

        private System.Nullable<decimal> orderAdditionalDiscount2PercentField;

        private bool orderAdditionalDiscount2PercentFieldSpecified;

        private string text1Field;

        private string text2Field;

        private decimal netTotalField;

        private bool netTotalFieldSpecified;

        private decimal discountTotalField;

        private bool discountTotalFieldSpecified;

        private System.Nullable<decimal> chargesTotalField;

        private bool chargesTotalFieldSpecified;

        private decimal taxTotalField;

        private bool taxTotalFieldSpecified;

        private decimal grossTotalField;

        private bool grossTotalFieldSpecified;

        private System.Nullable<decimal> costTotalField;

        private bool costTotalFieldSpecified;

        private System.Nullable<decimal> profitTotalField;

        private bool profitTotalFieldSpecified;

        private string contractField;

        //ProjectFeedEntry
        private Feed<FeedEntry> projectsField;

        //ProjectLineFeedEntry
        private Feed<FeedEntry> projectLinesField;

        //CaseFeedEntry
        private Feed<FeedEntry> casesField;

        private Feed<LocationFeedEntry> fulfillmentLocationsField;

        private Feed<FinancialAccountFeedEntry> financialAccountsField;

        //InteractionFeedEntry
        private Feed<FeedEntry> interactionsField;

        private Feed<NoteFeedEntry> notesField;




        /// <remarks/>
        public bool active
        {
            get
            {
                return this.activeField;
            }
            set
            {
                this.activeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool activeSpecified
        {
            get
            {
                return this.activeFieldSpecified;
            }
            set
            {
                this.activeFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool deleted
        {
            get
            {
                return this.deletedField;
            }
            set
            {
                this.deletedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deletedSpecified
        {
            get
            {
                return this.deletedFieldSpecified;
            }
            set
            {
                this.deletedFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        public string reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string reference2
        {
            get
            {
                return this.reference2Field;
            }
            set
            {
                this.reference2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<salesQuotationStatusenum> status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool statusSpecified
        {
            get
            {
                return this.statusFieldSpecified;
            }
            set
            {
                this.statusFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> statusFlag
        {
            get
            {
                return this.statusFlagField;
            }
            set
            {
                this.statusFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool statusFlagSpecified
        {
            get
            {
                return this.statusFlagFieldSpecified;
            }
            set
            {
                this.statusFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string statusFlagText
        {
            get
            {
                return this.statusFlagTextField;
            }
            set
            {
                this.statusFlagTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public TradingAccountFeedEntry tradingAccount
        {
            get
            {
                return this.tradingAccountField;
            }
            set
            {
                this.tradingAccountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string customerReference
        {
            get
            {
                return this.customerReferenceField;
            }
            set
            {
                this.customerReferenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public OpportunityFeedEntry opportunity
        {
            get
            {
                return this.opportunityField;
            }
            set
            {
                this.opportunityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public PriceListFeedEntry pricelist
        {
            get
            {
                return this.pricelistField;
            }
            set
            {
                this.pricelistField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public bool copyFlag
        {
            get
            {
                return this.copyFlagField;
            }
            set
            {
                this.copyFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool copyFlagSpecified
        {
            get
            {
                return this.copyFlagFieldSpecified;
            }
            set
            {
                this.copyFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public SalesQuotationFeedEntry originatorDocument
        {
            get
            {
                return this.originatorDocumentField;
            }
            set
            {
                this.originatorDocumentField = value;
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        //[System.Xml.Serialization.XmlArrayItemAttribute("postalAddress", Namespace="http://schemas.sage.com/sc/2009", IsNullable=false)]
        [System.Xml.Serialization.XmlElement("postalAddresses", IsNullable = true)]
        public Feed<PostalAddressFeedEntry> postalAddresses
        {
            get
            {
                return this.postalAddressesField;
            }
            set
            {
                this.postalAddressesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string deliveryMethod
        {
            get
            {
                return this.deliveryMethodField;
            }
            set
            {
                this.deliveryMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> deliveryRule
        {
            get
            {
                return this.deliveryRuleField;
            }
            set
            {
                this.deliveryRuleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryRuleSpecified
        {
            get
            {
                return this.deliveryRuleFieldSpecified;
            }
            set
            {
                this.deliveryRuleFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string deliveryTerms
        {
            get
            {
                return this.deliveryTermsField;
            }
            set
            {
                this.deliveryTermsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> deliveryDate
        {
            get
            {
                return this.deliveryDateField;
            }
            set
            {
                this.deliveryDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryDateSpecified
        {
            get
            {
                return this.deliveryDateFieldSpecified;
            }
            set
            {
                this.deliveryDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> dueDate
        {
            get
            {
                return this.dueDateField;
            }
            set
            {
                this.dueDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dueDateSpecified
        {
            get
            {
                return this.dueDateFieldSpecified;
            }
            set
            {
                this.dueDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public string operatingCompanyCurrency
        {
            get
            {
                return this.operatingCompanyCurrencyField;
            }
            set
            {
                this.operatingCompanyCurrencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> operatingCompanyCurrencyExchangeRate
        {
            get
            {
                return this.operatingCompanyCurrencyExchangeRateField;
            }
            set
            {
                this.operatingCompanyCurrencyExchangeRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool operatingCompanyCurrencyExchangeRateSpecified
        {
            get
            {
                return this.operatingCompanyCurrencyExchangeRateFieldSpecified;
            }
            set
            {
                this.operatingCompanyCurrencyExchangeRateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<rateOperatorenum> operatingCompanyCurrencyExchangeRateOperator
        {
            get
            {
                return this.operatingCompanyCurrencyExchangeRateOperatorField;
            }
            set
            {
                this.operatingCompanyCurrencyExchangeRateOperatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool operatingCompanyCurrencyExchangeRateOperatorSpecified
        {
            get
            {
                return this.operatingCompanyCurrencyExchangeRateOperatorFieldSpecified;
            }
            set
            {
                this.operatingCompanyCurrencyExchangeRateOperatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> operatingCompanyCurrencyExchangeRateDate
        {
            get
            {
                return this.operatingCompanyCurrencyExchangeRateDateField;
            }
            set
            {
                this.operatingCompanyCurrencyExchangeRateDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool operatingCompanyCurrencyExchangeRateDateSpecified
        {
            get
            {
                return this.operatingCompanyCurrencyExchangeRateDateFieldSpecified;
            }
            set
            {
                this.operatingCompanyCurrencyExchangeRateDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string customerTradingAccountCurrency
        {
            get
            {
                return this.customerTradingAccountCurrencyField;
            }
            set
            {
                this.customerTradingAccountCurrencyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> customerTradingAccountCurrencyExchangeRate
        {
            get
            {
                return this.customerTradingAccountCurrencyExchangeRateField;
            }
            set
            {
                this.customerTradingAccountCurrencyExchangeRateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customerTradingAccountCurrencyExchangeRateSpecified
        {
            get
            {
                return this.customerTradingAccountCurrencyExchangeRateFieldSpecified;
            }
            set
            {
                this.customerTradingAccountCurrencyExchangeRateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<rateOperatorenum> customerTradingAccountCurrencyExchangeRateOperator
        {
            get
            {
                return this.customerTradingAccountCurrencyExchangeRateOperatorField;
            }
            set
            {
                this.customerTradingAccountCurrencyExchangeRateOperatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customerTradingAccountCurrencyExchangeRateOperatorSpecified
        {
            get
            {
                return this.customerTradingAccountCurrencyExchangeRateOperatorFieldSpecified;
            }
            set
            {
                this.customerTradingAccountCurrencyExchangeRateOperatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> customerTradingAccountCurrencyExchangeRateDate
        {
            get
            {
                return this.customerTradingAccountCurrencyExchangeRateDateField;
            }
            set
            {
                this.customerTradingAccountCurrencyExchangeRateDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customerTradingAccountCurrencyExchangeRateDateSpecified
        {
            get
            {
                return this.customerTradingAccountCurrencyExchangeRateDateFieldSpecified;
            }
            set
            {
                this.customerTradingAccountCurrencyExchangeRateDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dateSpecified
        {
            get
            {
                return this.dateFieldSpecified;
            }
            set
            {
                this.dateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "time", IsNullable = true)]
        public System.Nullable<System.DateTime> time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool timeSpecified
        {
            get
            {
                return this.timeFieldSpecified;
            }
            set
            {
                this.timeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> validity
        {
            get
            {
                return this.validityField;
            }
            set
            {
                this.validityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validitySpecified
        {
            get
            {
                return this.validityFieldSpecified;
            }
            set
            {
                this.validityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> expirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool expirationDateSpecified
        {
            get
            {
                return this.expirationDateFieldSpecified;
            }
            set
            {
                this.expirationDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public Feed<FeedEntry> taxCodes
        {
            get
            {
                return this.taxCodesField;
            }
            set
            {
                this.taxCodesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public ContactFeedEntry buyerContact
        {
            get
            {
                return this.buyerContactField;
            }
            set
            {
                this.buyerContactField = value;
            }
        }

        /// <remarks/>
        public Feed<FeedEntry> salespersons
        {
            get
            {
                return this.salespersonsField;
            }
            set
            {
                this.salespersonsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string user
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
            }
        }

        /// <remarks/>
        public Feed<SalesQuotationFeedEntry> salesQuotationLines
        {
            get
            {
                return this.salesQuotationLinesField;
            }
            set
            {
                this.salesQuotationLinesField = value;
            }
        }

        /// <remarks/>
        public decimal lineCount
        {
            get
            {
                return this.lineCountField;
            }
            set
            {
                this.lineCountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lineCountSpecified
        {
            get
            {
                return this.lineCountFieldSpecified;
            }
            set
            {
                this.lineCountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<discountTypeenum> orderDiscountType
        {
            get
            {
                return this.orderDiscountTypeField;
            }
            set
            {
                this.orderDiscountTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderDiscountTypeSpecified
        {
            get
            {
                return this.orderDiscountTypeFieldSpecified;
            }
            set
            {
                this.orderDiscountTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderDiscountAmount
        {
            get
            {
                return this.orderDiscountAmountField;
            }
            set
            {
                this.orderDiscountAmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderDiscountAmountSpecified
        {
            get
            {
                return this.orderDiscountAmountFieldSpecified;
            }
            set
            {
                this.orderDiscountAmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderDiscountPercent
        {
            get
            {
                return this.orderDiscountPercentField;
            }
            set
            {
                this.orderDiscountPercentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderDiscountPercentSpecified
        {
            get
            {
                return this.orderDiscountPercentFieldSpecified;
            }
            set
            {
                this.orderDiscountPercentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<discountTypeenum> orderAdditionalDiscount1Type
        {
            get
            {
                return this.orderAdditionalDiscount1TypeField;
            }
            set
            {
                this.orderAdditionalDiscount1TypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderAdditionalDiscount1TypeSpecified
        {
            get
            {
                return this.orderAdditionalDiscount1TypeFieldSpecified;
            }
            set
            {
                this.orderAdditionalDiscount1TypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderAdditionalDiscount1Amount
        {
            get
            {
                return this.orderAdditionalDiscount1AmountField;
            }
            set
            {
                this.orderAdditionalDiscount1AmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderAdditionalDiscount1AmountSpecified
        {
            get
            {
                return this.orderAdditionalDiscount1AmountFieldSpecified;
            }
            set
            {
                this.orderAdditionalDiscount1AmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderAdditionalDiscount1Percent
        {
            get
            {
                return this.orderAdditionalDiscount1PercentField;
            }
            set
            {
                this.orderAdditionalDiscount1PercentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderAdditionalDiscount1PercentSpecified
        {
            get
            {
                return this.orderAdditionalDiscount1PercentFieldSpecified;
            }
            set
            {
                this.orderAdditionalDiscount1PercentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<discountTypeenum> orderAdditionalDiscount2
        {
            get
            {
                return this.orderAdditionalDiscount2Field;
            }
            set
            {
                this.orderAdditionalDiscount2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderAdditionalDiscount2Specified
        {
            get
            {
                return this.orderAdditionalDiscount2FieldSpecified;
            }
            set
            {
                this.orderAdditionalDiscount2FieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderAdditionalDiscount2Amount
        {
            get
            {
                return this.orderAdditionalDiscount2AmountField;
            }
            set
            {
                this.orderAdditionalDiscount2AmountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderAdditionalDiscount2AmountSpecified
        {
            get
            {
                return this.orderAdditionalDiscount2AmountFieldSpecified;
            }
            set
            {
                this.orderAdditionalDiscount2AmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderAdditionalDiscount2Percent
        {
            get
            {
                return this.orderAdditionalDiscount2PercentField;
            }
            set
            {
                this.orderAdditionalDiscount2PercentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderAdditionalDiscount2PercentSpecified
        {
            get
            {
                return this.orderAdditionalDiscount2PercentFieldSpecified;
            }
            set
            {
                this.orderAdditionalDiscount2PercentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string text1
        {
            get
            {
                return this.text1Field;
            }
            set
            {
                this.text1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string text2
        {
            get
            {
                return this.text2Field;
            }
            set
            {
                this.text2Field = value;
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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool netTotalSpecified
        {
            get
            {
                return this.netTotalFieldSpecified;
            }
            set
            {
                this.netTotalFieldSpecified = value;
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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool discountTotalSpecified
        {
            get
            {
                return this.discountTotalFieldSpecified;
            }
            set
            {
                this.discountTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> chargesTotal
        {
            get
            {
                return this.chargesTotalField;
            }
            set
            {
                this.chargesTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool chargesTotalSpecified
        {
            get
            {
                return this.chargesTotalFieldSpecified;
            }
            set
            {
                this.chargesTotalFieldSpecified = value;
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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool taxTotalSpecified
        {
            get
            {
                return this.taxTotalFieldSpecified;
            }
            set
            {
                this.taxTotalFieldSpecified = value;
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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool grossTotalSpecified
        {
            get
            {
                return this.grossTotalFieldSpecified;
            }
            set
            {
                this.grossTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> costTotal
        {
            get
            {
                return this.costTotalField;
            }
            set
            {
                this.costTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costTotalSpecified
        {
            get
            {
                return this.costTotalFieldSpecified;
            }
            set
            {
                this.costTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> profitTotal
        {
            get
            {
                return this.profitTotalField;
            }
            set
            {
                this.profitTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool profitTotalSpecified
        {
            get
            {
                return this.profitTotalFieldSpecified;
            }
            set
            {
                this.profitTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string contract
        {
            get
            {
                return this.contractField;
            }
            set
            {
                this.contractField = value;
            }
        }

        /// <remarks/>
        public Feed<FeedEntry> projects
        {
            get
            {
                return this.projectsField;
            }
            set
            {
                this.projectsField = value;
            }
        }

        /// <remarks/>
        public Feed<FeedEntry> projectLines
        {
            get
            {
                return this.projectLinesField;
            }
            set
            {
                this.projectLinesField = value;
            }
        }

        /// <remarks/>
        public Feed<FeedEntry> cases
        {
            get
            {
                return this.casesField;
            }
            set
            {
                this.casesField = value;
            }
        }

        /// <remarks/>
        public Feed<LocationFeedEntry> fulfillmentLocations
        {
            get
            {
                return this.fulfillmentLocationsField;
            }
            set
            {
                this.fulfillmentLocationsField = value;
            }
        }

        /// <remarks/>
        public Feed<FinancialAccountFeedEntry> financialAccounts
        {
            get
            {
                return this.financialAccountsField;
            }
            set
            {
                this.financialAccountsField = value;
            }
        }

        /// <remarks/>
        public Feed<FeedEntry> interactions
        {
            get
            {
                return this.interactionsField;
            }
            set
            {
                this.interactionsField = value;
            }
        }

        /// <remarks/>
        public Feed<NoteFeedEntry> notes
        {
            get
            {
                return this.notesField;
            }
            set
            {
                this.notesField = value;
            }
        }
    }
}
