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
    public class PhoneNumberTransformation : TransformationBase, ITransformation<PhoneDocument, PhoneNumberPayload>
    {
        #region Ctor.

        public PhoneNumberTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.emails)
        {
        }

        #endregion

        #region ITransformation<PhoneDocument,PhoneNumberPayload> Members

        public PhoneDocument GetTransformedDocument(PhoneNumberPayload payload, List<SyncFeedEntryLink> links)
        {
            PhoneDocument phone = new PhoneDocument();
            phoneNumbertype phoneNumber = payload.PhoneNumbertype;
            phone.areacode.Value = phoneNumber.areaCode;
            phone.countrycode.Value = phoneNumber.countryCode;
            phone.Id = phoneNumber.applicationID;
            phone.number.Value = phoneNumber.number;
            phone.fullnumber.Value = phoneNumber.text;
            
            if (!String.IsNullOrEmpty(phoneNumber.type))
            {
                if (phoneNumber.type.Equals("general", StringComparison.InvariantCultureIgnoreCase))
                    phone.type.Value ="Business";
                if (phoneNumber.type.Equals("fax", StringComparison.InvariantCultureIgnoreCase))
                    phone.type.Value = "Fax";
                //phone.type.Value = phoneNumber.type;
                //CRMSelections.Link_PersPhon_Fax
                //CRMSelections.Link_PersPhon_Business
                //general, fax
            }
            return phone;
        }

        public PhoneNumberPayload GetTransformedPayload(PhoneDocument document, out List<SyncFeedEntryLink> links)
        {
            links = new List<SyncFeedEntryLink>();
            PhoneNumberPayload payload = new PhoneNumberPayload();
            payload.PhoneNumbertype.areaCode = (document.areacode.IsNull) ? null : document.areacode.Value.ToString();
            payload.PhoneNumbertype.countryCode = (document.countrycode.IsNull) ? null : document.countrycode.Value.ToString();
            payload.PhoneNumbertype.number = (document.number.IsNull) ? null : document.number.Value.ToString();
            payload.PhoneNumbertype.text = (document.fullnumber.IsNull) ? null : document.fullnumber.Value.ToString();
            
            if (!document.type.IsNull)
            {

                if (document.type.Value.ToString().Equals("Business", StringComparison.InvariantCultureIgnoreCase))
                    payload.PhoneNumbertype.type = "general";
                if (document.type.Value.ToString().Equals("Fax", StringComparison.InvariantCultureIgnoreCase))
                    payload.PhoneNumbertype.type = "fax";
            }

            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;
            payload.PhoneNumbertype.applicationID = document.Id;
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.phoneNumbers, document.Id));
            links.Add(selfLink);
            
            return payload;
        }

        #endregion
    }
}
