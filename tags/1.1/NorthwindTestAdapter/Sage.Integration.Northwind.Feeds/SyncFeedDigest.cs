using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

namespace Sage.Integration.Northwind.Feeds
{
    [XmlInclude(typeof(SyncFeedDigestEntry))]
    [XmlType("digest", Namespace = Namespaces.syncNamespace)]
    [XmlRoot("digest", Namespace = Namespaces.syncNamespace)]
    public class SyncFeedDigest
    {
        private string _origin;
        [XmlElement("origin", Namespace = Namespaces.syncNamespace)]
        public string Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        private System.Collections.IList _entries = new List<SyncFeedDigestEntry>();
        [XmlElement("digestEntry", Namespace = Namespaces.syncNamespace, Type = typeof(SyncFeedDigestEntry))]
        //[XmlArrayItem(ElementName = "digestEntry", Type = typeof(SyncFeedDigestEntry), Namespace = Namespaces.syncNamespace)]
        public System.Collections.IList Entries
        {
            get { return _entries; }
            set { _entries = value; }
        }
    }
}
