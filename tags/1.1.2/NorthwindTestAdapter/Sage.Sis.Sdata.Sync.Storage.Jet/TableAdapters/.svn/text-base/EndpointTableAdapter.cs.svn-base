#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using System.Data.OleDb;
using Sage.Sis.Common.Data.OleDb;
using System.Data;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    class EndpointTableAdapter : IEndpointTableAdapter
    {
        #region Class Variables

        private readonly IEndpointTable _endpointTable;

        #endregion

        #region Ctor.

        public EndpointTableAdapter(IEndpointTable endpointTable, SdataContext context)
        {
            _endpointTable = endpointTable;
            this.Context = context;
        }

        #endregion

        #region IEndpointTableAdapter Members


        public void SetOriginEndPoint(string endPointBaseUrl, IJetTransaction jetTransaction)
        {
            List<EndpointInfo> updateInfos = new List<EndpointInfo>();
            if (String.IsNullOrEmpty(endPointBaseUrl))
                return;
            if (!endPointBaseUrl.EndsWith("/"))
                endPointBaseUrl += "/";
            string localHost = "localhost/";

            OleDbCommand selectCommand = jetTransaction.CreateOleCommand();
            selectCommand.CommandText = string.Format(@"SELECT [ID], Endpoint FROM {0} WHERE (((Endpoint) Like ""{1}%""));", _endpointTable.TableName, localHost);
            OleDbDataReader reader = selectCommand.ExecuteReader();
            int tmpId;
            string tmpEndpoint;
            while (reader.Read())
            {
                tmpId = Convert.ToInt32(reader["ID"]);
                tmpEndpoint = Convert.ToString(reader["Endpoint"]);
                tmpEndpoint = tmpEndpoint.Replace(localHost, endPointBaseUrl);
                updateInfos.Add(new EndpointInfo(tmpId, tmpEndpoint));
            }


            foreach (EndpointInfo info in updateInfos)
            {

                OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

                oleDbCommand.CommandText = string.Format("UPDATE {0} SET [Endpoint] = @Endpoint WHERE [ID]=@ID;", _endpointTable.TableName);
                oleDbCommand.Parameters.AddWithValue("@Endpoint", info.Endpoint);
                oleDbCommand.Parameters.AddWithValue("@ID", info.Id);

                oleDbCommand.ExecuteNonQuery();
            }

        }

        public EndpointInfo GetOrCreate(string endpoint, IJetTransaction jetTransaction)
        {
            EndpointInfo resultInfo = null;
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            oleDbCommand.CommandText = string.Format("SELECT [ID] FROM {0} WHERE [Endpoint]=@Endpoint;", _endpointTable.TableName);
            oleDbCommand.Parameters.AddWithValue("@Endpoint", endpoint);

            OleDbDataReader reader = oleDbCommand.ExecuteReader(CommandBehavior.SingleRow);

            if (!reader.HasRows)
            {
                resultInfo = this.Add(endpoint, jetTransaction);
            }
            else
            {
                reader.Read();

                int id = Convert.ToInt32(reader["ID"]);
                resultInfo = new EndpointInfo(id, endpoint);
            }

            return resultInfo;
        }

        public EndpointInfo[] GetAll(IJetTransaction jetTransaction)
        {
            List<EndpointInfo> resultInfos = new List<EndpointInfo>();
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            oleDbCommand.CommandText = string.Format("SELECT [ID], [Endpoint] FROM {0};", _endpointTable.TableName);

            OleDbDataReader reader = oleDbCommand.ExecuteReader();

            int tmpId;
            string tmpEndpoint;
            while(reader.Read())
            {
                tmpId = Convert.ToInt32(reader["ID"]);
                tmpEndpoint = Convert.ToString(reader["Endpoint"]);
                resultInfos.Add(new EndpointInfo(tmpId, tmpEndpoint));
            }

            return resultInfos.ToArray();
        }

        public IEndpointTable Table { get { return _endpointTable; } }

        #endregion

        #region ITableAdapter Members

        public SdataContext Context { get; private set; }

        ITable ITableAdapter.Table { get { return this.Table; } }

        #endregion

        #region Private Helpers

        private EndpointInfo Add(string endpoint, IJetTransaction jetTransaction)
        {
            EndpointInfo resultInfo = null;
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            oleDbCommand.CommandText = string.Format("INSERT INTO [{0}] ([Endpoint]) VALUES (@Endpoint);", _endpointTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@Endpoint", endpoint);

            oleDbCommand.ExecuteNonQuery();

            oleDbCommand.Parameters.Clear();
            oleDbCommand.CommandText = string.Format("SELECT @@IDENTITY FROM [{0}];", _endpointTable.TableName);
            oleDbCommand.CommandType = CommandType.Text;

            int newId = (int)oleDbCommand.ExecuteScalar();

            resultInfo = new EndpointInfo(newId, endpoint);

            return resultInfo;
        }

        #endregion


    }
}
