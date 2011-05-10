#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Tick;
using Sage.Sis.Sdata.Sync.Storage.Jet;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Integration.Northwind.Adapter.Sync
{
    internal class TickProvider : ISyncTickProvider
    {
        #region Class Variables

        private readonly ITickProvider _tickProvider;
        private readonly object lockObj = new object();

        #endregion

        #region Ctor.

        public TickProvider(IJetConnectionProvider jetConnectionProvider, SdataContext context)
        {
            _tickProvider = new Int32TickProvider(jetConnectionProvider, context);
        }

        #endregion

        #region ISyncTickProvider Members

        public int CreateNextTick(string resourceKind)
        {
            lock (lockObj)
            {
                return (int)_tickProvider.CreateNextTick(resourceKind);
            }
        }

        #endregion

        #region ITickProvider Members

        object ITickProvider.CreateNextTick(string resourceKind)
        {
            return this.CreateNextTick(resourceKind);
        }

        #endregion
    }
}
