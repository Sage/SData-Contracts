#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

#endregion

namespace Sage.Sis.Sdata.Etag
{
    [Serializable]
    public class EtagComputeException : ApplicationException
    {
        #region Ctor.

        public EtagComputeException()
        {
        }
        public EtagComputeException(string message)
            : base(message)
        {
        }
        public EtagComputeException(string message, Exception inner)
            : base(message, inner) 
        {
        }
        protected EtagComputeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion
    }
}
