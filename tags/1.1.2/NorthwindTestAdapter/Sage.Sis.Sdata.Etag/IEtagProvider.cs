#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Sis.Sdata.Etag
{
    /// <summary>
    /// Implementations of this interface computes an etag value for a given object.
    /// </summary>
    public interface IEtagProvider
    {
        /// <summary>
        /// Returns an etag for a given object. 
        /// </summary>
        /// <param name="obj">The object which etag representation should be computed.</param>
        /// <returns>The etag representaion of the object.</returns>
        /// <exception cref="EtagComputeException"></exception>
        /// <remarks>Must support a null value for parameter 'obj'!</remarks>
        string ComputeEtag(object obj);
    }

}
