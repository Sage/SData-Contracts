using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

namespace Sage.Integration.Northwind.Sync.Syndication
{
    [XmlRootAttribute("syncState", Namespace = Namespaces.syncNamespace, IsNullable = false)]
    public class SyncState
    {
        private string _endpoint;
        private int _tick;
        private DateTime _stamp;

        public SyncState() : this("",0)
        {
        }

        public SyncState(string endPoint, int tick)
        {
            _endpoint = endPoint;
            _tick = tick;
            _stamp = DateTime.Now;
        }

        


        [XmlElement(ElementName = "endpoint", Namespace = Namespaces.syncNamespace)]
        public string Endpoint
        {
            get { return _endpoint; }
            set { _endpoint = value; }
        }

        [XmlElement(ElementName = "tick", Namespace = Namespaces.syncNamespace)]
        public int Tick
        {
            get { return _tick; }
            set { _tick = value; }
        }

        [XmlElement(ElementName = "stamp", Namespace = Namespaces.syncNamespace)]
        public DateTime Stamp
        {
            get { return _stamp; }
            set { _stamp = value; }
        }

        
        
        [XmlNamespaceDeclarations()]
        [XmlIgnore()]
        private static XmlSerializerNamespaces SerializerNamespaces
        {
            get
            {
                return NameSpaceHelpers.SerializerNamespaces;
            }
        }
    }
}
