#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Etag.Crc
{
    public interface IEtagSerializer
    {
        string Serialize(object obj);
    }
}
