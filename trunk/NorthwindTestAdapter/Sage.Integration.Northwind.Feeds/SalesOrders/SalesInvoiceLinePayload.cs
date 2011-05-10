#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("salesInvoiceLine", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class SalesInvoiceLinePayload : PayloadBase
    {
        #region Ctor.

        public SalesInvoiceLinePayload()
        {
            this.SalesInvoiceLinetype = new salesInvoiceLinetype();
        }

        #endregion

        [XmlElement(ElementName = "salesInvoiceLine", Namespace = Namespaces.northwindNamespace)]
        public salesInvoiceLinetype SalesInvoiceLinetype { get; set; }
    }
}
