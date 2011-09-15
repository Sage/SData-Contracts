#region Usings

using System;

#endregion

namespace Sage.Sis.Sdata.Sync.Results.Syndication
{
    public class SyncResultEntryInfo
    {
        #region Properties

        public string HttpMethod { get; private set; }
        public int HttpStatus { get; private set; }
        public string HttpLocation { get; private set; }
        public string HttpMessage { get; private set; }

        public string DiagnosisXml { get; private set; }

        public string PayloadXml { get; private set; }

        public string EndPoint { get; private set; }

        public DateTime Stamp { get; private set; }

        #endregion

        #region Ctor.

        public SyncResultEntryInfo(
            string httpMethod, 
            int httpStatus, 
            string httpLocation,
            string httpMessage,
            string diagnosisXml,
            string payloadXml,
            DateTime stamp,
            string EndPoint)
        {
            this.HttpMethod = httpMethod;
            this.HttpStatus = httpStatus;
            this.HttpLocation = httpLocation;
            this.HttpMessage = httpMessage;
            this.DiagnosisXml = diagnosisXml;
            this.PayloadXml = payloadXml;
            this.Stamp = stamp;
            this.EndPoint = EndPoint;
        }

        public SyncResultEntryInfo(string httpMethod, int httpStatus, string EndPoint)
            : this(httpMethod, httpStatus, null, null, null, null, DateTime.Now, EndPoint)
        {
        }

        #endregion
    }
}
