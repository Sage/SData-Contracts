#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using Sage.Integration.Northwind.Adapter.Sync;
using Sage.Integration.Northwind.Application;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Results;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Sis.Sdata.Sync.Storage.Jet;

#endregion

namespace Sage.Integration.Northwind.Sync
{
    public class StoreLocator
    {
        #region CLASS: ConnStringFactory

        private static class ConnStringFactory
        {
            private static string stat_connectionString;

            static ConnStringFactory()
            {
                // TEMPORARY
                NorthwindConfig config = new NorthwindConfig();
                config.CurrencyCode = "EUR";
                config.Path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Northwind");
                stat_connectionString = config.ConnectionString;    
            }
            public static string GetConnectionString(SdataContext context)
            {
                return stat_connectionString;
            }
        }

        #endregion

        #region CLASS: StoreResolver

        private class ContextStoreResolver
        {
            private readonly static Dictionary<SdataContext, Dictionary<Type, object>> stat_types = new Dictionary<SdataContext, Dictionary<Type, object>>();

            public static bool TryResolve<T>(SdataContext context, out T obj)
            {
                obj = default(T);
                Dictionary<Type, object> entry;

                if (!stat_types.TryGetValue(context, out entry))
                    return false;


                Type type = typeof(T);
                object value;
                if (entry.TryGetValue(type, out value))
                {
                    obj = (T)value;
                    return true;
                }
                return false;
            }

            public static void Register<T>(SdataContext context, object obj)
            {
                Type type = (typeof(T));
                if (obj is T == false)
                    throw new InvalidOperationException(string.Format("The supplied instance does not implement {0}", type.FullName));

                
                if (stat_types.ContainsKey(context))
                    stat_types[context].Add(type, obj);
                else
                {
                    Dictionary<Type, object> newEntry = new Dictionary<Type, object>();
                    newEntry.Add(type, obj);
                    stat_types.Add(context, newEntry);
                }
            }
        }

        #endregion

        public IAppBookmarkInfoStore GetAppBookmarkStore(SdataContext context)
        {
            IAppBookmarkInfoStore store;

            string connectionString = ConnStringFactory.GetConnectionString(context);

            if (!ContextStoreResolver.TryResolve<IAppBookmarkInfoStore>(context, out store))
            {
                IJetConnectionProvider connectionProvider = new SimpleJetConnectionProvider(connectionString);
                IAppBookmarkSerializer appBookmarkSerializer = new AppBookmarkXmlSerializer();
                IAppBookmarkInfoStoreProvider provider = new AppBookmarkInfoStoreProvider(connectionProvider, appBookmarkSerializer, context);

                store = new AppBookmarkInfoStore(provider);
                ContextStoreResolver.Register<IAppBookmarkInfoStore>(context, store);
            }

            return store;
        }

        public ISyncSyncDigestInfoStore GetSyncDigestStore(SdataContext context)
        {
            ISyncSyncDigestInfoStore store;

            string connectionString = ConnStringFactory.GetConnectionString(context);

            if (!ContextStoreResolver.TryResolve<ISyncSyncDigestInfoStore>(context, out store))
            {
                IJetConnectionProvider connectionProvider = new SimpleJetConnectionProvider(connectionString);
                ISyncDigestInfoStoreProvider provider = new SyncDigestInfoStoreProvider(connectionProvider, context);

                store = new SyncDigestInfoStore(provider);
                ContextStoreResolver.Register<ISyncSyncDigestInfoStore>(context, store);
            }

            return store;
        }

        public ICorrelatedResSyncInfoStore GetCorrelatedResSyncStore(SdataContext context)
        {
            ICorrelatedResSyncInfoStore store;

            string connectionString = ConnStringFactory.GetConnectionString(context);

            if (!ContextStoreResolver.TryResolve<ICorrelatedResSyncInfoStore>(context, out store))
            {
                IJetConnectionProvider connectionProvider = new SimpleJetConnectionProvider(connectionString);
                ICorrelatedResSyncInfoStoreProvider provider = new CorrelatedResSyncStoreProvider(connectionProvider, context);

                store = new CorrelatedResSyncInfoStore(provider);
                ContextStoreResolver.Register<ICorrelatedResSyncInfoStore>(context, store);
            }

            return store;
        }

        public ISyncTickProvider GetTickProvider(SdataContext context)
        {
            ISyncTickProvider tickProvider;

            string connectionString = ConnStringFactory.GetConnectionString(context);

            if (!ContextStoreResolver.TryResolve<ISyncTickProvider>(context, out tickProvider))
            {
                IJetConnectionProvider connectionProvider = new SimpleJetConnectionProvider(connectionString);
                
                tickProvider = new TickProvider(connectionProvider, context);

                ContextStoreResolver.Register<ISyncTickProvider>(context, tickProvider);
            }

            return tickProvider;
        }

        public ISyncResultInfoStore GetSyncResultStore(SdataContext context)
        {
            ISyncResultInfoStore store;

            string connectionString = ConnStringFactory.GetConnectionString(context);

            if (!ContextStoreResolver.TryResolve<ISyncResultInfoStore>(context, out store))
            {
                IJetConnectionProvider connectionProvider = new SimpleJetConnectionProvider(connectionString);

                store = new SyncResultInfoStore(connectionProvider, context);
                
                ContextStoreResolver.Register<ISyncResultInfoStore>(context, store);
            }

            return store;
        }
    }
}
