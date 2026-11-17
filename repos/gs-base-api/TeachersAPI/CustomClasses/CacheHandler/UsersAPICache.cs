using System;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using Newtonsoft.Json;
using TeachersAPI.Utils;
using TeachersAPI.Domain.Models.Lessons.Response;
using System;
using CacheHandlerLibrary;

namespace TeachersAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// To handle the cache for external apis.
    /// </summary>
    public class UsersAPICache
    {
        private const string USERS_STRING = "users";
        private const string COLON_STRING = ":";
        private const string USER_STRING = "user";

        /// <summary>
        /// To get the user access details from Redis Cache.
        /// </summary>
        /// <param name="token">The user's access token.</param>
        /// <param name="permission">The permission's name.</param>
        /// <param name="ipAddress">The user's IP Address.</param>
        /// <returns>Returns the Validation Response Model with user details if valid, else, it will return null.</returns>
        public ValidationResponseModel GetUserAccessDetails(string token, string permission, string ipAddress)
        {
            try
            {
                // Use default cache.
                var key = $"{USERS_STRING}{COLON_STRING}{token}{COLON_STRING}{permission}{COLON_STRING}{ipAddress}";
               

                if (!ConfigCache.IsRedisOn())
                {
                    return null;
                }

                ValidationResponseModel accessDetails = null;
                var value = RedisCacheHandler.Read(key);
                if (value is not null)
                    accessDetails = JsonConvert.DeserializeObject<ValidationResponseModel>(value.ToString());

                return accessDetails;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Sets the access details in the cache.
        /// </summary>
        /// <param name="token">The user's access token.</param>
        /// <param name="permission">The permission's name.</param>
        /// <param name="ipAddress">The user's IP Address.</param>
        /// <param name="responseSession">Returns a ValidationResponseModel if not null.</param>
        public void SetAccessDetails(string token, string permission, string ipAddress, ValidationResponseModel responseSession)
        {
            try
            {
                var key = $"{USERS_STRING}{COLON_STRING}{token}{COLON_STRING}{permission}{COLON_STRING}{ipAddress}";
               
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }

                RedisCacheHandler.Write(key,
                    JsonConvert.SerializeObject(responseSession),
                    ConfigCache.GetTokenExpiration());
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// To get if user a can handle user b.
        /// </summary>
        /// <param name="userA">User's id A.</param>
        /// <param name="userB">User's id B.</param>
        /// <returns>Returns a boolean if can be handle or not.</returns>
        public bool GetUserCanHandleUser(string userA, string userB)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return false;
                }
                var value = RedisCacheHandler.Read($"{USER_STRING}{COLON_STRING}{userA}{COLON_STRING}{USER_STRING}{COLON_STRING}{userB}");
                if (value is not null)
                    return JsonConvert.DeserializeObject<bool>(value.ToString()); ;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// To set if user can handle another user. 
        /// </summary>
        /// <param name="userA">User's id A.</param>
        /// <param name="userB">User's id B.</param>
        /// <param name="canBeHandle">A boolean if user a handle user b.</param>
        public void SetUserCanHandleUser(string userA, string userB, bool canBeHandle)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }

                if (canBeHandle) // Only stores the ones that can be handled. Otherwise, do not store them, because some teachers add the student a same day.
                {
                    RedisCacheHandler.Write($"{USER_STRING}{COLON_STRING}{userA}{COLON_STRING}{USER_STRING}{COLON_STRING}{userB}",
                        JsonConvert.SerializeObject(canBeHandle),
                        ConfigCache.GetUserCanHandleUserExpiration());
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        /// To get the user general info in cache.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>The general information.</returns>
        public UserResponse GetUserGeneralInfoInCache(string userId)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return null;
                }
                var value = RedisCacheHandler.Read(GenerateCacheKey(userId));
                if (value is not null)
                    return JsonConvert.DeserializeObject<UserResponse>(value.ToString()); ;
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Set user general info in cache.
        /// </summary>
        /// <param name="userId">The user id to save.</param>
        /// <param name="userResponse">The user's information.</param>
        public void SetUserGeneralInfoInCache(string userId, UserResponse userResponse)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }
                RedisCacheHandler.Write(GenerateCacheKey(userId),
                        JsonConvert.SerializeObject(userResponse),
                        ConfigCache.GetUserGeneralInfoExpiration());
            }
            catch (Exception)
            {
                // ignored
            }

        }

        /// <summary>
        /// To get the user general name in cache
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>The general information.</returns>
        public UserFullNameModel GetUserGeneralNameInCache(string userId)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return null;
                }
                var value = RedisCacheHandler.Read("UserId_Name" + userId);
                if (value is not null)
                    return JsonConvert.DeserializeObject<UserFullNameModel>(value.ToString()); ;
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Set user general name in cache.
        /// </summary>
        /// <param name="userId">The user id to save.</param>
        /// <param name="userResponse">The user's information.</param>
        public void SetUserGeneralNameInCache(string userId, UserFullNameModel userResponse)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }
                RedisCacheHandler.Write("UserId_Name" + userId,
                        JsonConvert.SerializeObject(userResponse),
                        ConfigCache.GetUserGeneralInfoExpiration());
            }
            catch (Exception)
            {
                // ignored
            }

        }

        // Helper method to generate a unique cache key based on user ID
        private static string GenerateCacheKey(string userId)
        {
            // You can create a unique cache key based on the user ID
            return "UserId_" + userId;
        }
    }
}