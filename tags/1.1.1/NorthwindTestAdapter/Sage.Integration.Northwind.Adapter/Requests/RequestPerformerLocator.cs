#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    internal class RequestPerformerLocator
    {
        #region Class Variables

        private Dictionary<Guid, ITrackingPerformer> _trackingPerformerItems;
        private object lockObj = new object();

        #endregion

        #region Ctor.

        public RequestPerformerLocator()
        {
            _trackingPerformerItems = new Dictionary<Guid, ITrackingPerformer>();
        }

        #endregion

        public bool Delete(RequestContext context, out Exception error)
        {
            bool result = false;
            error = null;
            lock (lockObj)
            {
                try
                {
                    Guid trackingId = context.TrackingId;
                    result = _trackingPerformerItems.Remove(trackingId);
                }
                catch (Exception e)
                {
                    error = e;
          

                }
            }
            return result;
        }

        /// <summary>
        /// Receives or creates a new performer by a given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns>null or an instance of type T</returns>
        /// <remarks>
        /// returns null if performer type is not supported.
        /// - Performers implementing ITrackingPerformer are created and stored in a static dictionary in relation to a unique tracking id.
        /// - Performers implementing ITrackingConsumer are created and injected with the coresponding ITrackingPerformer instance.
        /// - All other performers are created.
        /// </remarks>
        public T Resolve<T>(RequestContext context)
            where T:IRequestPerformer, new()
        {
            T requestPerformer = default(T);
            Type type = typeof(T);
            Type iTrackingPerformerType = typeof(ITrackingPerformer);
            Type iTrackingConsumerType = typeof(ITrackingConsumer);

            // just for testing
            //requestPerformer = new T();
            //requestPerformer.Initialize(context);
            //return requestPerformer;

            lock (lockObj)
            {
                if (false == (iTrackingPerformerType.IsAssignableFrom(type) || iTrackingConsumerType.IsAssignableFrom(type)))
                {
                    // create a new performer instance
                    requestPerformer = new T();
                    requestPerformer.Initialize(context);
                }
                else
                {
                    if (iTrackingPerformerType.IsAssignableFrom(type))
                    {
                        ITrackingPerformer trackingPerformer;
                        Guid trackingId = context.TrackingId;
                        if (_trackingPerformerItems.TryGetValue(trackingId, out trackingPerformer))
                            throw new RequestException("Tracking Id already in use.");

                        // create a new performer instance
                        requestPerformer = new T();
                        requestPerformer.Initialize(context);
                        
                        _trackingPerformerItems.Add(trackingId, (ITrackingPerformer)requestPerformer); 
                        
                    }
                    if (iTrackingConsumerType.IsAssignableFrom(type))
                    {
                        ITrackingPerformer trackingPerformer;
                        Guid trackinId = context.TrackingId;
                        if (!_trackingPerformerItems.TryGetValue(trackinId, out trackingPerformer))
                            throw new RequestException("Tracking Id not in use.");

                        // create a new performer instance
                        requestPerformer = new T();
                        requestPerformer.Initialize(context);
                        ((ITrackingConsumer)requestPerformer).Initialize(trackingPerformer);    // injection
                    }
                }
            }
            return requestPerformer;
        }       
    }
}
