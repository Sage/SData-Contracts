using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    //UNUSED
    public class BankAccountFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool activeFieldSpecified;

        private bool deletedField;

        private bool deletedFieldSpecified;

        private string labelField;

        private string typeField;

        private string referenceField;

        private string nameField;

        private string descriptionField;

        private string branchIdentifierField;

        private string accountNumberField;

        private string iBANNumberField;

        private string bICSwiftCodeField;

        private string rollNumberField;

        private string currencyField;

        private string operatingCompanyCurrencyField;

        private TradingAccountFeedEntry tradingAccountField;

        private Feed<ContactFeedEntry> contactsField;

        private Feed<PhoneNumberFeedEntry> phonesField;

        private Feed<EmailFeedEntry> emailsField;

        private string websiteField;

        private PostalAddressFeedEntry postalAddressField;

        private string companyReferenceField;

        private System.Nullable<bool> paymentAllowedFlagField;

        private bool paymentAllowedFlagFieldSpecified;

        private string paymentReferenceField;

        private System.Nullable<bool> receiptAllowedFlagField;

        private bool receiptAllowedFlagFieldSpecified;

        private string receiptReferenceField;

        private string reference2Field;

        private decimal balanceField;

        private bool balanceFieldSpecified;

        private System.Nullable<decimal> limitField;

        private bool limitFieldSpecified;

        private Feed<FinancialAccountFeedEntry> financialAccountsField;

        private System.Nullable<System.DateTime> lastStatementDateField;

        private bool lastStatementDateFieldSpecified;

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
        public string branchIdentifier
        {
            get
            {
                return this.branchIdentifierField;
            }
            set
            {
                this.branchIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string accountNumber
        {
            get
            {
                return this.accountNumberField;
            }
            set
            {
                this.accountNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string iBANNumber
        {
            get
            {
                return this.iBANNumberField;
            }
            set
            {
                this.iBANNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string bICSwiftCode
        {
            get
            {
                return this.bICSwiftCodeField;
            }
            set
            {
                this.bICSwiftCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string rollNumber
        {
            get
            {
                return this.rollNumberField;
            }
            set
            {
                this.rollNumberField = value;
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
        [System.Xml.Serialization.XmlElement("phones", IsNullable = true)]
        public Feed<PhoneNumberFeedEntry> phones
        {
            get
            {
                return this.phonesField;
            }
            set
            {
                this.phonesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("emails", IsNullable = true)]
        public Feed<EmailFeedEntry> emails
        {
            get
            {
                return this.emailsField;
            }
            set
            {
                this.emailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string website
        {
            get
            {
                return this.websiteField;
            }
            set
            {
                this.websiteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public PostalAddressFeedEntry postalAddress
        {
            get
            {
                return this.postalAddressField;
            }
            set
            {
                this.postalAddressField = value;
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
        public System.Nullable<bool> paymentAllowedFlag
        {
            get
            {
                return this.paymentAllowedFlagField;
            }
            set
            {
                this.paymentAllowedFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool paymentAllowedFlagSpecified
        {
            get
            {
                return this.paymentAllowedFlagFieldSpecified;
            }
            set
            {
                this.paymentAllowedFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string paymentReference
        {
            get
            {
                return this.paymentReferenceField;
            }
            set
            {
                this.paymentReferenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> receiptAllowedFlag
        {
            get
            {
                return this.receiptAllowedFlagField;
            }
            set
            {
                this.receiptAllowedFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool receiptAllowedFlagSpecified
        {
            get
            {
                return this.receiptAllowedFlagFieldSpecified;
            }
            set
            {
                this.receiptAllowedFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string receiptReference
        {
            get
            {
                return this.receiptReferenceField;
            }
            set
            {
                this.receiptReferenceField = value;
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
        public decimal balance
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> limit
        {
            get
            {
                return this.limitField;
            }
            set
            {
                this.limitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool limitSpecified
        {
            get
            {
                return this.limitFieldSpecified;
            }
            set
            {
                this.limitFieldSpecified = value;
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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> lastStatementDate
        {
            get
            {
                return this.lastStatementDateField;
            }
            set
            {
                this.lastStatementDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lastStatementDateSpecified
        {
            get
            {
                return this.lastStatementDateFieldSpecified;
            }
            set
            {
                this.lastStatementDateFieldSpecified = value;
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
