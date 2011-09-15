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
    [XmlType(TypeName = "salesOrderLine", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class SalesOrderLineFeedEntry : FeedEntry
    {
    
        private SalesOrderFeedEntry salesOrderField;
        
        private bool activeField;
        
        private bool deletedField;
        
        private System.Nullable<lineTypeenum> typeField;
        
        private CommodityFeedEntry commodityField;
        
        private UnitOfMeasureFeedEntry unitOfMeasureField;
        
        private decimal quantityField;
        
        private decimal initialPriceField;
        
        private PriceListFeedEntry pricelistField;
        
        private System.Nullable<decimal> orderLineDiscountPercentField;
        
        private decimal netTotalField;

        private decimal discountTotalField;

        private decimal? costTotalField;
        
        /// <remarks/>
        public SalesOrderFeedEntry salesOrder
        {
            get {
                return this.salesOrderField;
            }
            set {
                this.salesOrderField = value;
                SetPropertyChanged("salesOrder", true);
            }
        }
        
        /// <remarks/>
        public bool active {
            get {
                return this.activeField;
            }
            set {
                this.activeField = value;
                SetPropertyChanged("active", true);
            }
        }
        
        /// <remarks/>
        public bool deleted {
            get {
                return this.deletedField;
            }
            set {
                this.deletedField = value;
                SetPropertyChanged("deleted", true);
            }
        }
        
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<lineTypeenum> type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
                SetPropertyChanged("type", true);
            }
        }
        
        /// <remarks/>
        [XmlElement("commodity", IsNullable = true)]
        [SMERelationshipProperty(MinOccurs = 0, IsCollection = false, IncludeByDefault = true, Namespace = Namespaces.northwindNamespace)]
        public CommodityFeedEntry commodity
        {
            get {
                return this.commodityField;
            }
            set {
                this.commodityField = value;
                SetPropertyChanged("commodity", true);
            }
        }


        [XmlElement("unitOfMeasure", IsNullable = true)]
        [SMERelationshipProperty(MinOccurs = 0, IsCollection = false, IncludeByDefault = true, Namespace = Namespaces.northwindNamespace)]
        public UnitOfMeasureFeedEntry unitOfMeasure
        {
            get {
                return this.unitOfMeasureField;
            }
            set {
                this.unitOfMeasureField = value;
                SetPropertyChanged("unitOfMeasure", true);
            }
        }
        
        /// <remarks/>
        public decimal quantity {
            get {
                return this.quantityField;
            }
            set {
                this.quantityField = value;
                SetPropertyChanged("quantity", true);
            }
        }
        
        /// <remarks/>
        public decimal initialPrice {
            get {
                return this.initialPriceField;
            }
            set {
                this.initialPriceField = value;
                SetPropertyChanged("initialPrice", true);
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public PriceListFeedEntry pricelist
        {
            get {
                return this.pricelistField;
            }
            set {
                this.pricelistField = value;
                SetPropertyChanged("pricelist", true);
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public System.Nullable<decimal> orderLineDiscountPercent {
            get {
                return this.orderLineDiscountPercentField;
            }
            set {
                this.orderLineDiscountPercentField = value;
                SetPropertyChanged("orderLineDiscoutPercent", true);
            }
        }
        
        /// <remarks/>
        public decimal netTotal {
            get {
                return this.netTotalField;
            }
            set {
                this.netTotalField = value;
                SetPropertyChanged("netTotalField", true);
            }
        }

        public decimal discountTotal
        {
            get { return discountTotalField; }
            set { discountTotalField = value; }
        }

        [XmlElement(IsNullable=true)]
        public decimal? costTotal
        {
            get { return costTotalField; }
            set { costTotalField = value; }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "salesOrderLine", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    public class SalesOrderLineFeed : Feed<SalesOrderLineFeedEntry> { }
}
