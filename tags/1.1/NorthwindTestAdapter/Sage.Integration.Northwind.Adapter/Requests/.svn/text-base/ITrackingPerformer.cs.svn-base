#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Messaging.Model;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    interface ITrackingPerformer : IRequestPerformer
    {
        Guid TrackingId { get; }
        void GetTrackingState(IRequest request);
    }
}
