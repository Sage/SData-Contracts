#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.Tables
{
    interface ITable
    {
        string TableName { get; }
        void CreateTable(IJetTransaction jetTransaction);
    }
}
