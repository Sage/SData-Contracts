#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.TradingAccounts
{
    [ResourceDescription("postaladdresses", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class PostalAddressPayload : PayloadBase
    {
        #region Ctor.

        public PostalAddressPayload()
        {
            this.PostalAddresstype = new postalAddresstype();
        }

        #endregion

        [XmlElement(ElementName = "postalAddress", Namespace = Namespaces.northwindNamespace)]
        public postalAddresstype PostalAddresstype { get; set; }
    }
}
