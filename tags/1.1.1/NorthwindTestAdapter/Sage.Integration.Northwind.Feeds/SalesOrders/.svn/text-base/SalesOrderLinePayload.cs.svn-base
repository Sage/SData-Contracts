#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("SalesOrderLine", "", false)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class SalesOrderLinePayload : PayloadBase
    {
        #region Ctor.

        public SalesOrderLinePayload()
        {
            this.SalesOrderLinetype = new salesOrderLinetype();
        }

        #endregion

        [XmlElement(ElementName = "salesOrderLine", Namespace = Namespaces.northwindNamespace)]
        public salesOrderLinetype SalesOrderLinetype { get; set; }
    }
}
