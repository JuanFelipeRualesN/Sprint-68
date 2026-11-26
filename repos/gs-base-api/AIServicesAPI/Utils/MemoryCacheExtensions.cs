using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace GSAIServicesAPI.Utils
{
    /// <summary>
    /// Cache reads that load a missing value only once, even when many requests ask for it at the same time.
    /// </summary>
    public static class MemoryCacheExtensions
    {
        // The loads running now, by cache key
        private static readonly ConcurrentDictionary<string, Lazy<Task>> Loads = new();

        /// <summary>
        /// To get a value from the cache, or load it when it is missing. The requests that arrive while it loads
        /// wait for that same load. A failed load is not cached, so the next request tries again.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <param name="load">Loads the value. It does not take a request's cancellation, since other requests may be waiting for it.</param>
        /// <param name="duration">How long the value is kept.</param>
        /// <param name="shouldCache">Whether to keep the loaded value</param>
        /// <param name="cancellationToken">Stops this request's wait, not the load.</param>
        /// <returns>Returns the cached or loaded value.</returns>
        public static async Task<T> GetOrLoadOnceAsync<T>(this IMemoryCache cache, string key, Func<Task<T>> load, TimeSpan duration,
            Func<T, bool>? shouldCache = null, CancellationToken cancellationToken = default)
        {
            if (cache.TryGetValue(key, out T? cached) && cached is not null)
                return cached;

            var pending = Loads.GetOrAdd(key, _ => new Lazy<Task>(() => LoadAndCacheAsync(cache, key, load, duration, shouldCache)));
            return await ((Task<T>)pending.Value).WaitAsync(cancellationToken);
        }

        private static async Task<T> LoadAndCacheAsync<T>(IMemoryCache cache, string key, Func<Task<T>> load, TimeSpan duration, Func<T, bool>? shouldCache)
        {
            try
            {
                // Another load may have finished between the caller's cache read and this one starting
                if (cache.TryGetValue(key, out T? cached) && cached is not null)
                    return cached;

                var value = await load();
                if (value is not null && (shouldCache?.Invoke(value) ?? true))
                    cache.Set(key, value, duration);

                return value;
            }
            finally
            {
                Loads.TryRemove(key, out _);
            }
        }
    }
}
