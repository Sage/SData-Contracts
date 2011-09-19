#region Usings

using System;
using System.ComponentModel;
using System.Net;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Sync.Syndication;
using System.Xml.Serialization;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    public enum HttpMethod
    {
        [XmlEnumAttribute("PUT")]
        PUT,
        [XmlEnumAttribute("POST")]
        POST,
        [XmlEnumAttribute("DELETE")]
        DELETE,

    }

    public class SdataTransactionResult
    {
        public SupportedResourceKinds ResourceKind { get; set; }

        public string LocalId { get; set; }

        public string Uuid { get; set; }

        public HttpStatusCode HttpStatus { get; set; }

        public string HttpMessage { get; set; }

        public string Location { get; set; }

        public HttpMethod HttpMethod { get; set; }

       // public string Etag { get; set; }
        
        public Diagnosis Diagnosis { get; set; }

    }
}
