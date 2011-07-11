#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Northwind.Feeds.TradingAccounts;
using Sage.Integration.Northwind.Feeds.SalesOrders;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{

   
    public static class ResourceKindHelpers
    {


        public static string GetSingleResourceUrl(string baseUrl, string payloadpath, string id)
        {
            if (!baseUrl.EndsWith("/"))
                baseUrl = baseUrl + "/";
            SupportedResourceKinds resKind = GetResourceKind(payloadpath);

            return String.Format("{0}{1}('{2}')", baseUrl, resKind.ToString(), id);
        }

        public static SupportedResourceKinds GetResourceKind(string payloadPath)
        {
            string reskind = payloadPath;
            if (reskind.IndexOf("/") > 0)
            {
                reskind = reskind.Substring(reskind.LastIndexOf("/") + 1);
            }

            if (reskind.IndexOf("[") > 0)
            {
                reskind = reskind.Substring(0,reskind.IndexOf("["));
            }
            if (reskind.Equals("commodity", StringComparison.InvariantCultureIgnoreCase))
                return SupportedResourceKinds.commodities;

            if (reskind.Equals("unitOfMeasure", StringComparison.InvariantCultureIgnoreCase))
                return SupportedResourceKinds.unitsOfMeasure;

            if (reskind.Equals("salesOrderLines", StringComparison.InvariantCultureIgnoreCase))
                return SupportedResourceKinds.salesOrderLines;


            SupportedResourceKinds result = SupportedResourceKinds.None;

            try
            {
                object obj = Enum.Parse(typeof(SupportedResourceKinds), reskind + "s", true);
                if (obj is SupportedResourceKinds)
                    result = (SupportedResourceKinds)obj;
            }
            catch (Exception)
            {
            }

            if (result != SupportedResourceKinds.None)
                return result;

            
            try
            {
                object obj = Enum.Parse(typeof(SupportedResourceKinds), reskind, true);
                if (obj is SupportedResourceKinds)
                    result = (SupportedResourceKinds)obj;
            }
            catch (Exception)
            {
            }

            return result;

        }
        /// <summary>
        /// Returns a dictionary mapping all SupportedResourceKinds with their Payload types
        /// </summary>
        /// <returns></returns>
        public static Dictionary<SupportedResourceKinds, Type> GetAllResourcePayloadTypes()
        {
            Dictionary<SupportedResourceKinds, Type> types = new Dictionary<SupportedResourceKinds, Type>();
            foreach (SupportedResourceKinds supportedResourceKinds in Enum.GetValues(typeof(SupportedResourceKinds)))
            {
                Type t = GetPayloadType(supportedResourceKinds);

                if (null != t)
                    types.Add(supportedResourceKinds, t);
            }

            return types;
        }

        /// <summary>
        /// Returns the payload type for a given resource kind type
        /// </summary>
        /// <param name="resourceKind"></param>
        /// <returns></returns>
        public static Type GetPayloadType(SupportedResourceKinds resourceKind)
        {
            switch (resourceKind)
            {
                case SupportedResourceKinds.contacts:
                    return typeof(ContactPayload);
                case SupportedResourceKinds.emails:
                    return typeof(EmailPayload);
                case SupportedResourceKinds.phoneNumbers:
                    return typeof(PhoneNumberPayload);
                case SupportedResourceKinds.postalAddresses:
                    return typeof(PostalAddressPayload);
                case SupportedResourceKinds.tradingAccounts:
                    return typeof(TradingAccountPayload);

                case SupportedResourceKinds.commodityGroups:
                    return typeof(CommodityGroupPayload);
                case SupportedResourceKinds.unitsOfMeasureGroup:
                    return typeof(UnitOfMeasureGroupPayload);
                case SupportedResourceKinds.unitsOfMeasure:
                    return typeof(UnitOfMeasurePayload);
                case SupportedResourceKinds.commodities:
                    return typeof(CommodityPayload);
                case SupportedResourceKinds.priceLists:
                    return typeof(PriceListPayload);
                case SupportedResourceKinds.prices:
                    return typeof(PricePayload);
                case SupportedResourceKinds.salesOrders:
                    return typeof(SalesOrderPayload);
                case SupportedResourceKinds.salesOrderLines:
                    return typeof(SalesOrderLinePayload);

                case SupportedResourceKinds.salesInvoices:
                    return typeof(SalesInvoicePayload);

                case SupportedResourceKinds.salesInvoiceLines:
                    return typeof(SalesInvoiceLinePayload);
            }

            return null;
        }
    }
}
