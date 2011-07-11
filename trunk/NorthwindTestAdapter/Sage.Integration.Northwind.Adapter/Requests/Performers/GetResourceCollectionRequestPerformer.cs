#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Common;
using System.Diagnostics;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Performers
{
    internal class GetResourceCollectionRequestPerformer : IRequestPerformer
    {
        #region Class Variables

        private RequestContext _requestContext;

        #endregion

        #region IRequestPerformer Members

        public void DoWork(IRequest request)
        {
            Feed<ResourceFeedEntry> feed = new Feed<ResourceFeedEntry>();
            feed.Title = "Available Resources";

            Dictionary<SupportedResourceKinds, Type> resourcePayloadTypes = ResourceKindHelpers.GetAllResourcePayloadTypes();

            string resourceName;
            string resourceDescription;
            string resourceLink;

            foreach (SupportedResourceKinds resKind in  resourcePayloadTypes.Keys)
            {
                Type resourceType = resourcePayloadTypes[resKind];
                ResourceDescriptionAttribute resourceDescriptionAttr;

                if (ReflectionHelpers.TryGetSingleCustomAttribute<ResourceDescriptionAttribute>(resourceType, out resourceDescriptionAttr))
                {
                    if (resourceDescriptionAttr.CanGet)
                    {
                        resourceName = resourceDescriptionAttr.Name;
                        resourceDescription = resourceDescriptionAttr.Description;
                        resourceLink = string.Format("{0}{1}/", _requestContext.DatasetLink, resKind.ToString());

                        ResourceFeedEntry entry = new ResourceFeedEntry();
                        entry.Id = resourceLink;
                        entry.Name = resourceName;
                        entry.Description = resourceDescription;
                        entry.Link = resourceLink;
                        entry.Updated = DateTime.Now;
                        entry.Summary = resourceDescription;
                        entry.Source = resourceDescription;
                        FeedLink feedentryLink = new FeedLink(resourceLink, LinkType.Self, MediaType.Atom, entry.Description);
                        entry.Links.Add(feedentryLink);

                        feedentryLink = new FeedLink(resourceLink, LinkType.Related, MediaType.Atom, entry.Description);
                        entry.Links.Add(feedentryLink);


                        feed.Entries.Add(entry);
                    }
                }
                else
                {
                    // only in debug mode!
                    //this.ThrowResourceDescriptionAttributeMissing(resourceType);
                }
            }        
            
            request.Response.Feed = feed;
        }

        public void Initialize(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        #endregion


        #region Private Helpers

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void ThrowResourceDescriptionAttributeMissing(Type resourcePayloadType)
        {
            throw new ApplicationException(string.Format("Attribute '{0}' missing for payload class {1}.", typeof(ResourceDescriptionAttribute).FullName, resourcePayloadType.FullName));
        }

        #endregion
    }
}
