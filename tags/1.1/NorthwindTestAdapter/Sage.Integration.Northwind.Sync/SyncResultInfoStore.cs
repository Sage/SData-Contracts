#region Usings

using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Results;
using Sage.Sis.Sdata.Sync.Storage.Jet;
using Sage.Sis.Sdata.Sync.Results.Syndication;

#endregion

namespace Sage.Integration.Northwind.Sync
{
    // THREADSAFE CALLS TO PROVIDER METHODS
    internal class SyncResultInfoStore : ISyncResultInfoStore
    {
        #region Class Variables

        private readonly ISyncResultsInfoStoreProvider _provider;
        private readonly object lockObj = new object();

        #endregion

        #region Ctor.

        public SyncResultInfoStore(IJetConnectionProvider jetConnectionProvider, SdataContext context)
        {
            _provider = new SyncResultsInfoStoreProvider(jetConnectionProvider, context);
        }

        #endregion

        #region ISyncResultInfoStore Members

        public void Add(string resourceKind, SyncResultEntryInfo[] syncResultEntryInfos)
        {
            lock (lockObj)
            {
                _provider.Add(resourceKind, syncResultEntryInfos);
            }
        }

        public void RemoveAll(string resourceKind)
        {
            lock (lockObj)
            {
                _provider.RemoveAll(resourceKind);
            }
        }

        #endregion
    }
}
