#region Usings



#endregion

using Sage.Sis.Sdata.Sync.Results.Syndication;
namespace Sage.Sis.Sdata.Sync.Results
{
    public interface ISyncResultsInfoStoreProvider
    {
        void Add(string resourceKind, SyncResultEntryInfo[] syncResultEntryInfos);

        void RemoveAll(string resourceKind);
    }
}
