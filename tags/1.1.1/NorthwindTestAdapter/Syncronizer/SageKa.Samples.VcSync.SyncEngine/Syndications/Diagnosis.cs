#region Usings

using System;
using System.Diagnostics;
using System.Xml.Serialization;

#endregion

namespace SageKa.Samples.VcSync.SyncEngine.Syndications
{
    [Serializable]
    [DebuggerStepThrough]
    [XmlType(TypeName = "diagnosis--type", Namespace = "http://schemas.sage.com/sdata/2008/1")]
    [XmlRoot("diagnosis", Namespace = "http://schemas.sage.com/sdata/2008/1", IsNullable = false)]
    public class Diagnosis
    {
        // Severity of the diagnosis entry.
        [XmlElement("severity")]
        public DiagonsisSeverity Severity { get; set; }

        //The SData diagnosis code 
        [XmlElement("sdataCode")]
        public DiagnosisCode SdataCode { get; set; }

        // The application specific diagnosis code.
        // This should only be set when sdataCode is set to ApplicationDiagnosis.
        [XmlElement("applicationCode")]
        public string ApplicationCode { get; set; }

        // A friendly message for the diagnosis.
        [XmlElement("message")]
        public string Message { get; set; }

        // The stack trace for the error. It should only be filled when the service is run in “development” mode.
        [XmlElement("stackTrace")]
        public string StackTrace { get; set; }

        // XPath expression that refers to the payload element responsible for the error.
        // This element is only used when the error is related to a specific piece of data being sent to the service provider. 
        // For example when updating a resource, or when submitting a service request.
        [XmlElement("payloadPath")]
        public string PayloadPath { get; set; }

    }
}
