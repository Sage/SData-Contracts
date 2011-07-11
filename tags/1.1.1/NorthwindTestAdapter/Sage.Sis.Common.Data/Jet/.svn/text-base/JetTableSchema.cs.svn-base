#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.Internal;
using System.Data.OleDb;

#endregion

namespace Sage.Sis.Common.Data.OleDb
{
    public class JetTableSchema : IJetTableSchema
    {
        #region Class Variables

        private readonly string[] _sqlQueries;
        private readonly string _tableName;

        #endregion

        #region Ctor.

        public JetTableSchema(string tableName, string[] sql)
        {
            _tableName = tableName;
            _sqlQueries = sql;
        }

        #endregion

        #region IOleDbTableSchema Members

        public string TableName
        {
            get { return _tableName; }
        }

        public void CreateTable(IJetTransaction oleDbTransaction)
        {
            OleDbCommand command = oleDbTransaction.CreateOleCommand();
            foreach (string sqlQuery in _sqlQueries)
            {
                command.CommandText = sqlQuery;
                command.ExecuteNonQuery();
            }
        }

        #endregion

        #region ITableSchema Members

        void ITableSchema.CreateTable(ITransaction transaction)
        {
            this.CreateTable((IJetTransaction)transaction);
        }

        #endregion
    }
}
