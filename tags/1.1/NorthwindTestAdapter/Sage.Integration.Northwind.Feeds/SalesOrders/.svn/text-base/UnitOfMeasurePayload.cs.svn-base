#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("UnitOfMeasure", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class UnitOfMeasurePayload : PayloadBase
    {
        #region Usings

        public UnitOfMeasurePayload()
        {
            this.UnitOfMeasuretype = new unitOfMeasuretype();
        }

        #endregion

        [XmlElement(ElementName = "unitOfMeasure", Namespace = Namespaces.northwindNamespace)]
        public unitOfMeasuretype UnitOfMeasuretype { get; set; }
    }
}
