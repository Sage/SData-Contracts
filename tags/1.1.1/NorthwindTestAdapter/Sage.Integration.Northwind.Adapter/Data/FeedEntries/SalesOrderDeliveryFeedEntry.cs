using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    //UNUSED
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="salesOrderDelivery--type", Namespace="http://schemas.sage.com/crmErp/2008")]
    [System.Xml.Serialization.XmlRootAttribute("salesOrderDelivery", Namespace="http://schemas.sage.com/crmErp/2008", IsNullable=false)]
    public class SalesOrderDeliveryFeedEntry : FeedEntry
    {
        private bool activeField;
        
        private bool activeFieldSpecified;
        
        private bool deletedField;
        
        private bool deletedFieldSpecified;
        
        private string labelField;
        
        //private ResourcePayloadContainerCollection<SalesOrderPayload> salesOrderField;
        
        private string referenceField;
        
        private string reference2Field;
        
        //private System.Nullable<salesOrderDeliveryLineStatusenum> statusField;
        
        private bool statusFieldSpecified;
        
        //private TradingAccountPayload tradingAccountField;
        
        private string customerReferenceField;
        
        //private PriceListPayload pricelistField;
        
        //private TradingAccountPayload supplierTradingAccountField;
        
        private string supplierReferenceField;
        
        private string typeField;
        
        //private PostalAddressPayload postalAddressField;
        
        private string deliveryMethodField;
        
        //private TradingAccountPayload carrierTradingAccountField;
        
        //private taxCodetype[] carrierTaxCodesField;
        
        private string carrierReferenceField;
        
        private System.Nullable<decimal> carrierNetPriceField;
        
        private bool carrierNetPriceFieldSpecified;
        
        private System.Nullable<decimal> carrierTaxPriceField;
        
        private bool carrierTaxPriceFieldSpecified;
        
        private System.Nullable<decimal> carrierTotalPriceField;
        
        private bool carrierTotalPriceFieldSpecified;
        
        //private SalesInvoicePayload carrierSalesInvoiceField;
        
        //private purchaseInvoicetype carrierPurchaseInvoiceField;
        
        private string currencyField;
        
        private string operatingCompanyCurrencyField;
        
        private System.Nullable<decimal> operatingCompanyCurrencyExchangeRateField;
        
        private bool operatingCompanyCurrencyExchangeRateFieldSpecified;
        
        private System.Nullable<rateOperatorenum> operatingCompanyCurrencyExchangeRateOperatorField;
        
        private bool operatingCompanyCurrencyExchangeRateOperatorFieldSpecified;
        
        private System.Nullable<System.DateTime> operatingCompanyCurrencyExchangeRateDateField;
        
        private bool operatingCompanyCurrencyExchangeRateDateFieldSpecified;
        
        private string invoiceCurrencyField;
        
        private System.Nullable<decimal> invoiceCurrencyExchangeRateField;
        
        private bool invoiceCurrencyExchangeRateFieldSpecified;
        
        private System.Nullable<rateOperatorenum> invoiceCurrencyExchangeRateOperatorField;
        
        private bool invoiceCurrencyExchangeRateOperatorFieldSpecified;
        
        private System.Nullable<System.DateTime> invoiceCurrencyExchangeRateDateField;
        
        private bool invoiceCurrencyExchangeRateDateFieldSpecified;
        
        private string customerTradingAccountCurrencyField;
        
        private System.Nullable<decimal> customerTradingAccountCurrencyExchangeRateField;
        
        private bool customerTradingAccountCurrencyExchangeRateFieldSpecified;
        
        private System.Nullable<rateOperatorenum> customerTradingAccountCurrencyExchangeRateOperatorField;
        
        private bool customerTradingAccountCurrencyExchangeRateOperatorFieldSpecified;
        
        private System.Nullable<System.DateTime> customerTradingAccountCurrencyExchangeRateDateField;
        
        private bool customerTradingAccountCurrencyExchangeRateDateFieldSpecified;
        
        private System.Nullable<System.DateTime> requestedDeliveryDateField;
        
        private bool requestedDeliveryDateFieldSpecified;
        
        private System.DateTime actualDeliveryDateField;
        
        private bool actualDeliveryDateFieldSpecified;
        
        private System.Nullable<System.DateTime> actualDeliveryTimeField;
        
        private bool actualDeliveryTimeFieldSpecified;
        
        private string dateExceptionReasonField;
        
        //private taxCodetype[] taxcodesField;
        
        //private ContactPayload deliveryContactField;
        
        //private salesOrderDeliveryLinetype[] salesOrderDeliveryLinesField;
        
        private decimal lineCountField;
        
        private bool lineCountFieldSpecified;
        
        private System.Nullable<decimal> requestedQuantityField;
        
        private bool requestedQuantityFieldSpecified;
        
        private decimal deliveredQuantityField;
        
        private bool deliveredQuantityFieldSpecified;
        
        private System.Nullable<discountTypeenum> deliveryDiscountTypeField;
        
        private bool deliveryDiscountTypeFieldSpecified;
        
        private System.Nullable<decimal> deliveryDiscountAmountField;
        
        private bool deliveryDiscountAmountFieldSpecified;
        
        private System.Nullable<decimal> deliveryDiscountPercentField;
        
        private bool deliveryDiscountPercentFieldSpecified;
        
        private System.Nullable<discountTypeenum> deliveryAdditionalDiscount1TypeField;
        
        private bool deliveryAdditionalDiscount1TypeFieldSpecified;
        
        private System.Nullable<decimal> deliveryAdditionalDiscount1AmountField;
        
        private bool deliveryAdditionalDiscount1AmountFieldSpecified;
        
        private System.Nullable<decimal> deliveryAdditionalDiscount1PercentField;
        
        private bool deliveryAdditionalDiscount1PercentFieldSpecified;
        
        private System.Nullable<discountTypeenum> deliveryAdditionalDiscount2Field;
        
        private bool deliveryAdditionalDiscount2FieldSpecified;
        
        private System.Nullable<decimal> deliveryAdditionalDiscount2AmountField;
        
        private bool deliveryAdditionalDiscount2AmountFieldSpecified;
        
        private System.Nullable<decimal> deliveryAdditionalDiscount2PercentField;
        
        private bool deliveryAdditionalDiscount2PercentFieldSpecified;
        
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
        
        //private receipttype[] receiptsField;
        
        //private TradingAccountPayload invoiceTradingAccountField;
        
        private string invoiceCountryField;
        
        private string deliveryCountryField;
        
        private string originCountryField;
        
        private string typeOfBusinessField;
        
        private string statisticalProcessField;
        
        private string userField;
        
        //private casetype[] casesField;
        
        //private financialAccounttype[] financialAccountsField;
        
        //private interactiontype[] interactionsField;
        
        //private notetype[] notesField;
        
       
        
        /// <remarks/>
        public bool active {
            get {
                return this.activeField;
            }
            set {
                this.activeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool activeSpecified {
            get {
                return this.activeFieldSpecified;
            }
            set {
                this.activeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool deleted {
            get {
                return this.deletedField;
            }
            set {
                this.deletedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deletedSpecified {
            get {
                return this.deletedFieldSpecified;
            }
            set {
                this.deletedFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string label {
            get {
                return this.labelField;
            }
            set {
                this.labelField = value;
            }
        }
        
        /*/// <remarks/>
        //[System.Xml.Serialization.XmlArrayItemAttribute("salesOrder", IsNullable=false)]
        [System.Xml.Serialization.XmlElement("salesOrder", IsNullable = true)]
        public ResourcePayloadContainerCollection<SalesOrderPayload> salesOrder {
            get {
                return this.salesOrderField;
            }
            set {
                this.salesOrderField = value;
            }
        }*/
        
        /// <remarks/>
        public string reference {
            get {
                return this.referenceField;
            }
            set {
                this.referenceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string reference2 {
            get {
                return this.reference2Field;
            }
            set {
                this.reference2Field = value;
            }
        }
        
        /*/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<salesOrderDeliveryLineStatusenum> status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }*/
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool statusSpecified {
            get {
                return this.statusFieldSpecified;
            }
            set {
                this.statusFieldSpecified = value;
            }
        }
        
       /* /// <remarks/>
        public TradingAccountPayload tradingAccount {
            get {
                return this.tradingAccountField;
            }
            set {
                this.tradingAccountField = value;
            }
        }*/
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string customerReference {
            get {
                return this.customerReferenceField;
            }
            set {
                this.customerReferenceField = value;
            }
        }
        
        /*/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public PriceListPayload pricelist {
            get {
                return this.pricelistField;
            }
            set {
                this.pricelistField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public TradingAccountPayload supplierTradingAccount {
            get {
                return this.supplierTradingAccountField;
            }
            set {
                this.supplierTradingAccountField = value;
            }
        }*/
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string supplierReference {
            get {
                return this.supplierReferenceField;
            }
            set {
                this.supplierReferenceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
        
        /*/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public PostalAddressPayload postalAddress {
            get {
                return this.postalAddressField;
            }
            set {
                this.postalAddressField = value;
            }
        }*/
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string deliveryMethod {
            get {
                return this.deliveryMethodField;
            }
            set {
                this.deliveryMethodField = value;
            }
        }
        
        /*/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public TradingAccountPayload carrierTradingAccount {
            get {
                return this.carrierTradingAccountField;
            }
            set {
                this.carrierTradingAccountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("taxCode", IsNullable=false)]
        public taxCodetype[] carrierTaxCodes {
            get {
                return this.carrierTaxCodesField;
            }
            set {
                this.carrierTaxCodesField = value;
            }
        }*/
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string carrierReference {
            get {
                return this.carrierReferenceField;
            }
            set {
                this.carrierReferenceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> carrierNetPrice {
            get {
                return this.carrierNetPriceField;
            }
            set {
                this.carrierNetPriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool carrierNetPriceSpecified {
            get {
                return this.carrierNetPriceFieldSpecified;
            }
            set {
                this.carrierNetPriceFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> carrierTaxPrice {
            get {
                return this.carrierTaxPriceField;
            }
            set {
                this.carrierTaxPriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool carrierTaxPriceSpecified {
            get {
                return this.carrierTaxPriceFieldSpecified;
            }
            set {
                this.carrierTaxPriceFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> carrierTotalPrice {
            get {
                return this.carrierTotalPriceField;
            }
            set {
                this.carrierTotalPriceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool carrierTotalPriceSpecified {
            get {
                return this.carrierTotalPriceFieldSpecified;
            }
            set {
                this.carrierTotalPriceFieldSpecified = value;
            }
        }
        
        /*/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public SalesInvoicePayload carrierSalesInvoice {
            get {
                return this.carrierSalesInvoiceField;
            }
            set {
                this.carrierSalesInvoiceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public purchaseInvoicetype carrierPurchaseInvoice {
            get {
                return this.carrierPurchaseInvoiceField;
            }
            set {
                this.carrierPurchaseInvoiceField = value;
            }
        }*/
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string currency {
            get {
                return this.currencyField;
            }
            set {
                this.currencyField = value;
            }
        }
        
        /// <remarks/>
        public string operatingCompanyCurrency {
            get {
                return this.operatingCompanyCurrencyField;
            }
            set {
                this.operatingCompanyCurrencyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> operatingCompanyCurrencyExchangeRate {
            get {
                return this.operatingCompanyCurrencyExchangeRateField;
            }
            set {
                this.operatingCompanyCurrencyExchangeRateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool operatingCompanyCurrencyExchangeRateSpecified {
            get {
                return this.operatingCompanyCurrencyExchangeRateFieldSpecified;
            }
            set {
                this.operatingCompanyCurrencyExchangeRateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<rateOperatorenum> operatingCompanyCurrencyExchangeRateOperator {
            get {
                return this.operatingCompanyCurrencyExchangeRateOperatorField;
            }
            set {
                this.operatingCompanyCurrencyExchangeRateOperatorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool operatingCompanyCurrencyExchangeRateOperatorSpecified {
            get {
                return this.operatingCompanyCurrencyExchangeRateOperatorFieldSpecified;
            }
            set {
                this.operatingCompanyCurrencyExchangeRateOperatorFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", IsNullable=true)]
        public System.Nullable<System.DateTime> operatingCompanyCurrencyExchangeRateDate {
            get {
                return this.operatingCompanyCurrencyExchangeRateDateField;
            }
            set {
                this.operatingCompanyCurrencyExchangeRateDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool operatingCompanyCurrencyExchangeRateDateSpecified {
            get {
                return this.operatingCompanyCurrencyExchangeRateDateFieldSpecified;
            }
            set {
                this.operatingCompanyCurrencyExchangeRateDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string invoiceCurrency {
            get {
                return this.invoiceCurrencyField;
            }
            set {
                this.invoiceCurrencyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> invoiceCurrencyExchangeRate {
            get {
                return this.invoiceCurrencyExchangeRateField;
            }
            set {
                this.invoiceCurrencyExchangeRateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool invoiceCurrencyExchangeRateSpecified {
            get {
                return this.invoiceCurrencyExchangeRateFieldSpecified;
            }
            set {
                this.invoiceCurrencyExchangeRateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<rateOperatorenum> invoiceCurrencyExchangeRateOperator {
            get {
                return this.invoiceCurrencyExchangeRateOperatorField;
            }
            set {
                this.invoiceCurrencyExchangeRateOperatorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool invoiceCurrencyExchangeRateOperatorSpecified {
            get {
                return this.invoiceCurrencyExchangeRateOperatorFieldSpecified;
            }
            set {
                this.invoiceCurrencyExchangeRateOperatorFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", IsNullable=true)]
        public System.Nullable<System.DateTime> invoiceCurrencyExchangeRateDate {
            get {
                return this.invoiceCurrencyExchangeRateDateField;
            }
            set {
                this.invoiceCurrencyExchangeRateDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool invoiceCurrencyExchangeRateDateSpecified {
            get {
                return this.invoiceCurrencyExchangeRateDateFieldSpecified;
            }
            set {
                this.invoiceCurrencyExchangeRateDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string customerTradingAccountCurrency {
            get {
                return this.customerTradingAccountCurrencyField;
            }
            set {
                this.customerTradingAccountCurrencyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> customerTradingAccountCurrencyExchangeRate {
            get {
                return this.customerTradingAccountCurrencyExchangeRateField;
            }
            set {
                this.customerTradingAccountCurrencyExchangeRateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customerTradingAccountCurrencyExchangeRateSpecified {
            get {
                return this.customerTradingAccountCurrencyExchangeRateFieldSpecified;
            }
            set {
                this.customerTradingAccountCurrencyExchangeRateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<rateOperatorenum> customerTradingAccountCurrencyExchangeRateOperator {
            get {
                return this.customerTradingAccountCurrencyExchangeRateOperatorField;
            }
            set {
                this.customerTradingAccountCurrencyExchangeRateOperatorField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customerTradingAccountCurrencyExchangeRateOperatorSpecified {
            get {
                return this.customerTradingAccountCurrencyExchangeRateOperatorFieldSpecified;
            }
            set {
                this.customerTradingAccountCurrencyExchangeRateOperatorFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", IsNullable=true)]
        public System.Nullable<System.DateTime> customerTradingAccountCurrencyExchangeRateDate {
            get {
                return this.customerTradingAccountCurrencyExchangeRateDateField;
            }
            set {
                this.customerTradingAccountCurrencyExchangeRateDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customerTradingAccountCurrencyExchangeRateDateSpecified {
            get {
                return this.customerTradingAccountCurrencyExchangeRateDateFieldSpecified;
            }
            set {
                this.customerTradingAccountCurrencyExchangeRateDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date", IsNullable=true)]
        public System.Nullable<System.DateTime> requestedDeliveryDate {
            get {
                return this.requestedDeliveryDateField;
            }
            set {
                this.requestedDeliveryDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool requestedDeliveryDateSpecified {
            get {
                return this.requestedDeliveryDateFieldSpecified;
            }
            set {
                this.requestedDeliveryDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime actualDeliveryDate {
            get {
                return this.actualDeliveryDateField;
            }
            set {
                this.actualDeliveryDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actualDeliveryDateSpecified {
            get {
                return this.actualDeliveryDateFieldSpecified;
            }
            set {
                this.actualDeliveryDateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="time", IsNullable=true)]
        public System.Nullable<System.DateTime> actualDeliveryTime {
            get {
                return this.actualDeliveryTimeField;
            }
            set {
                this.actualDeliveryTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actualDeliveryTimeSpecified {
            get {
                return this.actualDeliveryTimeFieldSpecified;
            }
            set {
                this.actualDeliveryTimeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string dateExceptionReason {
            get {
                return this.dateExceptionReasonField;
            }
            set {
                this.dateExceptionReasonField = value;
            }
        }
        
        /*/// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("taxCode", IsNullable=false)]
        public taxCodetype[] taxcodes {
            get {
                return this.taxcodesField;
            }
            set {
                this.taxcodesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public ContactPayload deliveryContact {
            get {
                return this.deliveryContactField;
            }
            set {
                this.deliveryContactField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("salesOrderDeliveryLine", IsNullable=false)]
        public salesOrderDeliveryLinetype[] salesOrderDeliveryLines {
            get {
                return this.salesOrderDeliveryLinesField;
            }
            set {
                this.salesOrderDeliveryLinesField = value;
            }
        }*/
        
        /// <remarks/>
        public decimal lineCount {
            get {
                return this.lineCountField;
            }
            set {
                this.lineCountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lineCountSpecified {
            get {
                return this.lineCountFieldSpecified;
            }
            set {
                this.lineCountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> requestedQuantity {
            get {
                return this.requestedQuantityField;
            }
            set {
                this.requestedQuantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool requestedQuantitySpecified {
            get {
                return this.requestedQuantityFieldSpecified;
            }
            set {
                this.requestedQuantityFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal deliveredQuantity {
            get {
                return this.deliveredQuantityField;
            }
            set {
                this.deliveredQuantityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveredQuantitySpecified {
            get {
                return this.deliveredQuantityFieldSpecified;
            }
            set {
                this.deliveredQuantityFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<discountTypeenum> deliveryDiscountType {
            get {
                return this.deliveryDiscountTypeField;
            }
            set {
                this.deliveryDiscountTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryDiscountTypeSpecified {
            get {
                return this.deliveryDiscountTypeFieldSpecified;
            }
            set {
                this.deliveryDiscountTypeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> deliveryDiscountAmount {
            get {
                return this.deliveryDiscountAmountField;
            }
            set {
                this.deliveryDiscountAmountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryDiscountAmountSpecified {
            get {
                return this.deliveryDiscountAmountFieldSpecified;
            }
            set {
                this.deliveryDiscountAmountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> deliveryDiscountPercent {
            get {
                return this.deliveryDiscountPercentField;
            }
            set {
                this.deliveryDiscountPercentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryDiscountPercentSpecified {
            get {
                return this.deliveryDiscountPercentFieldSpecified;
            }
            set {
                this.deliveryDiscountPercentFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<discountTypeenum> deliveryAdditionalDiscount1Type {
            get {
                return this.deliveryAdditionalDiscount1TypeField;
            }
            set {
                this.deliveryAdditionalDiscount1TypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryAdditionalDiscount1TypeSpecified {
            get {
                return this.deliveryAdditionalDiscount1TypeFieldSpecified;
            }
            set {
                this.deliveryAdditionalDiscount1TypeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> deliveryAdditionalDiscount1Amount {
            get {
                return this.deliveryAdditionalDiscount1AmountField;
            }
            set {
                this.deliveryAdditionalDiscount1AmountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryAdditionalDiscount1AmountSpecified {
            get {
                return this.deliveryAdditionalDiscount1AmountFieldSpecified;
            }
            set {
                this.deliveryAdditionalDiscount1AmountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> deliveryAdditionalDiscount1Percent {
            get {
                return this.deliveryAdditionalDiscount1PercentField;
            }
            set {
                this.deliveryAdditionalDiscount1PercentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryAdditionalDiscount1PercentSpecified {
            get {
                return this.deliveryAdditionalDiscount1PercentFieldSpecified;
            }
            set {
                this.deliveryAdditionalDiscount1PercentFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<discountTypeenum> deliveryAdditionalDiscount2 {
            get {
                return this.deliveryAdditionalDiscount2Field;
            }
            set {
                this.deliveryAdditionalDiscount2Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryAdditionalDiscount2Specified {
            get {
                return this.deliveryAdditionalDiscount2FieldSpecified;
            }
            set {
                this.deliveryAdditionalDiscount2FieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> deliveryAdditionalDiscount2Amount {
            get {
                return this.deliveryAdditionalDiscount2AmountField;
            }
            set {
                this.deliveryAdditionalDiscount2AmountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryAdditionalDiscount2AmountSpecified {
            get {
                return this.deliveryAdditionalDiscount2AmountFieldSpecified;
            }
            set {
                this.deliveryAdditionalDiscount2AmountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> deliveryAdditionalDiscount2Percent {
            get {
                return this.deliveryAdditionalDiscount2PercentField;
            }
            set {
                this.deliveryAdditionalDiscount2PercentField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryAdditionalDiscount2PercentSpecified {
            get {
                return this.deliveryAdditionalDiscount2PercentFieldSpecified;
            }
            set {
                this.deliveryAdditionalDiscount2PercentFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string text1 {
            get {
                return this.text1Field;
            }
            set {
                this.text1Field = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string text2 {
            get {
                return this.text2Field;
            }
            set {
                this.text2Field = value;
            }
        }
        
        /// <remarks/>
        public decimal netTotal {
            get {
                return this.netTotalField;
            }
            set {
                this.netTotalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool netTotalSpecified {
            get {
                return this.netTotalFieldSpecified;
            }
            set {
                this.netTotalFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal discountTotal {
            get {
                return this.discountTotalField;
            }
            set {
                this.discountTotalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool discountTotalSpecified {
            get {
                return this.discountTotalFieldSpecified;
            }
            set {
                this.discountTotalFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> chargesTotal {
            get {
                return this.chargesTotalField;
            }
            set {
                this.chargesTotalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool chargesTotalSpecified {
            get {
                return this.chargesTotalFieldSpecified;
            }
            set {
                this.chargesTotalFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal taxTotal {
            get {
                return this.taxTotalField;
            }
            set {
                this.taxTotalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool taxTotalSpecified {
            get {
                return this.taxTotalFieldSpecified;
            }
            set {
                this.taxTotalFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal grossTotal {
            get {
                return this.grossTotalField;
            }
            set {
                this.grossTotalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool grossTotalSpecified {
            get {
                return this.grossTotalFieldSpecified;
            }
            set {
                this.grossTotalFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> costTotal {
            get {
                return this.costTotalField;
            }
            set {
                this.costTotalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool costTotalSpecified {
            get {
                return this.costTotalFieldSpecified;
            }
            set {
                this.costTotalFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> profitTotal {
            get {
                return this.profitTotalField;
            }
            set {
                this.profitTotalField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool profitTotalSpecified {
            get {
                return this.profitTotalFieldSpecified;
            }
            set {
                this.profitTotalFieldSpecified = value;
            }
        }
        
        /*/// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("receipt", IsNullable=false)]
        public receipttype[] receipts {
            get {
                return this.receiptsField;
            }
            set {
                this.receiptsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public TradingAccountPayload invoiceTradingAccount {
            get {
                return this.invoiceTradingAccountField;
            }
            set {
                this.invoiceTradingAccountField = value;
            }
        }*/
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string invoiceCountry {
            get {
                return this.invoiceCountryField;
            }
            set {
                this.invoiceCountryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string deliveryCountry {
            get {
                return this.deliveryCountryField;
            }
            set {
                this.deliveryCountryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string originCountry {
            get {
                return this.originCountryField;
            }
            set {
                this.originCountryField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string typeOfBusiness {
            get {
                return this.typeOfBusinessField;
            }
            set {
                this.typeOfBusinessField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string statisticalProcess {
            get {
                return this.statisticalProcessField;
            }
            set {
                this.statisticalProcessField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string user {
            get {
                return this.userField;
            }
            set {
                this.userField = value;
            }
        }
        
        /*/// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("case", IsNullable=false)]
        public casetype[] cases {
            get {
                return this.casesField;
            }
            set {
                this.casesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("financialAccount", IsNullable=false)]
        public financialAccounttype[] financialAccounts {
            get {
                return this.financialAccountsField;
            }
            set {
                this.financialAccountsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("interaction", IsNullable=false)]
        public interactiontype[] interactions {
            get {
                return this.interactionsField;
            }
            set {
                this.interactionsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute("note", Namespace="http://schemas.sage.com/sc/2009", IsNullable=false)]
        public notetype[] notes {
            get {
                return this.notesField;
            }
            set {
                this.notesField = value;
            }
        }*/
    }
}
