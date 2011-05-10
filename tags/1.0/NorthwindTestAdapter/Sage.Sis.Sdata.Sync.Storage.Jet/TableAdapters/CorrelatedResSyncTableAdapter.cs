#region Usings

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Storage.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    class CorrelatedResSyncTableAdapter : ICorrelatedResSyncTableAdapter
    {
        #region Class Variables

        private readonly ICorrelatedResSyncTable _correlatedResSyncTable;

        #endregion

        #region Ctor.

        public CorrelatedResSyncTableAdapter(ICorrelatedResSyncTable correlatedResSyncTable, SdataContext context)
        {
            _correlatedResSyncTable = correlatedResSyncTable;
            this.Context = context;
        }

        #endregion

        #region ICorrelatedResSyncTableAdapter Members

        public CorrelatedResSyncInfo[] GetByLocalId(string[] localIds, IJetTransaction jetTransaction)
        {
            List<CorrelatedResSyncInfo> resultInfos = new List<CorrelatedResSyncInfo>();
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string localIdsParameterValue = string.Empty;
            for (int i = 0; i < localIds.Length; i++)
            {
                localIdsParameterValue += localIds[i] + ",";
            }

            // remove last ','
            localIdsParameterValue = localIdsParameterValue.Remove(localIdsParameterValue.Length - 1);

            string sqlQuery = string.Empty;
            sqlQuery = "SELECT {0}.Uuid, {0}.Tick, {0}.ModifiedStamp, {0}.Etag, {0}.LocalId, {1}.Endpoint FROM {1} INNER JOIN {0} ON {1}.ID = {0}.FKEndpointId WHERE ((({0}.LocalId) IN (@LocalIds)) AND (({0}.FKResourceKindId)=@ResourceKindId))";
            //sqlQuery = "SELECT {0}.Uuid, {0}.Tick, {0}.ModifiedStamp, {0}.Etag, {1}.Endpoint, {2}.LocalId FROM {2} INNER JOIN ({1} INNER JOIN {0} ON {1}.ID = {0}.FKEndpointId) ON {2}.Uuid = {0}.Uuid WHERE ((({2}.LocalId) IN (@LocalIds)) AND (({0}.FKResourceKindId)=@ResourceKindId));";

            oleDbCommand.CommandText = string.Format(sqlQuery, _correlatedResSyncTable.TableName, _correlatedResSyncTable.EndpointTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@LocalIds", localIdsParameterValue);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", _correlatedResSyncTable.ResourceKindId);
            

            OleDbDataReader reader = oleDbCommand.ExecuteReader();

            string tmpLocalId;
            Guid tmpUuid;
            string tmpEndpoint;
            int tmpTick;
            string tmpEtag;
            DateTime tmpModifiedStamp;
            while (reader.Read())
            {
                tmpLocalId = Convert.ToString(reader["LocalId"]);
                tmpUuid = new Guid(Convert.ToString(reader["Uuid"]));
                tmpEndpoint = Convert.ToString(reader["Endpoint"]);
                tmpTick = Convert.ToInt32(reader["Tick"]);
                tmpEtag = Convert.ToString(reader["Etag"]);
                tmpModifiedStamp = Convert.ToDateTime(reader["ModifiedStamp"]);

                resultInfos.Add(new CorrelatedResSyncInfo(tmpLocalId, new ResSyncInfo(tmpUuid, tmpEndpoint, tmpTick, tmpEtag, tmpModifiedStamp)));
            }

            return resultInfos.ToArray();
        }

        public CorrelatedResSyncInfo[] GetByUuids(Guid[] uuids, IJetTransaction jetTransaction)
        {
            List<CorrelatedResSyncInfo> resultInfos = new List<CorrelatedResSyncInfo>();
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string uuidsParameterValue = string.Empty;
            for (int i = 0; i < uuids.Length; i++)
            {
                uuidsParameterValue += "\"{" + uuids[i].ToString() + "}\",";
            }

            // remove last ','
            uuidsParameterValue = uuidsParameterValue.Remove(uuidsParameterValue.Length - 1);

            string sqlQuery = string.Empty;
            sqlQuery = "SELECT {0}.Uuid, {0}.Tick, {0}.ModifiedStamp, {0}.Etag, {0}.LocalId, {1}.Endpoint FROM {1} INNER JOIN {0} ON {1}.ID = {0}.FKEndpointId WHERE ((({0}.Uuid) In ({2})) AND (({0}.FKResourceKindId)=@ResourceKindId));";

            oleDbCommand.CommandText = string.Format(sqlQuery, _correlatedResSyncTable.TableName, _correlatedResSyncTable.EndpointTable.TableName, uuidsParameterValue);

            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", _correlatedResSyncTable.ResourceKindId);

            OleDbDataReader reader = oleDbCommand.ExecuteReader();

            string tmpLocalId;
            Guid tmpUuid;
            string tmpEndpoint;
            int tmpTick;
            string tmpEtag;
            DateTime tmpModifiedStamp;
            while (reader.Read())
            {
                tmpLocalId = Convert.ToString(reader["LocalId"]);
                tmpUuid = new Guid(Convert.ToString(reader["Uuid"]));
                tmpEndpoint = Convert.ToString(reader["Endpoint"]);
                tmpTick = Convert.ToInt32(reader["Tick"]);
                tmpEtag = Convert.ToString(reader["Etag"]);
                tmpModifiedStamp = Convert.ToDateTime(reader["ModifiedStamp"]);

                resultInfos.Add(new CorrelatedResSyncInfo(tmpLocalId, new ResSyncInfo(tmpUuid, tmpEndpoint, tmpTick, tmpEtag, tmpModifiedStamp)));
            }

            return resultInfos.ToArray();
        }

        public CorrelatedResSyncInfo[] GetAll(IJetTransaction jetTransaction)
        {
            List<CorrelatedResSyncInfo> resultInfos = new List<CorrelatedResSyncInfo>();
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;
            sqlQuery = "SELECT {0}.Uuid, {0}.Tick, {0}.ModifiedStamp, {0}.Etag, {0}.LocalId, {1}.Endpoint FROM {1} INNER JOIN {0} ON {1}.ID = {0}.FKEndpointId WHERE ((({0}.FKResourceKindId)=@ResourceKindId));";
            //sqlQuery = "SELECT {0}.Uuid, {0}.Tick, {0}.ModifiedStamp, {0}.Etag, {1}.Endpoint, {2}.LocalId FROM {2} INNER JOIN ({1} INNER JOIN {0} ON {1}.ID = {0}.FKEndpointId) ON {2}.Uuid = {0}.Uuid WHERE ((({0}.FKResourceKindId)=@ResourceKindId));";
            oleDbCommand.CommandText = string.Format(sqlQuery, _correlatedResSyncTable.TableName, _correlatedResSyncTable.EndpointTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", _correlatedResSyncTable.ResourceKindId);


            OleDbDataReader reader = oleDbCommand.ExecuteReader();

            string tmpLocalId;
            Guid tmpUuid;
            string tmpEndpoint;
            int tmpTick;
            string tmpEtag;
            DateTime tmpModifiedStamp;
            while (reader.Read())
            {
                tmpLocalId = Convert.ToString(reader["LocalId"]);
                tmpUuid = new Guid(Convert.ToString(reader["Uuid"]));
                tmpEndpoint = Convert.ToString(reader["Endpoint"]);
                tmpTick = Convert.ToInt32(reader["Tick"]);
                tmpEtag = Convert.ToString(reader["Etag"]);
                tmpModifiedStamp = Convert.ToDateTime(reader["ModifiedStamp"]);

                resultInfos.Add(new CorrelatedResSyncInfo(tmpLocalId, new ResSyncInfo(tmpUuid, tmpEndpoint, tmpTick, tmpEtag, tmpModifiedStamp)));
            }

            return resultInfos.ToArray();
        }

        public CorrelatedResSyncInfo[] GetSinceTick(int endpointId, int tick, IJetTransaction jetTransaction)
        {
            List<CorrelatedResSyncInfo> resultInfos = new List<CorrelatedResSyncInfo>();
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;
            sqlQuery = "SELECT {0}.Uuid, {0}.Tick, {0}.ModifiedStamp, {0}.Etag, {0}.LocalId, {1}.Endpoint FROM {1} INNER JOIN {0} ON {1}.ID = {0}.FKEndpointId WHERE ((({0}.Tick)>@Tick) AND (({0}.FKResourceKindId)=@ResourceKindId) AND (({1}.ID)=@EndpointId)) ORDER BY {0}.Tick;";
            //sqlQuery = "SELECT {0}.Uuid, {0}.Tick, {0}.ModifiedStamp, {0}.Etag, {1}.Endpoint, {2}.LocalId FROM {2} INNER JOIN ({1} INNER JOIN {0} ON {1}.ID = {0}.FKEndpointId) ON {2}.Uuid = {0}.Uuid WHERE ((({0}.Tick)>@Tick) AND (({0}.FKResourceKindId)=@ResourceKindId) AND (({1}.ID)=@EndpointId)) ORDER BY {0}.Tick;";
            oleDbCommand.CommandText = string.Format(sqlQuery, _correlatedResSyncTable.TableName, _correlatedResSyncTable.EndpointTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@Tick", tick);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", _correlatedResSyncTable.ResourceKindId);
            oleDbCommand.Parameters.AddWithValue("@EndpointId", endpointId);


            OleDbDataReader reader = oleDbCommand.ExecuteReader();

            string tmpLocalId;
            Guid tmpUuid;
            string tmpEndpoint;
            int tmpTick;
            string tmpEtag;
            DateTime tmpModifiedStamp;
            while (reader.Read())
            {
                tmpLocalId = Convert.ToString(reader["LocalId"]);
                tmpUuid = new Guid(Convert.ToString(reader["Uuid"]));
                tmpEndpoint = Convert.ToString(reader["Endpoint"]);
                tmpTick = Convert.ToInt32(reader["Tick"]);
                tmpEtag = Convert.ToString(reader["Etag"]);
                tmpModifiedStamp = Convert.ToDateTime(reader["ModifiedStamp"]);

                resultInfos.Add(new CorrelatedResSyncInfo(tmpLocalId, new ResSyncInfo(tmpUuid, tmpEndpoint, tmpTick, tmpEtag, tmpModifiedStamp)));
            }

            return resultInfos.ToArray();
        }

        public void Insert(CorrelatedResSyncInfo info, IJetTransaction jetTransaction)
        {
            IEndpointTableAdapter endpointTableAdapter = StoreEnvironment.Resolve<IEndpointTableAdapter>(this.Context);
            EndpointInfo endpointInfo = endpointTableAdapter.GetOrCreate(info.ResSyncInfo.Endpoint, jetTransaction);

            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;
            sqlQuery = "INSERT INTO {0} ([Uuid], [Tick], [ModifiedStamp], [Etag], [LocalId], [FKEndpointId], [FKResourceKindId]) VALUES (@Uuid, @Tick, @ModifiedStamp, @Etag, @LocalId, @EndpointId, @ResourceKindId);";
            oleDbCommand.CommandText = string.Format(sqlQuery, _correlatedResSyncTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@Uuid", info.ResSyncInfo.Uuid);
            oleDbCommand.Parameters.AddWithValue("@Tick", info.ResSyncInfo.Tick);
            oleDbCommand.Parameters.AddWithValue("@ModifiedStamp", info.ResSyncInfo.ModifiedStamp.ToString());
            oleDbCommand.Parameters.AddWithValue("@Etag", info.ResSyncInfo.Etag);
            oleDbCommand.Parameters.AddWithValue("@LocalId", info.LocalId);
            oleDbCommand.Parameters.AddWithValue("@EndpointId", endpointInfo.Id);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", _correlatedResSyncTable.ResourceKindId);

            oleDbCommand.ExecuteNonQuery();
        }
        public bool Update(CorrelatedResSyncInfo info, IJetTransaction jetTransaction)
        {
            IEndpointTableAdapter endpointTableAdapter = StoreEnvironment.Resolve<IEndpointTableAdapter>(this.Context);
            EndpointInfo endpointInfo = endpointTableAdapter.GetOrCreate(info.ResSyncInfo.Endpoint, jetTransaction);
            
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            
            string sqlQuery = string.Empty;
            sqlQuery = "UPDATE [{0}] SET [Tick]=@Tick, [ModifiedStamp]=@ModifiedStamp, [Etag]=@Etag, [LocalId]=@LocalId , [FKEndpointId]=@EndpointId  WHERE (Uuid=@Uuid);";

            oleDbCommand.CommandText = string.Format(sqlQuery, _correlatedResSyncTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@Tick", info.ResSyncInfo.Tick);
            oleDbCommand.Parameters.AddWithValue("@ModifiedStamp", info.ResSyncInfo.ModifiedStamp.ToString());
            oleDbCommand.Parameters.AddWithValue("@Etag", info.ResSyncInfo.Etag);
            oleDbCommand.Parameters.AddWithValue("@LocalId", info.LocalId);
            oleDbCommand.Parameters.AddWithValue("@EndpointId", endpointInfo.Id);
            oleDbCommand.Parameters.AddWithValue("@Uuid", info.ResSyncInfo.Uuid);
            

            return (0 != oleDbCommand.ExecuteNonQuery());
        }
        public void Remove(Guid uuid, IJetTransaction jetTransaction)
        {
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            oleDbCommand.CommandText = string.Format("DELETE FROM {0} WHERE [Uuid] = @Uuid", _correlatedResSyncTable.TableName);
            oleDbCommand.Parameters.AddWithValue("@Uuid", uuid);
            oleDbCommand.ExecuteNonQuery();
        }

        public ICorrelatedResSyncTable Table { get { return _correlatedResSyncTable; } }
        #endregion

        #region ITableAdapter Members

        public SdataContext Context { get; private set; }
        ITable ITableAdapter.Table { get { return this.Table; } }

        #endregion
    }
}
