#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Syndication
{
    public class SyncDigestEntryInfo
    {        
        #region Ctor.

        public SyncDigestEntryInfo(string EndPoint, long tick, int conflictPriority, DateTime stamp)
        {
            this.EndPoint = EndPoint;
            this.Tick = tick;
            this.ConflictPriority = conflictPriority;
            this.Stamp = stamp;
        }

        #endregion

        #region Properties

        public string EndPoint { get; private set; }
        public long Tick { get; set; }
        public DateTime Stamp { get; set; }
        public int ConflictPriority { get; set; }

        #endregion
    }
}
