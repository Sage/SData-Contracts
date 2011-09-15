#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    interface ISyncDigestTableAdapter : ITableAdapter
    {
        SyncDigestInfo Get(int resourceKindId, IJetTransaction jetTransaction);

        SyncDigestEntryInfo Get(int resourceKindId, int EndPointId, IJetTransaction jetTransaction);

        void Insert(int resourceKindId, SyncDigestEntryInfo[] syncDigestEntryInfo, IJetTransaction jetTransaction);

        //void Delete(int resourceKindId, IJetTransaction jetTransaction);

        bool Update(int resourceKindId, SyncDigestEntryInfo syncDigestEntryInfo, IJetTransaction jetTransaction);

        new ISyncDigestTable Table { get; }
    }
}
