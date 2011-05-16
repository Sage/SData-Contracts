#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;

#endregion

namespace Sage.Sis.Common.Data.OleDb
{
    public interface IJetTransaction : ITransaction
    {
        OleDbConnection OleDbConnection { get; }
        //OleDbTransaction OleDbTransaction { get; }

        OleDbCommand CreateOleCommand();
    }
}
