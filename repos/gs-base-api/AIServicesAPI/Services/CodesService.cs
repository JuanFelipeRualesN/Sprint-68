using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.Helpers;
using Microsoft.Extensions.Caching.Memory;

namespace GSAIServicesAPI.Services
{
    /// <summary>
    /// Retrieves paginated <see cref="CodeHandleModel"/> collections from the Athenas API
    /// and caches the results in memory to avoid redundant upstream calls.
    /// </summary>
    public class CodesService
    {
        private readonly AthenasFeignAPI _athenasFeignAPI;
        private readonly IMemoryCache    _cache;
        private readonly ILogger<CodesService> _logger;

        // How long a codes result set stays fresh in cache.
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

        public const int StandardTypeId = 1;

        public CodesService(
            AthenasFeignAPI athenasFeignAPI,
            IMemoryCache cache,
            ILogger<CodesService> logger)
        {
            _athenasFeignAPI = athenasFeignAPI;
            _cache           = cache;
            _logger          = logger;
        }

        /// <summary>
        /// Returns a page of codes of one type (standards, expectations…) filtered by subject and level codes.
        /// Results are served from cache when available; otherwise fetched from Athenas and cached.
        /// </summary>
        public async Task<PaginationModel<CodeHandleModel>> GetCodesAsync(
            List<string> subjectCodes,
            List<string> levelCodes,
            int fromPage          = 0,
            int limiter           = 100,
            bool uniqueExpectations = false,
            string langCode       = "en",
            int typeId            = StandardTypeId)
        {
            var cacheKey = CacheKeys.Codes(subjectCodes, levelCodes, fromPage, limiter, uniqueExpectations, langCode, typeId);

            if (_cache.TryGetValue(cacheKey, out PaginationModel<CodeHandleModel>? cached) && cached is not null)
            {
                _logger.LogDebug("Cache hit for codes key: {Key}", cacheKey);
                return cached;
            }

            _logger.LogInformation("Cache miss – fetching codes from Athenas. Key: {Key}", cacheKey);

            // Build a CodeHandleModel that acts as a filter.
            // The Athenas API accepts nested subject/level lists inside the body model.
            var filterModel = new CodeHandleModel
            {
                CodeSubjectModels = subjectCodes
                    .Select(sc => new CodeSubjectModel { SubjectCode = sc })
                    .ToList(),

                CodeLevelModels = levelCodes
                    .Select(lc => new CodeLevelModel { LevelCode = lc })
                    .ToList(),
                TypeId = typeId.ToString()
            };

            var result = await _athenasFeignAPI.GetCodesAsync(
                filterModel, fromPage, limiter, uniqueExpectations, langCode);

            // Only cache meaningful responses; empty pages fall through every time.
            if (result.Items?.Count > 0)
            {
                _cache.Set(cacheKey, result, CacheDuration);
                _logger.LogInformation("Cached {Count} code(s) under key: {Key}", result.Items.Count, cacheKey);
            }

            return result;
        }
    }
}
