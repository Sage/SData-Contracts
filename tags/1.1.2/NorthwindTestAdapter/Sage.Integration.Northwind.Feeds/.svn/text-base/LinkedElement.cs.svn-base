#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    [XmlType("linked", Namespace = Namespaces.syncNamespace)]
    [XmlRoot("linked", Namespace = Namespaces.syncNamespace)]
    public class LinkedElement
    {
        [XmlElement("resource", Namespace = Namespaces.syncNamespace)]
        public string Resource { get; set; }
        [XmlElement("uuid", Namespace = Namespaces.syncNamespace)]
        public Guid Uuid { get; set; }
    }
}
