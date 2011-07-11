#region Usings

using System;
using Sage.Integration.Northwind.Feeds;

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
        public static EuroCurrencyConverter Create(computePriceRequesttype computePriceRequest)
        {
            // Currency
            CurrencyCodes iso4217CurrencyCode = DEFAULT_CODE; // default

            if (string.IsNullOrEmpty(computePriceRequest.pricingDocumentCurrency))
            {
                iso4217CurrencyCode = DEFAULT_CODE;
            }
            else
            {
                try
                {
                    iso4217CurrencyCode = (CurrencyCodes)Enum.Parse(typeof(CurrencyCodes), computePriceRequest.pricingDocumentCurrency);
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
