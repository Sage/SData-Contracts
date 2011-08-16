using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;

namespace Sage.Integration.Northwind.Adapter.Feeds
{
    public class LocationFeedEntry : FeedEntry
    {
        private bool activeField;

        private bool activeFieldSpecified;

        private bool deletedField;

        private bool deletedFieldSpecified;

        private string labelField;

        private string typeField;

        private string referenceField;

        private string reference2Field;

        private string nameField;

        private string descriptionField;

        //private System.Nullable<supplierFlagenum> customerSupplierFlagField;

        private bool customerSupplierFlagFieldSpecified;

        private PostalAddressFeedEntry postalAddressField;

        private System.Nullable<bool> saleFlagField;

        private bool saleFlagFieldSpecified;

        private System.Nullable<bool> purchaseFlagField;

        private bool purchaseFlagFieldSpecified;

        private string openingDaysField;

        private System.Nullable<System.DateTime> openingTimesField;

        private bool openingTimesFieldSpecified;

        private string gPSPositionField;

        private Feed<PhoneNumberFeedEntry> phonesField;

        private Feed<EmailFeedEntry> emailsField;

        private string websiteField;

        private Feed<ContactFeedEntry> contactsField;

        private System.Nullable<bool> transitFlagField;

        private bool transitFlagFieldSpecified;

        private Feed<NoteFeedEntry> notesField;

        private LocationFeedEntry parentLocationField;

        private Feed<LocationFeedEntry> childLocationField;



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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool activeSpecified
        {
            get
            {
                return this.activeFieldSpecified;
            }
            set
            {
                this.activeFieldSpecified = value;
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deletedSpecified
        {
            get
            {
                return this.deletedFieldSpecified;
            }
            set
            {
                this.deletedFieldSpecified = value;
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
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
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
            }
        }

        /*/// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<supplierFlagenum> customerSupplierFlag
        {
            get
            {
                return this.customerSupplierFlagField;
            }
            set
            {
                this.customerSupplierFlagField = value;
            }
        }*/

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool customerSupplierFlagSpecified
        {
            get
            {
                return this.customerSupplierFlagFieldSpecified;
            }
            set
            {
                this.customerSupplierFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public PostalAddressFeedEntry postalAddress
        {
            get
            {
                return this.postalAddressField;
            }
            set
            {
                this.postalAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> saleFlag
        {
            get
            {
                return this.saleFlagField;
            }
            set
            {
                this.saleFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool saleFlagSpecified
        {
            get
            {
                return this.saleFlagFieldSpecified;
            }
            set
            {
                this.saleFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> purchaseFlag
        {
            get
            {
                return this.purchaseFlagField;
            }
            set
            {
                this.purchaseFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool purchaseFlagSpecified
        {
            get
            {
                return this.purchaseFlagFieldSpecified;
            }
            set
            {
                this.purchaseFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string openingDays
        {
            get
            {
                return this.openingDaysField;
            }
            set
            {
                this.openingDaysField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date", IsNullable = true)]
        public System.Nullable<System.DateTime> openingTimes
        {
            get
            {
                return this.openingTimesField;
            }
            set
            {
                this.openingTimesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool openingTimesSpecified
        {
            get
            {
                return this.openingTimesFieldSpecified;
            }
            set
            {
                this.openingTimesFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string gPSPosition
        {
            get
            {
                return this.gPSPositionField;
            }
            set
            {
                this.gPSPositionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("phones", IsNullable = true)]
        public Feed<PhoneNumberFeedEntry> phones
        {
            get
            {
                return this.phonesField;
            }
            set
            {
                this.phonesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("emails", IsNullable = true)]
        public Feed<EmailFeedEntry> emails
        {
            get
            {
                return this.emailsField;
            }
            set
            {
                this.emailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string website
        {
            get
            {
                return this.websiteField;
            }
            set
            {
                this.websiteField = value;
            }
        }

        /// <remarks/>
        ///
        [System.Xml.Serialization.XmlElement("contacts", IsNullable = true)]
        public Feed<ContactFeedEntry> contacts
        {
            get
            {
                return this.contactsField;
            }
            set
            {
                this.contactsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> transitFlag
        {
            get
            {
                return this.transitFlagField;
            }
            set
            {
                this.transitFlagField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool transitFlagSpecified
        {
            get
            {
                return this.transitFlagFieldSpecified;
            }
            set
            {
                this.transitFlagFieldSpecified = value;
            }
        }

        /// <remarks/>
        public Feed<NoteFeedEntry> notes
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public LocationFeedEntry parentLocation
        {
            get
            {
                return this.parentLocationField;
            }
            set
            {
                this.parentLocationField = value;
            }
        }

        /// <remarks/>
        public Feed<LocationFeedEntry> childLocation
        {
            get
            {
                return this.childLocationField;
            }
            set
            {
                this.childLocationField = value;
            }
        }
    }
}
