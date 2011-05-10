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
    public class EmailAdressTransformation :TransformationBase, ITransformation<EmailDocument, EmailPayload>
    {
        #region Ctor.

        public EmailAdressTransformation(RequestContext context)
            : base(context, SupportedResourceKinds.emails)
        {
        }

        #endregion

        #region ITransformation<EmailDocument,EmailPayload> Members

        public EmailDocument GetTransformedDocument(EmailPayload payload, List<SyncFeedEntryLink> links)
        {
            EmailDocument document = new EmailDocument();
            emailtype email = payload.Emailtype;
            if (String.IsNullOrEmpty(payload.LocalID))
            {
                document.CrmId = payload.SyncUuid.ToString();//
                document.Id = GetLocalId(payload.SyncUuid);
            }
            else
            {
                document.Id = payload.LocalID;
            }
            document.emailaddress.Value = email.address;
            document.type.Value = email.type;
            return document;
        }

        public EmailPayload GetTransformedPayload(EmailDocument document, out List<SyncFeedEntryLink> links)
        {
            emailtype email = new emailtype();
            links = new List<SyncFeedEntryLink>();
            EmailPayload payload = new EmailPayload();
            payload.SyncUuid = GetUuid(document.Id, document.CrmId);
            payload.LocalID = document.Id;
            email.uuid = payload.SyncUuid.ToString();
            email.applicationID = document.Id;
            SyncFeedEntryLink selfLink = SyncFeedEntryLink.CreateSelfLink(String.Format("{0}{1}('{2}')", _datasetLink, SupportedResourceKinds.emails, document.Id));
            links.Add(selfLink);
            email.type = (document.type.IsNull) ? null : document.type.Value.ToString(); ;
            email.address = (document.emailaddress.IsNull) ? null : document.emailaddress.Value.ToString(); ;
            
            payload.Emailtype = email;
            
            return payload;
        }

        #endregion
    }
}
