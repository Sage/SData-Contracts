using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using System.IO;
using System.Xml.Schema;
using Sage.Integration.Northwind.Common;
using Sage.Common.Metadata;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    [XmlType(TypeName = "tradingAccount", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class TradingAccountFeedEntry : FeedEntry
    {

        private bool activeField;

        private bool deletedField;

        private supplierFlagenum customerSupplierFlagField;

        private string nameField;

        private System.Nullable<tradingAccountTypeenum> typeField;

        private PostalAddressFeed postalAddressesField;

        private PhoneNumberFeed phonesField;

        private EmailFeed emailsField;

        private ContactFeed contactsField;

        private System.Nullable<bool> deliveryRuleField;

        private string currencyField;

        private System.Nullable<decimal> settlementDiscountAmountField;

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
                SetPropertyChanged("active", true);
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
                SetPropertyChanged("deleted", true);
            }
        }

        /// <remarks/>
        public supplierFlagenum customerSupplierFlag
        {
            get
            {
                return this.customerSupplierFlagField;
            }
            set
            {
                this.customerSupplierFlagField = value;
                SetPropertyChanged("customerSupplierFlag", true);
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
                SetPropertyChanged("name", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<tradingAccountTypeenum> type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                SetPropertyChanged("type", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArray("postalAddresses", IsNullable = true)]
        [SMERelationshipProperty(MinOccurs = 0, IsCollection = true, IncludeByDefault = true, Namespace = Namespaces.northwindNamespace)]
        public PostalAddressFeed postalAddresses
        {
            get
            {
                return this.postalAddressesField;
            }
            set
            {
                this.postalAddressesField = value;
                SetPropertyChanged("postalAddresses", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArray("phones", IsNullable = true, Namespace = Namespaces.northwindNamespace)]
        [SMERelationshipProperty(MinOccurs = 0, IsCollection = true, IncludeByDefault = true, Namespace = Namespaces.northwindNamespace)]
        public PhoneNumberFeed phones
        {
            get
            {
                return this.phonesField;
            }
            set
            {
                this.phonesField = value;
                SetPropertyChanged("phones", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("emails", IsNullable = true)]
        [SMERelationshipProperty(MinOccurs = 0, IsCollection = true, IncludeByDefault = true)]
        public EmailFeed emails
        {
            get
            {
                return this.emailsField;
            }
            set
            {
                this.emailsField = value;
                SetPropertyChanged("emails", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArray("contacts", IsNullable = true)]
        [SMERelationshipProperty(MinOccurs = 0, IsCollection = true, IncludeByDefault = true)]
        public ContactFeed contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
                SetPropertyChanged("contacts", true);
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
                SetPropertyChanged("deliveryRule", true);
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
                SetPropertyChanged("currency", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> settlementDiscountAmount
        {
            get
            {
                return this.settlementDiscountAmountField;
            }
            set
            {
                this.settlementDiscountAmountField = value;
                SetPropertyChanged("settlementDiscountAmount", true);
            }
        }


        public override string ToString()
        {
            return string.Format("{0} - {1}", this.name, this.Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "tradingAccount", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    public class TradingAccountFeed : Feed<TradingAccountFeedEntry> { }

    [XmlType(TypeName = "tradingAccount", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] 
    public class TradingAccountFeedEntryTemplate : FeedEntry
    {

        public bool active
        {
            get { return true; }
        }

        public bool deleted
        {
            get { return false; }
        }

        public supplierFlagenum customerSupplierFlag
        {
            get { return supplierFlagenum.Customer; }
        }

        public override string ToString()
        {
            return string.Format("{0}", this.Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "tradingAccount", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

}
