#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

using Sage.Integration.Northwind.Common;

#endregion

namespace Sage.Integration.Northwind.Feeds
{
    [XmlRootAttribute("tracking", Namespace = Namespaces.sdataNamespace, IsNullable = false)]
    public class SyncTracking
    {
        [XmlElement("elapsedSeconds", Namespace = Namespaces.sdataNamespace)]
        public double ElapsedSeconds { get; set; }
        [XmlElement("phase", Namespace = Namespaces.sdataNamespace)]
        public string Phase { get; set; }
        [XmlElement("phaseDetail", Namespace = Namespaces.sdataNamespace)]
        public string PhaseDetail { get; set; }
        [XmlElement("pollingMillis", Namespace = Namespaces.sdataNamespace)]
        public double PollingMillis { get; set; }
        [XmlElement("progress", Namespace = Namespaces.sdataNamespace)]
        public double Progress { get; set; }
        [XmlElement("remainingSeconds", Namespace = Namespaces.sdataNamespace)]
        public double RemainingSeconds { get; set; }
    }
}
