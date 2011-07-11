#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet.Tables
{
    interface ICorrelatedResSyncTable : ITable
    {
        int ResourceKindId { get; }

        IResourceKindTable ResourceKindTable { get; }
        IEndpointTable EndpointTable { get; }
    }
}
