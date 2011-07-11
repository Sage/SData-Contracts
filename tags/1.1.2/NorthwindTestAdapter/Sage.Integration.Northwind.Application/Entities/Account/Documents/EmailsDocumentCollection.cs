using System;
using Sage.Integration.Northwind.Application.Base;

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class EmailsDocumentCollection : DocumentCollection
    {
        public EmailsDocumentCollection()
            : base("emails")
        { }

        public override Document GetDocumentTemplate()
        {
            return new EmailDocument();
        }
    }
}
