#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.SalesOrders
{
    [ResourceDescription("UnitOfMeasureGroup", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class UnitOfMeasureGroupPayload : PayloadBase
    {
        #region Ctor.

        public UnitOfMeasureGroupPayload()
        {
            this.UnitOfMeasureGrouptype = new unitOfMeasureGrouptype();
        }

        #endregion

        [XmlElement(ElementName = "unitOfMeasureGroup", Namespace = Namespaces.northwindNamespace)]
        public unitOfMeasureGrouptype UnitOfMeasureGrouptype { get; set; }
    }
}
