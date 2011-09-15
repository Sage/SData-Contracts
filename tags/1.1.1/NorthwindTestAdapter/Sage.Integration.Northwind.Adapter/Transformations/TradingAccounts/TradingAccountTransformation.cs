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
    public class TradingAccountTransformation : TransformationBase, ITransformation<AccountDocument, TradingAccountPayload>
    {
        #region Class Variabnles

        private readonly PostalAdressTransformation _postalAdressTransformation;
        private readonly ContactTransformation _contactTransformation;
        private readonly EmailAdressTransformation _emailAdressTransformation;
        private readonly PhoneNumberTransformation _phoneNumberTransformation;

        #endregion

        #region Ctor.

        public TradingAccountTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.tradingAccounts)
        {
            _postalAdressTransformation =
                TransformationFactory.GetTransformation<PostalAdressTransformation>(
                SupportedResourceKinds.postalAddresses, context);
            
            _contactTransformation =
                TransformationFactory.GetTransformation<ContactTransformation>(
                SupportedResourceKinds.contacts, context);

            _emailAdressTransformation =
                TransformationFactory.GetTransformation<EmailAdressTransformation>(
                SupportedResourceKinds.emails, context);

            _phoneNumberTransformation =
                TransformationFactory.GetTransformation<PhoneNumberTransformation>(
                SupportedResourceKinds.phoneNumbers, context);
        }

        #endregion

        #region ITransformation<AccountDocument,tradingAccountTypeFeedEntry> Members

        public AccountDocument GetTransformedDocument(TradingAccountPayload payload, List<SyncFeedEntryLink> links)
        {
            AccountDocument document = new AccountDocument();
            tradingAccounttype tradingAccount = payload.TradingAccount;
            document.addresses = new AddressDocumentCollection();
            if (tradingAccount.postalAddresses != null && tradingAccount.postalAddresses != null)
                foreach (postalAddresstype postalAddress in tradingAccount.postalAddresses)
                {
                    PostalAddressPayload postalAdressPayload = new PostalAddressPayload();
                    postalAdressPayload.PostalAddresstype = postalAddress;
                    postalAdressPayload.SyncUuid = StringToGuid(postalAddress.uuid);
                    AddressDocument address = _postalAdressTransformation.GetTransformedDocument(postalAdressPayload, Helper.ReducePayloadPath(links));
                    document.addresses.Add(address);
                }
            bool hasMainAdress = false;
            for (int index = 0; index < document.addresses.documents.Count; index++)
            {
                AddressDocument address = document.addresses.documents[index] as AddressDocument;
                if ((address.primaryaddress.Value != null) && (address.primaryaddress.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase)))
                    hasMainAdress = true;
            }
            if ((!hasMainAdress) && (document.addresses.documents.Count > 0))
            {
                AddressDocument address = document.addresses.documents[0] as AddressDocument;
                address.primaryaddress.Value = "true";
            }
            if (String.IsNullOrEmpty(payload.LocalID))
            {
                document.CrmId = payload.SyncUuid.ToString();//
                document.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                document.Id = payload.LocalID;
            }
            document.currencyid.Value = tradingAccount.currency;
            document.emails = new EmailsDocumentCollection();
            if (tradingAccount.emails != null && tradingAccount.emails != null)
                foreach (emailtype email in tradingAccount.emails)
                {
                    EmailPayload emailPayload = new EmailPayload();
                    emailPayload.Emailtype = email;
                    emailPayload.SyncUuid = StringToGuid(email.uuid);
                    EmailDocument emailDocument = _emailAdressTransformation.GetTransformedDocument(emailPayload, Helper.ReducePayloadPath(links));
                    document.emails.Add(emailDocument);
                }
            document.name.Value = tradingAccount.name;
            // ????? document.onhold
            document.people = new PeopleDocumentCollection();
            if (tradingAccount.contacts != null && tradingAccount.contacts != null)
                foreach (contacttype contact in tradingAccount.contacts)
                {
                    ContactPayload contactPayload = new ContactPayload();
                    contactPayload.Contacttype = contact;
                    contactPayload.SyncUuid = StringToGuid( contact.uuid);
                    PersonDocument person = _contactTransformation.GetTransformedDocument(contactPayload, Helper.ReducePayloadPath(links));
                    document.people.Add(person);
                }
            bool hasMainPerson = false;
            for (int index = 0; index < document.people.documents.Count; index++)
            {
                PersonDocument person = document.people.documents[index] as PersonDocument;
                if ((person.primaryperson.Value != null) && (person.primaryperson.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase)))
                    hasMainPerson = true;
            }
            if ((!hasMainPerson) && (document.people.documents.Count > 0))
            {
                PersonDocument person = document.people.documents[0] as PersonDocument;
                person.primaryperson.Value = "true";
            }

            document.phones = new PhonesDocumentCollection();
            if (tradingAccount.phones != null && tradingAccount.phones != null)
                foreach (phoneNumbertype phoneNumber in tradingAccount.phones)
                {
                    PhoneNumberPayload phoneNumberPayload = new PhoneNumberPayload();
                    phoneNumberPayload.PhoneNumbertype = phoneNumber;
                    //phoneNumberPayload.SyncUuid = GetUuid(phoneNumber.applicationID);
                    PhoneDocument phone = _phoneNumberTransformation.GetTransformedDocument(phoneNumberPayload, Helper.ReducePayloadPath(links));
                    document.phones.Add(phone);
                }
            document.customerSupplierFlag.Value = tradingAccount.customerSupplierFlag;
            return document;
        }


        public TradingAccountPayload GetTransformedPayload(AccountDocument document, out List<SyncFeedEntryLink> links)
        {
            links = new List<SyncFeedEntryLink>();
            TradingAccountPayload payload = new TradingAccountPayload();
            tradingAccounttype tradingAccount = new tradingAccounttype();
            tradingAccount.accountingType = tradingAccountAccountingTypeenum.Unknown;
            tradingAccount.customerSupplierFlag = (document.customerSupplierFlag.IsNull) ? null : document.customerSupplierFlag.Value.ToString();
            tradingAccount.active = true;
            //tradingAccount.postalAddresses = new postalAddresstype[0]();
            //tradingAccount.contacts = new contacttype[0]();
            //tradingAccount.phones = new phoneNumbertype[0]();
            tradingAccount.deleted = false;
            tradingAccount.deliveryContact = null;
            tradingAccount.deliveryMethod = null;
            tradingAccount.deliveryRule = false;
            //tradingAccount.emails = new emailtype[0]();
            tradingAccount.applicationID = document.Id;
            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;
            tradingAccount.uuid = payload.SyncUuid.ToString();
            tradingAccount.label = SupportedResourceKinds.tradingAccounts.ToString();
            tradingAccount.name = (document.name.IsNull) ? null : document.name.Value.ToString(); 

            
            //Many more things should set to default values


            // adresses
            int adressCount = document.addresses.documents.Count;
            tradingAccount.postalAddresses = new postalAddresstype[adressCount];
            for (int index = 0; index < adressCount; index ++ )
            {
                List<SyncFeedEntryLink> addressLinks;
                AddressDocument address = document.addresses.documents[index] as AddressDocument;
                PostalAddressPayload postalAdressPayload;
                postalAdressPayload = _postalAdressTransformation.GetTransformedPayload(address, out addressLinks);
                tradingAccount.postalAddresses[index] = postalAdressPayload.PostalAddresstype;
                links.Add(SyncFeedEntryLink.CreateRelatedLink(
                        Common.ResourceKindHelpers.GetSingleResourceUrl(
                        _context.DatasetLink, SupportedResourceKinds.postalAddresses.ToString(), postalAdressPayload.LocalID),
                        "postalAddresses", 
                        "postalAddresses[" + index.ToString() + "]",
                        postalAdressPayload.SyncUuid.ToString()));
            }

            //emails
            int emailsCount = document.emails.documents.Count;
            tradingAccount.emails = new emailtype[emailsCount];
            for (int index = 0; index < emailsCount; index++)
            {
                List<SyncFeedEntryLink> emailLinks;
                EmailDocument email = document.emails.documents[index] as EmailDocument;
                EmailPayload EmailPayload;
                EmailPayload = _emailAdressTransformation.GetTransformedPayload(email, out emailLinks);
                tradingAccount.emails[index] = EmailPayload.Emailtype;

                links.Add(SyncFeedEntryLink.CreateRelatedLink(
                        Common.ResourceKindHelpers.GetSingleResourceUrl(
                        _context.DatasetLink, SupportedResourceKinds.emails.ToString(), EmailPayload.LocalID),
                        "emails", 
                        "emails[" + index.ToString() + "]",
                        EmailPayload.SyncUuid.ToString()));
            }


            //phones
            int phonesCount = document.phones.documents.Count;
            tradingAccount.phones = new phoneNumbertype[phonesCount];
            for (int index = 0; index < phonesCount; index++)
            {
                List<SyncFeedEntryLink> phoneLinks;
                PhoneDocument phone = document.phones.documents[index] as PhoneDocument;
                PhoneNumberPayload phoneNumberPayload;
                phoneNumberPayload = _phoneNumberTransformation.GetTransformedPayload(phone, out phoneLinks);
                tradingAccount.phones[index] = phoneNumberPayload.PhoneNumbertype;

                links.Add(SyncFeedEntryLink.CreateRelatedLink(
                        Common.ResourceKindHelpers.GetSingleResourceUrl(
                        _context.DatasetLink, SupportedResourceKinds.phoneNumbers.ToString(), phoneNumberPayload.LocalID),
                        "phones",
                        "phones[" + index.ToString() + "]",
                        phoneNumberPayload.SyncUuid.ToString()));
            }

            //contacts
            int contactsCount = document.people.documents.Count;
            tradingAccount.contacts = new contacttype[contactsCount];
            for (int index = 0; index < contactsCount; index++)
            {
                List<SyncFeedEntryLink> contactLinks;
                PersonDocument person = document.people.documents[index] as PersonDocument;
                ContactPayload contactPayload;
                contactPayload = _contactTransformation.GetTransformedPayload(person, out contactLinks);
                tradingAccount.contacts[index] = contactPayload.Contacttype;
                links.Add(SyncFeedEntryLink.CreateRelatedLink(
                        Common.ResourceKindHelpers.GetSingleResourceUrl(
                        _context.DatasetLink, SupportedResourceKinds.contacts.ToString(), contactPayload.LocalID),
                        "contacts",
                        "contacts[" + index.ToString() + "]",
                        contactPayload.SyncUuid.ToString()));
            }


            payload.TradingAccount = tradingAccount;
            
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.tradingAccounts, document.Id));
            links.Add(selfLink);
            return payload;

        }


       

        #endregion
    }
}
