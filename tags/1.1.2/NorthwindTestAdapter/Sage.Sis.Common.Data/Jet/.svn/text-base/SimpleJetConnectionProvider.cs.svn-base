#region Ctor.

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

#endregion

namespace Sage.Sis.Common.Data.OleDb
{
    public class SimpleJetConnectionProvider : IJetConnectionProvider
    {
        #region Ctor.

        public SimpleJetConnectionProvider(string connString)
        {
            this.ConnectionString = connString;
        }

        #endregion

        #region IJetConnectionProvider Members

        public IJetTransaction GetTransaction(bool readOnly)
        {
            JetTransaction oleDbTransaction = new JetTransaction(new OleDbConnection(this.ConnectionString), readOnly, true);

            return oleDbTransaction;
        }

        public string ConnectionString { get; private set; }

        #endregion

        #region IConnectionProvider Members

        ITransaction IConnectionProvider.GetTransaction(bool readOnly)
        {
            return this.GetTransaction(readOnly);
        }

        #endregion
    }
}
