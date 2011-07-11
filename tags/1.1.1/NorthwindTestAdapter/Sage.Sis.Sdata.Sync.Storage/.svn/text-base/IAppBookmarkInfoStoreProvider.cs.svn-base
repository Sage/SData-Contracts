#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage
{
    public interface IAppBookmarkInfoStoreProvider
    {
        /// <summary>
        /// Tries to read the application bookmark of a resource kind from the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="applicationBookmark">Will contain the stored application bookmark if it exists; otherwise null.</param>
        /// <returns>true if an application bookmark exists for the given resource kind; otherwise false.</returns>
        bool Get(string resourceKind, Type type, out object applicationBookmark);

        /// <summary>
        /// Adds an application bookmark of a resource into the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="applicationBookmark">The application bookmark value.</param>
        /// <exception cref="StoreException">If an application bookmark for the resource kind already exists in the store.</exception>
        void Add(string resourceKind, object applicationBookmark);

        /// <summary>
        /// Replaces an existing application bookmark of a given resource kind.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="applicationBookmark">The value of the new application bookmark.</param>
        /// <returns>The deserialized value of the application bookmark.</returns>
        /// <exception cref="StoreException">If no application bookmark exists for the given resource kind.</exception>
        void Update(string resourceKind, object applicationBookmark);

        /// <summary>
        /// Deletes all application bookmarks of a resource kind from the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        void Delete(string resourceKind);
    }
}
