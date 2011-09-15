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
    [XmlType(TypeName = "salesInvoice", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class SalesInvoiceFeedEntry : FeedEntry
    {
        private bool activeField;

        private TradingAccountFeedEntry tradingAccountField;

        private PriceListFeedEntry pricelistField;

        private System.DateTime dateField;

        private PostalAddressFeed postalAddressesField;

        private SalesInvoiceLineFeed salesInvoiceLinesField;

        private decimal netTotalField;

        private string currencyField;

        private decimal discountTotalField;

        private decimal carrierTotalPriceField;

        private decimal grossTotalField;

        private DateTime deliveryDateField;

        private string deliveryMethodField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("postalAddresses", IsNullable = true)]
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
        [System.Xml.Serialization.XmlArray("salesInvoiceLines", IsNullable = true, Namespace = Namespaces.northwindNamespace)]
        [SMERelationshipProperty(MinOccurs = 0, IsCollection = true, IncludeByDefault = true)]
        public SalesInvoiceLineFeed salesInvoiceLines
        {
            get
            {
                return this.salesInvoiceLinesField;
            }
            set
            {
                this.salesInvoiceLinesField = value;
                SetPropertyChanged("salesInvoiceLines", true);
                SetPropertyChanged("lineCount", true);
            }
        }

        public decimal lineCount
        {
            get { 
                return salesInvoiceLinesField == null ? 0 : salesInvoiceLinesField.Entries.Count;
            }
        }

        /// <remarks/>
        public decimal netTotal
        {
            get
            {
                return this.netTotalField;
            }
            set
            {
                this.netTotalField = value;
                SetPropertyChanged("netTotal", true);
            }
        }

        public string currency
        {
            get { return currencyField; }
            set 
            { 
                currencyField = value;
                SetPropertyChanged("currency", true);
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

        public decimal carrierTotalPrice
        {
            get { return carrierTotalPriceField; }
            set 
            { 
                carrierTotalPriceField = value;
                SetPropertyChanged("carrierTotalPrice", true);
            }
        }


        public decimal grossTotal
        {
            get { return grossTotalField; }
            set 
            { 
                grossTotalField = value;
                SetPropertyChanged("grossTotal", true);
            }
        }


        public DateTime deliveryDate
        {
            get { return deliveryDateField; }
            set 
            { 
                deliveryDateField = value;
                SetPropertyChanged("deliveryDate", true);
            }
        }


        public string deliveryMethod
        {
            get { return deliveryMethodField; }
            set 
            { 
                deliveryMethodField = value; 
            }
        }        

        public override string ToString()
        {
            return string.Format("{0}", this.Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "salesInvoice", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }
}
