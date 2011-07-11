#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.Tables
{
    internal class TickTable : ITickTable
    {
        #region Class Variables

        private readonly IJetTableSchema _jetTableImp;

        #endregion

        #region Ctor.

        public TickTable(IResourceKindTable resourceKindTable)
        {
            this.ResourceKindTable = resourceKindTable;

            string tableName;
            string[] sqlQueries = new string[3];

            tableName = string.Format("{0}tblTick", Settings.Default.TablePrefix);

            sqlQueries[0] = string.Format("CREATE TABLE [{0}] ", tableName);
            sqlQueries[0] += "([FKResourceKindId]     INTEGER NOT NULL, ";
            sqlQueries[0] += " [Tick]                 INTEGER NOT NULL);";

            sqlQueries[1] = string.Format("CREATE UNIQUE INDEX PK ON [{0}] ([FKResourceKindId]) WITH PRIMARY;", tableName);
            sqlQueries[2] = string.Format("ALTER TABLE {0} ADD CONSTRAINT FK_{0}_FKResourceKindId FOREIGN KEY (FKResourceKindId) REFERENCES {1} (ID);", tableName, this.ResourceKindTable.TableName);
            
            _jetTableImp = new JetTableSchema(tableName, sqlQueries);
        }

        #endregion

        #region ITickTable Members

        public IResourceKindTable ResourceKindTable { get; private set; }

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
    }
}
