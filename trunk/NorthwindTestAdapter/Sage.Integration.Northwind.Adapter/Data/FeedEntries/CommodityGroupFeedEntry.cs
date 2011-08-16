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
    [XmlType(TypeName = "commodityGroup", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")] [SMEResource(HasUuid=true)]
    public class CommodityGroupFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool deletedField;

        private string labelField;

        private System.Nullable<commodityGroupTypeenum> typeField;

        private string nameField;

        private string descriptionField;

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

        public System.Nullable<commodityGroupTypeenum> type
        {
            get { return typeField; }
            set { typeField = value; }
        }

        /// <remarks/>
        public string label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
                SetPropertyChanged("label", true);
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

        public override string ToString()
        {
            return string.Format("{0} - {1}", name, Id);
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "commodityGroup", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    [XmlType(TypeName = "commodityGroup", Namespace = Namespaces.northwindNamespace)]
    [XmlSchemaProvider("GetSchema")]
    public class CommodityGroupFeedEntryTemplate : FeedEntry
    {
        public bool active
        {
            get { return true; }
        }

        public static XmlQualifiedName GetSchema(XmlSchemaSet xs)
        {
            return FeedEntryHelper.GetSchema(xs, "commodityGroup", Namespaces.northwindNamespace, "Sage.Integration.Northwind.Adapter.crmErp.xsd");
        }
    }

    [XmlType(TypeName = "commodityGroupType--enum", Namespace = "http://schemas.sage.com/crmErp/2008")]
    public enum commodityGroupTypeenum
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
