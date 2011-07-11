#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    interface IAppBookmarkTableAdapter : ITableAdapter
    {
        bool Get(int resourceKindId, out byte[] applicationBookmark, out string assemblyQualifiedName, IJetTransaction jetTransaction);
        void Insert(int resourceKindId, object applicationBookmark, string assemblyQualifiedName, IJetTransaction jetTransaction);
        void Delete(int resourceKindId, IJetTransaction jetTransaction);
        bool Update(int resourceKindId, object applicationBookmark, string assemblyQualifiedName, IJetTransaction jetTransaction);

        new IAppBookmarkTable Table { get; }
    }
}
