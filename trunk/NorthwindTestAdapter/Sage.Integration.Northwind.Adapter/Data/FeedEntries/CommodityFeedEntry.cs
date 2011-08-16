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
    [XmlType(TypeName = "commodity", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class CommodityFeedEntry : FeedEntry
    {
        private string nameField;

        private bool activeField;

        private bool deletedField;

        private CommodityGroupFeedEntry commodityGroupField;

        /*private PriceListFeed pricelistsField;

        private PriceFeed pricesField;*/

        private commodityTypeenum typeField;

        private UnitOfMeasureFeedEntry unitOfMeasureField;

        public UnitOfMeasureFeedEntry unitOfMeasure
        {
            get { return unitOfMeasureField; }
            set { 
                unitOfMeasureField = value;
                SetPropertyChanged("unitOfMeasure", true);
            }
        }

        public string name
        {
            get { return nameField; }
            set { 
                nameField = value;
                SetPropertyChanged("name", true);
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
        public CommodityGroupFeedEntry commodityGroup
        {
            get
            {
                return this.commodityGroupField;
            }
            set
            {
                this.commodityGroupField = value;
                SetPropertyChanged("commodityGroup", true);
            }
        }

        public commodityTypeenum type
        {
            get { return typeField; }
            set { typeField = value; }
        }


        
/*        /// <remarks/>
        [XmlElement("pricelists", IsNullable = true)]
        public PriceListFeed pricelists
        {
            get
            {
                return this.pricelistsField;
            }
            set
            {
                this.pricelistsField = value;
                SetPropertyChanged("pricelists", true);
            }
        }

        /// <remarks/>
        [XmlElement("prices", IsNullable = true)]
        public PriceFeed prices
        {
            get
            {
                return this.pricesField;
            }
            set
            {
                this.pricesField = value;
                SetPropertyChanged("prices", true);
            }
        }*/

        public override string ToString()
        {
            return string.Format("{0} - {1}", name, Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "commodity", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    public class CommodityFeed : Feed<CommodityFeedEntry> { }

        /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "commodityType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum commodityTypeenum
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Product Stock")]
        ProductStock,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Product Non Stock")]
        ProductNonStock,

        /// <remarks/>
        Service,

        /// <remarks/>
        BillOfMaterial,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("Kit.")]
        Kit,

        /// <remarks/>
        Subcontract,

        /// <remarks/>
        Description,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("User defined")]
        Userdefined,

        /// <remarks/>
        Unknown,
    }
}
