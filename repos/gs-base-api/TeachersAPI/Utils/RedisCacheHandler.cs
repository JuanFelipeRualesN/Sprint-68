using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using TeachersAPI;

namespace TeachersAPI.Utils
{
    /// <summary>
    /// To handle redis cache (new class. Previous one is conflicting with Azure).
    /// </summary>
    public static class RedisCacheHandler
    {
        private static readonly string ConnectionString = AppConfiguration.Configuration["AppSettings:redis_cache_endpoints"];

        /// <summary>
        /// To store a value.
        /// </summary>
        /// <param name="key">The key to store.</param>
        /// <param name="value">The value to store.</param>
        /// <param name="expirationHours">How many hours before expiring.</param>
        public static void Write(string key, string value, int expirationHours)
        {
            try
            {
                // Connect to Redis Cache
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConnectionString);

                // Get a reference to the Redis database
                IDatabase cache = redis.GetDatabase();

                // Store a value in the cache
                cache.StringSet(key, value, TimeSpan.FromHours(expirationHours));
            }
            catch (Exception)
            {
                // pass
            }
        }

        /// <summary>
        /// To read the a value by key.
        /// </summary>
        /// <param name="key">The key to read.</param>
        /// <returns>Returns the value by key.</returns>
        public static string Read(string key)
        {
            try
            {
                // Connect to Redis Cache
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConnectionString);

                // Get a reference to the Redis database
                IDatabase cache = redis.GetDatabase();
                return cache.StringGet(key);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// To delete cache by key.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void Delete(string key)
        {
            try
            {
                // Connect to Redis Cache
                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(ConnectionString);

                // Get a reference to the Redis database
                IDatabase cache = redis.GetDatabase();
                cache.KeyDelete(key);
            }
            catch (Exception)
            {
                // pass
            }
        }
    }
}