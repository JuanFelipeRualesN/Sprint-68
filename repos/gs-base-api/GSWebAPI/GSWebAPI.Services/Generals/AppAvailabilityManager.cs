using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Infrastructure.Repositories.Generals;

namespace GSWebAPI.GSWebAPI.Services.Generals
{
    /// <summary>
    /// Handles APP availability.
    /// </summary>
    public class AppAvailabilityManager
    {
        private readonly string _connectionString;
        private readonly string _connectionStringRead;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppAvailabilityManager"/> class.
        /// To set the generics as connection string.
        /// </summary>
        /// <param name="connectionString">The connections tring.</param>
        /// <param name="connectionStringRead">The connection string read.</param>
        public AppAvailabilityManager(string connectionString, string connectionStringRead)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
        }

        /// <summary>
        /// Get the list of available country with their ISOs.
        /// </summary>
        /// <returns>Returns the list of country isos where the app is available.</returns>
        public List<CountryAbleModel> GetAvailableCountryIsos()
        {
            return new CountryAbleDAO().Read(_connectionString, new CountryAbleModel(), false, false);
        }

        /// <summary>
        /// Returns true if the app is available for the country, else, returns false.
        /// </summary>
        /// <param name="countryIso2">The country ISO code 2.</param>
        /// <returns>True if the app is available for the country, else false.</returns>
        public bool CheckIfAppAvailableForCountry(string countryIso2)
        {
            CountryAbleModel countryAbleModel = new CountryAbleModel
            {
                CountryIsoCode2 = countryIso2,
            };
            return new CountryAbleDAO().Read(_connectionStringRead, countryAbleModel, true, true).Count > 0;
        }
    }
}