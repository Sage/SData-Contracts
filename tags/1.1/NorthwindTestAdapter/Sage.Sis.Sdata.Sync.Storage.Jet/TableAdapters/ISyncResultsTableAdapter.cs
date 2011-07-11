#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Results.Syndication;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    interface ISyncResultsTableAdapter : ITableAdapter
    {
        void Insert(SyncResultEntryInfo[] syncResultEntryInfos, IJetTransaction jetTransaction);
        void Delete(IJetTransaction jetTransaction);

        new ISyncResultsTable Table { get; }
    }
}
