#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.Paging;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Paging
{
    public class PageController
    {
        #region Properties

        public int StartIndex { get; private set; }
        public int ItemsPerPage { get; private set; }
        public int TotalResults { get; private set; }
        public string Url { get; private set; }
        
        public int LastIndex { get; private set; }

        #endregion

        #region Ctor.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalResult"></param>
        /// <param name="count">limits the number of entries</param>
        /// <param name="url"></param>
        public PageController(int startIndex, int itemsPerPage, int totalResult, int count, string url)
        {
            // TODO: Validate input parameters ???

            this.StartIndex = startIndex;
            this.TotalResults = totalResult;
            this.Url = url;


            if (count < itemsPerPage && count >= 0)
                itemsPerPage = count;

            //itemsPerPage = totalResult - this.StartIndex + 1;

            this.LastIndex = this.StartIndex + itemsPerPage -1;
            if (this.LastIndex > totalResult)
                this.LastIndex = totalResult;
            this.ItemsPerPage = itemsPerPage;
        }

        #endregion

        #region LINKS

        public string GetLinkFirst()
        {
            // the query parameters are separated by &amp; rather than &. 
            // This is because the ampersand character is a special character in XML
            // and it needs to be specially marked in this context.
            // THIS IS DONE BY SIF
            return String.Format("{0}?startIndex=1&count={1}", this.Url, this.ItemsPerPage);
        }
        public string GetLinkSelf()
        {
            // the query parameters are separated by &amp; rather than &. 
            // This is because the ampersand character is a special character in XML
            // and it needs to be specially marked in this context.
            // THIS IS DONE BY SIF
            return String.Format("{0}?startIndex={1}&count={2}", this.Url, this.StartIndex, this.ItemsPerPage);
        }
        public bool GetLinkNext(out string linkUrl)
        {
            linkUrl = null;

            if (this.StartIndex + this.ItemsPerPage >= this.TotalResults)
                return false;

            // the query parameters are separated by &amp; rather than &. 
            // This is because the ampersand character is a special character in XML
            // and it needs to be specially marked in this context.
            // THIS IS DONE BY SIF
            linkUrl = string.Format("{0}?startIndex={1}&count={2}", this.Url, this.StartIndex + this.ItemsPerPage, this.ItemsPerPage);
            return true;
        }
        public bool GetLinkPrevious(out string linkUrl)
        {
            linkUrl = null;

            int realStartIndex;

            if (this.StartIndex == 1)
            {
                return false;
            }
            else
            {
                realStartIndex = this.StartIndex - this.ItemsPerPage;
                if (realStartIndex <= 0)
                    realStartIndex = 1;
            }
            // the query parameters are separated by &amp; rather than &. 
            // This is because the ampersand character is a special character in XML
            // and it needs to be specially marked in this context.
            // THIS IS DONE BY SIF
            linkUrl = string.Format("{0}?startIndex={1}&count={2}", this.Url, realStartIndex, this.ItemsPerPage);

            return true;
        }
        public string GetLinkLast()
        {
            // INFO:
            // totalResults=122; ItemsPerPage=10 => StartIndex=121?count=10
            // totalResults=120; ItemsPerPage=10 => StartIndex=111?count=10

            int realStartIndex;
            int remainingItems = this.TotalResults % this.ItemsPerPage;

            if (remainingItems == 0)
                realStartIndex = this.TotalResults - this.ItemsPerPage + 1;
            else
                realStartIndex = this.TotalResults - remainingItems + 1;

            // the query parameters are separated by &amp; rather than &. 
            // This is because the ampersand character is a special character in XML
            // and it needs to be specially marked in this context.
            // THIS IS DONE BY SIF
            return string.Format("{0}?startIndex={1}&count={2}", this.Url, realStartIndex, this.ItemsPerPage);
        }

        #endregion

        #region OPENSEARCH

        public IItemsPerPageElement GetOpensearch_ItemsPerPageElement()
        {
            IItemsPerPageElement element = FeedComponentFactory.Create<IItemsPerPageElement>();
            element.Value = this.ItemsPerPage;

            return element;
        }

        public IStartIndexElement GetOpensearch_StartIndexElement()
        {
            IStartIndexElement element = FeedComponentFactory.Create<IStartIndexElement>();
            element.Value = this.StartIndex;

            return element;
        }

        public ITotalResultsElement GetOpensearch_TotalResultsElement()
        {
            ITotalResultsElement element = FeedComponentFactory.Create<ITotalResultsElement>();
            element.Value = this.TotalResults;

            return element;
        }

        #endregion
    }
}
