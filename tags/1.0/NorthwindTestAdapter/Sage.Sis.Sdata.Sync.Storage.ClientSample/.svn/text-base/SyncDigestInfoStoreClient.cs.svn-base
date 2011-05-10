#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.ClientSample
{
    // THREADSAFE CALLS TO PROVIDER METHODS
    internal class SyncDigestInfoStoreClient : ISyncDigestInfoStore
    {
        #region Class Variables

        private readonly ISyncDigestInfoStoreProvider _provider;
        private readonly object lockObj = new object();

        #endregion

        #region Ctor.

        public SyncDigestInfoStoreClient(ISyncDigestInfoStoreProvider provider)
        {
            _provider = provider;
        }

        #endregion

        #region ISyncDigestInfoStore Members

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
