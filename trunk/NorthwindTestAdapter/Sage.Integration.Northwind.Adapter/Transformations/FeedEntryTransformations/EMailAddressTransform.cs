using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Feeds;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Adapter.Transformations;

namespace Sage.Integration.Northwind.Adapter.Transform
{
    class EMailAddressTransform : TransformationBase, IFeedEntryTransformation<EmailDocument, EmailFeedEntry>
    {
        private Adapter.Common.RequestContext context;

        public EMailAddressTransform(Adapter.Common.RequestContext context) : base(context, Adapter.Common.SupportedResourceKinds.emails){ }

        public EmailDocument GetTransformedDocument(EmailFeedEntry feedEntry)
        {
            EmailDocument document = new EmailDocument();
            if (feedEntry.UUID == null || feedEntry.UUID == Guid.Empty)
            {
                document.Id = feedEntry.Key;
            }
            else
            {
                document.CrmId = feedEntry.UUID.ToString();
                document.Id = GetLocalId(feedEntry.UUID);
            }

            if (feedEntry.IsPropertyChanged("address"))
                document.emailaddress.Value = feedEntry.address;
            return document;
        }

        public EmailFeedEntry GetTransformedPayload(EmailDocument document)
        {
            EmailFeedEntry entry = new EmailFeedEntry();

            entry.address = (document.emailaddress.IsNull) ? null : document.emailaddress.Value.ToString();

            SetCommonProperties(document, entry.address, entry);
            return entry;
        }
    }
}
