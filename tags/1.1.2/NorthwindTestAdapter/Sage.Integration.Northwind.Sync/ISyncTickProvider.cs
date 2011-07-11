#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Tick;

#endregion

namespace Sage.Integration.Northwind.Adapter.Sync
{
    public interface ISyncTickProvider : ITickProvider
    {
        new int CreateNextTick(string resourceKind);
    }
}
