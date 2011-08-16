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
    [XmlType(TypeName = "salesInvoiceLine", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class SalesInvoiceLineFeedEntry : FeedEntry
    {
        private SalesInvoiceFeedEntry salesInvoiceField;

        private bool activeField;

        private bool deletedField;

        private System.Nullable<lineTypeenum> typeField;

        private CommodityFeedEntry commodityField;

        private UnitOfMeasureFeedEntry unitOfMeasureField;

        private decimal quantityField;

        private System.Nullable<decimal> initialPriceField;

        private PriceListFeedEntry pricelistField;

        private decimal invoiceLineDiscountPercentField;

        private decimal discountTotalField;

        private decimal costTotalField;

        private decimal netTotalField;


        public SalesInvoiceFeedEntry salesInvoice
        {
            get 
            { 
                return salesInvoiceField; 
            }
            set 
            { 
                salesInvoiceField = value;
                SetPropertyChanged("salesInvoice", true);
            }
        }

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
        public System.Nullable<lineTypeenum> type
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public UnitOfMeasureFeedEntry unitOfMeasure
        {
            get
            {
                return this.unitOfMeasureField;
            }
            set
            {
                this.unitOfMeasureField = value;
                SetPropertyChanged("unitOfMeasure", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> initialPrice
        {
            get
            {
                return this.initialPriceField;
            }
            set
            {
                this.initialPriceField = value;
                SetPropertyChanged("initialPrice", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
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


        public CommodityFeedEntry commodity
        {
            get { return commodityField; }
            set 
            {
                commodityField = value;
                SetPropertyChanged("commodity", true);
            }
        }

        public decimal quantity
        {
            get { return quantityField; }
            set 
            { 
                quantityField = value;
                SetPropertyChanged("quantity", true);
            }
        }

        public decimal invoiceLineDiscountPercent
        {
            get { return invoiceLineDiscountPercentField; }
            set 
            {
                invoiceLineDiscountPercentField = value;
                SetPropertyChanged("invoiceLineDiscountPercent", true);
            }
        }

        public decimal discountTotal
        {
            get { return discountTotalField; }
            set 
            { 
                discountTotalField = value;
                SetPropertyChanged("discountTotal", true);
            }
        }

        [XmlElement(IsNullable = true)]
        public decimal costTotal
        {
            get { return costTotalField; }
            set 
            { 
                costTotalField = value;
                SetPropertyChanged("costTotal", true);
            }
        }

        public decimal netTotal
        {
            get { return netTotalField; }
            set 
            { 
                netTotalField = value;
                SetPropertyChanged("netTotal", true);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "salesInvoiceLine", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }

    }

    [XmlSchemaProvider("GetSchema")]
    public class SalesInvoiceLineFeed : Feed<SalesInvoiceLineFeedEntry> { }
}
