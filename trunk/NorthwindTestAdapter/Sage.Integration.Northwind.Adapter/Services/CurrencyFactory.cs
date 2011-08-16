#region Usings

using System;
using Sage.Integration.Northwind.Adapter.Feeds;

#endregion

namespace Sage.Integration.Northwind.Adapter.Services
{
    internal class CurrencyConverterFactory
    {
        private const CurrencyCodes DEFAULT_CODE = CurrencyCodes.EUR;

        public static CurrencyCodes[] SupportedCurrencyCodes
        {
            get { return new CurrencyCodes[] { CurrencyCodes.EUR, CurrencyCodes.GBP, CurrencyCodes.USD }; }
        }

        // returns a CurrencyConverter from EUR to currency parsed from computePriceRequest

        public static EuroCurrencyConverter Create(ComputePriceRequestFeedEntry computePriceRequest)
        {
            return Create(computePriceRequest.pricingDocumentCurrency);
        }

        public static EuroCurrencyConverter Create(string pricingDocumentCurrency)
        {
            // Currency
            CurrencyCodes iso4217CurrencyCode = DEFAULT_CODE; // default

            if (string.IsNullOrEmpty(pricingDocumentCurrency))
            {
                iso4217CurrencyCode = DEFAULT_CODE;
            }
            else
            {
                try
                {
                    iso4217CurrencyCode = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), pricingDocumentCurrency);
                }
                catch (ArgumentException)
                {
                    throw new NotSupportedException(string.Format("Currency code {0} in payload not supported or not ISO 4217 conform. Please choose one of following codes: {0}.", CurrencyConverterFactory.SupportedCurrencyCodes.ToString()));
                }
            }

            return new EuroCurrencyConverter(iso4217CurrencyCode);
        }
    }
}
