#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.tick;
using Sage.Sis.Sdata.Sync.Storage.Jet;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Integration.Northwind.Adapter.Sync
{
    internal class tickProvider : ISynctickProvider
    {
        #region Class Variables

        private readonly ItickProvider _tickProvider;
        private readonly object lockObj = new object();

        #endregion

        #region Ctor.

        public tickProvider(IJetConnectionProvider jetConnectionProvider, SdataContext context)
        {
            _tickProvider = new Int32tickProvider(jetConnectionProvider, context);
        }

        #endregion

        #region ISynctickProvider Members

        public int CreateNexttick(string resourceKind)
        {
            lock (lockObj)
            {
                return (int)_tickProvider.CreateNexttick(resourceKind);
            }
        }

        #endregion

        #region ItickProvider Members

        object ItickProvider.CreateNexttick(string resourceKind)
        {
            return this.CreateNexttick(resourceKind);
        }

        #endregion
    }
}
