#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Sdata.Etag.Crc;
using Sage.Common.Syndication;
using System.Diagnostics;

#endregion

namespace Sage.Integration.Northwind.Etag
{
    public class EtagServices
    {
        private static EtagServices stat_etagSevices;
        private static object lockObj = new object();

        #region Class Variables

        private readonly EtagBuilder _simpleEtagBuilder;
        private readonly EtagBuilder _deepEtagBuilder;

        #endregion

        #region Ctor.

        static EtagServices()
        {
            lock (lockObj)
            {
                if (null == stat_etagSevices)
                {
                    stat_etagSevices = new EtagServices();
                }
            }
        }

        private EtagServices()
        {
            IEtagSerializer simpleSerializer = new PayloadEtagSerializer();
            IEtagSerializer deepSerializer = new DeepPayloadEtagSerializer();

            _simpleEtagBuilder = new EtagBuilder(new Crc32EtagProvider(simpleSerializer));
            _deepEtagBuilder = new EtagBuilder(new Crc32EtagProvider(deepSerializer));
        }

        #endregion

        /// <summary>
        /// Gets the etag value of a feed entry.
        /// </summary>
        /// <param name="feedEntry">The FeedEntry to compute the etag for.</param>
        /// <param name="recursive">Should the etag contain values of complex types?</param>
        /// <returns>A string representing the etag.</returns>
        public static string ComputeEtag(FeedEntry payload, bool recursive)
        {
            return (recursive) ? stat_etagSevices._deepEtagBuilder.Compute(payload) : stat_etagSevices._simpleEtagBuilder.Compute(payload);
        }
    }
}
