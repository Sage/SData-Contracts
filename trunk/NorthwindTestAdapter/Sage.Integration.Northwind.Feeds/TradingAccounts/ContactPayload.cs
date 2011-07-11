#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.TradingAccounts
{
    [ResourceDescription("Contact", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class ContactPayload : PayloadBase
    {
        #region Ctor.

        public ContactPayload()
        {
            this.Contacttype = new contacttype();
        }

        #endregion

        [XmlElement(ElementName = "contact", Namespace = Namespaces.northwindNamespace)]
        public contacttype Contacttype { get; set; }
    }
}
