#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Common.Data
{
    public interface IConnectionProvider
    {
        ITransaction GetTransaction(bool readOnly);
    }
}
