using System;
using System.Collections.Generic;
using TeachersAPI.Domain.Models.Lessons;
using Newtonsoft.Json;
using CacheHandlerLibrary;

namespace TeachersAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// To handle cache related to quiz types.
    /// </summary>
    public class QuizTypeCache : DefaultCache
    {
        private const string QUIZ_STRING = "quiz";
        private const string COLON_STRING = ":";
        private const string TYPES_STRING = "types";

        /// <summary>
        /// To get the quiz types.
        /// </summary>
        /// <returns>Returns the list of quiz type models.</returns>
        public List<QuizTypeModel> GetQuizTypes()
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
            {
                return null;
            }

            if (!ConfigCache.IsDebugMode())
            {
                var value = Read($"{QUIZ_STRING}{COLON_STRING}{TYPES_STRING}");
                if (!value.IsNullOrEmpty)
                    return JsonConvert.DeserializeObject<List<QuizTypeModel>>(value.ToString());
            }
         

            return null;
        }

        /// <summary>
        /// Sets the quiz types in cache.
        /// </summary>
        /// <param name="quizTypeModelList">Quiz type model list.</param>
        public void SetQuizTypes(List<QuizTypeModel> quizTypeModelList)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
            {
                return;
            }

            var expirationHours = ConfigCache.GetQuizTypesExpiration();

            if (!ConfigCache.IsDebugMode())
            {
                var obtToStore = JsonConvert.SerializeObject(quizTypeModelList);
                Write($"{QUIZ_STRING}{COLON_STRING}{TYPES_STRING}", obtToStore, expirationHours);
            }
           
        }
    }
}