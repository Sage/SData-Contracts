#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("Commodity", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class CommodityPayload : PayloadBase
    {
        #region Ctor.

        public CommodityPayload()
        {
            this.Commoditytype = new commoditytype();
        }

        #endregion

        [XmlElement(ElementName = "commodity", Namespace = Namespaces.northwindNamespace)]
        public commoditytype Commoditytype { get; set; }
    }
}
