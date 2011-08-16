#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using Sage.Sis.Common.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters
{
    interface IEndPointTableAdapter : ITableAdapter
    {

        void SetOriginEndPoint(string EndPointBaseUrl, IJetTransaction jetTransaction);
        EndPointInfo GetOrCreate(string EndPoint, IJetTransaction jetTransaction);
        EndPointInfo[] GetAll(IJetTransaction jetTransaction);

        new IEndPointTable Table { get; }
    }
}
