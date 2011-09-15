#region Usings

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common.Paging
{

    public class PagedArray<T> : IEnumerable<T>
    {
        public PagedArray(T[] array, int offset, int count)
        {
            this.Array = array;
            this.Offset = offset;
            this.Count = count;
        }

        public T[] Array { get; private set; }
        public int Offset { get; private set; }
        public int Count { get; private set; }


        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = this.Offset; i < this.Offset + this.Count; i++)
            {
                yield return this.Array[i];
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }

    public static class PagingHelpers
    {
        public static int PAGESSIZE = 10;

        public static PageInfo Normalize(long startIndex, long? count, int entryCount)
        {
            int normCount;
            int normStartIndex;

            if (null == count)
                normCount = PAGESSIZE;
            else if (count < 0)
                normCount = 0;
            else if (count >= 0)
                normCount = Convert.ToInt32(count);
            else
                normCount = PAGESSIZE;

            if (startIndex < 1)
            {
                normStartIndex = 1;
            }
            else if (startIndex > entryCount)
            {
                normStartIndex = entryCount;
            }
            else
            {
                normStartIndex = Convert.ToInt32(startIndex);
            }


            return new PageInfo(normStartIndex, normCount);
        }
        public static PageInfo Normalize(PageInfo pageInfo, int entryCount)
        {
            if (null == pageInfo)
                return new PageInfo(1, PAGESSIZE);

            return Normalize(pageInfo.StartIndex, pageInfo.Count, entryCount);
        }


        public static IEnumerator<T> GetPagedEnumerator<T>(PageInfo normalizedPageInfo, T[] array)
        {
            int totalCount = array.Length;
            int count;

            if (normalizedPageInfo.StartIndex-1 + normalizedPageInfo.Count > totalCount)
                count = totalCount - (normalizedPageInfo.StartIndex-1);
            else
                count = normalizedPageInfo.Count;


            return (new PagedArray<T>(array, normalizedPageInfo.StartIndex-1, count)).GetEnumerator();
        }



        private static PageInfo GetFirstPageInfo(int startIndex, int count, int entryCount)
        {
            //int normCount;

            //if (count < PAGESSIZE && count >= 0)
            //    normCount = count;
            //else
            //    normCount = PAGESSIZE;

            //return new PageInfo(1, normCount);


            return new PageInfo(1, count);
        }
        public static PageInfo GetFirstPageInfo(PageInfo pageInfo, int entryCount)
        {
            return GetFirstPageInfo(pageInfo.StartIndex, pageInfo.Count, entryCount);
        }

        private static PageInfo GetLastPageInfo(int count, int entryCount)
        {
            if (count > 0)
            {
                int realStartIndex;
                int remainingItems = entryCount % count;

                if (remainingItems == 0)
                    realStartIndex = entryCount - count + 1;
                else
                    realStartIndex = entryCount - remainingItems + 1;

                return new PageInfo(realStartIndex, count);
            }
            return new PageInfo(0, 0);

            //int realStartIndex;
            //int remainingItems = entryCount % PAGESSIZE;

            //if (remainingItems == 0)
            //    realStartIndex = entryCount - PAGESSIZE + 1;
            //else
            //    realStartIndex = entryCount - remainingItems + 1;

            //return new PageInfo(realStartIndex, count);
        }
        public static PageInfo GetLastPageInfo(PageInfo normalizedPageInfo, int entryCount)
        {
            return GetLastPageInfo(normalizedPageInfo.Count, entryCount);
        }

        private static PageInfo GetPreviousPageInfo(int startIndex, int count)
        {
            if (startIndex <= 1)
                return null;        // no previous page exists.

            if (startIndex - count < 1)
                return new PageInfo(1, startIndex - 1);
            else
                return new PageInfo(startIndex - count, count);

            //int itemsPerPage;
            //if (count < PAGESSIZE && count >= 0)
            //    itemsPerPage = count;
            //else
            //    itemsPerPage = PAGESSIZE;

            //if (startIndex <= 1)
            //    return null;        // no previous page exists.

            //if (startIndex - itemsPerPage < 1)
            //    return new PageInfo(1, startIndex - 1);
            //else
            //    return new PageInfo(startIndex - itemsPerPage, itemsPerPage);
        }
        public static PageInfo GetPreviousPageInfo(PageInfo normalizedPageInfo)
        {
            return GetPreviousPageInfo(normalizedPageInfo.StartIndex, normalizedPageInfo.Count);
        }

        private static PageInfo GetNextPageInfo(int startIndex, int count, int entryCount)
        {
            int nextStartIndex;

            nextStartIndex = startIndex + count;

            if (nextStartIndex > entryCount)
                return null;


            return new PageInfo(nextStartIndex, count);




            //int itemsPerPage;
            //int nextStartIndex;
            //int nextItemsPerPage;

            //if (startIndex > entryCount)
            //    return null;

            //if (count < PAGESSIZE && count >= 0)
            //    itemsPerPage = count;
            //else
            //    itemsPerPage = PAGESSIZE;

            //nextStartIndex = startIndex + itemsPerPage;

            //if (nextStartIndex > entryCount)
            //    return null;

            //nextItemsPerPage = itemsPerPage;
            ////nextItemsPerPage = (entryCount - nextStartIndex) + 1;
            ////if (nextItemsPerPage > itemsPerPage)
            ////    nextItemsPerPage = itemsPerPage;

            //return new PageInfo(nextStartIndex, nextItemsPerPage);
        }
        public static PageInfo GetNextPageInfo(PageInfo normalizedPageInfo, int entryCount)
        {
            return GetNextPageInfo(normalizedPageInfo.StartIndex, normalizedPageInfo.Count, entryCount);
        }







        public static int GetPageNumber(PageInfo pageInfo)
        {
            return GetPageNumber(pageInfo.StartIndex, pageInfo.Count);
        }
        public static int GetPageNumber(long startIndex, long? count)
        {
            int normCount;
            int normStartIndex;

            if (null == count)
                normCount = PAGESSIZE;
            else if (count < 0)
                normCount = 0;
            else if (count < PAGESSIZE && count >= 0)
                normCount = Convert.ToInt32(count);
            else
                normCount = PAGESSIZE;

            if (startIndex < 1)
            {
                normStartIndex = 1;
            }
            else
            {
                normStartIndex = Convert.ToInt32(startIndex);
            }
            return ((int)(normStartIndex / normCount)) + 1;
        }
    }
}
