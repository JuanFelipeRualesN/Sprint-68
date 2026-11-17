using System.Collections.Generic;
using System.Configuration;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using SchoolsAPI.SchoolsAPI.Services.Schools;
using SchoolsAPI.CustomClasses;

namespace SchoolsAPI.SchoolsAPI.Services.Generals
{
    /// <summary>
    /// To handle the API keys validations.
    /// </summary>
    public static class ValidateAPIKey
    {
        /// <summary>
        /// To validate an API key.
        /// </summary>
        /// <param name="apiKey">The API Key to validate.</param>
        /// <param name="langCode">The language code of the user to return an error message if the API key does not exist.</param>
        /// <returns>Returns the API Key validation.</returns>
        public static ResponseModel ValidateKey(string apiKey, string langCode)
        {

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return new ResponseModel()
                {
                    Valid = false,
                };
            }

            var schoolsReader = new SchoolsReader(
                         Settings.CONNECTION_STRING_READ,
                         langCode);

            var messageSchools = new MessageManager(Settings.CONNECTION_STRING_READ, langCode);

            var platformModelListToFind = schoolsReader.GetPlatformModels(
                new List<PlatformModel>()
                {
                            new PlatformModel()
                            {
                                Code = apiKey,
                            },
                });

            if (platformModelListToFind == null || platformModelListToFind.Count == 0)
            {
                string errorMsg = messageSchools.GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_schools_not_platform_id"]);
                return
                    new ResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                                    errorMsg,
                        },
                    };
            }
            else
            {
                return new ResponseModel()
                {
                    Valid = true,
                };
            }
        }
    }
}