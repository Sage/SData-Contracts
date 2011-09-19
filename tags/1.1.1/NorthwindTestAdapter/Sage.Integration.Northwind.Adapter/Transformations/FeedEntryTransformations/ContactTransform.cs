using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class ContactTransform : TransformationBase, IFeedEntryTransformation<PersonDocument, ContactFeedEntry>
    {
        public ContactTransform(RequestContext context) :  base(context, SupportedResourceKinds.contacts){}

        public string TradingAccountId { get; set; }

        public PersonDocument GetTransformedDocument(ContactFeedEntry feedEntry)
        {
            PersonDocument result = new PersonDocument();

            if(feedEntry.IsPropertyChanged("familyName"))
                result.lastname.Value = feedEntry.familyName;
            if (feedEntry.IsPropertyChanged("firstName"))
                result.firstname.Value = feedEntry.firstName;
            if (feedEntry.IsPropertyChanged("middleName"))
                result.middlename.Value = feedEntry.middleName;
            /*if (feedEntry.IsPropertyChanged("salutation"))
                result.salutation.Value = feedEntry.salutation;*/
            if (feedEntry.IsPropertyChanged("suffix"))
                result.suffix.Value = feedEntry.suffix;
            if (feedEntry.IsPropertyChanged("title"))
                result.title.Value = feedEntry.title;
            if (feedEntry.IsPropertyChanged("fullName"))
                result.fullname.Value = feedEntry.fullName;
            if (feedEntry.IsPropertyChanged("primacyIndicator"))
                result.primaryperson.Value = true;

            result.Id = feedEntry.Key;

            return result;
        }

        public ContactFeedEntry GetTransformedPayload(PersonDocument document)
        {
            //PersonDocument document = (PersonDocument)accountDocument.people.documents[0];
            ContactFeedEntry contact = new ContactFeedEntry();

            contact.familyName = (document.lastname.IsNull) ? null : document.lastname.Value.ToString();
            contact.firstName = (document.firstname.IsNull) ? null : document.firstname.Value.ToString();
            contact.middleName = (document.middlename.IsNull) ? null : document.middlename.Value.ToString();
            //contact.salutation = (document.salutation.IsNull) ? null : document.salutation.Value.ToString();
            contact.suffix = (document.suffix.IsNull) ? null : document.suffix.Value.ToString();
            contact.title = (document.title.IsNull) ? null : document.title.Value.ToString();
            contact.fullName = (document.fullname.IsNull) ? null : document.fullname.Value.ToString();
            contact.active = true;

            if (TradingAccountId != null)
            {
                contact.tradingAccount = new TradingAccountFeedEntry();
                contact.tradingAccount.UUID = GetTradingAccountUuid(TradingAccountId);
                contact.tradingAccount.Key = TradingAccountId;
                contact.tradingAccount.Uri = new SDataUri(_context.SdataUri.Uri).SetPath(new string[] { _context.SdataUri.Product, _context.SdataUri.Contract, _context.SdataUri.CompanyDataset, "tradingAccounts('" + TradingAccountId + "')" }).Uri.ToString();
            }

            if ((!document.primaryperson.IsNull) && (document.primaryperson.Value.ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase)))
                contact.primacyIndicator = true;

            if (string.IsNullOrEmpty(contact.fullName))
                return null;

            SetCommonProperties(document, contact.fullName, contact);
            return contact;
        }

        private Guid GetTradingAccountUuid(string localId)
        {
            if (String.IsNullOrEmpty(localId))
            {
                return Guid.Empty;
            }
            CorrelatedResSyncInfo[] results = _correlatedResSyncInfoStore.GetByLocalId(
                SupportedResourceKinds.tradingAccounts.ToString(), new string[] { localId });
            if (results.Length > 0)
                return results[0].ResSyncInfo.Uuid;

            return Guid.Empty;

        }

        private static string GetRelationshipUri(RequestContext request, FeedEntry entry, string suffix)
        {
            if (request.SdataUri.HasCollectionPredicate)
            {
                return new SDataUri(request.SdataUri).AppendPath(new UriPathSegment(suffix)).ToString();
            }
            else
            {
                 return entry.Id + suffix;
            }
        }


        private string buildId(SDataUri uri, string p)
        {
            if (!uri.HasCollectionPredicate)
                return uri.FullPath + "('" + p + "')";
            return uri.ToString();
        }
    }
}
