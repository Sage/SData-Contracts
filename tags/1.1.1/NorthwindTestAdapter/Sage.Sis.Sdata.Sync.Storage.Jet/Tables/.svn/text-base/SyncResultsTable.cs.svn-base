#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.Tables
{
    class SyncResultsTable : ISyncResultsTable
    {
        #region Class Variables

        private readonly IJetTableSchema _jetTableImp;

        #endregion

        #region Ctor.

        public SyncResultsTable(int resourceKindId, IResourceKindTable resourceKindTable)
        {
            this.ResourceKindId = resourceKindId;
            this.ResourceKindTable = resourceKindTable;
            string tableName;
            string[] sqlQueries = new string[2];

            tableName = string.Format("{0}tblSyncResults{1}", Settings.Default.TablePrefix, resourceKindId);

            sqlQueries[0] = string.Format("CREATE TABLE [{0}] ", tableName);
            sqlQueries[0] += "([ID]                     COUNTER, ";
            sqlQueries[0] += " [HttpMethod]             TEXT(40) NOT NULL, ";
            sqlQueries[0] += " [HttpStatus]             INTEGER NOT NULL, ";
            sqlQueries[0] += " [HttpMessage]            TEXT(255) DEFAULT NULL, ";
            sqlQueries[0] += " [HttpLocation]           TEXT(255) DEFAULT NULL, ";
            sqlQueries[0] += " [Diagnoses]              MEMO DEFAULT NULL, ";
            sqlQueries[0] += " [Payload]                MEMO DEFAULT NULL, ";
            sqlQueries[0] += " [Endpoint]               TEXT(255) NOT NULL, ";
            sqlQueries[0] += " [Stamp]                  TIMESTAMP NOT NULL);";
            

            sqlQueries[1] = string.Format("CREATE UNIQUE INDEX PK_ID ON [{0}] ([ID]) WITH PRIMARY;", tableName);
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

        #region ISyncResultsTable Members

        public int ResourceKindId { get; private set; }
        public IResourceKindTable ResourceKindTable { get; private set; }        

        #endregion
    }
}
