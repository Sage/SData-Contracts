#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common.Paging;

#endregion

namespace Sage.Integration.Northwind.Adapter.Requests
{
    public static class LinkFactory
    {
        /// <summary>
        /// Creates and returns an array of feed entrylinks.
        /// </summary>
        /// <param name="resourcePayloadContainer"></param>
        /// <returns></returns>
        public static FeedLink[] CreateEntryLinks(RequestContext context, FeedEntry resourcePayloadContainer)
        {
            List<FeedLink> links = new List<FeedLink>();
#warning TODO!!!     

            #region self link
            
            links.Add(new FeedLink(resourcePayloadContainer.Uri, LinkType.Self, MediaType.AtomEntry, "Refresh"));

            #endregion
            
            #region edit link
            
            switch(resourcePayloadContainer.GetType().Name)
            {
                case "":
                case "a":

                    break;
            }

            #endregion

            #region schema link

            links.Add(new FeedLink(String.Format("{0}{1}/$schema", context.DatasetLink, context.ResourceKind.ToString()), LinkType.Schema, MediaType.Xml));
            
            #endregion

            #region template link

            //switch(resourcePayloadContainer.GetType().Name)
            //{
            //    case "":
            //    case "a":

            //        break;
            //}

            links.Add(new FeedLink(String.Format("{0}{1}/$template", context.DatasetLink, context.ResourceKind.ToString()), LinkType.Template, MediaType.AtomEntry));

            #endregion

            #region service link

            switch(resourcePayloadContainer.GetType().Name)
            {
                case "":
                case "a":

                    break;
            }
            
            #endregion

            #region related links

            switch(resourcePayloadContainer.GetType().Name)
            {
                case "":
                case "a":

                    break;
            }

            #endregion

            return links.ToArray();
        }      


        public static FeedLink[] CreatePagingLinks(PageInfo normalizedPageInfo, int totalCount, string requestUriWithoutQuery)
        {
            List<FeedLink> links = new List<FeedLink>();

            PageInfo firstPageInfo = PagingHelpers.GetFirstPageInfo(normalizedPageInfo, totalCount);
            links.Add(new FeedLink(string.Format("{0}?startIndex={1}&count={2}", requestUriWithoutQuery, firstPageInfo.StartIndex, firstPageInfo.Count), LinkType.First, MediaType.Atom, "First Page"));

            PageInfo lastPageInfo = PagingHelpers.GetLastPageInfo(normalizedPageInfo, totalCount);
            links.Add(new FeedLink(string.Format("{0}?startIndex={1}&count={2}", requestUriWithoutQuery, lastPageInfo.StartIndex, lastPageInfo.Count), LinkType.Last, MediaType.Atom, "Last Page"));

            PageInfo nextPageInfo = PagingHelpers.GetNextPageInfo(normalizedPageInfo, totalCount);
            if (null != nextPageInfo)
                links.Add(new FeedLink(string.Format("{0}?startIndex={1}&count={2}", requestUriWithoutQuery, nextPageInfo.StartIndex, nextPageInfo.Count), LinkType.Next, MediaType.Atom, "Next Page"));

            
            PageInfo previousPageInfo = PagingHelpers.GetPreviousPageInfo(normalizedPageInfo);
            if (null != previousPageInfo)
                links.Add(new FeedLink(string.Format("{0}?startIndex={1}&count={2}", requestUriWithoutQuery, previousPageInfo.StartIndex, previousPageInfo.Count), LinkType.Previous, MediaType.Atom, "Previous Page"));


            return links.ToArray();
        }

        public static FeedLink[] CreateFeedLinks(RequestContext context, string feedUrl)
        {
            List<FeedLink> links = new List<FeedLink>();
            string urlBasePath = (new Uri(feedUrl)).GetLeftPart(UriPartial.Path);

            #region self link

            links.Add(new FeedLink(feedUrl, LinkType.Self, MediaType.Atom, "Refresh"));

            #endregion

            #region schema link

            links.Add(new FeedLink(String.Format("{0}/$schema", urlBasePath), LinkType.Schema, MediaType.Xml, "Schema"));

            #endregion

            #region post/template links

            links.Add(new FeedLink(urlBasePath, LinkType.Post, MediaType.Atom, "Post"));
            links.Add(new FeedLink(String.Format("{0}/$template", urlBasePath), LinkType.Template, MediaType.Atom, "Template"));

            #endregion

            return links.ToArray();
        }
    }
}
