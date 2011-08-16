using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Adapter.Transformations;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class PostalAdressTransform : TransformationBase, IFeedEntryTransformation<AddressDocument, PostalAddressFeedEntry>
    {
        private Adapter.Common.RequestContext context;
        private Adapter.Common.SupportedResourceKinds supportedResourceKinds;

        public PostalAdressTransform(Adapter.Common.RequestContext context) : base(context, Adapter.Common.SupportedResourceKinds.postalAddresses){}

        public AddressDocument GetTransformedDocument(PostalAddressFeedEntry feedEntry)
        {
            AddressDocument address = new AddressDocument();

            if (GuidIsNullOrEmpty(feedEntry.UUID))
            {
                address.Id = feedEntry.Key;
            }
            else
            {
                address.CrmId = feedEntry.UUID.ToString();
                address.Id = GetLocalId(feedEntry.UUID);
            }
            if(feedEntry.IsPropertyChanged("address1"))
                address.address1.Value = feedEntry.address1;
            if (feedEntry.IsPropertyChanged("address2"))
                address.address2.Value = feedEntry.address2;
            if (feedEntry.IsPropertyChanged("address3"))
                address.address3.Value = feedEntry.address3;
            if (feedEntry.IsPropertyChanged("address4"))
                address.address4.Value = feedEntry.address4;

            if (feedEntry.IsPropertyChanged("country"))
                address.country.Value = feedEntry.country;
            if (feedEntry.IsPropertyChanged("townCity"))
                address.City.Value = feedEntry.townCity;
            if (feedEntry.IsPropertyChanged("zipPostCode"))
                address.postcode.Value = feedEntry.zipPostCode;

            if (!String.IsNullOrEmpty(address.Id))
            {
                address.primaryaddress.Value = "true";
            }
            else
                address.primaryaddress.Value = feedEntry.primacyIndicator.ToString();
            if (!address.primaryaddress.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                if (feedEntry.type == postalAddressTypeenum.Correspondence)
                    address.primaryaddress.Value = "true";

            }
            return address;
        }

        public PostalAddressFeedEntry GetTransformedPayload(AddressDocument document)
        {
            PostalAddressFeedEntry entry = new PostalAddressFeedEntry();
            //entry.UUID = GetUuidAsGuid(document.Id, document.CrmId);

            entry.active = true;
            entry.deleted = false;

            if(!document.address1.NotSet)
                entry.address1 = (document.address1.IsNull) ? null : document.address1.Value.ToString();
            if(!document.address2.NotSet)
                entry.address2 = (document.address2.IsNull) ? null : document.address2.Value.ToString();
            if(!document.address3.NotSet)
                entry.address3 = (document.address3.IsNull) ? null : document.address3.Value.ToString();
            if (!document.address4.NotSet)
                entry.address4 = (document.address4.IsNull) ? null : document.address4.Value.ToString();
            if (!document.country.NotSet)
                entry.country = (document.country.IsNull) ? null : document.country.Value.ToString();
            if (!document.state.NotSet)
                entry.stateRegion = (document.state.IsNull) ? null : document.state.Value.ToString();
            if (!document.City.NotSet)
                entry.townCity = (document.City.IsNull) ? null : document.City.Value.ToString();
            if (!document.postcode.NotSet)
                entry.zipPostCode = (document.postcode.IsNull) ? null : document.postcode.Value.ToString();

            if ((!document.primaryaddress.IsNull) && (document.primaryaddress.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase)))
                entry.primacyIndicator = true;

            entry.type = postalAddressTypeenum.Correspondence;
            SetCommonProperties(document, entry.address1, entry);
            return entry;
        }
    }
}
