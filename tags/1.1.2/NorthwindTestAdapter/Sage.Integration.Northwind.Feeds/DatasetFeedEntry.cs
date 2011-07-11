#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using Sage.Common.Metadata;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    public class DatasetFeedEntry : FeedEntry
    {
        #region Ctor.

        public DatasetFeedEntry()
            : base()
        {
        }

        #endregion

        [SMEStringProperty(Namespace = Namespaces.northwindNamespace)]
        public string DatabaseName { get; set; }

        [SMEUriProperty(Name = "Link", AverageLength = 100, Namespace = Namespaces.northwindNamespace)]
        public string Link { get; set; }
    }
}
