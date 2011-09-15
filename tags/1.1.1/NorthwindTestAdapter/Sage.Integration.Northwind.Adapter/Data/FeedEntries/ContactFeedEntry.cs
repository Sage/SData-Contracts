using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using System.Xml.Serialization;
using Sage.Integration.Northwind.Common;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Reflection;
using Sage.Common.Metadata;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    [XmlType(TypeName = "contact", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class ContactFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool deletedField;

        private string fullNameField;

        //private string salutationField;

        private string firstNameField;

        private string familyNameField;

        private string titleField;

        private string middleNameField;

        private string suffixField;

        private TradingAccountFeedEntry tradingAccountField;

        private bool primacyIndicatorField;

        public override bool ContainsPayload
        {
            get
            {
                return GetChangedProperties() != null && GetChangedProperties().Length > 0;
            }
        }

        /// <remarks/>
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public bool active
        {
            get
            {
                return this.activeField;
            }
            set
            {
                this.activeField = value;
                base.SetPropertyChanged("active", true);
            }
        }

        /// <remarks/>
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public bool deleted
        {
            get
            {
                return this.deletedField;
            }
            set
            {
                this.deletedField = value;
                base.SetPropertyChanged("deleted", true);
            }
        }

        /// <remarks/>
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public string fullName
        {
            get
            {
                return this.fullNameField;
            }
            set
            {
                this.fullNameField = value;
                base.SetPropertyChanged("fullName", true);
            }
        }

        /// <remarks/>
        /*[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public string salutation
        {
            get
            {
                return this.salutationField;
            }
            set
            {
                this.salutationField = value;
                base.SetPropertyChanged("salutation", true);
            }
        }*/

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public string firstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
                base.SetPropertyChanged("firstName", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public string familyName
        {
            get
            {
                return this.familyNameField;
            }
            set
            {
                this.familyNameField = value;
                base.SetPropertyChanged("familyName", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
                base.SetPropertyChanged("title", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public string middleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
                base.SetPropertyChanged("middleName", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public string suffix
        {
            get
            {
                return this.suffixField;
            }
            set
            {
                this.suffixField = value;
                base.SetPropertyChanged("suffix", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public TradingAccountFeedEntry tradingAccount
        {
            get
            {
                return this.tradingAccountField;
            }
            set
            {
                this.tradingAccountField = value;
                base.SetPropertyChanged("tradingAccount", true);
            }
        }

        /// <remarks/>
        [Sage.Common.Metadata.SMEBoolProperty(IsReadOnly = true)]
        public bool primacyIndicator
        {
            get
            {
                return this.primacyIndicatorField;
            }
            set
            {
                this.primacyIndicatorField = value;
                SetPropertyChanged("primacyIndicator", true);
            }
        }

        public override string ToString()
        {
            return String.Format("{0} {1} - {2}", this.firstName, this.familyName, this.Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "contact", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    public class ContactFeed : Feed<ContactFeedEntry> { }

    [XmlType(TypeName = "contact", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")]
    public class ContactFeedEntryTemplate : FeedEntry
    {
        public bool primacyIndicator
        {
            get
            {
                return true;
            }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "contact", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    [XmlType(TypeName = "contact", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class ContactFeedEntryEmpty : FeedEntry
    {
        
        public bool primacyIndicator;
        

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "contact", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }
}
