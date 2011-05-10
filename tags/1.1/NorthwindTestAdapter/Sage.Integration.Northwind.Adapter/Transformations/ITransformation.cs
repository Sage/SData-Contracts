#region Usings

using System.Collections.Generic;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Feeds;

#endregion

namespace Sage.Integration.Northwind.Adapter.Transformations
{
    #region INTERFACE: ITransformation

    public interface ITransformation
    {
    }

    #endregion

    #region INTERFACE: ITransformation<D, P>

    public interface ITransformation<D, P> : ITransformation
        where D : Document
        where P : PayloadBase
    {
        D GetTransformedDocument(P payload, List<SyncFeedEntryLink> links);
        P GetTransformedPayload(D document, out List<SyncFeedEntryLink> links);
    }

    #endregion
}
