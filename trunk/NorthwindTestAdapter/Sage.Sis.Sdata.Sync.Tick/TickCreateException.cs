using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Sis.Sdata.Sync.Tick
{
    [global::System.Serializable]
    public class TickCreateException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TickCreateException() { }
        public TickCreateException(string message) : base(message) { }
        public TickCreateException(string message, Exception inner) : base(message, inner) { }
        protected TickCreateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
