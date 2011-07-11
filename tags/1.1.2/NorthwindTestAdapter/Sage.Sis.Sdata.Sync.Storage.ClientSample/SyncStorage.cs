#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet;
using System.IO;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.ClientSample
{
    public static class SyncStorage
    {
        #region CLASS: ConnStringFactory

        private static class ConnStringFactory
        {
            public static string GetConnectionString(SdataContext context)
            {
                string connString;
                string currentDir = Directory.GetCurrentDirectory();

                string mdbFilePath = Path.Combine(currentDir, string.Format(@"SyncStores\{0}\{1}\{2}\syncStore.mdb", context.Application, context.Contract, context.DataSet));

                // TODO: Create an empty mdb file..

                connString = @"PROVIDER=Microsoft.Jet.OLEDB.4.0; DATA SOURCE=" + mdbFilePath;

                return connString;
            }
        }

        #endregion

        #region CLASS: StoreResolver

        private class ContextStoreResolver
        {
            private readonly static Dictionary<string, Dictionary<Type, object>> stat_types = new Dictionary<string, Dictionary<Type, object>>();

            public static bool TryResolve<T>(string context, out T obj)
            {
                obj = default(T);
                Dictionary<Type, object> entry;

                if (!stat_types.TryGetValue(context, out entry))
                    return false;

                obj = default(T);
                Type type = typeof(T);
                object value;
                if (entry.TryGetValue(type, out value))
                {
                    obj = (T)value;
                    return true;
                }
                return false;
            }

            public static void Register<T>(string context, object obj)
            {
                Type type = (typeof(T));
                if (obj is T == false)
                    throw new InvalidOperationException(string.Format("The supplied instance does not implement {0}", type.FullName));

                Dictionary<Type,object> newEntry = new Dictionary<Type,object>();
                newEntry.Add(type, obj);
                
                stat_types.Add(context, newEntry);
            }
        }

        #endregion

        public static IAppBookmarkInfoStore GetAppBookmarkStore(SdataContext context)
        {
            IAppBookmarkInfoStore store;

            string connectionString = ConnStringFactory.GetConnectionString(context);

            if (!ContextStoreResolver.TryResolve<IAppBookmarkInfoStore>(connectionString, out store))
            {
                IJetConnectionProvider connectionProvider = new SimpleJetConnectionProvider(connectionString);
                IAppBookmarkInfoStoreProvider provider = new AppBookmarkInfoStoreProvider(connectionProvider, context);

                store = new AppBookmarkInfoStoreClient(provider);
                ContextStoreResolver.Register<IAppBookmarkInfoStore>(connectionString, store);
            }

            return store;
        }

        public static ISyncDigestInfoStore GetSyncDigestStore(SdataContext context)
        {
            ISyncDigestInfoStore store;

            string connectionString = ConnStringFactory.GetConnectionString(context);

            if (!ContextStoreResolver.TryResolve<ISyncDigestInfoStore>(connectionString, out store))
            {
                IJetConnectionProvider connectionProvider = new SimpleJetConnectionProvider(connectionString);
                ISyncDigestInfoStoreProvider provider = new SyncDigestInfoStoreProvider(connectionProvider, context);

                store = new SyncDigestInfoStoreClient(provider);
                ContextStoreResolver.Register<ISyncDigestInfoStore>(connectionString, store);
            }

            return store;
        }

        public static ICorrelatedResSyncInfoStore GetCorrelatedResSyncStore(SdataContext context)
        {
            ICorrelatedResSyncInfoStore store;

            string connectionString = ConnStringFactory.GetConnectionString(context);

            if (!ContextStoreResolver.TryResolve<ICorrelatedResSyncInfoStore>(connectionString, out store))
            {
                IJetConnectionProvider connectionProvider = new SimpleJetConnectionProvider(connectionString);
                ICorrelatedResSyncInfoStoreProvider provider = new CorrelatedResSyncStoreProvider(connectionProvider, context);

                store = new CorrelatedResSyncInfoStoreClient(provider);
                ContextStoreResolver.Register<ICorrelatedResSyncInfoStore>(connectionString, store);
            }

            return store;
        }
    }
}
