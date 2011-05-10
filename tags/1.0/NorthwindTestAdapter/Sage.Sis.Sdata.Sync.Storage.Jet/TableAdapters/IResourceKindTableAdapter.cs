#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    interface IResourceKindTableAdapter : ITableAdapter
    {
        ResourceKindInfo GetOrCreate(string resourceKind, IJetTransaction jetTransaction);
        ResourceKindInfo[] GetAll(IJetTransaction jetTransaction);

        new IResourceKindTable Table { get; }
    }
}
