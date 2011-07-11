#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("SalesOrders", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class SalesOrderPayload : PayloadBase
    {
        #region Ctor.

        public SalesOrderPayload()
        {
            this.SalesOrdertype = new salesOrdertype();
        }

        #endregion

        [XmlElement(ElementName = "salesOrder", Namespace = Namespaces.northwindNamespace)]
        public salesOrdertype SalesOrdertype { get; set; }
    }
}
