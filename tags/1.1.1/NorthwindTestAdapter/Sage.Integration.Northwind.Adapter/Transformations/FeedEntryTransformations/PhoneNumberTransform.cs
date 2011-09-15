using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Adapter.Feeds;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class PhoneNumberTransform : TransformationBase, IFeedEntryTransformation<PhoneDocument, PhoneNumberFeedEntry>
    {
        public PhoneNumberTransform(Adapter.Common.RequestContext context) : base(context, Adapter.Common.SupportedResourceKinds.phoneNumbers){}

        public PhoneDocument GetTransformedDocument(PhoneNumberFeedEntry feedEntry)
        {
            PhoneDocument phone = new PhoneDocument();
            if (GuidIsNullOrEmpty(feedEntry.UUID))
            {
                phone.Id = feedEntry.Key;
            }
            else
            {
                phone.CrmId = feedEntry.UUID.ToString();
                phone.Id = GetLocalId(feedEntry.UUID);
            }
            phone.Id = feedEntry.Key;

            if(feedEntry.IsPropertyChanged("areaCode"))
                phone.areacode.Value = feedEntry.areaCode;
            if(feedEntry.IsPropertyChanged("countryCode"))
                phone.countrycode.Value = feedEntry.countryCode;
            if(feedEntry.IsPropertyChanged("number"))
                phone.number.Value = feedEntry.number;
            if(feedEntry.IsPropertyChanged("text"))
                phone.fullnumber.Value = feedEntry.text;

            
            if (feedEntry.type == phoneNumberTypeenum.BusinessPhone)
                phone.type.Value = "Business";
            if (feedEntry.type == phoneNumberTypeenum.BusinessFax)
                phone.type.Value = "Fax";

            return phone;
        }

        public PhoneNumberFeedEntry GetTransformedPayload(PhoneDocument document)
        {
            PhoneNumberFeedEntry entry = new PhoneNumberFeedEntry();

            if(!document.areacode.NotSet)
                entry.areaCode = (document.areacode.IsNull) ? null : document.areacode.Value.ToString();
            if (!document.countrycode.NotSet)
                entry.countryCode = (document.countrycode.IsNull) ? null : document.countrycode.Value.ToString();
            if (!document.number.NotSet)
                entry.number = (document.number.IsNull) ? null : document.number.Value.ToString();
            if (!document.fullnumber.NotSet)
                entry.text = (document.fullnumber.IsNull) ? null : document.fullnumber.Value.ToString();

            if (!document.type.IsNull)
            {
                if (document.type.Value.ToString().Equals("Business", StringComparison.InvariantCultureIgnoreCase))
                    entry.type = phoneNumberTypeenum.BusinessPhone;
                if (document.type.Value.ToString().Equals("Fax", StringComparison.InvariantCultureIgnoreCase))
                    entry.type = phoneNumberTypeenum.BusinessFax;
            }

            entry.active = true;

            SetCommonProperties(document, entry.text, entry);
            return entry;
        }
    }
}
