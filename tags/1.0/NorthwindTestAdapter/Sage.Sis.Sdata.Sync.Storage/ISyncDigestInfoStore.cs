#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage
{
    public interface ISyncDigestInfoStore
    {
        /// <summary>
        /// Tries to read the sync digest of a resource kind from the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <returns>An object of type <see cref="SyncDigestInfo"/> or null if no sync digest entry found.</returns>
        /// <exception cref="ArgumentException">If resourceKind is null or empty.</exception>
        SyncDigestInfo Get(string resourceKind);

        /// <summary>
        /// Replaces or adds a sync digest for a given resource kind in the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="info">The sync digest to store.</param>
        /// <exception cref="ArgumentException">If resourceKind is null or empty.</exception>
        /// <exception cref="ArgumentException">If info is null or invalid.</exception>
        // void Put(string resourceKind, SyncDigestInfo info);

        /// <summary>
        /// Replaces or adds a sync digest entry for a given resource in the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="info">The sync digest entry to store.</param>
        /// <exception cref="ArgumentException">If resourceKind is null or empty.</exception>
        /// <exception cref="ArgumentException">If info is null or invalid.</exception>
        void Put(string resourceKind, SyncDigestEntryInfo info);
    }
}
