#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Results;
using Sage.Sis.Sdata.Sync.Results.Syndication;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using System.Data.OleDb;
using System.Reflection;
using System.IO;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet
{
    public class SyncResultsInfoStoreProvider : ISyncResultsInfoStoreProvider
    {
        #region Class Variables

        private readonly IJetConnectionProvider _jetConnectionProvider;
        private readonly SdataContext _context;
        private string _runName = null;
        private string _runStamp = null;

        #endregion

        #region Ctor.

        public SyncResultsInfoStoreProvider(IJetConnectionProvider jetConnectionProvider, SdataContext context)
        {
            _jetConnectionProvider = jetConnectionProvider;
            _context = context;

            StoreEnvironment.Initialize(jetConnectionProvider, context);
        }

        #endregion

        #region ISyncResultsInfoStoreProvider Members

        public void Add(string resourceKind, SyncResultEntryInfo[] syncResultEntryInfos)
        {
            ISyncResultsTableAdapter syncResultsTableAdapter = this.GetAdapter(resourceKind);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);
            
            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                syncResultsTableAdapter.Insert(syncResultEntryInfos, _runName, _runStamp, jetTransaction);
                
                jetTransaction.Commit();
            }
        }

        public void RemoveAll(string resourceKind)
        {
            ISyncResultsTableAdapter syncResultsTableAdapter = StoreEnvironment.Resolve<ISyncResultsTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
//                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                try
                {
                    syncResultsTableAdapter.Delete(jetTransaction);
                }
                catch
                {
                    return;
                }

                jetTransaction.Commit();
            }
        }

        public void SetRunName(string runName)
        {
            _runName = runName;
        }
        public void SetRunStamp(string runStamp)
        {
            _runStamp = runStamp;
        }

        #endregion

        private ISyncResultsTableAdapter GetAdapter(string resourceKind)
        {
            ISyncResultsTableAdapter syncResultsTableAdapter;

            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            Dictionary<string, ISyncResultsTableAdapter> adapters = StoreEnvironment.Resolve<Dictionary<string, ISyncResultsTableAdapter>>(_context);
            if (!adapters.TryGetValue(resourceKind, out syncResultsTableAdapter))
            {
                TableAdapterFactory factory = new TableAdapterFactory(_context, _jetConnectionProvider);
                syncResultsTableAdapter = factory.CreateSyncResultsTableAdapter(resourceKind, resourceKindTableAdapter);
                adapters.Add(resourceKind, syncResultsTableAdapter);
                StoreEnvironment.Set<Dictionary<string, ISyncResultsTableAdapter>>(_context, adapters);
            }

            return syncResultsTableAdapter;
        }
        
    }
}
