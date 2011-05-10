#region Usings

using System.Data.OleDb;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet
{
    public class SyncDigestInfoStoreProvider : ISyncDigestInfoStoreProvider
    {
        #region Class Variables

        private readonly IJetConnectionProvider _jetConnectionProvider;
        private readonly SdataContext _context;

        #endregion

        #region Ctor.

        public SyncDigestInfoStoreProvider(IJetConnectionProvider jetConnectionProvider, SdataContext context)
        {
            _jetConnectionProvider = jetConnectionProvider;
            _context = context;

            StoreEnvironment.Initialize(jetConnectionProvider, context);
        }

        #endregion

        #region ISyncDigestInfoStoreProvider Members

        public SyncDigestEntryInfo Get(string resourceKind, string endPoint)
        {

            SyncDigestEntryInfo resultInfo = null;
            ISyncDigestTableAdapter syncDigestTableAdapter = StoreEnvironment.Resolve<ISyncDigestTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);
            IEndpointTableAdapter endpointTableAdapter = StoreEnvironment.Resolve<IEndpointTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);
                EndpointInfo endpointInfo = endpointTableAdapter.GetOrCreate(endPoint, jetTransaction);
                resultInfo = syncDigestTableAdapter.Get(resourceKindInfo.Id, endpointInfo.Id, jetTransaction);

                jetTransaction.Commit();
            }

            return resultInfo;
            
        }


        public SyncDigestInfo Get(string resourceKind)
        {
            SyncDigestInfo resultInfo = null;
            ISyncDigestTableAdapter syncDigestTableAdapter = StoreEnvironment.Resolve<ISyncDigestTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                resultInfo = syncDigestTableAdapter.Get(resourceKindInfo.Id, jetTransaction);

                jetTransaction.Commit();
            }

            return resultInfo;
        }

        public void Add(string resourceKind, SyncDigestInfo info)
        {
            ISyncDigestTableAdapter syncDigestTableAdapter = StoreEnvironment.Resolve<ISyncDigestTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);
                try
                {
                    syncDigestTableAdapter.Insert(resourceKindInfo.Id, info.ToArray(), jetTransaction);
                }
                catch (OleDbException exception)
                {
                    if (exception.Errors.Count == 1 && exception.Errors[0].SQLState == "3022")
                        throw new StoreException(string.Format("An error occured while adding a new syncDigest. A syncDigest already exists for the resourceKind '{0}'.", resourceKind), exception);

                    throw;
                }
                jetTransaction.Commit();
            }
        }

        public void Add(string resourceKind, SyncDigestEntryInfo info)
        {
            ISyncDigestTableAdapter syncDigestTableAdapter = StoreEnvironment.Resolve<ISyncDigestTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                try
                {
                    syncDigestTableAdapter.Insert(resourceKindInfo.Id, new SyncDigestEntryInfo[] { info }, jetTransaction);
                }
                catch (OleDbException exception)
                {
                    if (exception.Errors.Count == 1 && exception.Errors[0].SQLState == "3022")
                        throw new StoreException(string.Format("An error occured while adding a new sync digest entry. A sync digest entry already exists for the resource kind '{0}' and endpoint '{1}.", resourceKind, info.Endpoint), exception);

                    throw;
                }

                jetTransaction.Commit();
            }
        }
        /*
        public void Update(string resourceKind, SyncDigestInfo info)
        {
            ISyncDigestTableAdapter syncDigestTableAdapter = StoreEnvironment.Resolve<ISyncDigestTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                try
                {
                    syncDigestTableAdapter.Update(resourceKindInfo.Id, info.ToArray(), jetTransaction);
                }
                catch (StoreException exception)
                {
                    throw new StoreException(string.Format("An error occured while updating a sync digest entry. No sync digest entry exists for the resource kind '{0}'.", resourceKind), exception);
                }
                jetTransaction.Commit();
            }
        }
        */
        public bool Update(string resourceKind, SyncDigestEntryInfo info)
        {
            bool result = false;
            ISyncDigestTableAdapter syncDigestTableAdapter = StoreEnvironment.Resolve<ISyncDigestTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                result = syncDigestTableAdapter.Update(resourceKindInfo.Id,  info , jetTransaction);

                jetTransaction.Commit();
            }
            return result;
        }
        
        #endregion

    }
}
