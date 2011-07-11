#region Usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace Sage.Integration.Northwind.Adapter.Services
{
    public enum CurrencyCodes { EUR, GBP, USD }

    public class EuroCurrencyConverter
    {
        private Dictionary<CurrencyCodes, decimal> _currencyExchangerRates = new Dictionary<CurrencyCodes, decimal>();
        private DateTime stat_CurrencyExchangeRateDate = new DateTime(2009, 6, 29);

        #region Properties

        public string Code { get; private set; }
        public decimal ExchangeRate { get; private set; }
        public DateTime ExchangeRateDate { get; private set; }

        #endregion

        #region Ctor.

        public EuroCurrencyConverter(CurrencyCodes targetCurrencyCode)
        {
            this.Initialize(targetCurrencyCode);
        }

        #endregion

        protected virtual void Initialize(CurrencyCodes targetCurrencyCode)
        {
            // initialize currency
            _currencyExchangerRates.Add(CurrencyCodes.EUR, 1);
            _currencyExchangerRates.Add(CurrencyCodes.GBP, new decimal(0.84));
            _currencyExchangerRates.Add(CurrencyCodes.USD, new decimal(1.40));

            if (!_currencyExchangerRates.ContainsKey(targetCurrencyCode))
                throw new NotSupportedException(string.Format("Currency code {0} in payload not supported or not ISO 4217 conform. Please chose one of following codes: {0}.", targetCurrencyCode.ToString(), _currencyExchangerRates.Keys.ToString()));

            this.Code = targetCurrencyCode.ToString();
            this.ExchangeRate = _currencyExchangerRates[targetCurrencyCode];
        }

        public virtual decimal Convert(decimal baseValue)
        {
            return baseValue * this.ExchangeRate;
        }
    }
}
