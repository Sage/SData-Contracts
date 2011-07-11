#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.Services
{
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class ComputePriceServiceResponse : PayloadBase
    {
        #region Ctor.

        public ComputePriceServiceResponse()
        {
            this.Response = new ComputePriceResponsePayload();
        }

        #endregion

        [XmlElement(ElementName = "response", Namespace = Namespaces.northwindNamespace)]
        public ComputePriceResponsePayload Response { get; set; }
    }
}
