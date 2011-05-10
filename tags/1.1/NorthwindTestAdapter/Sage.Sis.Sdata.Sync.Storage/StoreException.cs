using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Sis.Sdata.Sync.Storage
{
    [Serializable]
    public class StoreException : ApplicationException
    {
        #region Ctor.

        public StoreException()
            : base()
        {}
        public StoreException(string message)
            : base(message)
        {}
        public StoreException(string message, Exception innerException)
            : base(message, innerException)
        { }

        #endregion
    }
}
