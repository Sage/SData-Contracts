#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.TradingAccounts
{
    [ResourceDescription("TradingAccounts", "", true)]
    [XmlRootAttribute("payload", DataType = "payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class TradingAccountPayload : PayloadBase
    {
        #region Ctor.

        public TradingAccountPayload()
        {
            this.TradingAccount = new tradingAccounttype();
        }

        #endregion

        [XmlElement(ElementName = "tradingAccount", Namespace = Namespaces.northwindNamespace)]
        public tradingAccounttype TradingAccount { get; set; }
    }
}
