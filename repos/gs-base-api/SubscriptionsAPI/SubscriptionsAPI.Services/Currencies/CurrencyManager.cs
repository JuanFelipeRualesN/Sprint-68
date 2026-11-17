using System.Collections.Generic;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Currencies;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Currencies;

namespace SubscriptionsAPI.SubscriptionsAPI.Services.Currencies
{
    /// <summary>
    /// To handle the currencies. The currencies are used when creating plans. 
    /// Example of currencies: USD. 
    /// </summary>
    public class CurrencyManager
    {
        private readonly string _connectionStringRead;
        private readonly string _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (read).</param>
        /// <param name="langCode">The language code.</param>
        public CurrencyManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Return the available currencies.
        /// </summary>
        /// <returns>Returns the currency model list.</returns>
        public List<CurrencyModel> GetAvailableCurrencies()
        {
            return new CurrencyDAO().Read(_connectionStringRead, new CurrencyModel() { Available = "1" }, false, false);
        }

        /// <summary>
        /// Returns the currency model details by the currency iso.
        /// </summary>
        /// <param name="iso">The iso code of the currency.</param>
        /// <returns>Returns null if nothing was found. Else, return the currency details of the iso requested.</returns>
        public CurrencyModel GetCurrencyDetailsByCurrencyIso(string iso)
        {
            // The currency to find
            List<CurrencyModel> currencyModelResult = new CurrencyDAO().Read(_connectionStringRead, new CurrencyModel
            {
                IsoCode = iso
            }, true, true);
            if (currencyModelResult.Count == 0)
                return null;
            else
                return currencyModelResult[0];
        }
    }
}
