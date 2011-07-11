#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("salesInvoice", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class SalesInvoicePayload : PayloadBase
    {
        #region Ctor.

        public SalesInvoicePayload()
        {
            this.SalesInvoicetype = new salesInvoicetype();
        }

        #endregion

        [XmlElement(ElementName = "salesInvoice", Namespace = Namespaces.northwindNamespace)]
        public salesInvoicetype SalesInvoicetype { get; set; }
    }
}
