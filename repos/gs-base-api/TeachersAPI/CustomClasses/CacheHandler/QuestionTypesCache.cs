using System.Collections.Generic;
using Newtonsoft.Json;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Utils;
using CacheHandlerLibrary;

namespace TeachersAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// To handle cache related to question types.
    /// </summary>
    public class QuestionTypesCache
    {
        private const string QUESTION_STRING = "question";
        private const string COLON_STRING = ":";
        private const string TYPES_STRING = "types";
        
        /// <summary>
        /// To get the question types.
        /// </summary>
        /// <returns>Returns the list of question type models.</returns>

        public List<QuizQuestionTypesModel> GetQuestionTypes()
        {
            
            if (!ConfigCache.IsRedisOn())
            {
                return null;
            }
            
            var value = RedisCacheHandler.Read($"{QUESTION_STRING}{COLON_STRING}{TYPES_STRING}");
            if (value is not null)
                return JsonConvert.DeserializeObject<List<QuizQuestionTypesModel>>(value.ToString());
            
            return null;
            
        }
        
        /// <summary>
        /// Sets the quiz types in cache.
        /// </summary>
        /// <params name="questionTypeModelList">Question type model list.</params>
        public void SetQuestionTypes(List<QuizQuestionTypesModel> questionTypeModelList)
        {
            if (!ConfigCache.IsRedisOn())
            {
                return;
            }
            RedisCacheHandler.Write($"{QUESTION_STRING}{COLON_STRING}{TYPES_STRING}", 
                JsonConvert.SerializeObject(questionTypeModelList), ConfigCache.GetQuestionTypesExpiration());
        }
        
    }
}