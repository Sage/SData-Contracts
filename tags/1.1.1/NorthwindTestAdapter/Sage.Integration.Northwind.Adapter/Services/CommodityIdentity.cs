#region Usings

using System;


#endregion

namespace Sage.Integration.Northwind.Adapter.Services
{
    internal class CommodityIdentity
    {
        #region Fields

        private readonly string _commodityId;

        #endregion

        #region Static Ctor.

        static CommodityIdentity()
        {
            Empty = new CommodityIdentity();
        }

        #endregion

        #region Ctors.

        private CommodityIdentity()
        {
        }

        public CommodityIdentity(string productId)
        {
            if (string.IsNullOrEmpty(productId))
                throw new ArgumentException("Parameter 'commodityId' must not be emty or null.", "commodityId");

            _commodityId = productId;
        }

        #endregion

        #region Properties

        public string CommodityId
        {
            get
            {
                if (null == _commodityId)
                    throw new InvalidOperationException("Object is empty. No commodityId set.");
                
                return _commodityId;
            }
        }

        public bool IsEmpty()
        {
            return (null == _commodityId);
        }

        #endregion

        // Empty
        public static CommodityIdentity Empty;
    }
}
