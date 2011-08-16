using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Common;
using System.Xml;
using System.Xml.Schema;
using Sage.Common.Metadata;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    [XmlType(TypeName = "phoneNumber", Namespace = Namespaces.sc)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class PhoneNumberFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool deletedField;

        private phoneNumberTypeenum typeField;

        private string textField;

        private string countryCodeField;

        private string areaCodeField;

        private string numberField;

        private string extensionField;

        private bool primacyIndicatorField;


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
        public phoneNumberTypeenum type
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
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
                SetPropertyChanged("text", true);
            }
        }

        /// <remarks/>
        public string countryCode
        {
            get
            {
                return this.countryCodeField;
            }
            set
            {
                this.countryCodeField = value;
                SetPropertyChanged("countryCode", true);
            }
        }

        /// <remarks/>
        public string areaCode
        {
            get
            {
                return this.areaCodeField;
            }
            set
            {
                this.areaCodeField = value;
                SetPropertyChanged("areaCode", true);
            }
        }

        /// <remarks/>
        public string number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
                SetPropertyChanged("number", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string extension
        {
            get
            {
                return this.extensionField;
            }
            set
            {
                this.extensionField = value;
                SetPropertyChanged("extension", true);
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
                SetPropertyChanged("primacyIndicator", true);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.text, this.Id); 
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "phoneNumber", Namespaces.sc, "Sage.Integration.Northwind.Adapter.common.xsd");
        }

    }

    public class PhoneNumberFeed : Feed<PhoneNumberFeedEntry>{}

    public enum phoneNumberTypeenum
    {
        [System.Xml.Serialization.XmlEnumAttribute("Business Phone")]
        BusinessPhone,

        [System.Xml.Serialization.XmlEnumAttribute("Business Fax")]
        BusinessFax,

        [System.Xml.Serialization.XmlEnumAttribute("Business Mobile")]
        BusinessMobile,

        [System.Xml.Serialization.XmlEnumAttribute("Personal Phone")]
        PersonalPhone,

        [System.Xml.Serialization.XmlEnumAttribute("Personal Fax")]
        PersonalFax,

        [System.Xml.Serialization.XmlEnumAttribute("Personal Mobile")]
        PersonalMobile,

        [System.Xml.Serialization.XmlEnumAttribute("Pager")]
        Pager,

        [System.Xml.Serialization.XmlEnumAttribute("Toll Free")]
        TollFree,

        [System.Xml.Serialization.XmlEnumAttribute("Other Phone")]
        OtherPhone,

        [System.Xml.Serialization.XmlEnumAttribute("Other Fax")]
        OtherFax,

        [System.Xml.Serialization.XmlEnumAttribute("Unknown")]
        Unknown
    }
}
