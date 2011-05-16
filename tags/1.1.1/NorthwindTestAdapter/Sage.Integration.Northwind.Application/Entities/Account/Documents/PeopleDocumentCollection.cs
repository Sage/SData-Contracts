using System;
using Sage.Integration.Northwind.Application.Base;

namespace Sage.Integration.Northwind.Application.Entities.Account.Documents
{
    public class PeopleDocumentCollection : DocumentCollection
    {
        public PeopleDocumentCollection()
            : base("people")
        {}


        public override Document GetDocumentTemplate()
        {
            return new PersonDocument();
        }
    }
}
