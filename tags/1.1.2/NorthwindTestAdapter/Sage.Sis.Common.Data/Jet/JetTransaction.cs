#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.Internal;
using System.Data.OleDb;

#endregion

namespace Sage.Sis.Common.Data.OleDb
{
    public class JetTransaction : IJetTransaction
    {
        #region Class Variables

        private DbTransaction _transactionImpl;

        #endregion

        #region Ctor.

        public JetTransaction(OleDbConnection oleDbConnection, bool readOnly, bool ownsConnection)
        {
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
                oleDbConnection.Open();
            _transactionImpl = new DbTransaction(oleDbConnection, readOnly, ownsConnection);
        }

        #endregion

        #region ITransaction Members

        public void Commit()
        {
            _transactionImpl.Commit();
        }

        #endregion

        #region IOleDbTransaction Members

        public OleDbConnection OleDbConnection
        {
            get { return (OleDbConnection)_transactionImpl.Connection; }
        }

        //public OleDbTransaction OleDbTransaction
        //{
        //    get { return (OleDbTransaction)_transactionImpl.Transaction; }
        //}

        public OleDbCommand CreateOleCommand()
        {
            return (OleDbCommand)_transactionImpl.CreateCommand();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (null != _transactionImpl)
                _transactionImpl.Dispose();
        }

        #endregion

    }
}
