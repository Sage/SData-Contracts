#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    interface ItickTableAdapter : ITableAdapter
    {
        bool TryGet(int resourceKindId, out int tick, IJetTransaction jetTransaction);
        void Update(int resourceKindId, int tick, IJetTransaction jetTransaction);
        void Insert(int resourceKindId, int tick, IJetTransaction jetTransaction);

        new ItickTable Table { get; }
    }
}
