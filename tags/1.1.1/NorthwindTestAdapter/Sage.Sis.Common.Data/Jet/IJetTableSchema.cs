#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Common.Data.OleDb
{
    public interface IJetTableSchema : ITableSchema
    {
        void CreateTable(IJetTransaction oleDbTransaction);
    }
}
