#region Usings

using System;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Integration.Northwind.Sync.Syndication;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Integration.Northwind.Sync
{
    public interface ISyncSyncDigestInfoStore : ISyncDigestInfoStore
    {
        //void PersistNewer(string resourceKind, SyncState syncState);
        void PersistNewer(string resourceKind, ResSyncInfo resSyncInfo);
    }
}
