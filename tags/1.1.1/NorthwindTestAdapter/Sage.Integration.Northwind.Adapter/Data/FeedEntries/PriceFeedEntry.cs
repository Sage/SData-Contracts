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
    [XmlType(TypeName = "price", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class PriceFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool deletedField;

        private CommodityFeedEntry commodityField;

        private PriceListFeedEntry pricelistField;

        private string currencyField;

        private decimal priceField;

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
        [System.Xml.Serialization.XmlArray("commodity", IsNullable = true)]
        [SMERelationshipProperty(MinOccurs = 0, IsCollection = false, IncludeByDefault = true, Namespace = Namespaces.northwindNamespace)]
        public CommodityFeedEntry commodity
        {
            get
            {
                return this.commodityField;
            }
            set
            {
                this.commodityField = value;
                SetPropertyChanged("commodity", true);
            }
        }/*

        /// <remarks/>
        public PriceListFeedEntry pricelist
        {
            get
            {
                return this.pricelistField;
            }
            set
            {
                this.pricelistField = value;
                SetPropertyChanged("pricelist", true);
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
        }*/

        /// <remarks/>
        public decimal price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
                SetPropertyChanged("price", true);
            }
        }
        public override string ToString()
        {
            return string.Format("{0} - {1}", this.price, this.Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "price", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }

    }

    public class PriceFeed : Feed<PriceFeedEntry> { }
}
