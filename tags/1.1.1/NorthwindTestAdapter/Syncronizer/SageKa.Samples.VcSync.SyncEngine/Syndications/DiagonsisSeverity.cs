namespace SageKa.Samples.VcSync.SyncEngine.Syndications
{
    public enum DiagonsisSeverity
    {
        // Informational message - does not require any special attention. 
        Info,

        // Warning message - does not prevent operation from succeeding but may require attention. 
        Warning,

        // Transient error - operation failed but may succeed later in the same condition (record locked for 'xxx'). 
        Transient,

        // Error: Operation failed - request should be modified before resubmitting. 
        Error,

        // Severe error - operation should not be reattempted. Other operations are likely to fail too.
        Fatal
    }
}
