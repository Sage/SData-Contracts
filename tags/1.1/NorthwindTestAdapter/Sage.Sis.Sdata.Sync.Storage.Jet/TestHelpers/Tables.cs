#if DEBUG

#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using Sage.Sis.Sdata.Sync.Context;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TestHelpers
{
    public class Tables
    {
        public static void Reset(SdataContext context)
        {
            StoreEnvironment.Remove(context);
        }
    }
}

#endif
