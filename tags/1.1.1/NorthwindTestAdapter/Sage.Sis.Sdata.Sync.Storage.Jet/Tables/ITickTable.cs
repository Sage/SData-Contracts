#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.Tables
{
    interface ItickTable : ITable
    {
        IResourceKindTable ResourceKindTable { get; }
    }
}
