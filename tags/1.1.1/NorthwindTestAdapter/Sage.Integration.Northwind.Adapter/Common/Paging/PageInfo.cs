using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Integration.Northwind.Adapter.Common.Paging
{
    public class PageInfo
    {
        public int StartIndex { get; private set; }
        public int Count { get; private set; }

        public PageInfo(int startIndex, int count)
        {
            this.StartIndex = startIndex;
            this.Count = count;
        }
    }
}
