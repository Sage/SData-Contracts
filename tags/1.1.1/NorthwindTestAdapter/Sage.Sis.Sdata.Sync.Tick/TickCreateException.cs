using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Sis.Sdata.Sync.tick
{
    [global::System.Serializable]
    public class tickCreateException : ApplicationException
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public tickCreateException() { }
        public tickCreateException(string message) : base(message) { }
        public tickCreateException(string message, Exception inner) : base(message, inner) { }
        protected tickCreateException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
