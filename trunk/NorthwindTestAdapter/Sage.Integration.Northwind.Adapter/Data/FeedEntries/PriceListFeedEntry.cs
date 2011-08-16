using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using System.Xml.Serialization;
using Sage.Integration.Northwind.Common;
using System.Xml;
using System.Xml.Schema;
using Sage.Common.Metadata;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    [XmlType(TypeName = "priceList", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class PriceListFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool deletedField;

        private System.Nullable<priceListTypeenum> typeField;

        private string nameField;

        private System.Nullable<decimal> calculationPercentField;

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
                SetPropertyChanged("delete", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<priceListTypeenum> type
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
        public System.Nullable<decimal> calculationPercent
        {
            get
            {
                return this.calculationPercentField;
            }
            set
            {
                this.calculationPercentField = value;
                SetPropertyChanged("calculationPercent", true);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.name, this.Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "priceList", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    public class PriceListFeed : Feed<PriceListFeedEntry> { }
}
