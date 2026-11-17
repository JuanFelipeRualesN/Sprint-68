using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using Microsoft.Extensions.Caching.Memory;
using CacheHandlerLibrary;

namespace GSWebAPI.CustomClasses.CacheHandler
{
    public class AddressAPICache : DefaultCache
    {
        private const string COUNTRIES_STRING = "countries";
        private const string COLON_STRING = ":";

        private readonly IMemoryCache _memoryCache;

        // Inject IMemoryCache via DI
        public AddressAPICache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// To get the countries iso.
        /// </summary>
        /// <param name="isoList">The iso list.</param>
        /// <returns>Returns the response iso model.</returns>
        public List<ResponseISOModel> GetCountriesByISO(List<string> isoList)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return new List<ResponseISOModel>();

            var responseIsoModelList = new List<ResponseISOModel>();

            foreach (string iso in isoList)
            {
                var key = $"{COUNTRIES_STRING}{COLON_STRING}{iso}";

                // Debug mode: use in-process IMemoryCache instead of HttpContext.Current.Cache
                if (_memoryCache.TryGetValue(key, out ResponseISOModel cached))
                {
                    responseIsoModelList.Add(cached);
                }
            }

            return responseIsoModelList;
        }

        /// <summary>
        /// Sets the countries details by ISO in Cache.
        /// </summary>
        /// <param name="responseISOModels">The response Isos to store.</param>
        public void SetCountriesByISO(List<ResponseISOModel> responseISOModels)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            var expirationHours = ConfigCache.GetCountriesExpiration();

            foreach (var response in responseISOModels)
            {
                var key = $"{COUNTRIES_STRING}{COLON_STRING}{response.IsoCode2}";
                // Debug mode: IMemoryCache with absolute expiration
                if (!_memoryCache.TryGetValue(key, out _))
                {
                    _memoryCache.Set(
                        key,
                        response,
                        new MemoryCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                        }
                    );
                }
            }
        }
    }
}