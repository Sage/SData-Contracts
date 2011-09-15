#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using System.Data;
using System.Data.OleDb;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.Tables
{
    class SyncDigestTable : ISyncDigestTable, ITableFieldsUpdated
    {
        #region Class Variables

        private readonly IJetTableSchema _jetTableImp;

        #endregion

        #region Ctor.

        public SyncDigestTable(IResourceKindTable resourceKindTable, IEndPointTable EndPointTable)
        {
            this.ResourceKindTable = resourceKindTable;
            this.EndPointTable = EndPointTable;

            string tableName;
            string[] sqlQueries = new string[4];

            tableName = string.Format("{0}tblSyncDigest", Settings.Default.TablePrefix);

            sqlQueries[0] = string.Format("CREATE TABLE [{0}] ", tableName);
            sqlQueries[0] += "([tick]                   INTEGER NOT NULL, ";
            sqlQueries[0] += " [ConflictPriority]       INTEGER NOT NULL, ";
            sqlQueries[0] += " [Stamp]                  TIMESTAMP NOT NULL, ";
            sqlQueries[0] += " [FKEndPointId]           INTEGER NOT NULL, ";
            sqlQueries[0] += " [FKResourceKindId]       INTEGER NOT NULL);";

            sqlQueries[1] = string.Format("CREATE UNIQUE INDEX PK_ID ON [{0}] ([FKEndPointId],[FKResourceKindId]) WITH PRIMARY;", tableName);

            sqlQueries[2] = string.Format("ALTER TABLE {0} ADD CONSTRAINT FK_{0}_FKEndPointId FOREIGN KEY (FKEndPointId) REFERENCES {1} (ID)", tableName, EndPointTable.TableName);
            sqlQueries[3] = string.Format("ALTER TABLE {0} ADD CONSTRAINT FK_{0}_FKResourceKindId FOREIGN KEY (FKResourceKindId) REFERENCES {1} (ID)", tableName, resourceKindTable.TableName);

            _jetTableImp = new JetTableSchema(tableName, sqlQueries);
        }

        #endregion

        #region ITable Members

        public string TableName
        {
            get { return _jetTableImp.TableName; }
        }

        public void CreateTable(IJetTransaction oleDbTransaction)
        {
            _jetTableImp.CreateTable(oleDbTransaction);
        }

        #endregion

        #region ISyncDigestTable Members

        public IResourceKindTable ResourceKindTable { get; private set; }
        public IEndPointTable EndPointTable { get; private set; }

        #endregion

        #region ITableFieldsUpdated Members

        void ITableFieldsUpdated.UpdateFields(IJetTransaction jetTransaction)
        {
            // CHECK IF COLUMN EXISTS
            bool rowStampExists = false;

            string[] restrictions = new string[] { null, null, this.TableName, null };
            System.Data.DataTable DataTable1 = jetTransaction.OleDbConnection.GetSchema("Columns", restrictions);

            string restriction = string.Format("[TABLE_NAME]='{0}'", this.TableName);
            DataRow[] rows = jetTransaction.OleDbConnection.GetSchema("Columns").Select(restriction);
            if (null == rows || rows.Length == 0)
                throw new StoreException("Cannot update table '{0}'. The column names cannot be requested.");

            foreach (DataRow dataRow in rows)
            {
                if (Convert.ToString(dataRow["COLUMN_NAME"]) == "Stamp")
                {
                    rowStampExists = true;
                    break;
                }
            }


            // ADD COLUMN AND VALUES
            if (!rowStampExists)
            {
                OleDbCommand command = jetTransaction.CreateOleCommand();

                string sql = string.Format("ALTER TABLE [{0}] ADD COLUMN [Stamp] TIMESTAMP NOT NULL", this.TableName);
                command.CommandText = sql;
                command.ExecuteNonQuery();

                
                sql = "UPDATE [{0}] SET [Stamp]=@Stamp;";
                command.CommandText = string.Format(sql, this.TableName);
                command.Parameters.AddWithValue("@Stamp", DateTime.Now.ToString());

                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
