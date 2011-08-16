using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Common;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    //UNUSED
    public class OpportunityFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool activeFieldSpecified;

        private bool deletedField;

        private bool deletedFieldSpecified;

        private string labelField;

        private System.Nullable<opportunityStatusenum> statusField;

        private bool statusFieldSpecified;

        private System.Nullable<bool> statusFlagField;

        private bool statusFlagFieldSpecified;

        private string statusFlagTextField;

        private string referenceField;

        private string nameField;

        private string descriptionField;

        private string companyReferenceField;

        private string customerReferenceField;

        private TradingAccountFeedEntry customerTradingAccountField;

        private string tradingAccountReferenceField;

        //CommodityFeedEntry
        private Feed<FeedEntry> commoditiesField;

        private string textField;

        private string currencyField;

        private string operatingCompanyCurrencyField;

        private System.Nullable<decimal> operatingCompanyCurrencyExchangeRateField;

        private bool operatingCompanyCurrencyExchangeRateFieldSpecified;

        private System.Nullable<rateOperatorenum> operatingCompanyCurrencyExchangeRateOperatorField;

        private bool operatingCompanyCurrencyExchangeRateOperatorFieldSpecified;

        private System.Nullable<System.DateTime> operatingCompanyCurrencyExchangeRateDateField;

        private bool operatingCompanyCurrencyExchangeRateDateFieldSpecified;

        private Feed<ContactFeedEntry> contactsField;

        private string sourceField;

        //SalesPersonFeedEntry
        private Feed<FeedEntry> salespersonField;

        private System.Nullable<System.DateTime> dateField;

        private bool dateFieldSpecified;

        private System.Nullable<System.DateTime> closeDateField;

        private bool closeDateFieldSpecified;

        private string openDaysField;

        private System.Nullable<opportunityPriorityenum> priorityField;

        private bool priorityFieldSpecified;

        private System.Nullable<opportunityProbabilityIndicatorTypeenum> probabilityIndicatorTypeField;

        private bool probabilityIndicatorTypeFieldSpecified;

        private string probabilityIndicatorstringField;

        private System.Nullable<decimal> probabilityIndicatorPercentField;

        private bool probabilityIndicatorPercentFieldSpecified;

        private string workflowIndicator1Field;

        private string workflowIndicator2Field;

        private System.Nullable<decimal> netTotalField;

        private bool netTotalFieldSpecified;

        private System.Nullable<decimal> discountTotalField;

        private bool discountTotalFieldSpecified;

        private System.Nullable<decimal> chargesTotalField;

        private bool chargesTotalFieldSpecified;

        private System.Nullable<decimal> taxTotalField;

        private bool taxTotalFieldSpecified;

        private System.Nullable<decimal> grossTotalField;

        private bool grossTotalFieldSpecified;

        private System.Nullable<decimal> costTotalField;

        private bool costTotalFieldSpecified;

        private System.Nullable<decimal> profitTotalField;

        private bool profitTotalFieldSpecified;

        private System.Nullable<decimal> quotationNetTotalField;

        private bool quotationNetTotalFieldSpecified;

        private System.Nullable<decimal> quotationDiscountTotalField;

        private bool quotationDiscountTotalFieldSpecified;

        private System.Nullable<decimal> quotationChargesTotalField;

        private bool quotationChargesTotalFieldSpecified;

        private System.Nullable<decimal> quotationTaxTotalField;

        private bool quotationTaxTotalFieldSpecified;

        private System.Nullable<decimal> quotationGrossTotalField;

        private bool quotationGrossTotalFieldSpecified;

        private System.Nullable<decimal> quotationCostTotalField;

        private bool quotationCostTotalFieldSpecified;

        private System.Nullable<decimal> quotationProfitTotalField;

        private bool quotationProfitTotalFieldSpecified;

        private Feed<SalesQuotationFeedEntry> salesQuotationsField;

        private System.Nullable<decimal> orderNetTotalField;

        private bool orderNetTotalFieldSpecified;

        private System.Nullable<decimal> orderDiscountTotalField;

        private bool orderDiscountTotalFieldSpecified;

        private System.Nullable<decimal> orderChargesTotalField;

        private bool orderChargesTotalFieldSpecified;

        private System.Nullable<decimal> orderTaxTotalField;

        private bool orderTaxTotalFieldSpecified;

        private System.Nullable<decimal> orderGrossTotalField;

        private bool orderGrossTotalFieldSpecified;

        private System.Nullable<decimal> orderCostTotalField;

        private bool orderCostTotalFieldSpecified;

        private System.Nullable<decimal> orderProfitTotalField;

        private bool orderProfitTotalFieldSpecified;

        private Feed<SalesOrderFeedEntry> salesOrdersField;

        //InteractionFeedEntry
        private Feed<FeedEntry> interactionsField;

        private bool privacyFlagField;

        private bool privacyFlagFieldSpecified;

        private NoteFeedEntry notesField;



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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<opportunityStatusenum> status
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
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string companyReference
        {
            get
            {
                return this.companyReferenceField;
            }
            set
            {
                this.companyReferenceField = value;
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
        public TradingAccountFeedEntry customerTradingAccount
        {
            get
            {
                return this.customerTradingAccountField;
            }
            set
            {
                this.customerTradingAccountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string tradingAccountReference
        {
            get
            {
                return this.tradingAccountReferenceField;
            }
            set
            {
                this.tradingAccountReferenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("commodities", IsNullable = true)]
        public Feed<FeedEntry> commodities
        {
            get
            {
                return this.commoditiesField;
            }
            set
            {
                this.commoditiesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
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
        [System.Xml.Serialization.XmlElement("contacts", IsNullable = true)]
        public Feed<ContactFeedEntry> contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("source", Namespace = Namespaces.northwindNamespace, IsNullable = true)]
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        public Feed<FeedEntry> salesperson
        {
            get
            {
                return this.salespersonField;
            }
            set
            {
                this.salespersonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> date
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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> closeDate
        {
            get
            {
                return this.closeDateField;
            }
            set
            {
                this.closeDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool closeDateSpecified
        {
            get
            {
                return this.closeDateFieldSpecified;
            }
            set
            {
                this.closeDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string openDays
        {
            get
            {
                return this.openDaysField;
            }
            set
            {
                this.openDaysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<opportunityPriorityenum> priority
        {
            get
            {
                return this.priorityField;
            }
            set
            {
                this.priorityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool prioritySpecified
        {
            get
            {
                return this.priorityFieldSpecified;
            }
            set
            {
                this.priorityFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<opportunityProbabilityIndicatorTypeenum> probabilityIndicatorType
        {
            get
            {
                return this.probabilityIndicatorTypeField;
            }
            set
            {
                this.probabilityIndicatorTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool probabilityIndicatorTypeSpecified
        {
            get
            {
                return this.probabilityIndicatorTypeFieldSpecified;
            }
            set
            {
                this.probabilityIndicatorTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string probabilityIndicatorstring
        {
            get
            {
                return this.probabilityIndicatorstringField;
            }
            set
            {
                this.probabilityIndicatorstringField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> probabilityIndicatorPercent
        {
            get
            {
                return this.probabilityIndicatorPercentField;
            }
            set
            {
                this.probabilityIndicatorPercentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool probabilityIndicatorPercentSpecified
        {
            get
            {
                return this.probabilityIndicatorPercentFieldSpecified;
            }
            set
            {
                this.probabilityIndicatorPercentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string workflowIndicator1
        {
            get
            {
                return this.workflowIndicator1Field;
            }
            set
            {
                this.workflowIndicator1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string workflowIndicator2
        {
            get
            {
                return this.workflowIndicator2Field;
            }
            set
            {
                this.workflowIndicator2Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> netTotal
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> discountTotal
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> taxTotal
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> grossTotal
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
        public System.Nullable<decimal> quotationNetTotal
        {
            get
            {
                return this.quotationNetTotalField;
            }
            set
            {
                this.quotationNetTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quotationNetTotalSpecified
        {
            get
            {
                return this.quotationNetTotalFieldSpecified;
            }
            set
            {
                this.quotationNetTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> quotationDiscountTotal
        {
            get
            {
                return this.quotationDiscountTotalField;
            }
            set
            {
                this.quotationDiscountTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quotationDiscountTotalSpecified
        {
            get
            {
                return this.quotationDiscountTotalFieldSpecified;
            }
            set
            {
                this.quotationDiscountTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> quotationChargesTotal
        {
            get
            {
                return this.quotationChargesTotalField;
            }
            set
            {
                this.quotationChargesTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quotationChargesTotalSpecified
        {
            get
            {
                return this.quotationChargesTotalFieldSpecified;
            }
            set
            {
                this.quotationChargesTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> quotationTaxTotal
        {
            get
            {
                return this.quotationTaxTotalField;
            }
            set
            {
                this.quotationTaxTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quotationTaxTotalSpecified
        {
            get
            {
                return this.quotationTaxTotalFieldSpecified;
            }
            set
            {
                this.quotationTaxTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> quotationGrossTotal
        {
            get
            {
                return this.quotationGrossTotalField;
            }
            set
            {
                this.quotationGrossTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quotationGrossTotalSpecified
        {
            get
            {
                return this.quotationGrossTotalFieldSpecified;
            }
            set
            {
                this.quotationGrossTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> quotationCostTotal
        {
            get
            {
                return this.quotationCostTotalField;
            }
            set
            {
                this.quotationCostTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quotationCostTotalSpecified
        {
            get
            {
                return this.quotationCostTotalFieldSpecified;
            }
            set
            {
                this.quotationCostTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> quotationProfitTotal
        {
            get
            {
                return this.quotationProfitTotalField;
            }
            set
            {
                this.quotationProfitTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool quotationProfitTotalSpecified
        {
            get
            {
                return this.quotationProfitTotalFieldSpecified;
            }
            set
            {
                this.quotationProfitTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        public Feed<SalesQuotationFeedEntry> salesQuotations
        {
            get
            {
                return this.salesQuotationsField;
            }
            set
            {
                this.salesQuotationsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderNetTotal
        {
            get
            {
                return this.orderNetTotalField;
            }
            set
            {
                this.orderNetTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderNetTotalSpecified
        {
            get
            {
                return this.orderNetTotalFieldSpecified;
            }
            set
            {
                this.orderNetTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderDiscountTotal
        {
            get
            {
                return this.orderDiscountTotalField;
            }
            set
            {
                this.orderDiscountTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderDiscountTotalSpecified
        {
            get
            {
                return this.orderDiscountTotalFieldSpecified;
            }
            set
            {
                this.orderDiscountTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderChargesTotal
        {
            get
            {
                return this.orderChargesTotalField;
            }
            set
            {
                this.orderChargesTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderChargesTotalSpecified
        {
            get
            {
                return this.orderChargesTotalFieldSpecified;
            }
            set
            {
                this.orderChargesTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderTaxTotal
        {
            get
            {
                return this.orderTaxTotalField;
            }
            set
            {
                this.orderTaxTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderTaxTotalSpecified
        {
            get
            {
                return this.orderTaxTotalFieldSpecified;
            }
            set
            {
                this.orderTaxTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderGrossTotal
        {
            get
            {
                return this.orderGrossTotalField;
            }
            set
            {
                this.orderGrossTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderGrossTotalSpecified
        {
            get
            {
                return this.orderGrossTotalFieldSpecified;
            }
            set
            {
                this.orderGrossTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderCostTotal
        {
            get
            {
                return this.orderCostTotalField;
            }
            set
            {
                this.orderCostTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderCostTotalSpecified
        {
            get
            {
                return this.orderCostTotalFieldSpecified;
            }
            set
            {
                this.orderCostTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderProfitTotal
        {
            get
            {
                return this.orderProfitTotalField;
            }
            set
            {
                this.orderProfitTotalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool orderProfitTotalSpecified
        {
            get
            {
                return this.orderProfitTotalFieldSpecified;
            }
            set
            {
                this.orderProfitTotalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("salesOrders", IsNullable = true)]
        public Feed<SalesOrderFeedEntry> salesOrders
        {
            get
            {
                return this.salesOrdersField;
            }
            set
            {
                this.salesOrdersField = value;
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
        public bool privacyFlag
        {
            get
            {
                return this.privacyFlagField;
            }
            set
            {
                this.privacyFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool privacyFlagSpecified
        {
            get
            {
                return this.privacyFlagFieldSpecified;
            }
            set
            {
                this.privacyFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public NoteFeedEntry notes
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
