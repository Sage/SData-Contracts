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

        public SyncDigestEntryInfo Get(int resourceKindId, int EndPointId, IJetTransaction jetTransaction)
        {
            SyncDigestEntryInfo resultInfo = null;

            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;

            sqlQuery = "SELECT {0}.Tick, {0}.ConflictPriority, {0}.Stamp, {1}.EndPoint FROM {1} INNER JOIN {0} ON {1}.ID={0}.FKEndPointId " +
            "WHERE ({0}.FKResourceKindId)=@ResourceKindId " +
            "AND ({0}.FKEndPointId)=@EndPointId;";

            
            oleDbCommand.CommandText = string.Format(sqlQuery, _syncDigestTable.TableName, _syncDigestTable.EndPointTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);
            oleDbCommand.Parameters.AddWithValue("@EndPointId", EndPointId);

            using (OleDbDataReader reader = oleDbCommand.ExecuteReader(CommandBehavior.Default))
            {
                if (reader.Read())
                {
                    string EndPoint;
                    int tick;
                    int conflictPriority;
                    DateTime stamp;

                    EndPoint = Convert.ToString(reader["EndPoint"]);
                    tick = Convert.ToInt32(reader["tick"]);
                    conflictPriority = Convert.ToInt32(reader["ConflictPriority"]);
                    stamp = Convert.ToDateTime(reader["Stamp"]);
                    resultInfo = new SyncDigestEntryInfo(EndPoint, tick, conflictPriority, stamp);
                }
            }
            return resultInfo;
        }

        public SyncDigestInfo Get(int resourceKindId, IJetTransaction jetTransaction)
        {
            SyncDigestInfo resultInfo = new SyncDigestInfo();

            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;

            sqlQuery = "SELECT {0}.Tick, {0}.ConflictPriority, {0}.Stamp, {1}.EndPoint FROM {1} INNER JOIN {0} ON {1}.ID={0}.FKEndPointId WHERE ((({0}.FKResourceKindId)=@ResourceKindId));";
            oleDbCommand.CommandText = string.Format(sqlQuery, _syncDigestTable.TableName, _syncDigestTable.EndPointTable.TableName);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);

            using (OleDbDataReader reader = oleDbCommand.ExecuteReader(CommandBehavior.Default))
            {
                while (reader.Read())
                {
                    string EndPoint;
                    int tick;
                    int conflictPriority;
                    DateTime stamp;

                    EndPoint = Convert.ToString(reader["EndPoint"]);
                    tick = Convert.ToInt32(reader["tick"]);
                    conflictPriority = Convert.ToInt32(reader["ConflictPriority"]);
                    stamp = Convert.ToDateTime(reader["Stamp"]);
                    resultInfo.Add(new SyncDigestEntryInfo(EndPoint, tick, conflictPriority, stamp));
                }
            }

            return resultInfo;
        }

        public void Insert(int resourceKindId, SyncDigestEntryInfo[] syncDigestEntryInfo, IJetTransaction jetTransaction)
        {
            IEndPointTableAdapter EndPointTableAdapter = StoreEnvironment.Resolve<IEndPointTableAdapter>(this.Context);
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            EndPointInfo tmpEndPointInfo;

            string sqlQuery = string.Empty;
            sqlQuery = "INSERT INTO [{0}] ([tick], [ConflictPriority], [Stamp], [FKResourceKindId], [FKEndPointId]) VALUES (@tick, @ConflictPriority, @Stamp, @ResourceKindId, @EndPointId);";

            oleDbCommand.CommandText = string.Format(sqlQuery, _syncDigestTable.TableName);

            foreach (SyncDigestEntryInfo info in syncDigestEntryInfo)
            {
                // TODO: Use prepared query
                tmpEndPointInfo = EndPointTableAdapter.GetOrCreate(info.EndPoint, jetTransaction);
                //oleDbCommand.Parameters.Clear();
                oleDbCommand.Parameters.AddWithValue("@tick", info.Tick);
                oleDbCommand.Parameters.AddWithValue("@ConflictPriority", info.ConflictPriority);
                oleDbCommand.Parameters.AddWithValue("@Stamp", info.Stamp.ToString());
                oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);
                oleDbCommand.Parameters.AddWithValue("@EndPointId", tmpEndPointInfo.Id);

                oleDbCommand.ExecuteNonQuery();
            }
        }

        public bool Update(int resourceKindId, SyncDigestEntryInfo syncDigestEntryInfo, IJetTransaction jetTransaction)
        {
            IEndPointTableAdapter EndPointTableAdapter = StoreEnvironment.Resolve<IEndPointTableAdapter>(this.Context);
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            EndPointInfo tmpEndPointInfo;

            string sqlQuery = string.Empty;
            sqlQuery = "UPDATE [{0}] SET [tick]=@tick, [ConflictPriority]=@ConflictPriority, [Stamp]=@Stamp WHERE (FKResourceKindId=@ResourceKindId AND FKEndPointId=@EndPointId);";

            oleDbCommand.CommandText = string.Format(sqlQuery, _syncDigestTable.TableName);

            // TODO: Use prepared query
            tmpEndPointInfo = EndPointTableAdapter.GetOrCreate(syncDigestEntryInfo.EndPoint, jetTransaction);
            oleDbCommand.Parameters.AddWithValue("@tick", syncDigestEntryInfo.Tick);
            oleDbCommand.Parameters.AddWithValue("@ConflictPriority", syncDigestEntryInfo.ConflictPriority);
            oleDbCommand.Parameters.AddWithValue("@Stamp", syncDigestEntryInfo.Stamp.ToString());
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);
            oleDbCommand.Parameters.AddWithValue("@EndPointId", tmpEndPointInfo.Id);

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
