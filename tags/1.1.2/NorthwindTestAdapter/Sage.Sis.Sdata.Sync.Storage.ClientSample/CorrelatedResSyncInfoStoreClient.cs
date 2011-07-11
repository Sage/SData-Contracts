#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.ClientSample
{
    // THREADSAFE CALLS TO PROVIDER METHODS
    class CorrelatedResSyncInfoStoreClient : ICorrelatedResSyncInfoStore
    {
        #region Class Variables

        private readonly ICorrelatedResSyncInfoStoreProvider _provider;
        private readonly object lockObj = new object();

        #endregion

        #region Ctor.

        public CorrelatedResSyncInfoStoreClient(ICorrelatedResSyncInfoStoreProvider provider)
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

        #endregion
    }
}
