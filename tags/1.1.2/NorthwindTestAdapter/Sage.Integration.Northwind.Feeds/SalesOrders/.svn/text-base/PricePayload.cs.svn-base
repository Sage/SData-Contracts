#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("Price", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class PricePayload : PayloadBase
    {
        #region Ctor.

        public PricePayload()
        {
            this.Pricetype = new pricetype();
        }

        #endregion

        [XmlElement(ElementName = "price", Namespace = Namespaces.northwindNamespace)]
        public pricetype Pricetype { get; set; }
    }
}
