using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Sis.Sdata.Sync.Storage;
using Sage.Integration.Northwind.Adapter.Data.SalesOrders;

namespace Sage.Integration.Northwind.Adapter.Data
{
    public static class EntityWrapperFactory
    {
        public static IEntityWrapper Create(SupportedResourceKinds resourceKind,
            RequestContext context)
        {
            IEntityWrapper result;
            switch (resourceKind)
            {
                case SupportedResourceKinds.tradingAccounts:
                    result = new TradingAccountWrapper(context);
                    return result;

                case SupportedResourceKinds.phoneNumbers:
                    result = new PhoneNumberWrapper(context);
                    return result;
                case SupportedResourceKinds.postalAddresses:
                    
                    result = new PostalAddressWrapper(context);
                    return result;

                case SupportedResourceKinds.contacts:

                    result = new ContactWrapper(context);
                    return result;

                case SupportedResourceKinds.commodityGroups:

                    result = new CommodityGroupWrapper(context);
                    return result;

                case SupportedResourceKinds.commodities:

                    result = new CommodityWrapper(context);
                    return result;

                case SupportedResourceKinds.unitsOfMeasureGroup:

                    result = new UnitOfMeasureGroupWrapper(context);
                    return result;

                case SupportedResourceKinds.unitsOfMeasure:

                    result = new UnitOfMeasureWrapper(context);
                    return result;


                case SupportedResourceKinds.priceLists:

                    result = new PriceListWrapper(context);
                    return result;

                case SupportedResourceKinds.prices:

                    result = new PriceWrapper(context);
                    return result;

                case SupportedResourceKinds.emails:

                    result = new EmailWrapper(context);
                    return result;

                case SupportedResourceKinds.salesOrders:

                    result = new SalesOrderWrapper(context);
                    return result;

                case SupportedResourceKinds.salesOrderLines:

                    result = new SalesOrderLineWrapper(context);
                    return result;

                case SupportedResourceKinds.salesInvoices:

                    result = new SalesInvoicesWrapper(context);
                    return result;

                case SupportedResourceKinds.salesInvoiceLines :

                    result = new SalesInvoiceLineWrapper(context);
                    return result;
                default:
                    throw new NotSupportedException(string.Format("ResourceKind '{0}' not supported.", resourceKind));
            }

        }
    }
}
