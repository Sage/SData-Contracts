#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Jet;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.ClientSample
{
    // THREADSAFE CALLS TO PROVIDER METHODS
    internal class AppBookmarkInfoStoreClient : IAppBookmarkInfoStore
    {
        #region Class Variables

        private readonly IAppBookmarkInfoStoreProvider _provider;
        private readonly object lockObj = new object();

        #endregion

        #region Ctor.

        public AppBookmarkInfoStoreClient(IAppBookmarkInfoStoreProvider provider)
        {
            _provider = provider;
        }

        #endregion

        #region IAppBookmarkInfoStore Members

        public bool Get(string resourceKind, out object applicationBookmark)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            lock (lockObj)
            {
                return _provider.Get(resourceKind, out applicationBookmark);
            }
        }

        public bool Get<T>(string resourceKind, out T applicationBookmark)
        {
            
            object result;
            bool succeeded;
            
            lock(lockObj)
            {
                succeeded = this.Get(resourceKind, out result);
            }

            if (succeeded)
            {
                applicationBookmark = (T)result;
                return true;
            }
            else
            {
                applicationBookmark = default(T);
                return false;
            }
        }

        public void Put(string resourceKind, object applicationBookmark)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            // We set the priority on 'update'.
            lock (lockObj)
            {
                try
                {
                    _provider.Update(resourceKind, applicationBookmark);
                }
                catch (StoreException)
                {
                    _provider.Add(resourceKind, applicationBookmark);
                }
            }
        }

        public void Delete(string resourceKind)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            lock (lockObj)
            {
                _provider.Delete(resourceKind);
            }
        }

        #endregion
    }
}
