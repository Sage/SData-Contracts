#region Usings

using System;
using System.Collections.Generic;
using Sage.Integration.Northwind.Adapter.Common;
using Sage.Integration.Northwind.Adapter.Transformations.SalesOrders;
using Sage.Integration.Northwind.Adapter.Transformations.TradingAccounts;
using Sage.Sis.Sdata.Sync.Storage;

#endregion

namespace Sage.Integration.Northwind.Adapter.Transformations
{
    public static class TransformationFactory
    {
        private static Dictionary<string, ITransformation> stat_transformations = new Dictionary<string, ITransformation>();

        public static T GetTransformation<T>(SupportedResourceKinds resourceKind, 
            RequestContext context)
        {
            string endpoint = context.DatasetLink + resourceKind.ToString();
            if (stat_transformations.ContainsKey(endpoint))
                return (T)stat_transformations[endpoint];

            // create new transformation instance
            ITransformation transformation = null;
            
            switch(resourceKind)
            {
                case SupportedResourceKinds.tradingAccounts:
                    transformation = new TradingAccountTransformation(context);
                    break;
                case SupportedResourceKinds.emails:
                    transformation = new EmailAdressTransformation(context);
                    break;
                case SupportedResourceKinds.contacts:
                    transformation = new ContactTransformation(context);
                    break;
                case SupportedResourceKinds.phoneNumbers:
                    transformation = new PhoneNumberTransformation(context);
                    break;
                case SupportedResourceKinds.postalAddresses:
                    transformation = new PostalAdressTransformation(context);
                    break;

                case SupportedResourceKinds.commodityGroups:
                    transformation = new CommodityGroupTransformation(context);
                    break;
                case SupportedResourceKinds.unitsOfMeasureGroup:
                    transformation = new UnitOfMeasureGroupTransformation(context);
                    break;
                case SupportedResourceKinds.unitsOfMeasure:
                    transformation = new UnitOfMeasureTransformation(context);
                    break;
                case SupportedResourceKinds.commodities:
                    transformation = new CommodityTransformation(context);
                    break;
                case SupportedResourceKinds.priceLists:
                    transformation = new PriceListTransformation(context);
                    break;
                case SupportedResourceKinds.prices:
                    transformation = new PriceTransformation(context);
                    break;
                case SupportedResourceKinds.salesOrders:
                    transformation = new SalesOrderTransformation(context);
                    break;
                case SupportedResourceKinds.salesOrderLines:
                    transformation = new SalesOrderLineTransformation(context);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("ResourceKind {0} not supported.", resourceKind.ToString()));

            }

            T resultTransformation = (T)transformation;

            if (transformation != null)
                stat_transformations.Add(endpoint, transformation);

            return resultTransformation;
        }
    }
}
