#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.TradingAccounts
{
    [ResourceDescription("PhoneNumber", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class PhoneNumberPayload : PayloadBase
    {
        #region Ctor.

        public PhoneNumberPayload()
        {
            this.PhoneNumbertype = new phoneNumbertype();
        }

        #endregion

        [XmlElement(ElementName = "phoneNumber", Namespace = Namespaces.northwindNamespace)]
        public phoneNumbertype PhoneNumbertype { get; set; }
    }
}
