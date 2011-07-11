using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

namespace Sage.Integration.Northwind.Feeds
{
    [XmlType("digestEntry", Namespace=Namespaces.syncNamespace)]
    public class SyncFeedDigestEntry
    {
        private string _endpoint;
        [XmlElement("endpoint", Namespace = Namespaces.syncNamespace)]
        public string Endpoint
        {
            get { return _endpoint; }
            set { _endpoint = value; }
        }
        private int _tick;
        [XmlElement("tick", Namespace = Namespaces.syncNamespace)]
        public int Tick
        {
            get { return _tick; }
            set { _tick = value; }
        }
        private DateTime _stamp;
        [XmlElement("stamp", Namespace = Namespaces.syncNamespace)]
        public DateTime Stamp
        {
            get { return _stamp; }
            set { _stamp = value; }
        }
        private int _conflictPriority;
        [XmlElement("conflictPriority", Namespace = Namespaces.syncNamespace)]
        public int ConflictPriority
        {
            get { return _conflictPriority; }
            set { _conflictPriority = value; }
        }
    }
}
