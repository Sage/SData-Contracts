#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("PriceList", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class PriceListPayload : PayloadBase
    {
        #region Ctor.

        public PriceListPayload()
        {
            this.PriceListtype = new priceListtype();
        }

        #endregion

        [XmlElement(ElementName = "priceList", Namespace = Namespaces.northwindNamespace)]
        public priceListtype PriceListtype { get; set; }
    }
}
