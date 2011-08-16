#region Usings

using System;
using System.Xml.Serialization;

#endregion

namespace SageKa.Samples.VcSync.SyncEngine.Syndications
{
    [Serializable]
    public enum DiagnosisCode
    {
        //Invalid URL syntax
        [XmlEnum("BadUrlSyntax")]
        BadUrlSyntax,
        
        // Invalid Query Parameter
        [XmlEnum("BadUrlSyntax")]
        BadQueryParameter,

        // Application does not exist.
        [XmlEnum("ApplicationNotFound")]
        ApplicationNotFound,

        // Application exists but is not available.
        [XmlEnum("ApplicationUnavailable")]
        ApplicationUnavailable,

        // Dataset does not exist.
        [XmlEnum("DatasetNotFound")]
        DatasetNotFound,

        // Dataset exists but is not available.
        [XmlEnum("DatasetUnavailable")]
        DatasetUnavailable,

        // Contract does not exist. 
        [XmlEnum("ContractNotFound")]
        ContractNotFound,

        // Resource kind does not exist.
        [XmlEnum("ResourceKindNotFound")]
        ResourceKindNotFound,

        // Invalid syntax for a where condition.
        [XmlEnum("BadWhereSyntax")]
        BadWhereSyntax,

        // Application specific diagnosis, detail is in the applicationCode element.
        [XmlEnum("ApplicationDiagnosis")]
        ApplicationDiagnosis

    }
}
