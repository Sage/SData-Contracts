#region Usings

using System;

#endregion

namespace SageKa.Samples.VCSync.Synchronizer.Helpers
{
    public delegate T CreateExceptionDelegate<T>() where T:Exception;

    internal static class Validator
    {
        public static void ThrowIf(bool condition, CreateExceptionDelegate<Exception> createObject)
        {
            if (condition)
            {
                throw createObject();
            }
        }
    }
}
