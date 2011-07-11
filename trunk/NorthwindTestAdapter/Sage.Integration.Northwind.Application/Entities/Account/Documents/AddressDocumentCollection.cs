using System;
using Sage.Integration.Northwind.Application.Base;

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class AddressDocumentCollection : DocumentCollection
    {
        public AddressDocumentCollection()
            : base("addresses")
        {
        }

        public override Document GetDocumentTemplate()
        {
            return new AddressDocument();
        }
    }
}
