using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Common.Syndication;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    public class TradingAccountTransform : TransformationBase, IFeedEntryTransformation<AccountDocument, TradingAccountFeedEntry>
    {

        private readonly PostalAdressTransform _postalAdressTransformation;
        private readonly ContactTransform _contactTransformation;
        private readonly EMailAddressTransform _emailAdressTransformation;
        private readonly PhoneNumberTransform _phoneNumberTransformation;

        public TradingAccountTransform(RequestContext context) : base(context, SupportedResourceKinds.tradingAccounts)
        {
            _postalAdressTransformation = new PostalAdressTransform(context);

            _contactTransformation = new ContactTransform(context);

            _emailAdressTransformation = new EMailAddressTransform(context);

            _phoneNumberTransformation = new PhoneNumberTransform(context);
        }

        public AccountDocument GetTransformedDocument(TradingAccountFeedEntry feedEntry)
        {
            AccountDocument accountDocument = new AccountDocument();

            #region Postal adresses
            accountDocument.addresses = new AddressDocumentCollection();

            if (feedEntry.postalAddresses != null && feedEntry.postalAddresses != null)
                foreach (PostalAddressFeedEntry postalAdressEntry in feedEntry.postalAddresses.Entries)
                {
                    AddressDocument address = _postalAdressTransformation.GetTransformedDocument(postalAdressEntry);
                    accountDocument.addresses.Add(address);
                }

            bool hasMainAdress = false;
            for (int index = 0; index < accountDocument.addresses.documents.Count; index++)
            {
                AddressDocument address = accountDocument.addresses.documents[index] as AddressDocument;
                if ((address.primaryaddress.Value != null) && (address.primaryaddress.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase)))
                    hasMainAdress = true;
            }
            if ((!hasMainAdress) && (accountDocument.addresses.documents.Count > 0))
            {
                AddressDocument address = accountDocument.addresses.documents[0] as AddressDocument;
                address.primaryaddress.Value = "true";
            }

            #endregion postal adresses

            if (GuidIsNullOrEmpty(feedEntry.UUID))
            {
                accountDocument.Id = feedEntry.Key;
            }
            else
            {
                accountDocument.CrmId = feedEntry.UUID.ToString();
                accountDocument.Id = GetLocalId(feedEntry.UUID);
            }

            if(feedEntry.IsPropertyChanged("currency"))
                accountDocument.currencyid.Value = feedEntry.currency;


            #region emails
            accountDocument.emails = new EmailsDocumentCollection();
            if (feedEntry.emails != null)
                foreach (EmailFeedEntry emailEntry in feedEntry.emails.Entries)
                {
                    EmailDocument emailDocument = _emailAdressTransformation.GetTransformedDocument(emailEntry);
                    accountDocument.emails.Add(emailDocument);
                }
            #endregion

            if (feedEntry.IsPropertyChanged("name"))
                accountDocument.name.Value = feedEntry.name;

            // ????? document.onhold

            #region contacts
            accountDocument.people = new PeopleDocumentCollection();
            if (feedEntry.contacts != null)
                foreach (ContactFeedEntry contact in feedEntry.contacts.Entries)
                {
                    PersonDocument person = _contactTransformation.GetTransformedDocument(contact);
                    accountDocument.people.Add(person);
                }
            bool hasMainPerson = false;
            for (int index = 0; index < accountDocument.people.documents.Count; index++)
            {
                PersonDocument person = accountDocument.people.documents[index] as PersonDocument;
                if ((person.primaryperson.Value != null) && (person.primaryperson.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase)))
                    hasMainPerson = true;
            }
            if ((!hasMainPerson) && (accountDocument.people.documents.Count > 0))
            {
                PersonDocument person = accountDocument.people.documents[0] as PersonDocument;
                person.primaryperson.Value = "true";
            }
            #endregion

            #region phones
            accountDocument.phones = new PhonesDocumentCollection();
            if (feedEntry.phones != null)
                foreach (PhoneNumberFeedEntry phoneNumberPayload in feedEntry.phones.Entries)
                {
                    PhoneDocument phone = _phoneNumberTransformation.GetTransformedDocument(phoneNumberPayload);
                    accountDocument.phones.Add(phone);
                }
            #endregion

            if (feedEntry.IsPropertyChanged("customerSupplierFlag"))
                accountDocument.customerSupplierFlag.Value = Enum.GetName(typeof(supplierFlagenum), feedEntry.customerSupplierFlag);

            return accountDocument;
        }

        public TradingAccountFeedEntry GetTransformedPayload(AccountDocument document)
        {
            TradingAccountFeedEntry entry = new TradingAccountFeedEntry();
            entry.customerSupplierFlag = GetSupplierFlag(document.customerSupplierFlag);
            entry.active = true;

            entry.deleted = false;
            entry.deliveryRule = false;

            entry.name = (document.name.IsNull) ? null : document.name.Value.ToString();

            #region addresses
            int adressCount = document.addresses.documents.Count;
            entry.postalAddresses = new PostalAddressFeed();
            entry.postalAddresses.Id = GetSDataId(document.Id) + "/" + SupportedResourceKinds.postalAddresses.ToString();
            for (int index = 0; index < adressCount; index++)
            {
                AddressDocument address = document.addresses.documents[index] as AddressDocument;
                PostalAddressFeedEntry postalAdressEntry = _postalAdressTransformation.GetTransformedPayload(address);
                if(postalAdressEntry != null)
                    entry.postalAddresses.Entries.Add(postalAdressEntry);
            }
            #endregion

            #region emails
            int emailsCount = document.emails.documents.Count;
            entry.emails = new EmailFeed();
            for (int index = 0; index < emailsCount; index++)
            {
                EmailDocument email = document.emails.documents[index] as EmailDocument;
                EmailFeedEntry emailEntry = _emailAdressTransformation.GetTransformedPayload(email);
                entry.emails.Entries.Add(emailEntry);
            }
            #endregion


            #region phones
            int phonesCount = document.phones.documents.Count;
            entry.phones = new PhoneNumberFeed();
            for (int index = 0; index < phonesCount; index++)
            {
                PhoneDocument phone = document.phones.documents[index] as PhoneDocument;
                PhoneNumberFeedEntry phoneNumberEntry = _phoneNumberTransformation.GetTransformedPayload(phone);
                if(phoneNumberEntry != null)
                    entry.phones.Entries.Add(phoneNumberEntry);
            }
            #endregion

            #region contacts
            int contactsCount = document.people.documents.Count;
            entry.contacts = new ContactFeed();
            for (int index = 0; index < contactsCount; index++)
            {
                PersonDocument person = document.people.documents[index] as PersonDocument;
                ContactFeedEntry contactEntry = _contactTransformation.GetTransformedPayload(person);
                if(contactEntry != null)
                    entry.contacts.Entries.Add(contactEntry);
            }
            #endregion

            entry.currency = _config.CurrencyCode;
            SetCommonProperties(document, entry.name, entry);
            return entry;
        }

        private supplierFlagenum GetSupplierFlag(Application.Base.Property flag)
        {
            try
            {
                return (supplierFlagenum)Enum.Parse(typeof(supplierFlagenum), flag.Value.ToString().Trim(), true);
            }
            catch
            {
                return supplierFlagenum.Unknown;
            }
        }
    }
}
