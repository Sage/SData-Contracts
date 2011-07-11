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
    interface IEndpointTableAdapter : ITableAdapter
    {

        void SetOriginEndPoint(string endPointBaseUrl, IJetTransaction jetTransaction);
        EndpointInfo GetOrCreate(string endpoint, IJetTransaction jetTransaction);
        EndpointInfo[] GetAll(IJetTransaction jetTransaction);

        new IEndpointTable Table { get; }
    }
}
