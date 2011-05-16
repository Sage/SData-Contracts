#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Sis.Sdata.Sync.Storage;

#endregion

namespace Sage.Integration.Northwind.Sync
{
    // THREADSAFE CALLS TO PROVIDER METHODS
    internal class CorrelatedResSyncInfoStore : ICorrelatedResSyncInfoStore
    {
        #region Class Variables

        private readonly ICorrelatedResSyncInfoStoreProvider _provider;
        private readonly object lockObj = new object();

        #endregion

        #region Ctor.

        public CorrelatedResSyncInfoStore(ICorrelatedResSyncInfoStoreProvider provider)
        {
            _provider = provider;
        }

        #endregion

        #region ICorrelatedResSyncInfoStore Members

        public CorrelatedResSyncInfo[] GetByLocalId(string resourceKind, string[] localIds)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            lock (lockObj)
            {
                return _provider.GetByLocalId(resourceKind, localIds);
            }
        }

        public CorrelatedResSyncInfo[] GetByUuid(string resourceKind, Guid[] uuids)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            lock (lockObj)
            {
                return _provider.GetByUuid(resourceKind, uuids);
            }
        }

        public ICorrelatedResSyncInfoEnumerator GetSinceTick(string resourceKind, string endpoint, int tick)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            lock (lockObj)
            {
                return _provider.GetSinceTick(resourceKind, endpoint, tick);
            }
        }

        public ICorrelatedResSyncInfoEnumerator GetAll(string resourceKind)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            lock (lockObj)
            {
                return _provider.GetAll(resourceKind);
            }
        }

        public CorrelatedResSyncInfo[] GetPaged(string resourceKind, int pageNumber, int itemsPerPage, out int totalResult)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            lock (lockObj)
            {
                return _provider.GetPaged(resourceKind, pageNumber, itemsPerPage, out totalResult);
            }
        }

        public void Put(string resourceKind, CorrelatedResSyncInfo info)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            if (null == info)
                throw new ArgumentNullException("info");

            // We set the priority on 'update'.
            lock (lockObj)
            {
                try
                {
                    _provider.Update(resourceKind, info);
                }
                catch (StoreException)
                {
                    _provider.Add(resourceKind, info);
                }
            }
        }

        public void Add(string resourceKind, CorrelatedResSyncInfo info)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            if (null == info)
                throw new ArgumentNullException("info");

            lock (lockObj)
            {
                _provider.Add(resourceKind, info);
            }
        }

        public void Update(string resourceKind, CorrelatedResSyncInfo updateInfo)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            if (null == updateInfo)
                throw new ArgumentNullException("updateInfo");

            lock (lockObj)
            {
                _provider.Update(resourceKind, updateInfo);
            }
        }

        public void Delete(string resourceKind, Guid uuid)
        {
            lock (lockObj)
            {
                _provider.Delete(resourceKind, uuid);
            }
        }

        #endregion
    }
}
