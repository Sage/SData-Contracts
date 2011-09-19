#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Sis.Sdata.Sync.Storage;
using System.Data.OleDb;
using Sage.Integration.Northwind.Sync.Syndication;

#endregion

namespace Sage.Integration.Northwind.Sync
{
    // THREADSAFE CALLS TO PROVIDER METHODS
    internal class SyncDigestInfoStore : ISyncDigestInfoStore, ISyncSyncDigestInfoStore
    {
        #region Class Variables

        private readonly ISyncDigestInfoStoreProvider _provider;
        private readonly object lockObj = new object();

        #endregion

        #region Ctor.

        public SyncDigestInfoStore(ISyncDigestInfoStoreProvider provider)
        {
            _provider = provider;
        }

        #endregion

        #region ISyncSyncDigestInfoStore Members

        public SyncDigestInfo Get(string resourceKind)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            lock (lockObj)
            {
                return _provider.Get(resourceKind);
            }
        }
        /*
        public void Put(string resourceKind, SyncDigestInfo info)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            if (null == info)
                throw new ArgumentNullException("info");

            // TODO: Validate entry property values

            // We set the priority on 'update'.
            lock (lockObj)
            {
                //try
                //{
                //    _provider.Update(resourceKind, info);
                //}
                //catch (StoreException)
                //{
                //    _provider.Add(resourceKind, info);
                //}

#warning WORKAROUND for bug in Update Method
                // WORKAROUND
                // Update method does not throw any exception if entry did not exist.
                try
                {
                    _provider.Add(resourceKind, info);
                }
                catch (StoreException)
                {
                    _provider.Update(resourceKind, info);
                }
            }
        }
         * */

        //public void PersistNewer(string resourceKind, SyncState syncState)
        //{
        //    if (null == resourceKind)
        //        throw new ArgumentNullException("resourceKind");
        //    if (resourceKind == String.Empty)
        //        throw new ArgumentException("Parameter value is empty.", "resourceKind");
        //    if (null == syncState)
        //        throw new ArgumentNullException("syncState");
        //    lock (lockObj)
        //    {
        //        SyncDigestEntryInfo entry = _provider.Get(resourceKind, syncState.EndPoint);
        //        if (entry != null)
        //        {
        //            if (entry.Tick < syncState.tick+1)
        //            {
        //                entry.Tick = syncState.tick+1;
        //                _provider.Update(resourceKind, entry);
        //            }
        //        }
        //        else
        //        {
        //            entry = new SyncDigestEntryInfo(syncState.EndPoint, syncState.tick+1, 0, DateTime.Now);
        //            _provider.Add(resourceKind, entry);
        //        }
        //    }
        //}


        public void PersistNewer(string resourceKind, ResSyncInfo resSyncInfo)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");
            if (null == resSyncInfo)
                throw new ArgumentNullException("resSyncInfo");
            
            lock (lockObj)
            {
                SyncDigestEntryInfo entry = _provider.Get(resourceKind, resSyncInfo.EndPoint);
                if (entry != null)
                {
                    if (entry.Tick < resSyncInfo.Tick+1)
                    {
                        entry.Tick = resSyncInfo.Tick+1;
                        _provider.Update(resourceKind, entry);
                    }
                }
                else
                {
                    entry = new SyncDigestEntryInfo(resSyncInfo.EndPoint, resSyncInfo.Tick + 1, 1, DateTime.Now);
                    _provider.Add(resourceKind, entry);
                }
            }
        }

        public void Put(string resourceKind, SyncDigestEntryInfo info)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            if (null == info)
                throw new ArgumentNullException("info");

            // TODO: Validate entry property values

            // We set the priority on 'update'.
            lock (lockObj)
            {
                try
                {
                    _provider.Add(resourceKind, info);
                }
                catch (StoreException)
                {
                    _provider.Update(resourceKind, info);
                }
            }
        }

        #endregion
    }
}
