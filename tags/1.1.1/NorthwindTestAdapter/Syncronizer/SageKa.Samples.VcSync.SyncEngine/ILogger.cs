#region Usings

using System;
using SageKa.Samples.VcSync.SyncEngine.Syndications;

#endregion

namespace SageKa.Samples.VcSync.SyncEngine
{
    public interface ILogger
    {
        void Write(string msg, Severity severity);
        void Write(Exception exception);
        void Write(Diagnosis diagnosis);
    }

    public enum Severity
    {
        Error = 0,
        Warning = 1,
        Info = 2,
        Trace = 3
    }
}
