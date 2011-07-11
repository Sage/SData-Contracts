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
    public class ContactTransformation :TransformationBase, ITransformation<PersonDocument, ContactPayload>
    {
        #region Ctor.

        public ContactTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.contacts)
        {
        }

        #endregion

        #region ITransformation<PersonDocument,ContactPayload> Members

        public PersonDocument GetTransformedDocument(ContactPayload payload, List<SyncFeedEntryLink> links)
        {
            PersonDocument person = new PersonDocument();
            contacttype contact = payload.Contacttype;
            if (String.IsNullOrEmpty(payload.LocalID))
            {
                person.CrmId = payload.SyncUuid.ToString();//
                person.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                person.Id = payload.LocalID;
            }
            person.firstname.Value = contact.firstName;
            person.lastname.Value = contact.familyName;
            person.middlename.Value = contact.middleName;
            person.fullname.Value = contact.fullName;
            if (!String.IsNullOrEmpty(person.Id))
            {
                person.primaryperson.Value = "true";
            }
            else 
                person.primaryperson.Value = contact.primacyIndicator.ToString();

            person.salutation.Value = contact.salutation;
            person.suffix.Value = contact.suffix;
            person.title.Value = contact.title;
            return person;
        }

        public ContactPayload GetTransformedPayload(PersonDocument document, out List<SyncFeedEntryLink> links)
        {
            contacttype contact = new contacttype();
            ContactPayload payload = new ContactPayload();
            links = new List<SyncFeedEntryLink>();
             
            contact.uuid = GetUuid(document.Id, document.CrmId).ToString();
            contact.applicationID = document.Id;
            payload.SyncUuid = StringToGuid(contact.uuid);
            payload.LocalID = document.Id;
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.contacts, document.Id));
            links.Add(selfLink);

            contact.familyName = (document.lastname.IsNull) ? null : document.lastname.Value.ToString();
            contact.firstName = (document.firstname.IsNull) ? null : document.firstname.Value.ToString();
            contact.middleName = (document.middlename.IsNull) ? null : document.middlename.Value.ToString();
            contact.salutation = (document.salutation.IsNull) ? null : document.salutation.Value.ToString();
            contact.suffix = (document.suffix.IsNull) ? null : document.suffix.Value.ToString();
            contact.title = (document.title.IsNull) ? null : document.title.Value.ToString();
            contact.fullName = (document.fullname.IsNull) ? null : document.fullname.Value.ToString();
            if ((!document.primaryperson.IsNull)&&(document.primaryperson.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase)))
                contact.primacyIndicator = true;


            
            payload.Contacttype = contact;
            return payload;
        }

        #endregion
    }
}
