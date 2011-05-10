#region Usings

using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds.TradingAccounts
{
    [ResourceDescription("Emails", "", true)]
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class EmailPayload : PayloadBase
    {
        #region Ctor.

        public EmailPayload()
        {
            this.Emailtype = new emailtype();
        }

        #endregion

        [XmlElement(ElementName = "email", Namespace = Namespaces.northwindNamespace)]
        public emailtype Emailtype { get; set; }
    }
}
