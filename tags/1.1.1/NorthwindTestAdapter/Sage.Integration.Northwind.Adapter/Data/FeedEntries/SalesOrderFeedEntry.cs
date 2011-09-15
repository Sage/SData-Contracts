using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using System.Xml.Serialization;
using Sage.Integration.Northwind.Common;
using System.Xml;
using System.Xml.Schema;
using Sage.Common.Metadata;
using Sage.Common.Metadata.Model;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    [XmlType(TypeName = "salesOrder", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class SalesOrderFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool deletedField;

        private TradingAccountFeedEntry tradingAccountField;

        private PriceListFeedEntry pricelistField;

        private PostalAddressFeed postalAddressesField;

        private string deliveryMethodField;

        private System.Nullable<System.DateTime> deliveryDateField;

        private System.Nullable<System.DateTime> dueDateField;

        private System.Nullable<decimal> carrierTotalPriceField;

        private string currencyField;

        private System.Nullable<decimal> invoiceCurrencyExchangeRateField;

        private System.DateTime dateField;

        private SalesOrderLineFeed salesOrderLinesField;

        private decimal netTotalField;

        private decimal discountTotalField;

        private decimal grossTotalField;

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

        [SMERelationshipProperty(Relationship = RelationshipType.Reference, IncludeByDefault=true, Namespace = Namespaces.northwindNamespace)]
        public TradingAccountFeedEntry tradingAccount
        {
            get
            {
                return this.tradingAccountField;
            }
            set
            {
                this.tradingAccountField = value;
                SetPropertyChanged("tradingAccount", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true, DataType="priceList--type", Namespace=Namespaces.northwindNamespace)]
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
        [XmlElement("postalAddresses", IsNullable = true)]
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
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string deliveryMethod
        {
            get
            {
                return this.deliveryMethodField;
            }
            set
            {
                this.deliveryMethodField = value;
                SetPropertyChanged("deliveryMethod", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> deliveryDate
        {
            get
            {
                return this.deliveryDateField;
            }
            set
            {
                this.deliveryDateField = value;
                SetPropertyChanged("deliveryDate", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> dueDate
        {
            get
            {
                return this.dueDateField;
            }
            set
            {
                this.dueDateField = value;
                SetPropertyChanged("dueDate", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> carrierTotalPrice
        {
            get
            {
                return this.carrierTotalPriceField;
            }
            set
            {
                this.carrierTotalPriceField = value;
                SetPropertyChanged("carrierTotalPrice", true);
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
        public System.Nullable<decimal> invoiceCurrencyExchangeRate
        {
            get
            {
                return this.invoiceCurrencyExchangeRateField;
            }
            set
            {
                this.invoiceCurrencyExchangeRateField = value;
                SetPropertyChanged("invoiceCurrencyExchangeRate", true);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
                SetPropertyChanged("date", true);
            }
        }

        [XmlArray("salesOrderLines", IsNullable = true, Namespace = Namespaces.northwindNamespace)]
        [SMERelationshipProperty(MinOccurs = 0, IsCollection = true, IncludeByDefault = true)]
        public SalesOrderLineFeed salesOrderLines
        {
            get { return salesOrderLinesField; }
            set { 
                salesOrderLinesField = value;
                SetPropertyChanged("salesOrderLines", true);
            }
        }

        public int lineCount
        {
            get { return salesOrderLinesField == null ? 0 : salesOrderLinesField.Entries.Count; }
        }


        public decimal netTotal
        {
            get { return netTotalField; }
            set { 
                netTotalField = value;
                SetPropertyChanged("netTotal", true);
            }
        }


        public decimal discountTotal
        {
            get { return discountTotalField; }
            set { 
                discountTotalField = value;
                SetPropertyChanged("discountTotal", true);
            }
        }


        public decimal grossTotal
        {
            get { return grossTotalField; }
            set { 
                grossTotalField = value;
                SetPropertyChanged("grossTotal", true);
            }
        }

        public override string ToString()
        {
            return this.Id;
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "salesOrder", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }

    }


    [XmlType(TypeName = "salesOrder", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] 
    public class SalesOrderFeedEntryTemplate : FeedEntry
    {

        public bool active
        {
            get { return true; }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "salesOrder", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }
}
