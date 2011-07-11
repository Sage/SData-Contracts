#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Metadata;
using Sage.Common.Syndication;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    public class ContractFeedEntry : FeedEntry
    {
        public ContractFeedEntry()
            : base()
        {
        }

        [SMEStringProperty(Namespace = Namespaces.northwindNamespace)]
        public string Name { get; set; }
        
        [SMEStringProperty(Namespace = Namespaces.northwindNamespace)]
        public string Description { get; set; }

        //[SMEUriProperty(Name = "ContractSpecificationLink", Label="Contract Specification", AverageLength = 100, Namespace = Namespaces.northwindNamespace)]
        //public string ContractSpecificationLink { get; set; }

        [SMEUriProperty(Name = "Link", AverageLength = 100, Namespace = Namespaces.northwindNamespace)]
        public string Link { get; set; }

    }
}
