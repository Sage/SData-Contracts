#region Usings

using System;
using SageKa.Samples.VcSync.SyncEngine.Syndications;

#endregion

namespace SageKa.Samples.VcSync.SyncEngine.Loggers
{
    public class EmptyLogger : ILogger
    {
        #region ILogger Members

        public void Write(string msg, Severity severity)
        {
        }

        public void Write(Exception exception)
        {
        }

        public void Write(Diagnosis diagnosis)
        {
        }

        #endregion
    }
}
