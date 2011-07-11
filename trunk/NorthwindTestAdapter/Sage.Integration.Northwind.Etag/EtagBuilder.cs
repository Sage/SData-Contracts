#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Etag;

#endregion

namespace Sage.Integration.Northwind.Etag
{
    internal class EtagBuilder
    {
        #region Class Variables

        private readonly IEtagProvider _etagProvider;

        #endregion

        #region Ctor.

        public EtagBuilder(IEtagProvider etagProvider)
        {
            _etagProvider = etagProvider;
        }

        #endregion

        public string Compute(object obj)
        {
            return _etagProvider.ComputeEtag(obj);
        }
    }
}
