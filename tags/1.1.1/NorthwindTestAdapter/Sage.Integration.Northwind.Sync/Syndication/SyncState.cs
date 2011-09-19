using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

namespace Sage.Integration.Northwind.Sync.Syndication
{
    [XmlRootAttribute("syncState", Namespace = Namespaces.syncNamespace, IsNullable = false)]
    public class SyncState1
    {
        private string _EndPoint;
        private int _tick;
        private DateTime _stamp;

        public SyncState1() : this("",0)
        {
        }

        public SyncState1(string EndPoint, int tick)
        {
            _EndPoint = EndPoint;
            _tick = tick;
            _stamp = DateTime.Now;
        }

        


        [XmlElement(ElementName = "EndPoint", Namespace = Namespaces.syncNamespace)]
        public string EndPoint
        {
            get { return _EndPoint; }
            set { _EndPoint = value; }
        }

        [XmlElement(ElementName = "tick", Namespace = Namespaces.syncNamespace)]
        public int tick
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
    }
}
