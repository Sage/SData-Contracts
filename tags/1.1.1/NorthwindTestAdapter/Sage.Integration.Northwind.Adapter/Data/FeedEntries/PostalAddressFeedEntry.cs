using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Common;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;
using System.IO;
using Sage.Common.Metadata;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    [XmlType(TypeName = "postalAddress--type", Namespace = Namespaces.sc)]
    public class PostalAddressFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool deletedField;

        private string address1Field;

        private string address2Field;

        private string address3Field;

        private string address4Field;

        private string townCityField;

        private string countyField;

        private string stateRegionField;

        private string zipPostCodeField;

        private string countryField;

        private bool primacyIndicatorField;

        private postalAddressTypeenum typeField;

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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string address1
        {
            get
            {
                return this.address1Field;
            }
            set
            {
                this.address1Field = value;
                SetPropertyChanged("address1", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string address2
        {
            get
            {
                return this.address2Field;
            }
            set
            {
                this.address2Field = value;
                SetPropertyChanged("address2", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string address3
        {
            get
            {
                return this.address3Field;
            }
            set
            {
                this.address3Field = value;
                SetPropertyChanged("address3", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string address4
        {
            get
            {
                return this.address4Field;
            }
            set
            {
                this.address4Field = value;
                SetPropertyChanged("address4", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string townCity
        {
            get
            {
                return this.townCityField;
            }
            set
            {
                this.townCityField = value;
                SetPropertyChanged("townCity", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string county
        {
            get
            {
                return this.countyField;
            }
            set
            {
                this.countyField = value;
                SetPropertyChanged("county", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string stateRegion
        {
            get
            {
                return this.stateRegionField;
            }
            set
            {
                this.stateRegionField = value;
                SetPropertyChanged("stateRegion", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string zipPostCode
        {
            get
            {
                return this.zipPostCodeField;
            }
            set
            {
                this.zipPostCodeField = value;
                SetPropertyChanged("zipPostCode", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
                SetPropertyChanged("country", true);
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

        /// <remarks/>
        public postalAddressTypeenum type
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

        public override string ToString()
        {
            return String.Format("{0}, {1} {2} - {3}", this.address1, this.zipPostCode, this.townCity, this.Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "postalAddress", Namespaces.sc, "Sage.Integration.Northwind.Adapter.common.xsd");
        }
    }

    [XmlSchemaProvider("GetSchema")]
    public class PostalAddressFeed : Feed<PostalAddressFeedEntry>
    {
        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "postalAddresses", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }
}
