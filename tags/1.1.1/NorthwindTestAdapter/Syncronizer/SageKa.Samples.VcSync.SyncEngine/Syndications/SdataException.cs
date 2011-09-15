#region Usings

using System;
using System.Runtime.Serialization;


#endregion

namespace SageKa.Samples.VcSync.SyncEngine.Syndications
{
    [Serializable]
    public class SdataException : Exception
    {
        public Diagnosis Diagnosis { get; private set; }

        public SdataException()
            : base()
        {
        }

        public SdataException(string message)
            : base(message)
        {
        }

        public SdataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        public SdataException(Diagnosis diagnosis)
            : base(diagnosis.Message)
        {
            this.Diagnosis = diagnosis;
        }

        protected SdataException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            info.AddValue("diagnosis", this.Diagnosis, typeof(Diagnosis));
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            this.Diagnosis = (Diagnosis)info.GetValue("diagnosis", typeof(Diagnosis));
            base.GetObjectData(info, context);
        }
    }
}
