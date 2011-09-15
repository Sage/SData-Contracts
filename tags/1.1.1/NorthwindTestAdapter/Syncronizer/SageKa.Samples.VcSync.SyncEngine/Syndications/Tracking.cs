#region Usings

using System;
using System.Diagnostics;
using System.Xml.Serialization;

#endregion

namespace SageKa.Samples.VcSync.SyncEngine.Syndications
{
    [Serializable]
    [DebuggerStepThrough()]

    [XmlType(TypeName = "tracking--type", Namespace = "http://schemas.sage.com/sdata/2008/1")]//Namespace = "http://schemas.sage.com/sdata/2008/1")]
    [XmlRoot("tracking", Namespace = "http://schemas.sage.com/sdata/2008/1")]
   
    public class Tracking : ICloneable
    {
        [XmlElement("phase")]
        public string Phase { get; set; }

        [XmlElement("phaseDetail")]
        public string PhaseDetail { get; set; }

        [XmlElement("progress")]
        public decimal Progress { get; set; }

        [XmlElement("elapsedSeconds")]
        public decimal ElapsedSeconds { get; set; }

        [XmlElement("remainingSeconds")]
        public decimal RemainingSeconds { get; set; }

        [XmlElement("pollingMillis")]
        public int PollingMillis { get; set; }


        #region ICloneable Members

        object ICloneable.Clone()
        {
            return this.Clone();
        }

        #endregion

        public Tracking Clone()
        {
            return new Tracking()
            {
                ElapsedSeconds = this.ElapsedSeconds,
                Phase = this.Phase,
                PhaseDetail = this.PhaseDetail,
                PollingMillis = this.PollingMillis,
                Progress = this.Progress,
                RemainingSeconds = this.RemainingSeconds
            };
        }
    }
}
