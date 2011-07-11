#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage
{
    public interface ICorrelatedResSyncInfoStoreProvider
    {
        /// <summary>
        /// Reads all correlated resource synchronisation entries of a resource kind and filtered by local ids from store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="localIds">The local ids to filter for.</param>
        /// <returns>An array of objects of type <see cref="CorrelatedResSyncInfo"/>. If no entry was found an empty array is returned.</returns>
        CorrelatedResSyncInfo[] GetByLocalId(string resourceKind, string[] localIds);

        /// <summary>
        /// Reads all correlated resource synchronisation entries of a resource kind and filtered by uuids.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="uuids">The uuids to filter for.</param>
        /// <returns>An array of objects of type <see cref="CorrelatedResSyncInfo"/>. If no entry was found an empty array is returned.</returns>
        CorrelatedResSyncInfo[] GetByUuid(string resourceKind, Guid[] uuids);

        /// <summary>
        /// Returns an iterator over correlated resource synchronisation entries, that have a higher tick than the given one.
        /// Sorted by Tick.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="endpoint">The endpoint to filter for.</param>
        /// <param name="tick">The minimum tick value.</param>
        /// <returns>An iterator over the results sorted by Tick.</returns>
        ICorrelatedResSyncInfoEnumerator GetSinceTick(string resourceKind, string endpoint, int tick);

        /// <summary>
        /// Returns an iterator over all correlated resource synchronisation entries of a resource kind.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <returns>An iterator over all correlated resource synchronisation entries found.</returns>
        /// <exception cref="ArgumentException">If resourceKind is null or empty.</exception>
        ICorrelatedResSyncInfoEnumerator GetAll(string resourceKind);

        /// <summary>
        /// Reads correlated resource synchronisation entries filtered per page.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="pageNumber">The page number (starting at 1)</param>
        /// <param name="itemsPerPage">The number of entries in a page.</param>
        /// <param name="totalResult">The total number of correlated resource synchronisation entries in the store.</param>
        /// <returns>An array of objects of type <see cref="CorrelatedResSyncInfo"/>. If no entry was found an empty array is returned.</returns>
        /// <exception cref="ArgumentException">If resourceKind is null or empty.</exception>
        CorrelatedResSyncInfo[] GetPaged(string resourceKind, int pageNumber, int itemsPerPage, out int totalResult);

        /// <summary>
        /// Replaces a correlated resource syncronisation entry to the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="info">The correlated resource syncronisation entry to store.</param>
        /// <exception cref="StoreException">If correlated resource syncronisation entry for the resource does not exists.</exception>
        void Update(string resourceKind, CorrelatedResSyncInfo info);

        /// <summary>
        /// Adds a correlated resource syncronisation entry to the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="info">The correlated resource syncronisation entry to store.</param>
        /// <exception cref="StoreException">If correlated resource syncronisation entry for the resource already exists.</exception>
        void Add(string resourceKind, CorrelatedResSyncInfo info);

        /// <summary>
        /// Removes a correlated resource syncronisation entry from store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="uuid">The uuid of the resource synchronisation entry to remove.</param>
        void Delete(string resourceKind, Guid uuid);
    }
}
