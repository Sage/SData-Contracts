#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.Syndication
{
    public class EndPointInfo
    {
        #region Ctor.

        public EndPointInfo(int id, string EndPoint)
        {
            this.Id = id;
            this.EndPoint = EndPoint;
        }

        #endregion

        #region Properties

        public int Id { get; private set; }
        public string EndPoint { get; private set; }

        #endregion
    }
}
