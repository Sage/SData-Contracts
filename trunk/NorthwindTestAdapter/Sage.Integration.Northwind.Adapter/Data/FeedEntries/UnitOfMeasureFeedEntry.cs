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
    [XmlType(TypeName = "unitOfMeasure", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class UnitOfMeasureFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool deletedField;

        private string typeField;

        private string nameField;

        private string descriptionField;

        private bool primacyIndicatorField;

        private CommodityFeed commoditiesField;


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

        /*/// <remarks/>
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
                SetPropertyChanged("type", true);
            }
        }*/

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
                SetPropertyChanged("name", true);
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
                SetPropertyChanged("description", true);
            }
        }

        /*/// <remarks/>
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
        }*/

        /*/// <remarks/>
        [System.Xml.Serialization.XmlElement("commodities", IsNullable = true)]
        public CommodityFeed commodities
        {
            get
            {
                return this.commoditiesField;
            }
            set
            {
                this.commoditiesField = value;
                SetPropertyChanged("commodities", true);
            }
        }*/


        public override string ToString()
        {
            return string.Format("{0} - {1}", this.name, this.Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "unitOfMeasure", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    public class UnitOfMeasureFeed : Feed<UnitOfMeasureFeedEntry> { }


}
