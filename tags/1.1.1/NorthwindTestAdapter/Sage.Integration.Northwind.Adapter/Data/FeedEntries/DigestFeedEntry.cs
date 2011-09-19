using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using System.Xml.Serialization;

namespace Sage.Integration.Northwind.Adapter.Data.FeedEntries
{
   [XmlDetachedPayload("Digest", false)]
    public class DigestFeedEntry : FeedEntry
    {
        public Digest Digest { get; set; }
    }
}
