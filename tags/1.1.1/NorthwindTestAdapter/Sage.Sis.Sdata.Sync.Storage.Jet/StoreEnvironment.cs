#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet
{
    internal static class StoreEnvironment
    {
        private static Dictionary<SdataContext, StoreEnvironmentItem> stat_EnvironmentItems = new Dictionary<SdataContext, StoreEnvironmentItem>();
        private static object lockObj = new object();

        public static void Initialize(IJetConnectionProvider connProvider, SdataContext context)
        {
            StoreEnvironmentItem item = null;

            lock (lockObj)
            {
                if (stat_EnvironmentItems.TryGetValue(context, out item))
                    return;

                item = new StoreEnvironmentItem();

                item.Register<IJetConnectionProvider>(connProvider);

                TableAdapterFactory tableAdapterFactory = new TableAdapterFactory(context, connProvider);
                IResourceKindTableAdapter resourceKindTableAdapter = tableAdapterFactory.CreateResourceKindTableAdapter();
                item.Register<IResourceKindTableAdapter>(resourceKindTableAdapter);

                IEndpointTableAdapter endpointTableAdapter = tableAdapterFactory.CreateEndpointTableAdapter();
                item.Register<IEndpointTableAdapter>(endpointTableAdapter);

                item.Register<ISyncDigestTableAdapter>(tableAdapterFactory.CreateSyncDigestTableAdapter(resourceKindTableAdapter, endpointTableAdapter));
                item.Register<IAppBookmarkTableAdapter>(tableAdapterFactory.CreateAppBookmarkTableAdapter(resourceKindTableAdapter));
                item.Register<ITickTableAdapter>(tableAdapterFactory.CreateTickTableAdapter(resourceKindTableAdapter));
                

                Dictionary<string, ICorrelatedResSyncTableAdapter> correlatedResSyncTableAdapters = new Dictionary<string,ICorrelatedResSyncTableAdapter>();
                item.Register<Dictionary<string, ICorrelatedResSyncTableAdapter>>(correlatedResSyncTableAdapters);

                Dictionary<string, ISyncResultsTableAdapter> syncResultsTableAdapters = new Dictionary<string, ISyncResultsTableAdapter>();
                item.Register<Dictionary<string, ISyncResultsTableAdapter>>(syncResultsTableAdapters);
                //item.Register<ISyncResultsTableAdapter>(tableAdapterFactory.CreateSyncResultsTableAdapter(resourceKindTableAdapter));



                stat_EnvironmentItems.Add(context, item);
            
            }
        }

        public static T Resolve<T>(SdataContext context)
        {

            StoreEnvironmentItem item = null;
            
            lock (lockObj)
            {
                if (!stat_EnvironmentItems.TryGetValue(context, out item))
                    throw new InvalidOperationException(string.Format("No StoreEnvironment initialized for the given context {0}", context.ToString()));
            }

            return item.Resolve<T>();
        }

        public static void Remove(SdataContext context)
        {
            StoreEnvironmentItem item = null;

            lock (lockObj)
            {
                if (!stat_EnvironmentItems.TryGetValue(context, out item))
                    return;

                stat_EnvironmentItems.Remove(context);
            }
        }

        #region CLASS: StoreEnvironmentItem

        private class StoreEnvironmentItem
        {
            private readonly Dictionary<Type, object> _types = new Dictionary<Type,object>();

            public T Resolve<T>()
            {
                return (T)_types[typeof(T)];
            }

            public void Register<T>(object obj)
            {
                if (obj is T == false)
                    throw new InvalidOperationException(string.Format("The supplied instance does not implement {0}", typeof(T).FullName));

                _types.Add(typeof(T), obj);
            }

            public void Set<T>(object obj)
            {
                if (obj is T == false)
                    throw new InvalidOperationException(string.Format("The supplied instance does not implement {0}", typeof(T).FullName));

                _types[typeof(T)] = obj;
            }
        }

        #endregion

        internal static void Set<T>(SdataContext context, object obj)
        {
            if (obj is T == false)
                throw new InvalidOperationException(string.Format("The supplied instance does not implement {0}", typeof(T).FullName));

            StoreEnvironmentItem item = null;

            lock (stat_EnvironmentItems)
            {
                if (!stat_EnvironmentItems.TryGetValue(context, out item))
                    throw new InvalidOperationException(string.Format("No StoreEnvironment initialized for the given context {0}", context.ToString()));

                item.Set<T>(obj);
            }
           
        }
    }
}
