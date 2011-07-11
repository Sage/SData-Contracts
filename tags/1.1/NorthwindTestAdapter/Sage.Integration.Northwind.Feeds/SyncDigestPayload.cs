using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

namespace Sage.Integration.Northwind.Feeds
{
    [XmlRootAttribute("payload", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class SyncDigestPayload : PayloadBase
    {
        private SyncFeedDigest  _syncFeedDigest = new SyncFeedDigest();

        [XmlElement(ElementName = "digest", Namespace = Namespaces.syncNamespace)]
        public SyncFeedDigest Digest
        {
            get { return _syncFeedDigest; }
            set { _syncFeedDigest = value; }
        }
    }
}