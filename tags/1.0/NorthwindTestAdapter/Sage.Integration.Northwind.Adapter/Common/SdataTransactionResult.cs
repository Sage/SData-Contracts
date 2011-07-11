#region Usings

using System;
using System.Net;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    public class SdataTransactionResult
    {
        public SupportedResourceKinds ResourceKind { get; set; }
        
        public string LocalId { get; set; }
        
        public Guid Uuid { get; set; }
        
        public HttpStatusCode HttpStatus { get; set; }
        
        public string HttpMessage { get; set; }
        
        public string Location { get; set; }
        
        public string HttpMethod { get; set; }
        
        public string Etag { get; set; }
    }
}
