#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Entities.Account.Documents;
using Sage.Integration.Northwind.Feeds.TradingAccounts;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Integration.Northwind.Application.Entities.Email;
using Sage.Integration.Northwind.Adapter.Transformations;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Application.Base;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data
{
    public class EmailWrapper : EntityWrapperBase ,IEntityWrapper, IEntityQueryWrapper
    {
        #region Class Variables

        private ITransformation<EmailDocument, EmailPayload> _transformation;

        #endregion

        #region Ctor.

        public EmailWrapper(RequestContext context)
            : base(context, SupportedResourceKinds.emails)
        {
            _entity = new Email();
            _transformation = TransformationFactory.GetTransformation
                <ITransformation<EmailDocument, EmailPayload>>
                (SupportedResourceKinds.emails, context);
        }

        #endregion

        #region IEntityWrapper Members

        public override Document GetTransformedDocument(PayloadBase payload, List<SyncFeedEntryLink> links)
        {
            Document result = _transformation.GetTransformedDocument(payload as EmailPayload, Helper.ReducePayloadPath(links));
            return result;
        }

        public override PayloadBase GetTransformedPayload(Document document, out List<SyncFeedEntryLink> links)
        {
            PayloadBase result = _transformation.GetTransformedPayload(document as EmailDocument, out links);
            links = Helper.ExtendPayloadPath(links, "email");
            return result;
        }

        #endregion

        #region IEntityQueryWrapper Members

        string IEntityQueryWrapper.GetDbFieldName(string propertyName)
        {
            if (propertyName.Equals("applicationID", StringComparison.InvariantCultureIgnoreCase))
                return "id";
            if (propertyName.Equals("address", StringComparison.InvariantCultureIgnoreCase))
                return "Email";

            throw new InvalidOperationException(string.Format("Property {0} not supported.", propertyName));
        }

        #endregion
    }
}
