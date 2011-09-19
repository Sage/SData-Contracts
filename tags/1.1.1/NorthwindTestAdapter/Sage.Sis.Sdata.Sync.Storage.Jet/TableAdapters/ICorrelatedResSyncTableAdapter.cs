#region Usings

using System;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    interface ICorrelatedResSyncTableAdapter : ITableAdapter
    {
        CorrelatedResSyncInfo[] GetByLocalId(string[] localIds, IJetTransaction jetTransaction);
        CorrelatedResSyncInfo[] GetByUuids(Guid[] uuids, IJetTransaction jetTransaction);
        CorrelatedResSyncInfo[] GetAll(IJetTransaction jetTransaction);
        CorrelatedResSyncInfo[] GetSincetick(int EndPointId, int tick, IJetTransaction jetTransaction);

        void Insert( CorrelatedResSyncInfo info, IJetTransaction jetTransaction);
        bool Update(CorrelatedResSyncInfo info, IJetTransaction jetTransaction);
        void Remove(Guid uuid, IJetTransaction jetTransaction);

        new ICorrelatedResSyncTable Table { get; }
    }
}
