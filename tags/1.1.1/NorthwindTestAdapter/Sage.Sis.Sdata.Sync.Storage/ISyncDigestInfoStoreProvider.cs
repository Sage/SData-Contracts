#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage
{
    public interface ISyncDigestInfoStoreProvider
    {
        /// <summary>
        /// Tries to read the sync digest of a resource kind from the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <returns>An object of type <see cref="SyncDigestInfo"/> or null if no sync digest entry found.</returns>
        SyncDigestInfo Get(string resourceKind);

        SyncDigestEntryInfo Get(string resourceKind, string EndPoint);

        /// <summary>
        /// Adds a sync digest for a given resource kind in the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="info">The sync digest to add.</param>
        /// <exception cref="StoreException">If a sync digest for the resource already exists.</exception>
        void Add(string resourceKind, SyncDigestInfo info);

        /// <summary>
        /// Adds a sync digest entry for a given resource in the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="info">The sync digest entry to store.</param>
        /// <exception cref="StoreException">If the sync digest entry for the resource already exists.</exception>
        void Add(string resourceKind, SyncDigestEntryInfo info);

        /// <summary>
        /// Replaces a sync digest for a given resource kind in the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="info">The sync digest to store.</param>
        /// <exception cref="StoreException">If a sync digest for the resource does not exists.</exception>
        //void Update(string resourceKind, SyncDigestInfo info);

        /// <summary>
        /// Replaces a sync digest entry for a given resource in the store.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <param name="info">The sync digest entry to store.</param>
        /// <exception cref="StoreException">If the sync digest entry for the resource does not exists.</exception>
        bool Update(string resourceKind, SyncDigestEntryInfo info);

        /// <summary>
        /// Deletes the sync digest of a resource kind.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <remarks>Must not raise any exception if no syncdigest for the resource exists.</remarks>
        //void Delete(string resourceKind);
    }
}
