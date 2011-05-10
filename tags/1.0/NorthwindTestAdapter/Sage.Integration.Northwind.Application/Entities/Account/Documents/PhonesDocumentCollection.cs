using System;
using Sage.Integration.Northwind.Application.Base;

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class PhonesDocumentCollection : DocumentCollection
    {
        public PhonesDocumentCollection()
            : base("phones")
        { }

        public override Document GetDocumentTemplate()
        {
            return new PhoneDocument();
        }
    }
}
