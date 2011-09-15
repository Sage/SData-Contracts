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

        public ResSyncInfo(Guid uuid, string EndPoint, long tick, string etag, DateTime modifiedStamp)
        {
            this.Uuid = uuid;
            this.EndPoint = EndPoint;
            this.Tick = tick;
            this.Etag = etag;
            this.ModifiedStamp = modifiedStamp;
        }

        #endregion

        #region Properties

        public Guid Uuid { get; private set; }
        public string EndPoint { get; set; }
        public long Tick { get; set; }
        public string Etag { get; set; }
        public DateTime ModifiedStamp { get; set; }

        #endregion
    }
}
