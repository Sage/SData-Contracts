#region Usings

using Sage.Sis.Sdata.Sync.Results.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Results
{
    public interface ISyncResultInfoStore
    {
      
        void Add(string resourceKind, SyncResultEntryInfo[] syncResultEntryInfos);
        void RemoveAll(string resourceKind);

        void SetRunName(string runName);
        void SetRunStamp(string runStamp);
    }
}
