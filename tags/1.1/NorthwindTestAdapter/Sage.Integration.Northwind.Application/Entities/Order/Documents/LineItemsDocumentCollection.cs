using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Toolkit;
using System.Xml;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Application.API;

namespace Sage.Integration.Northwind.Application.Entities.Order.Documents
{
    public class LineItemsDocumentCollection : DocumentCollection
    {
        public LineItemsDocumentCollection()
            : base(Constants.EntityNames.LineItemCollection)
        { }

        public override Document GetDocumentTemplate()
        {
            return new LineItemDocument();
        }
    }
}
