#region Usings

using System;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Common.Paging;
//using Sage.Integration.Northwind.Adapter.Common.Paging;

#endregion

namespace Sage.Integration.Northwind.Adapter.Requests
{
    internal static class FeedMetadataHelpers
    {
        public static int DEFAULT_ITEMS_PER_PAGE = 10;

        public static string BuildBaseUrl(RequestContext requestContext, RequestKeywordType keyword)
        {
            string resourceKindName = requestContext.ResourceKind.ToString();
            string keywordAsString = GetKeywordAsString(keyword);
            if (null == keywordAsString)
                return string.Format("{0}{1}", requestContext.DatasetLink, resourceKindName);
            else
                return string.Format("{0}{1}/{2}", requestContext.DatasetLink, resourceKindName, keywordAsString);
        }
        private static string GetKeywordAsString(RequestKeywordType keyword)
        {
            switch(keyword)
            {
                case RequestKeywordType.none:
                    return null;
                case RequestKeywordType.linked:
                    return Constants.linked;
                default:
                    throw new NotSupportedException("RequestKeyword '{0}' not supported.");
            }
        }

//        public static PageController GetPageLinkBuilder(RequestContext requestContext, int totalResults, RequestKeywordType keyword)
//        {
//            string url = BuildBaseUrl(requestContext, keyword);

//            int count;// = (int)requestContext.SdataUri.Count;
//            int startIndex;// = (int)requestContext.SdataUri.StartIndex;
//#warning THIS IS A WORKAROUND UNTIL SIF IS 1 BASED!
//            string strStartIndex;
//            if (!requestContext.SdataUri.QueryArgs.TryGetValue("startIndex", out strStartIndex))
//                startIndex = 1;
//            else
//                startIndex = Convert.ToInt32(strStartIndex);
//            //if (startIndex == 0) { startIndex = 1; }
//#warning THIS IS A WORKAROUND UNTIL SIF SUPPORTS COUNT=0 AND COUNT NOT SET
//            string strCount;
//            if (!requestContext.SdataUri.QueryArgs.TryGetValue("count", out strCount))
//                count = -1;
//            else
//                count = Convert.ToInt32(strCount);
//            return new PageController(startIndex, DEFAULT_ITEMS_PER_PAGE, totalResults, count, url);
//        }

        //public static FeedLinkCollection CreatePageFeedLinks(RequestContext requestContext, int totalResults, RequestKeywordType keyword)
        //{
        //    FeedLinkCollection feedLinks = new FeedLinkCollection();
        //    PageController builder = GetPageLinkBuilder(requestContext, totalResults, keyword);

        //    feedLinks.Add(new FeedLink(builder.GetLinkSelf(), LinkType.Self, MediaType.Atom, "Current Page"));
        //    feedLinks.Add(new FeedLink(builder.GetLinkFirst(), LinkType.First, MediaType.Atom, "First Page"));
        //    feedLinks.Add(new FeedLink(builder.GetLinkLast(), LinkType.Last, MediaType.Atom, "Last Page"));

        //    string linkUrl;
        //    if (builder.GetLinkNext(out linkUrl))
        //        feedLinks.Add(new FeedLink(linkUrl, LinkType.Next, MediaType.Atom, "Next Page"));
        //    if (builder.GetLinkPrevious(out linkUrl))
        //        feedLinks.Add(new FeedLink(linkUrl, LinkType.Previous, MediaType.Atom, "Previous Page"));

        //    return feedLinks;
        //}
        public static string BuildEntryResourceUrl(RequestContext requestContext, string localId)
        {
            return String.Format("{0}{1}('{2}')", requestContext.DatasetLink, requestContext.ResourceKind.ToString(), localId);
        }
        public static string BuildEntryResourceTitle(RequestContext requestContext, string localId)
        {
            return String.Format("{0}: {1}", requestContext.ResourceKind.ToString(), localId);
        }

        public static string BuildLinkedEntryUrl(RequestContext requestContext, Guid uuid)
        {
            string linkedBaseUrl = BuildBaseUrl(requestContext, RequestKeywordType.linked);
            return String.Format("{0}('{1}')", linkedBaseUrl, uuid);
        }
        public static string BuildLinkedEntryTitle(RequestContext requestContext, Guid uuid)
        {
            return string.Format("{0} link {1}", requestContext.ResourceKind.ToString(), uuid);
        }

        //public static int GetPageNumber(RequestContext requestContext)
        //{
        //    return ((int)(requestContext.SdataUri.StartIndex / FeedMetadataHelpers.DEFAULT_ITEMS_PER_PAGE)) + 1;
        //}
        

        #region ENUM: RequestKeywordType
        
        public enum RequestKeywordType { none, linked } 
        
        #endregion

        
    }
}
