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
    internal class CachedResourceKindTableAdapter : IResourceKindTableAdapter
    {
        #region Class Variables

        private readonly IResourceKindTableAdapter _tableAdapter;
        private readonly Dictionary<string, ResourceKindInfo> _cache = new Dictionary<string, ResourceKindInfo>();

        #endregion

        #region Ctor.

        public CachedResourceKindTableAdapter(IResourceKindTableAdapter resourceKindTableAdapter)
        {
            _tableAdapter = resourceKindTableAdapter;
        }

        #endregion

        public void Load(IJetTransaction jetTransaction)
        {
            ResourceKindInfo[] allResourceKindInfos = _tableAdapter.GetAll(jetTransaction);

            foreach (ResourceKindInfo info in allResourceKindInfos)
                _cache.Add(info.Name, info);
        }

        #region IResourceKindTableAdapter Members

        public ResourceKindInfo GetOrCreate(string resourceKind, IJetTransaction jetTransaction)
        {
            ResourceKindInfo info;
            string contextName = jetTransaction.OleDbConnection.ConnectionString;
            if (!_cache.TryGetValue(resourceKind, out info))
            {
                info = _tableAdapter.GetOrCreate(resourceKind, jetTransaction);
                _cache.Add(resourceKind, info);
            }

            return info;
        }

        public ResourceKindInfo[] GetAll(IJetTransaction jetTransaction)
        {
            return _tableAdapter.GetAll(jetTransaction);
        }

        public IResourceKindTable Table { get { return _tableAdapter.Table; } }

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
