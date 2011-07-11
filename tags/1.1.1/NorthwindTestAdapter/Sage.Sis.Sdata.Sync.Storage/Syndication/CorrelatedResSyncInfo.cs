#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Syndication
{
    public class CorrelatedResSyncInfo
    {
        #region Ctor.

        public CorrelatedResSyncInfo(string localId, ResSyncInfo resSyncInfo)
        {
            this.LocalId = localId;
            this.ResSyncInfo = resSyncInfo;
        }

        #endregion

        #region Properties

        public string LocalId {get; set;}
        public ResSyncInfo ResSyncInfo { get; private set; }

        #endregion
    }
}
