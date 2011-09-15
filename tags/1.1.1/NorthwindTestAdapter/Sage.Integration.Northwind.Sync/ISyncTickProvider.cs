#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Integration.Northwind.Adapter.Sync
{
    public interface ISynctickProvider : Sage.Sis.Sdata.Sync.tick.ItickProvider
    {
        new int CreateNexttick(string resourceKind);
    }
}
