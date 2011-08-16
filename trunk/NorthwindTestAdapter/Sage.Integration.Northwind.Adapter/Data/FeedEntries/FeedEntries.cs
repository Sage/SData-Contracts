using Sage.Common.Syndication;
using System.Xml.Serialization;
using Sage.Integration.Northwind.Common;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;
using System.IO;
using Sage.Integration.Northwind.Adapter.Common;
using System.Net;
using System;
using Sage.Common.Metadata;

namespace Sage.Integration.Northwind.Adapter.Feeds
{

    [XmlType(TypeName = "email", Namespace = Namespaces.sc)]
    [XmlSchemaProvider("GetSchema")]
    [SMEResource(HasUuid = true)]
    public class EmailFeedEntry : FeedEntry
    {


        private string addressField;

        /// <remarks/>
        public string address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }





    }

    public class EmailFeed : Feed<EmailFeedEntry> { }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "emailType--enum", Namespace = Namespaces.sc)]
    public enum emailTypeenum
    {

        /// <remarks/>
        Business,

        /// <remarks/>
        Personal,

        /// <remarks/>
        Other,

        /// <remarks/>
        Unknown,
    }



    /// <remarks/>
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "postalAddressType--enum", Namespace = Namespaces.sc)]
    public enum postalAddressTypeenum
    {

        /// <remarks/>
        Shipping,

        /// <remarks/>
        Billing,

        /// <remarks/>
        Correspondence,

        /// <remarks/>
        Home,

        /// <remarks/>
        Office,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Work Site")]
        WorkSite,

        /// <remarks/>
        Other,

        /// <remarks/>
        Unknown,
    }

    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "note--type", Namespace = "http://schemas.sage.com/sc/2009")]
    [System.Xml.Serialization.XmlRootAttribute("note", Namespace = "http://schemas.sage.com/sc/2009", IsNullable = false)]
    public partial class NoteFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool activeFieldSpecified;

        private bool deletedField;

        private bool deletedFieldSpecified;

        private string labelField;

        private string referenceField;

        private string reference2Field;

        private string textField;

        private bool privacyFlagField;

        private bool privacyFlagFieldSpecified;

        private System.DateTime dateStampField;

        private bool dateStampFieldSpecified;

        private bool primacyIndicatorField;

        private bool primacyIndicatorFieldSpecified;

        private string userField;



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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime dateStamp
        {
            get
            {
                return this.dateStampField;
            }
            set
            {
                this.dateStampField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dateStampSpecified
        {
            get
            {
                return this.dateStampFieldSpecified;
            }
            set
            {
                this.dateStampFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool primacyIndicator
        {
            get
            {
                return this.primacyIndicatorField;
            }
            set
            {
                this.primacyIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool primacyIndicatorSpecified
        {
            get
            {
                return this.primacyIndicatorFieldSpecified;
            }
            set
            {
                this.primacyIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
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
    }

    public class ContactGroupFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool activeFieldSpecified;

        private bool deletedField;

        private bool deletedFieldSpecified;

        private string labelField;

        private string nameField;

        private string referenceField;

        private string descriptionField;

        private Feed<ContactGroupFeedEntry> associatedContactGroupField;

        private Feed<ContactFeedEntry> contactsField;

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
        public Feed<ContactGroupFeedEntry> associatedContactGroup
        {
            get
            {
                return this.associatedContactGroupField;
            }
            set
            {
                this.associatedContactGroupField = value;
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

    public class FinancialAccountFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool activeFieldSpecified;

        private bool deletedField;

        private bool deletedFieldSpecified;

        private string labelField;

        private string referenceField;

        private string reference2Field;

        //private System.Nullable<financialAccountTypeenum> typeField;

        private bool typeFieldSpecified;

        //private System.Nullable<financialAccountReportingTypeenum> reportingTypeField;

        private bool reportingTypeFieldSpecified;

        //private System.Nullable<financialAccountAccountingTypeenum> accountingTypeField;

        private bool accountingTypeFieldSpecified;

        private string nameField;

        private string descriptionField;

        private string accountField;

        private string analysis01Field;

        private string analysis02Field;

        private string analysis03Field;

        private string analysis04Field;

        private string analysis05Field;

        private string analysis06Field;

        private string analysis07Field;

        private string analysis08Field;

        private string analysis09Field;

        private string analysis10Field;

        private string analysis11Field;

        private string analysis12Field;

        private string analysis13Field;

        private string analysis14Field;

        private string analysis15Field;

        private string currencyField;

        private System.Nullable<decimal> balanceField;

        private bool balanceFieldSpecified;

        private System.Nullable<System.DateTime> balanceDateField;

        private bool balanceDateFieldSpecified;

        private System.Nullable<System.DateTime> lastTransactionDateField;

        private bool lastTransactionDateFieldSpecified;

        private bool primacyIndicatorField;

        private bool primacyIndicatorFieldSpecified;

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

        /*/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<financialAccountTypeenum> type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }*/

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeSpecified
        {
            get
            {
                return this.typeFieldSpecified;
            }
            set
            {
                this.typeFieldSpecified = value;
            }
        }

        /*/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<financialAccountReportingTypeenum> reportingType
        {
            get
            {
                return this.reportingTypeField;
            }
            set
            {
                this.reportingTypeField = value;
            }
        }*/

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool reportingTypeSpecified
        {
            get
            {
                return this.reportingTypeFieldSpecified;
            }
            set
            {
                this.reportingTypeFieldSpecified = value;
            }
        }

        /*/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<financialAccountAccountingTypeenum> accountingType
        {
            get
            {
                return this.accountingTypeField;
            }
            set
            {
                this.accountingTypeField = value;
            }
        }*/

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool accountingTypeSpecified
        {
            get
            {
                return this.accountingTypeFieldSpecified;
            }
            set
            {
                this.accountingTypeFieldSpecified = value;
            }
        }

        /// <remarks/>
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
        public string account
        {
            get
            {
                return this.accountField;
            }
            set
            {
                this.accountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis01
        {
            get
            {
                return this.analysis01Field;
            }
            set
            {
                this.analysis01Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis02
        {
            get
            {
                return this.analysis02Field;
            }
            set
            {
                this.analysis02Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis03
        {
            get
            {
                return this.analysis03Field;
            }
            set
            {
                this.analysis03Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis04
        {
            get
            {
                return this.analysis04Field;
            }
            set
            {
                this.analysis04Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis05
        {
            get
            {
                return this.analysis05Field;
            }
            set
            {
                this.analysis05Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis06
        {
            get
            {
                return this.analysis06Field;
            }
            set
            {
                this.analysis06Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis07
        {
            get
            {
                return this.analysis07Field;
            }
            set
            {
                this.analysis07Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis08
        {
            get
            {
                return this.analysis08Field;
            }
            set
            {
                this.analysis08Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis09
        {
            get
            {
                return this.analysis09Field;
            }
            set
            {
                this.analysis09Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis10
        {
            get
            {
                return this.analysis10Field;
            }
            set
            {
                this.analysis10Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis11
        {
            get
            {
                return this.analysis11Field;
            }
            set
            {
                this.analysis11Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis12
        {
            get
            {
                return this.analysis12Field;
            }
            set
            {
                this.analysis12Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis13
        {
            get
            {
                return this.analysis13Field;
            }
            set
            {
                this.analysis13Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis14
        {
            get
            {
                return this.analysis14Field;
            }
            set
            {
                this.analysis14Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string analysis15
        {
            get
            {
                return this.analysis15Field;
            }
            set
            {
                this.analysis15Field = value;
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> balance
        {
            get
            {
                return this.balanceField;
            }
            set
            {
                this.balanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool balanceSpecified
        {
            get
            {
                return this.balanceFieldSpecified;
            }
            set
            {
                this.balanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> balanceDate
        {
            get
            {
                return this.balanceDateField;
            }
            set
            {
                this.balanceDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool balanceDateSpecified
        {
            get
            {
                return this.balanceDateFieldSpecified;
            }
            set
            {
                this.balanceDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> lastTransactionDate
        {
            get
            {
                return this.lastTransactionDateField;
            }
            set
            {
                this.lastTransactionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lastTransactionDateSpecified
        {
            get
            {
                return this.lastTransactionDateFieldSpecified;
            }
            set
            {
                this.lastTransactionDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool primacyIndicator
        {
            get
            {
                return this.primacyIndicatorField;
            }
            set
            {
                this.primacyIndicatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool primacyIndicatorSpecified
        {
            get
            {
                return this.primacyIndicatorFieldSpecified;
            }
            set
            {
                this.primacyIndicatorFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
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

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "person--type", Namespace = Namespaces.northwindNamespace)]
    [System.Xml.Serialization.XmlRootAttribute("person", Namespace = Namespaces.northwindNamespace, IsNullable = false)]
    public class PersonFeedEntry : FeedEntry {    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "contactCompanyContext--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum contactCompanyContextenum
    {

        /// <remarks/>
        Employee,

        /// <remarks/>
        Owner,

        /// <remarks/>
        Officer,

        /// <remarks/>
        Subcontractor,

        /// <remarks/>
        Agent,

        /// <remarks/>
        Other,

        /// <remarks/>
        Unknown,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "discountType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum discountTypeenum
    {

        /// <remarks/>
        Percent,

        /// <remarks/>
        Amount,

        /// <remarks/>
        Both,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "rateOperator--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum rateOperatorenum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("*")]
        Item,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("/")]
        Item1,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "opportunityStatus--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum opportunityStatusenum
    {

        /// <remarks/>
        Open,

        /// <remarks/>
        Won,

        /// <remarks/>
        Lost,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("In Progress")]
        InProgress,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "salesQuotationStatus--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum salesQuotationStatusenum
    {

        /// <remarks/>
        Entered,

        /// <remarks/>
        Confirmed,

        /// <remarks/>
        Ordered,

        /// <remarks/>
        Cancelled,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "opportunityPriority--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum opportunityPriorityenum
    {

        /// <remarks/>
        High,

        /// <remarks/>
        Medium,

        /// <remarks/>
        Low,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "opportunityProbabilityIndicatorType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum opportunityProbabilityIndicatorTypeenum
    {

        /// <remarks/>
        Percent,

        /// <remarks/>
        String,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "commodityCommissionType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum commodityCommissionTypeenum
    {

        /// <remarks/>
        Percent,

        /// <remarks/>
        Amount,

        /// <remarks/>
        Both,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "tradingAccountType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum tradingAccountTypeenum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Direct Sales/Cash")]
        DirectSalesCash,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("On Account")]
        OnAccount,

        /// <remarks/>
        Subcontract,

        /// <remarks/>
        Unknown,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "priceListType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum priceListTypeenum
    {

        /// <remarks/>
        Selling,

        /// <remarks/>
        Buying,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Buying & Selling")]
        BuyingSelling,

        /// <remarks/>
        Other,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "priceListCalculationType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum priceListCalculationTypeenum
    {

        /// <remarks/>
        Percent,

        /// <remarks/>
        Amount,

        /// <remarks/>
        Both,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "tradingAccountAccountingType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum tradingAccountAccountingTypeenum
    {

        /// <remarks/>
        Cash,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Open Item")]
        OpenItem,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Balance Forward")]
        BalanceForward,

        /// <remarks/>
        Unknown,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "lineType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum lineTypeenum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Standard Line")]
        StandardLine,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Free Text")]
        Freetext,

        /// <remarks/>
        Commentary,

        /// <remarks/>
        Component,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Discount/charge")]
        Discountcharge,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "priceChangeStatusType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum priceChangeStatusTypeenum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("No Change")]
        noChange,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Price Up")]
        priceUp,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Price Down")]
        priceDown,
    }

    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "supplierFlag--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum supplierFlagenum
    {
        Customer,
        Supplier,
        Both,
        Neither,
        Unknown
    }

    public static class FeedEntryHelper
    {
        public static XmlQualifiedName GetSchema(XmlSchemaSet xs, string xmlQualifiedName, string namespaceUri, string schemaResource)
        {
           string schemaResource2 = "Sage.Integration.Northwind.Adapter.crmErp.xsd";

           string schemaResource1 = "Sage.Integration.Northwind.Adapter.common.xsd";


            System.Xml.Serialization.XmlSerializer schemaSerializer = new System.Xml.Serialization.XmlSerializer(typeof(XmlSchema));

            Assembly assembly = Assembly.GetAssembly(typeof(Sage.Integration.Northwind.Adapter.NorthwindAdapter));
            /*XmlSchema xmlschema = null;
            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(schemaResource)))
            {
                xmlschema = XmlSchema.Read(sr, null);
            }
            */
            xs.XmlResolver = new NullXmlResolver();

            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(schemaResource1)))
            {
                XmlReader xmlReader = XmlReader.Create(sr);
                xs.Add(Namespaces.sc, xmlReader);
                xmlReader.Close();
            }


            using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(schemaResource2)))
            {
                XmlReader xmlReader = XmlReader.Create(sr);
                xs.Add(Namespaces.northwindNamespace, xmlReader);
                xmlReader.Close();
            }
            
            return new XmlQualifiedName(xmlQualifiedName, namespaceUri);
        }
    }
    public class NullXmlResolver : XmlResolver
    {
        #region XmlResolver Members

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            return null;
        }

        public override ICredentials Credentials
        {
            set { }
        }

        #endregion
    }
}