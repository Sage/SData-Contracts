#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.Tables
{
    class EndpointTable : IEndpointTable
    {
        #region Class Variables

        private readonly IJetTableSchema _jetTableImp;

        #endregion

        #region Ctor.

        public EndpointTable()
        {
            string tableName;
            string[] sqlQueries = new string[3];

            tableName = string.Format("{0}tblEndpoint", Settings.Default.TablePrefix);

            sqlQueries[0] = string.Format("CREATE TABLE [{0}] ", tableName);
            sqlQueries[0] += "([ID]             COUNTER, ";
            sqlQueries[0] += " [Endpoint]       TEXT(255) NOT NULL);";

            sqlQueries[1] = string.Format("CREATE UNIQUE INDEX PK_ID ON [{0}] ([ID]) WITH PRIMARY;", tableName);
            sqlQueries[2] = string.Format("CREATE UNIQUE INDEX IDX_ENDPOINT ON [{0}] ([Endpoint])", tableName);

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
    }
}
