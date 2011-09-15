using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Data;

namespace Sage.Integration.Northwind.Adapter.Data
{
    class FeedEntryWrapperFactory
    {
        public static IFeedEntryEntityWrapper Create(SupportedResourceKinds resourceKind,
           RequestContext context)
        {
            IFeedEntryEntityWrapper result;
            switch (resourceKind)
            {
                case SupportedResourceKinds.contacts:
                    result = new ContactFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.tradingAccounts:
                    result = new TradingAccountsFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.postalAddresses:
                    result = new PostalAddressFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.salesOrders:
                    result = new SalesOrderFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.phoneNumbers:
                    result = new PhoneNumberFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.emails:
                    result = new EmailFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.commodityGroups:
                    result = new CommodityGroupFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.commodities:
                    result = new CommodityFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.unitsOfMeasure:
                    result = new UnitOfMeasureFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.priceLists:
                    result = new PriceListFeedEntryWrapper(context);
                    return result;

                case SupportedResourceKinds.prices:
                    return new PriceFeedEntryWrapper(context);

               // case SupportedResourceKinds.salesInvoices:
                 //   return new SalesInvoiceFeedEntryWrapper(context);

                default:
                    throw new InvalidOperationException("Resource Kind not supported.");
            }
        }
    }
}
