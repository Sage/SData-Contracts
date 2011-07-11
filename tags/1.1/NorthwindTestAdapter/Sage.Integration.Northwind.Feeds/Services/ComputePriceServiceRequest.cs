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
    public class ComputePriceServiceRequest : PayloadBase
    {
        #region Ctor.

        public ComputePriceServiceRequest()
        {
            this.Request = new ComputePriceRequestPayload();
        }

        #endregion

        [XmlElement(ElementName = "request", Namespace = Namespaces.northwindNamespace)]
        public ComputePriceRequestPayload Request { get; set; }
    }
}
