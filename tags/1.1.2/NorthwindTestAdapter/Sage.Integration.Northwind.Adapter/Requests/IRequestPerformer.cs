#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Messaging.Model;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    interface IRequestPerformer
    {
        void DoWork(IRequest request);
        void Initialize(RequestContext requestContext);
    }
}
