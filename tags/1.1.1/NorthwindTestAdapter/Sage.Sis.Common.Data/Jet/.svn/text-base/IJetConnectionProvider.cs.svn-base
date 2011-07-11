#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Common.Data.OleDb
{
    public interface IJetConnectionProvider : IConnectionProvider
    {
        new IJetTransaction GetTransaction(bool readOnly);
        string ConnectionString { get; }
    }
}
