#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("CommodityGroup", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class CommodityGroupPayload : PayloadBase
    {
        #region Ctor.

        public CommodityGroupPayload()
        {
            this.CommodityGrouptype = new commodityGrouptype();
        }

        #endregion

        [XmlElement(ElementName = "commodityGroup", Namespace = Namespaces.northwindNamespace)]
        public commodityGrouptype CommodityGrouptype { get; set; }
    }
}
