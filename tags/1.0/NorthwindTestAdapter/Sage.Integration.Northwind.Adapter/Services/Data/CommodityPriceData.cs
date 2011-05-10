#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Application.Entities.Product.Documents;

#endregion

namespace Sage.Integration.Northwind.Adapter.Services.Data
{
    internal class CommodityPriceData : ICommodityPrice
    {
        #region Fields

        private CommodityIdentity _identity;
        private ProductPriceDocument _data;

        #endregion

        #region Properties

        public string LocalId
        {
            get
            {
                if (this.IsEmpty())
                    throw new InvalidOperationException("Object is empty.");

                return this.Identity.CommodityId;
            }
        }
        public decimal UnitPrice
        {
            get
            {
                if (this.IsEmpty())
                    throw new InvalidOperationException("Object is empty.");

                return this.Data.UnitPrice;
            }
        }

        public CommodityIdentity Identity { get; private set; }
        public ProductPriceDocument Data { get; private set; }

        public bool IsEmpty()
        {
            return (null == this.Identity);
        }

        #endregion

        #region Static Ctor.

        static CommodityPriceData()
        {
            Empty = new CommodityPriceData();
        }

        #endregion

        #region Ctors.

        private CommodityPriceData()
        {
        }

        public CommodityPriceData(CommodityIdentity identity, ProductPriceDocument data)
        {
            if (null == identity)
                throw new ArgumentNullException("Parameter 'identity' must not be null.", "identity");

            if (null == data)
                throw new ArgumentNullException("Parameter 'data' must not be null.", "data");

            this.Identity = identity;
            this.Data = data;
        }

        #endregion

        // Empty
        public static CommodityPriceData Empty;
    }
}
