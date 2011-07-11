#region Usings

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet
{
    public class CorrelatedResSyncStoreProvider : ICorrelatedResSyncInfoStoreProvider
    {
        #region Class Variables

        private readonly IJetConnectionProvider _jetConnectionProvider;
        private readonly SdataContext _context;

        #endregion

        #region Ctor.

        public CorrelatedResSyncStoreProvider(IJetConnectionProvider jetConnectionProvider, SdataContext context)
        {
            _jetConnectionProvider = jetConnectionProvider;
            _context = context;
            StoreEnvironment.Initialize(jetConnectionProvider, context);
        }

        #endregion

        #region ICorrelatedResSyncInfoStoreProvider Members

        public CorrelatedResSyncInfo[] GetByLocalId(string resourceKind, string[] localIds)
        {
            CorrelatedResSyncInfo[] resultInfos;

            ICorrelatedResSyncTableAdapter correlatedResSyncTableAdapter = this.GetAdapter(resourceKind);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                resultInfos = correlatedResSyncTableAdapter.GetByLocalId(localIds, jetTransaction);

                jetTransaction.Commit();
            }

            return (null != resultInfos) ? resultInfos : new CorrelatedResSyncInfo[0];
        }

        public CorrelatedResSyncInfo[] GetByUuid(string resourceKind, Guid[] uuids)
        {
            CorrelatedResSyncInfo[] resultInfos;

            ICorrelatedResSyncTableAdapter correlatedResSyncTableAdapter = this.GetAdapter(resourceKind);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                resultInfos = correlatedResSyncTableAdapter.GetByUuids(uuids, jetTransaction);

                jetTransaction.Commit();
            }

            return (null != resultInfos) ? resultInfos : new CorrelatedResSyncInfo[0];
        }

        public ICorrelatedResSyncInfoEnumerator GetAll(string resourceKind)
        {
            CorrelatedResSyncInfo[] resultInfos;

            ICorrelatedResSyncTableAdapter correlatedResSyncTableAdapter = this.GetAdapter(resourceKind);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                resultInfos = correlatedResSyncTableAdapter.GetAll(jetTransaction);

                jetTransaction.Commit();
            }

            return (null != resultInfos) ? new CorrelatedResSyncInfoEnumerator(resultInfos) : new CorrelatedResSyncInfoEnumerator(new CorrelatedResSyncInfo[0]);
        }

        public CorrelatedResSyncInfo[] GetPaged(string resourceKind, int pageNumber, int itemsPerPage, out int totalResult)
        {
            CorrelatedResSyncInfo[] resultInfos;

            ICorrelatedResSyncTableAdapter correlatedResSyncTableAdapter = this.GetAdapter(resourceKind);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                resultInfos = correlatedResSyncTableAdapter.GetAll(jetTransaction);

                jetTransaction.Commit();
            }

            // TODO: TO BE REVIEWED!!!
            totalResult = resultInfos.Length;
            if (totalResult == 0)
                return new CorrelatedResSyncInfo[0];

            int startIndex = ((pageNumber-1) * itemsPerPage) + 1;
            if (totalResult < startIndex)
                return new CorrelatedResSyncInfo[0];

            int realItemsPerPage;
            if ((totalResult - startIndex) > itemsPerPage)
                realItemsPerPage = itemsPerPage;
            else
                realItemsPerPage = totalResult - startIndex;

            CorrelatedResSyncInfo[] destinationArray = new CorrelatedResSyncInfo[realItemsPerPage];
            
            Array.Copy(resultInfos, startIndex-1, destinationArray, 0, realItemsPerPage);

            return destinationArray;
        }

        public ICorrelatedResSyncInfoEnumerator GetSinceTick(string resourceKind, string endpoint, int tick)
        {
            CorrelatedResSyncInfo[] resultInfos;

            ICorrelatedResSyncTableAdapter correlatedResSyncTableAdapter = this.GetAdapter(resourceKind);
            IEndpointTableAdapter endpointTableAdapter = StoreEnvironment.Resolve<IEndpointTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                EndpointInfo endpointInfo = endpointTableAdapter.GetOrCreate(endpoint, jetTransaction);

                resultInfos = correlatedResSyncTableAdapter.GetSinceTick(endpointInfo.Id, tick, jetTransaction);

                jetTransaction.Commit();
            }

            return (null != resultInfos) ? new CorrelatedResSyncInfoEnumerator(resultInfos) : new CorrelatedResSyncInfoEnumerator(new CorrelatedResSyncInfo[0]);
        }

        public void Add(string resourceKind, CorrelatedResSyncInfo info)
        {
            ICorrelatedResSyncTableAdapter correlatedResSyncTableAdapter = this.GetAdapter(resourceKind);
            
            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                try
                {
                    correlatedResSyncTableAdapter.Insert(info, jetTransaction);
                }
                catch (OleDbException exception)
                {
                    if (exception.Errors.Count == 1 && exception.Errors[0].SQLState == "3022")
                        throw new StoreException(string.Format("An error occured while adding a new correlated ResSync. A correlated ResSync already exists for the uuid '{0}' and local id '{1}.", info.ResSyncInfo.Uuid, info.LocalId), exception);

                    throw;
                }
                jetTransaction.Commit();
            }
        }
        
        public void Update(string resourceKind, CorrelatedResSyncInfo info)
        {
            ICorrelatedResSyncTableAdapter correlatedResSyncTableAdapter = this.GetAdapter(resourceKind);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                if (!correlatedResSyncTableAdapter.Update(info, jetTransaction))
                    throw new StoreException(string.Format("No correlated ResSync exists for the resource kind '{0}' that can be updated.", resourceKind));

                jetTransaction.Commit();
            }
        }

        public void Delete(string resourceKind, Guid uuid)
        {
            ICorrelatedResSyncTableAdapter correlatedResSyncTableAdapter = this.GetAdapter(resourceKind);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                try
                {
                    correlatedResSyncTableAdapter.Remove(uuid, jetTransaction);
                }
                catch (OleDbException exception)
                {
                    throw new StoreException(string.Format("An error occured while removing the correlated ResSyncs with uuid '{0}'. No correlated ResSync has been removed.", uuid), exception);
                }
                jetTransaction.Commit();
            }
        }

        #endregion

        #region Private Helpers

        private ICorrelatedResSyncTableAdapter GetAdapter(string resourceKind)
        {
            ICorrelatedResSyncTableAdapter correlatedResSyncTableAdapter;

            IEndpointTableAdapter endpointTableAdapter = StoreEnvironment.Resolve<IEndpointTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            Dictionary<string, ICorrelatedResSyncTableAdapter> adapters = StoreEnvironment.Resolve<Dictionary<string, ICorrelatedResSyncTableAdapter>>(_context);
            if (!adapters.TryGetValue(resourceKind, out correlatedResSyncTableAdapter))
            {
                TableAdapterFactory factory = new TableAdapterFactory(_context, _jetConnectionProvider);
                correlatedResSyncTableAdapter = factory.CreateCorrelatedResSyncTableAdapter(resourceKind, endpointTableAdapter, resourceKindTableAdapter);
                adapters.Add(resourceKind, correlatedResSyncTableAdapter);
                StoreEnvironment.Set<Dictionary<string, ICorrelatedResSyncTableAdapter>>(_context, adapters);
            }

            return correlatedResSyncTableAdapter;
        }

        #endregion
    }
}
