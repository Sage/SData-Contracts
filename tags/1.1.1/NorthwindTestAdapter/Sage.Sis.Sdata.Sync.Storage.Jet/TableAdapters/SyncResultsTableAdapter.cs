#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Context;
using Sage.Sis.Common.Data.OleDb;
using System.Data.OleDb;
using Sage.Sis.Sdata.Sync.Results.Syndication;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    class SyncResultsTableAdapter : ISyncResultsTableAdapter
    {
        #region Class Variables

        private readonly ISyncResultsTable _syncResultsTable;

        #endregion

        #region Ctor.

        public SyncResultsTableAdapter(ISyncResultsTable syncResultsTable, SdataContext context)
        {
            _syncResultsTable = syncResultsTable;
            this.Context = context;
        }

        #endregion

        #region SyncResultsTableAdapter Members

        public void Insert(SyncResultEntryInfo[] infos, string runName, string runStamp, IJetTransaction jetTransaction)
        {
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;
            sqlQuery = "INSERT INTO {0} ([HttpMethod], [HttpStatus], [HttpMessage], [HttpLocation], [Diagnoses], [Payload], [EndPoint], [Stamp], [RunName], [RunStamp]) VALUES (@HttpMethod, @HttpStatus, @HttpMessage, @HttpLocation, @Diagnoses, @Payload, @EndPoint, @Stamp, @RunName, @RunStamp);";

            oleDbCommand.CommandText = string.Format(sqlQuery, _syncResultsTable.TableName);

            foreach (SyncResultEntryInfo info in infos)
            {
                // TODO: Use prepared query
                //oleDbCommand.Parameters.Clear();
                oleDbCommand.Parameters.AddWithValue("@HttpMethod", info.HttpMethod);
                oleDbCommand.Parameters.AddWithValue("@HttpStatus", info.HttpStatus);
                if (null != info.HttpMessage) oleDbCommand.Parameters.AddWithValue("@HttpMessage", (info.HttpMessage.Length > 255 ? info.HttpMessage.Substring(0, 255) : info.HttpMessage));
                else oleDbCommand.Parameters.AddWithValue("@HttpMessage", DBNull.Value);
                if (null != info.HttpLocation) oleDbCommand.Parameters.AddWithValue("@HttpLocation", info.HttpLocation);
                else oleDbCommand.Parameters.AddWithValue("@HttpLocation", DBNull.Value);
                if (null != info.DiagnosisXml) oleDbCommand.Parameters.AddWithValue("@Diagnoses", info.DiagnosisXml);
                else oleDbCommand.Parameters.AddWithValue("@Diagnoses", DBNull.Value);
                if (null != info.PayloadXml) oleDbCommand.Parameters.AddWithValue("@Payload", info.PayloadXml);
                else oleDbCommand.Parameters.AddWithValue("@Payload", DBNull.Value);
                oleDbCommand.Parameters.AddWithValue("@EndPoint", info.EndPoint);
                oleDbCommand.Parameters.AddWithValue("@Stamp", info.Stamp.ToString());
                if (null != runName) oleDbCommand.Parameters.AddWithValue("@RunName", runName);
                else oleDbCommand.Parameters.AddWithValue("@RunName", DBNull.Value);
                if (null != runStamp) oleDbCommand.Parameters.AddWithValue("@RunStamp", runStamp);
                else oleDbCommand.Parameters.AddWithValue("@RunStamp", DBNull.Value);

                oleDbCommand.ExecuteNonQuery();
            }
        }

        public void Delete(IJetTransaction jetTransaction)
        {
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            oleDbCommand.CommandText = string.Format("DELETE FROM {0};", _syncResultsTable.TableName);
            oleDbCommand.ExecuteNonQuery();
        }

        public ISyncResultsTable Table { get { return _syncResultsTable; } }

        #endregion

        #region ITableAdapter Members

        public SdataContext Context { get; private set; }
        ITable ITableAdapter.Table { get { return this.Table; } }

        #endregion
    }
}
