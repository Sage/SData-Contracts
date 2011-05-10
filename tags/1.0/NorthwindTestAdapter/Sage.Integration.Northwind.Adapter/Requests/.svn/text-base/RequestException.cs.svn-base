#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    [global::System.Serializable]
    public class RequestException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public RequestException() { }
        public RequestException(string message) : base(message) { }
        public RequestException(string message, Exception inner) : base(message, inner) { }
        protected RequestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
