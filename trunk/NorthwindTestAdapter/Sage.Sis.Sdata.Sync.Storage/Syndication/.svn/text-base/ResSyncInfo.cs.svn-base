#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Syndication
{
    public class ResSyncInfo
    {
        #region Ctor.

        public ResSyncInfo(Guid uuid, string endpoint, int tick, string etag, DateTime modifiedStamp)
        {
            this.Uuid = uuid;
            this.Endpoint = endpoint;
            this.Tick = tick;
            this.Etag = etag;
            this.ModifiedStamp = modifiedStamp;
        }

        #endregion

        #region Properties

        public Guid Uuid { get; private set; }
        public string Endpoint { get; set; }
        public int Tick { get; set; }
        public string Etag { get; set; }
        public DateTime ModifiedStamp { get; set; }

        #endregion
    }
}
