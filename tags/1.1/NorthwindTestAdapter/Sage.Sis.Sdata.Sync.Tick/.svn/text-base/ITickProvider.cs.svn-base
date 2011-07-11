#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Sync.Tick
{
    public interface ITickProvider
    {
        /// <summary>
        /// Creates a new tick for a given resource kind.
        /// </summary>
        /// <param name="resourceKind">Kind of the resource. (I.e. account, contact, salesOrder, etc.)</param>
        /// <returns>The next tick.</returns>
        /// <remarks>Each call of this method returns a new value in the tick sequence.</remarks>
        object CreateNextTick(string resourceKind);
    }
}
