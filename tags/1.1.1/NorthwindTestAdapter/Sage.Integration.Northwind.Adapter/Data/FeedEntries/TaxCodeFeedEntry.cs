using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    //UNUSED
    class TaxCodeFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool deletedField;

        private string labelField;

        private string referenceField;

        private string reference2Field;

        private string typeField;

        private string taxationCountryField;

        private string taxationLocaleField;

        private decimal valueField;

        private bool valueFieldSpecified;

        private string valueTextField;

        private bool primacyIndicatorField;

        private System.Nullable<decimal> orderOfPrecedenceField;

        private NoteFeedEntry notesField;


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
            }
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
            }
        }

        /// <remarks/>
        public string reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string reference2
        {
            get
            {
                return this.reference2Field;
            }
            set
            {
                this.reference2Field = value;
            }
        }

        /// <remarks/>
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
            }
        }

        /// <remarks/>
        public string taxationCountry
        {
            get
            {
                return this.taxationCountryField;
            }
            set
            {
                this.taxationCountryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string taxationLocale
        {
            get
            {
                return this.taxationLocaleField;
            }
            set
            {
                this.taxationLocaleField = value;
            }
        }

        /// <remarks/>
        public decimal value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified
        {
            get
            {
                return this.valueFieldSpecified;
            }
            set
            {
                this.valueFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string valueText
        {
            get
            {
                return this.valueTextField;
            }
            set
            {
                this.valueTextField = value;
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
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<decimal> orderOfPrecedence
        {
            get
            {
                return this.orderOfPrecedenceField;
            }
            set
            {
                this.orderOfPrecedenceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public NoteFeedEntry notes
        {
            get
            {
                return this.notesField;
            }
            set
            {
                this.notesField = value;
            }
        }
    }

    class TaxCodeFeed : Feed<TaxCodeFeedEntry> { }
}
