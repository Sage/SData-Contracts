#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using System.Data.OleDb;
using System.Data;
using Sage.Sis.Sdata.Sync.Storage.Syndication;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    class SyncDigestTableAdapter : ISyncDigestTableAdapter
    {
        #region Class Variables

        private readonly ISyncDigestTable _syncDigestTable;

        #endregion

        #region Ctor.

        public SyncDigestTableAdapter(ISyncDigestTable syncDigestTable, SdataContext context)
        {
            _syncDigestTable = syncDigestTable;
            this.Context = context;
        }

        #endregion

        #region ISyncDigestTableAdapter Members

        public SyncDigestEntryInfo Get(int resourceKindId, int endPointId, IJetTransaction jetTransaction)
        {
            SyncDigestEntryInfo resultInfo = null;

            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;

            sqlQuery = "SELECT {0}.Tick, {0}.ConflictPriority, {0}.Stamp, {1}.Endpoint FROM {1} INNER JOIN {0} ON {1}.ID={0}.FKEndpointId " +
            "WHERE ({0}.FKResourceKindId)=@ResourceKindId " +
            "AND ({0}.FKEndpointId)=@EndpointId;";

            
            oleDbCommand.CommandText = string.Format(sqlQuery, _syncDigestTable.TableName, _syncDigestTable.EndpointTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);
            oleDbCommand.Parameters.AddWithValue("@EndpointId", endPointId);

            OleDbDataReader reader = oleDbCommand.ExecuteReader(CommandBehavior.Default);

            if (reader.Read())
            {
                string endpoint;
                int tick;
                int conflictPriority;
                DateTime stamp;

                endpoint = Convert.ToString(reader["Endpoint"]);
                tick = Convert.ToInt32(reader["Tick"]);
                conflictPriority = Convert.ToInt32(reader["ConflictPriority"]);
                stamp = Convert.ToDateTime(reader["Stamp"]);
                resultInfo = new SyncDigestEntryInfo(endpoint, tick, conflictPriority, stamp);
            }

            return resultInfo;
        }

        public SyncDigestInfo Get(int resourceKindId, IJetTransaction jetTransaction)
        {
            SyncDigestInfo resultInfo = new SyncDigestInfo();

            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;

            sqlQuery = "SELECT {0}.Tick, {0}.ConflictPriority, {0}.Stamp, {1}.Endpoint FROM {1} INNER JOIN {0} ON {1}.ID={0}.FKEndpointId WHERE ((({0}.FKResourceKindId)=@ResourceKindId));";
            oleDbCommand.CommandText = string.Format(sqlQuery, _syncDigestTable.TableName, _syncDigestTable.EndpointTable.TableName);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);

            OleDbDataReader reader = oleDbCommand.ExecuteReader(CommandBehavior.Default);

            while (reader.Read())
            {
                string endpoint;
                int tick;
                int conflictPriority;
                DateTime stamp;

                endpoint = Convert.ToString(reader["Endpoint"]);
                tick = Convert.ToInt32(reader["Tick"]);
                conflictPriority = Convert.ToInt32(reader["ConflictPriority"]);
                stamp = Convert.ToDateTime(reader["Stamp"]);
                resultInfo.Add(new SyncDigestEntryInfo(endpoint, tick, conflictPriority, stamp));
            }

            return resultInfo;
        }

        public void Insert(int resourceKindId, SyncDigestEntryInfo[] syncDigestEntryInfo, IJetTransaction jetTransaction)
        {
            IEndpointTableAdapter endpointTableAdapter = StoreEnvironment.Resolve<IEndpointTableAdapter>(this.Context);
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            EndpointInfo tmpEndpointInfo;

            string sqlQuery = string.Empty;
            sqlQuery = "INSERT INTO [{0}] ([Tick], [ConflictPriority], [Stamp], [FKResourceKindId], [FKEndpointId]) VALUES (@Tick, @ConflictPriority, @Stamp, @ResourceKindId, @EndpointId);";

            oleDbCommand.CommandText = string.Format(sqlQuery, _syncDigestTable.TableName);

            foreach (SyncDigestEntryInfo info in syncDigestEntryInfo)
            {
                // TODO: Use prepared query
                tmpEndpointInfo = endpointTableAdapter.GetOrCreate(info.Endpoint, jetTransaction);
                //oleDbCommand.Parameters.Clear();
                oleDbCommand.Parameters.AddWithValue("@Tick", info.Tick);
                oleDbCommand.Parameters.AddWithValue("@ConflictPriority", info.ConflictPriority);
                oleDbCommand.Parameters.AddWithValue("@Stamp", info.Stamp.ToString());
                oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);
                oleDbCommand.Parameters.AddWithValue("@EndpointId", tmpEndpointInfo.Id);

                oleDbCommand.ExecuteNonQuery();
            }
        }

        public bool Update(int resourceKindId, SyncDigestEntryInfo syncDigestEntryInfo, IJetTransaction jetTransaction)
        {
            IEndpointTableAdapter endpointTableAdapter = StoreEnvironment.Resolve<IEndpointTableAdapter>(this.Context);
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            EndpointInfo tmpEndpointInfo;

            string sqlQuery = string.Empty;
            sqlQuery = "UPDATE [{0}] SET [Tick]=@Tick, [ConflictPriority]=@ConflictPriority, [Stamp]=@Stamp WHERE (FKResourceKindId=@ResourceKindId AND FKEndpointId=@EndpointId);";

            oleDbCommand.CommandText = string.Format(sqlQuery, _syncDigestTable.TableName);

            // TODO: Use prepared query
            tmpEndpointInfo = endpointTableAdapter.GetOrCreate(syncDigestEntryInfo.Endpoint, jetTransaction);
            oleDbCommand.Parameters.AddWithValue("@Tick", syncDigestEntryInfo.Tick);
            oleDbCommand.Parameters.AddWithValue("@ConflictPriority", syncDigestEntryInfo.ConflictPriority);
            oleDbCommand.Parameters.AddWithValue("@Stamp", syncDigestEntryInfo.Stamp.ToString());
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);
            oleDbCommand.Parameters.AddWithValue("@EndpointId", tmpEndpointInfo.Id);

            int count = oleDbCommand.ExecuteNonQuery();
            return (count > 0);
        }

        public ISyncDigestTable Table { get { return _syncDigestTable; } }

        #endregion        

        #region ITableAdapter Members

        public SdataContext Context { get; private set; }

        ITable ITableAdapter.Table { get { return this.Table; } }

        #endregion

    }
}
