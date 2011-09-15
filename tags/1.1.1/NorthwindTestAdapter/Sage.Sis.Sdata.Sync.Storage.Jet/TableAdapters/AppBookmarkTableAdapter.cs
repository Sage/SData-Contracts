#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using System.Data.OleDb;
using System.Data;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    class AppBookmarkTableAdapter : IAppBookmarkTableAdapter
    {
        #region Class Variables

        private readonly IAppBookmarkTable _appBookmarkTable;

        #endregion

        #region Ctor.

        public AppBookmarkTableAdapter(IAppBookmarkTable appBookmarkTable, SdataContext context)
        {
            _appBookmarkTable = appBookmarkTable;
            this.Context = context;
        }

        #endregion

        #region IAppBookmarkTableAdapter Members

        public bool Get(int resourceKindId, out byte[] applicationBookmark, out string assemblyQualifiedName, IJetTransaction jetTransaction)
        {
            applicationBookmark = null;
            assemblyQualifiedName = null;

            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;
            sqlQuery = "SELECT [Type], [Value] FROM {0} WHERE [FKResourceKindId] = @ResourceKindId";

            oleDbCommand.CommandText = string.Format(sqlQuery, _appBookmarkTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);

            using (OleDbDataReader reader = oleDbCommand.ExecuteReader(CommandBehavior.SingleRow | CommandBehavior.SequentialAccess))
            {
                if (!reader.HasRows)
                    return false;

                reader.Read();  // only one row accepted
                assemblyQualifiedName = Convert.ToString(reader["Type"]);
                applicationBookmark = JetHelpers.ReadBlob(reader, 1);
            }
            return true;
        }

        public void Insert(int resourceKindId, object applicationBookmark, string assemblyQualifiedName, IJetTransaction jetTransaction)
        {
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();

            string sqlQuery = string.Empty;
            sqlQuery = "INSERT INTO {0} ([Type], [Value], [FKResourceKindId]) VALUES (@Type, @Value, @ResourceKindId);";

            oleDbCommand.CommandText = string.Format(sqlQuery, _appBookmarkTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@Type", assemblyQualifiedName);
            oleDbCommand.Parameters.AddWithValue("@Value", applicationBookmark);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);

            oleDbCommand.ExecuteNonQuery();
        }

        public void Delete(int resourceKindId, IJetTransaction jetTransaction)
        {
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            oleDbCommand.CommandText = string.Format("DELETE FROM {0} WHERE [FKResourceKindId] = @ResourceKindId", _appBookmarkTable.TableName);
            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);
            oleDbCommand.ExecuteNonQuery();
        }

        public bool Update(int resourceKindId, object applicationBookmark, string assemblyQualifiedName, IJetTransaction jetTransaction)
        {
            OleDbCommand oleDbCommand = jetTransaction.CreateOleCommand();
            oleDbCommand.CommandText = string.Format("UPDATE [{0}] SET [FKResourceKindId]=@ResourceKindId, [Type]=@Type, [Value]=@Value WHERE [FKResourceKindId]=@ResourceKindId;", _appBookmarkTable.TableName);

            oleDbCommand.Parameters.AddWithValue("@ResourceKindId", resourceKindId);
            oleDbCommand.Parameters.AddWithValue("@Type", assemblyQualifiedName);
            oleDbCommand.Parameters.AddWithValue("@Value", applicationBookmark);

            return (0 != oleDbCommand.ExecuteNonQuery());
        }

        public IAppBookmarkTable Table { get { return _appBookmarkTable; } }

        #endregion

        #region ITableAdapter Members

        public SdataContext Context { get; private set; }
        ITable ITableAdapter.Table { get { return this.Table; } }
        
        #endregion
    }
}
