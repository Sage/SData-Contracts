#region Usings

using System;
using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.TradingAccounts;
using Sage.Sis.Sdata.Sync.Storage;

#endregion

namespace Sage.Integration.Northwind.Adapter.Transformations.TradingAccounts
{
    public class PostalAdressTransformation : TransformationBase, ITransformation<AddressDocument, PostalAddressPayload>
    {
        #region Ctor.

        public PostalAdressTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.postalAddresses)
        {
        }

        #endregion

        #region ITransformation<AddressDocument,postalAddresstype> Members

        public AddressDocument GetTransformedDocument(PostalAddressPayload payload, List<SyncFeedEntryLink> links)
        {
            AddressDocument address = new AddressDocument();
            postalAddresstype postaladdress = payload.PostalAddresstype;

            if (String.IsNullOrEmpty(payload.LocalID))
            {
                address.CrmId = payload.SyncUuid.ToString();//
                address.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                address.Id = payload.LocalID;
            }
            address.address1.Value = postaladdress.address1;
            address.address2.Value = postaladdress.address2;
            address.address3.Value = postaladdress.address3;
            address.address4.Value = postaladdress.address4;

            address.country.Value = postaladdress.country;
            address.City.Value = postaladdress.townCity;
            address.postcode.Value = postaladdress.zipPostCode;
            if (!String.IsNullOrEmpty(address.Id))
            {
                address.primaryaddress.Value = "true";
            }
            else
                address.primaryaddress.Value = postaladdress.primacyIndicator.ToString();
            if (!address.primaryaddress.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase))
            {
                if (postaladdress.type == postalAddressTypeenum.Correspondance)
                    address.primaryaddress.Value = "true";

            }
            return address;
        }

        public PostalAddressPayload GetTransformedPayload(AddressDocument document, out List<SyncFeedEntryLink> links)
        {
            PostalAddressPayload payload = new PostalAddressPayload();
            links = new List<SyncFeedEntryLink>();


            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;
            payload.PostalAddresstype.uuid = payload.SyncUuid.ToString();
            payload.PostalAddresstype.applicationID = document.Id;
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')",_datasetLink, SupportedResourceKinds.postalAddresses, document.Id));
            links.Add(selfLink);

            payload.PostalAddresstype.active = true;
            payload.PostalAddresstype.address1 = (document.address1.IsNull) ? null : document.address1.Value.ToString();
            payload.PostalAddresstype.address2 = (document.address2.IsNull) ? null : document.address2.Value.ToString();
            payload.PostalAddresstype.address3 = (document.address3.IsNull) ? null : document.address3.Value.ToString();
            payload.PostalAddresstype.address4 = (document.address4.IsNull) ? null : document.address4.Value.ToString();
            payload.PostalAddresstype.country = (document.country.IsNull) ? null : document.country.Value.ToString();
            payload.PostalAddresstype.deleted = false;
            payload.PostalAddresstype.stateRegion = (document.state.IsNull) ? null : document.state.Value.ToString();
            payload.PostalAddresstype.townCity = (document.City.IsNull) ? null : document.City.Value.ToString();
            payload.PostalAddresstype.zipPostCode = (document.postcode.IsNull) ? null : document.postcode.Value.ToString();
            if ((!document.primaryaddress.IsNull)&&(document.primaryaddress.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase)))
                payload.PostalAddresstype.primacyIndicator = true;
            payload.PostalAddresstype.type = postalAddressTypeenum.Correspondance;
            return payload;
        }

        #endregion
    }
}
