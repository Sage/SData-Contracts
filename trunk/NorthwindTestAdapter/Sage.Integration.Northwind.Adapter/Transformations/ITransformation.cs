#region Usings

using System.Collections.Generic;
using Sage.Integration.Northwind.Application.Base;

using Sage.Common.Syndication;

#endregion

namespace Sage.Integration.Northwind.Adapter.Transformations
{
    #region INTERFACE: ITransformation

    public interface ITransformation
    {
    }

    #endregion

    #region INTERFACE: ITransformation<D, P>

/*public interface ITransformation<D, P> : ITransformation
        where D : Document
        where P : ResourcePayloadContainer
    {
        D GetTransformedDocument(P payload);
        P GetTransformedPayload(D document);
    }*/

    public interface IFeedEntryTransformation<D, F> : ITransformation
        where D : Document
        where F: FeedEntry
    {
        D GetTransformedDocument(F feedEntry);
        F GetTransformedPayload(D document);
    }

    #endregion
}
