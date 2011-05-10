#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    internal class CachedEndpointTableAdapter : IEndpointTableAdapter
    {
        #region Class Variables

        private readonly IEndpointTableAdapter _tableAdapter;
        private readonly Dictionary<string, EndpointInfo> _cache = new Dictionary<string, EndpointInfo>();

        #endregion

        #region Ctor.

        public CachedEndpointTableAdapter(IEndpointTableAdapter endpointTableAdapter)
        {
            _tableAdapter = endpointTableAdapter;
        }

        #endregion

        public void Load(IJetTransaction jetTransaction)
        {
            EndpointInfo[] allResourceKindInfos = _tableAdapter.GetAll(jetTransaction);

            foreach (EndpointInfo info in allResourceKindInfos)
                _cache.Add(info.Endpoint, info);
        }

        #region IEndpointTableAdapter Members

        public EndpointInfo GetOrCreate(string endpoint, IJetTransaction jetTransaction)
        {
            EndpointInfo info;
            string contextName = jetTransaction.OleDbConnection.ConnectionString;
            if (!_cache.TryGetValue(endpoint, out info))
            {
                info = _tableAdapter.GetOrCreate(endpoint, jetTransaction);
                _cache.Add(endpoint, info);
            }

            return info;
        }

        public EndpointInfo[] GetAll(IJetTransaction jetTransaction)
        {
            return _tableAdapter.GetAll(jetTransaction);
        }

        public IEndpointTable Table { get { return _tableAdapter.Table; } }

        #endregion

        #region ITableAdapter Members

        public SdataContext Context
        {
            get { return _tableAdapter.Context; }
        }
        ITable ITableAdapter.Table { get { return this.Table; } }

        #endregion
    }
}
