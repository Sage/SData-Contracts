#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using Sage.Common.Metadata;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    public class ResourceFeedEntry : FeedEntry
    {
        #region Ctor.

        public ResourceFeedEntry()
            : base()
        {
        }

        #endregion

        [SMEStringProperty(CanSort = true)]
        public string Name { get; set; }

        public string Description { get; set; }

        [SMEUriProperty("Link")]
        public string Link { get; set; }
    }
}
