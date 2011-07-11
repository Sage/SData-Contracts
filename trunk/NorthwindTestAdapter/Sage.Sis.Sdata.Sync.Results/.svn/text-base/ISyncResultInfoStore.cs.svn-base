#region Usings

using Sage.Sis.Sdata.Sync.Results.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Results
{
    public interface ISyncResultInfoStore
    {
        //void Add(string resourceKind, SyncResultInfo syncResultInfo);
        void Add(string resourceKind, SyncResultEntryInfo[] syncResultEntryInfos);

        //void Remove(string resourceKind, DateTime beforeDate);
        //void Remove(string resourceKind, string trackingId);
        void RemoveAll(string resourceKind);

        //SyncResultEntryInfo[] GetAll(string resourceKind);
    }
}
