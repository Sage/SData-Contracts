#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage
{
    public interface IAppBookmarkInfoStore
    {
        /// <summary>
        /// Tries to read the application bookmark of a resource kind from the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="applicationBookmark">Will contain the stored application bookmark if it exists; otherwise null.</param>
        /// <returns>true if an application bookmark exists for the given resource kind; otherwise false.</returns>
        /// <exception cref="ArgumentException">If resourceKind is null or empty.</exception>
        bool Get(string resourceKind, Type type, out object applicationBookmark);

        /// <summary>
        /// Tries to read the application bookmark of a resource kind from the store.
        /// </summary>
        /// <typeparam name="T">The expected type of the stored application bookmark.</typeparam>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>        
        /// <param name="applicationBookmark">Will contain the stored application bookmark if it exists; otherwise the default value of T.</param>
        /// <returns>true if an application bookmark exists for the given resource kind; otherwise false.</returns>
        /// <exception cref="InvalidCastException">If the application bookmark value found cannot be casted into T.</exception>
        /// <exception cref="ArgumentException">If resourceKind is null or empty.</exception>
        bool Get<T>(string resourceKind, out T applicationBookmark);

        /// <summary>
        /// Replaces or adds an application bookmark of a resource in the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="applicationBookmark">The application bookmark value.</param>
        /// <remarks>The implementation should prioritise replace than add in performance.</remarks>
        /// <exception cref="ArgumentException">If resourceKind is null or empty.</exception>
        void Put(string resourceKind, object applicationBookmark);

        /// <summary>
        /// Deletes all application bookmarks of a resource kind from the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <exception cref="ArgumentException">If resourceKind is null or empty.</exception>
        void Delete(string resourceKind);
    }
}
