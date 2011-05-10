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

        public SyncDigestEntryInfo(string endpoint, int tick, int conflictPriority, DateTime stamp)
        {
            this.Endpoint = endpoint;
            this.Tick = tick;
            this.ConflictPriority = conflictPriority;
            this.Stamp = stamp;
        }

        #endregion

        #region Properties

        public string Endpoint { get; private set; }
        public int Tick { get; set; }
        public DateTime Stamp { get; set; }
        public int ConflictPriority { get; set; }

        #endregion
    }
}
