#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Feeds;
using Sage.Integration.Northwind.Feeds.TradingAccounts;
using Sage.Integration.Northwind.Feeds.SalesOrders;

#endregion

namespace Sage.Integration.Northwind.Adapter.Data
{
    public static class PayloadFactory
    {
        public static PayloadBase CreatePayload(SupportedResourceKinds resource)
        {
            switch (resource)
            {

                case SupportedResourceKinds.tradingAccounts:
                    return new TradingAccountPayload();
                case SupportedResourceKinds.postalAddresses:
                    return new PostalAddressPayload();
                case SupportedResourceKinds.phoneNumbers:
                    return new PhoneNumberPayload();
                case SupportedResourceKinds.emails:
                    return new EmailPayload();
                case SupportedResourceKinds.contacts:
                    return new ContactPayload();

                case SupportedResourceKinds.commodityGroups:
                    return new CommodityGroupPayload();
                case SupportedResourceKinds.unitsOfMeasureGroup:
                    return new UnitOfMeasureGroupPayload();
                case SupportedResourceKinds.unitsOfMeasure:
                    return new UnitOfMeasurePayload();
                case SupportedResourceKinds.commodities:
                    return new CommodityPayload();
                case SupportedResourceKinds.priceLists:
                    return new PriceListPayload();
                case SupportedResourceKinds.prices:
                    return new PricePayload();
                case SupportedResourceKinds.salesOrders:
                    return new SalesOrderPayload();
                case SupportedResourceKinds.salesOrderLines:
                    return new SalesOrderLinePayload();

            }
            return null;
        }

        //public static T CreateTemplate<T>()
        //{
        //    Type type = typeof(T);
        //    object templateObj;

        //    if (type == typeof(Sage.Common.Syndication.ITracking))
        //    {
        //        templateObj = new SyncTracking();
        //        if (templateObj is T == false)
        //            throw new ApplicationException(string.Format("The supplied instance does not implement {0}", type.FullName));

        //        ITracking tracking = (ITracking)templateObj;
        //        tracking.Phase = "unknown";
        //        tracking.PhaseDetail = string.Empty;
        //        tracking.Progress = 0;
        //        tracking.ElapsedSeconds = 0;
        //        tracking.PollingMillis = App.Default.TrackingPayload_PollingMillis;
        //        tracking.RemainingSeconds = 999;
                
        //        return (T)templateObj;
        //    }

        //    throw new InvalidOperationException(string.Format("The supplied type '{0}' is not supported.", type.FullName));
        //}
    }
}
