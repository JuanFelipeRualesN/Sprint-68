using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsAPI.CustomClasses
{
    public class RequestHandler
    {
        private readonly static string configDefaultLanguageCode = AppConfiguration.Configuration["AppSettings:default_language_code"];

        /// <summary>
        /// Gets the language of the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Returns the langCode.</returns>
        public static string GetLanguage(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Accept-Language", out var langValues))
            {
                return langValues.FirstOrDefault();
            }
            else
            {
                return configDefaultLanguageCode;
            }
        }

        /// <summary>
        /// Gets the request token.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>Returns the token if anything.</returns>
        public static string GetToken(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Token", out var tokenValues))
            {
                return tokenValues.FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Gets the API Key.
        /// </summary>
        /// <param name="request">Request.</param>
        /// <returns>Returns the API Key if anything.</returns>
        public static string GetAPIKey(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-API-KEY", out var tokenValues))
            {
                return tokenValues.FirstOrDefault();
            }
            return null;
        }
    }
}
