#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Jet;
using Sage.Sis.Sdata.Sync.Storage;

#endregion

namespace Sage.Integration.Northwind.Sync
{
    // THREADSAFE CALLS TO PROVIDER METHODS
    internal class AppBookmarkInfoStore : IAppBookmarkInfoStore
    {
        #region Class Variables

        private readonly IAppBookmarkInfoStoreProvider _provider;
        private readonly object lockObj = new object();

        #endregion

        #region Ctor.

        public AppBookmarkInfoStore(IAppBookmarkInfoStoreProvider provider)
        {
            _provider = provider;
        }

        #endregion

        #region IAppBookmarkInfoStore Members

        public bool Get(string resourceKind, Type type, out object applicationBookmark)
        {
            if (null == resourceKind)
                throw new ArgumentNullException("resourceKind");
            if (resourceKind == String.Empty)
                throw new ArgumentException("Parameter value is empty.", "resourceKind");

            lock (lockObj)
            {
                return _provider.Get(resourceKind, type, out applicationBookmark);
            }
        }

        public bool Get<T>(string resourceKind, out T applicationBookmark)
        {
            object resultObj;
            bool succeeded;

            try
            {
                succeeded = this.Get(resourceKind, typeof(T), out resultObj);

                if (succeeded)
                {
                    applicationBookmark = (T)resultObj;
                    return true;
                }
                else
                {
                    applicationBookmark = default(T);
                    return false;
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                applicationBookmark = default(T);
                return false;
            }

            #region OLD

            //#warning this try is a workarount until the deserialisation is assembly version invariant
//            try
//            {
//                object result;
//                bool succeeded;

//                lock (lockObj)
//                {
//                    succeeded = this.Get(resourceKind, out result);
//                }

//                if (succeeded)
//                {
//                    applicationBookmark = (T)result;
//                    return true;
//                }
//                else
//                {
//                    applicationBookmark = default(T);
//                    return false;
//                }
//            }
//            catch (Exception)
//            {
//                applicationBookmark = default(T);
//                return false;
            //            }

            #endregion
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
