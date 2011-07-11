#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.Tables
{
    internal class CorrelatedResSyncTable : ICorrelatedResSyncTable
    {
        #region Class Variables

        private readonly IJetTableSchema _jetTableImp;

        #endregion

        #region Ctor.

        public CorrelatedResSyncTable(int resourceKindId, IResourceKindTable resourceKindTable, IEndpointTable endpointTable)
        {
            this.ResourceKindId = resourceKindId;
            this.ResourceKindTable = resourceKindTable;
            this.EndpointTable = endpointTable;

            string tableName;
            string[] sqlQueries = new string[4];

            tableName = string.Format("{0}tblCorrelatedResSync{1}", Settings.Default.TablePrefix, resourceKindId);

            sqlQueries[0] = string.Format("CREATE TABLE [{0}] ", tableName);
            sqlQueries[0] += "([Uuid]                 TEXT(38) NOT NULL, ";
            sqlQueries[0] += " [Tick]                 INTEGER NOT NULL, ";
            sqlQueries[0] += " [ModifiedStamp]        TIMESTAMP NOT NULL, ";
            sqlQueries[0] += " [Etag]                 TEXT(255) NOT NULL, ";
            sqlQueries[0] += " [LocalId]              TEXT(40) NOT NULL, ";
            sqlQueries[0] += " [FKEndpointId]         INTEGER NOT NULL, ";
            sqlQueries[0] += " [FKResourceKindId]     INTEGER NOT NULL); ";

            sqlQueries[1] = string.Format("CREATE UNIQUE INDEX PK ON [{0}] ([Uuid], [LocalId]) WITH PRIMARY;", tableName);

            sqlQueries[2] = string.Format("ALTER TABLE {0} ADD CONSTRAINT FK_{0}_FKEndpointId FOREIGN KEY (FKEndpointId) REFERENCES {1} (ID)", tableName, endpointTable.TableName);
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

        #region IResSyncTable Members

        public int ResourceKindId { get; private set; }
        public IResourceKindTable ResourceKindTable { get; private set; }
        public IEndpointTable EndpointTable { get; private set; }

        #endregion
    }
}
