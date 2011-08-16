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
    class EndPointTableAdapter : IEndPointTableAdapter
    {
        #region Class Variables

        private readonly IEndPointTable _EndPointTable;

        #endregion

        #region Ctor.

        public EndPointTableAdapter(IEndPointTable EndPointTable, SdataContext context)
        {
            _EndPointTable = EndPointTable;
            this.Context = context;
        }

        #endregion

        #region IEndPointTableAdapter Members


        public void SetOriginEndPoint(string EndPointBaseUrl, IJetTransaction jetTransaction)
        {
            List<EndPointInfo> updateInfos = new List<EndPointInfo>();
            if (String.IsNullOrEmpty(EndPointBaseUrl))
                return;
            if (!EndPointBaseUrl.EndsWith("/"))
                EndPointBaseUrl += "/";
            string localHost = "localhost/";

            OleDbCommand selectCommand = jetTransaction.CreateOleCommand();
            selectCommand.CommandText = string.Format(@"SELECT [ID], EndPoint FROM {0} WHERE (((EndPoint) Like ""{1}%""));", _EndPointTable.TableName, localHost);
            using (OleDbDataReader reader = selectCommand.ExecuteReader())
            {
                int tmpId;
                string tmpEndPoint;
                while (reader.Read())
                {
                    tmpId = Convert.ToInt32(reader["ID"]);
                    tmpEndPoint = Convert.ToString(reader["EndPoint"]);
                    tmpEndPoint = tmpEndPoint.Replace(localHost, EndPointBaseUrl);
                    updateInfos.Add(new EndPointInfo(tmpId, tmpEndPoint));
                }
            }

            foreach (EndPointInfo info in updateInfos)
            {

                OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

                oleDbCommand.CommandText = string.Format("UPDATE {0} SET [EndPoint] = @EndPoint WHERE [ID]=@ID;", _EndPointTable.TableName);
                oleDbCommand.Parameters.AddWithValue("@EndPoint", info.EndPoint);
                oleDbCommand.Parameters.AddWithValue("@ID", info.Id);

                oleDbCommand.ExecuteNonQuery();
            }

        }

        public EndPointInfo GetOrCreate(string EndPoint, IJetTransaction jetTransaction)
        {
            EndPointInfo resultInfo = null;
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            oleDbCommand.CommandText = string.Format("SELECT [ID] FROM {0} WHERE [EndPoint]=@EndPoint;", _EndPointTable.TableName);
            oleDbCommand.Parameters.AddWithValue("@EndPoint", EndPoint);

            using (OleDbDataReader reader = oleDbCommand.ExecuteReader(CommandBehavior.SingleRow))
            {

                if (!reader.HasRows)
                {
                    resultInfo = this.Add(EndPoint, jetTransaction);
                }
                else
                {
                    reader.Read();

                    int id = Convert.ToInt32(reader["ID"]);
                    resultInfo = new EndPointInfo(id, EndPoint);
                }
            }

            return resultInfo;
        }

        public EndPointInfo[] GetAll(IJetTransaction jetTransaction)
        {
            List<EndPointInfo> resultInfos = new List<EndPointInfo>();
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            oleDbCommand.CommandText = string.Format("SELECT [ID], [EndPoint] FROM {0};", _EndPointTable.TableName);

            using (OleDbDataReader reader = oleDbCommand.ExecuteReader())
            {
                int tmpId;
                string tmpEndPoint;
                while (reader.Read())
                {
                    tmpId = Convert.ToInt32(reader["ID"]);
                    tmpEndPoint = Convert.ToString(reader["EndPoint"]);
                    resultInfos.Add(new EndPointInfo(tmpId, tmpEndPoint));
                }
            }

            return resultInfos.ToArray();
        }

        public IEndPointTable Table { get { return _EndPointTable; } }

        #endregion

        #region ITableAdapter Members

        public SdataContext Context { get; private set; }

        ITable ITableAdapter.Table { get { return this.Table; } }

        #endregion

        #region Private Helpers

        private EndPointInfo Add(string EndPoint, IJetTransaction jetTransaction)
        {
            EndPointInfo resultInfo = null;
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            oleDbCommand.CommandText = string.Format("INSERT INTO [{0}] ([EndPoint]) VALUES (@EndPoint);", _EndPointTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@EndPoint", EndPoint);

            oleDbCommand.ExecuteNonQuery();

            oleDbCommand.Parameters.Clear();
            oleDbCommand.CommandText = string.Format("SELECT @@IDENTITY FROM [{0}];", _EndPointTable.TableName);
            oleDbCommand.CommandType = CommandType.Text;

            int newId = (int)oleDbCommand.ExecuteScalar();

            resultInfo = new EndPointInfo(newId, EndPoint);

            return resultInfo;
        }

        #endregion


    }
}
