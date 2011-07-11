#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

#endregion

namespace Sage.Sis.Common.Data.Internal
{
    internal class DbTransaction : ITransaction
    {
        #region Class Variables

        private IDbConnection _connection;
        //private IDbTransaction _transaction;
        private bool _ownsConnection;

        #endregion

        #region Ctor.

        public DbTransaction(IDbConnection connection, bool readOnly, bool ownsConnection)
        {
            _connection = connection;
            //_transaction = readOnly ? null : connection.BeginTransaction();
            _ownsConnection = ownsConnection;
        }

        #endregion

        #region Properties

        public IDbConnection Connection { get { return _connection; } }
        //public IDbTransaction Transaction { get { return _transaction; } }

        #endregion

        public IDbCommand CreateCommand()
        {
            IDbCommand command = _connection.CreateCommand();
            //command.Transaction = _transaction;
            return command;
        }

        #region ITransaction Members

        public void Commit()
        {
            //_transaction.Commit();
            //_transaction.Dispose();
            //_transaction = null;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //if (_transaction != null)
            //    _transaction.Dispose();
            //_transaction = null;
            if (_ownsConnection && _connection != null)
                _connection.Dispose();
            _connection = null;
        }

        #endregion
    }
}
