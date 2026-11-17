using CacheHandlerLibrary;
using StackExchange.Redis;
using System;

namespace CacheHandlerLibrary
{
    public class DefaultCache
    {
        /// <summary>
        /// Reads a value from the cache according to the key.
        /// </summary>
        /// <param name="key">The key to check in the cache.</param>
        /// <returns>Returns a RedisValue.</returns>
        public RedisValue Read(string key)
        {
            try
            {
                var redisValue = new RedisValue();

                //redis cache database can not be accessed from outside aws.
                if (ConfigCache.IsRedisOn())
                {
                    var cache = ConnectorCache.GetDatabase();
                    redisValue = cache.StringGet(key, CommandFlags.DemandReplica);
                }

                return redisValue;
            }
            catch (Exception ex)
            {
                throw new Exception($"there was a problem trying to read key: {key} from cache, error: {ex.Message}, inner exception: {ex.InnerException?.Message}");
            }
        }

        /// <summary>
        /// To store an object in the cache.
        /// </summary>
        /// <param name="key">The key's name.</param>
        /// <param name="value">The value to stored.</param>
        /// <param name="expirationHours">The expiration hours.</param>
        public void Write(string key, string value, int expirationHours)
        {
            try
            {
                //redis cache database can not be accessed from outside aws.
                if (ConfigCache.IsRedisOn())
                {
                    var cache = ConnectorCache.GetDatabase();
                    cache.StringSet(key, value, TimeSpan.FromHours(expirationHours), When.Always, CommandFlags.DemandMaster);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"there was a problem trying to write key: {key} to cache, error: {ex.Message}, inner exception: {ex.InnerException?.Message}");
            }
        }
    }
}