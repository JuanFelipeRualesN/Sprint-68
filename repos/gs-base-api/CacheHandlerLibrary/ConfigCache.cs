using System.Configuration;

namespace CacheHandlerLibrary
{
    /// <summary>
    /// To handle details related to cache.
    /// </summary>
    public class ConfigCache
    {
        private readonly static string configRedisCacheOnOff = AppConfiguration.Configuration["AppSettings:redis_cache_on"];
        private readonly static string configRedisCacheEndpoints = AppConfiguration.Configuration["AppSettings:redis_cache_endpoints"];
        private readonly static string configRedisCacheTokenTimeExpiration = AppConfiguration.Configuration["AppSettings:redis_cache_token_time_expiration"];
        private readonly static string configRedisCacheFileAttachmentRulesExpiration = AppConfiguration.Configuration["AppSettings:redis_cache_file_attachment_rules_expiration"];
        private readonly static string configRedisCountries = AppConfiguration.Configuration["AppSettings:redis_cache_countries_time_expiration"];
        private readonly static string configRedisLessonsExpiration = AppConfiguration.Configuration["AppSettings:redis_cache_lessons_time_expiration"];
        private readonly static string configRedisQuizExpiration = AppConfiguration.Configuration["AppSettings:redis_cache_quiz_time_expiration"];
        private readonly static string configRedisSubscriptionProfilesTry = AppConfiguration.Configuration["AppSettings:redis_cache_subscription_profiles_try_time_expiration"];



        public static int GetCountriesExpiration()
        {
            return int.Parse(configRedisCountries);
        }

    

        public static int GetSubscriptionProfilesTry()
        {
            return int.Parse(configRedisSubscriptionProfilesTry);
        }



        public static int GetVersionsExpiration()
        {
            // Default to 24 hours if not set
            return 24;
        }


        /// <summary>
        /// To get the subscription details expiration hours.
        /// </summary>
        /// <returns>Returns an int of the hours that the iso code will expire.</returns>
        public static int GetSubscriptionDetailsExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_subscription_details_time_expiration"]);
        }

        /// <summary>
        /// To get the country iso expiration.
        /// </summary>
        /// <returns>Returns an int of the hours that the iso code will expire.</returns>
        public static int GetCountryISOCodeExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_iso_code_time_expiration"]);
        }

        /// <summary>
        /// To get the school id according to subscription expiration time.
        /// </summary>
        /// <returns>Returns an int of the hours.</returns>
        public static int GetSchoolIdOfSubscriptionExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_school_id_subscription_time_expiration"]);
        }

        /// <summary>
        /// To get ORE school expirtation time in the cache.
        /// </summary>
        /// <returns>Return an int of the hourse.</returns>
        public static int GetOreSchoolsExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_ore_schools_expiration"]);
        }

        /// <summary>
        /// To get the entity details expiration time.
        /// </summary>
        /// <returns>Returns an int of the hours.</returns>
        public static int GetEntityDetailsExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_entity_details_time_expiration"]);
        }


        /// <summary>
        /// To check if is in Debug Mode. For Local debug, this has to be true.
        /// </summary>
        /// <returns>Returns True if debug is on, otherwise false.</returns>
        public static bool IsDebugMode()
        {
            return bool.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_debug_on"]);
        }

        /// <summary>
        /// To check if is in Debug Mode. For Local debug, this has to be true.
        /// </summary>
        /// <returns>Returns True if debug is on, otherwise false.</returns>
        public static bool IsRedisOn()
        {
            return bool.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_on"]);
        }

        /// <summary>
        /// To get the cache endpoints configured with AWS.
        /// </summary>
        /// <returns></returns>
        public static string GetEndpoints()
        {
            return AppConfiguration.Configuration["AppSettings:redis_cache_endpoints"];
        }

        /// <summary>
        /// To get the lesson expiration.
        /// </summary>
        /// <returns>Returns the expiration time of the lessons.</returns>
        public static int GetLessonExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_lesson_time_expiration"]);
        }

        /// <summary>
        /// To get the quiz expiration hours.
        /// </summary>
        /// <returns>Returns an int of the hours that the quiz will expire in the cache. After expiring, it will check in the endpoint again.</returns>
        public static int GetQuizExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_quiz_time_expiration"]);
        }

        /// <summary>
        /// To get the token time expiration hours.
        /// </summary>
        /// <returns>Returns an int of the hours that the token will expire in the cache. After expiring, it will check the DB again and if valid, it will be stored in the cache again.</returns>
        public static int GetTokenExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_token_time_expiration"]);
        }

        public static int GetStudentCoursesExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_student_courses_time_expiration"]);
        }

        /// <summary>
        /// To get the file rules expiration hours.
        /// </summary>
        /// <returns>Returns an int of the hours that the file attachment rules will expire.</returns>
        public static int GetFileAttachmentsRulesExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_file_attachment_rules_expiration"]);
        }

        /// <summary>
        /// To get the file types expiration hours.
        /// </summary>
        /// <returns>Returns an int of the hours that the file attachment rules will expire.</returns>
        public static int GetQuizTypesExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_quiz_types_expiration"]);
        }

        /// <summary>
        /// To get the question types expitation hours.
        /// </summary>
        /// <returns>Return an int of the hours that the question type will expire. </returns>
        public static int GetQuestionTypesExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_quiz_question_types_expiration"]);
        }

        /// <summary>
        /// To get the lesson expiration hours.
        /// </summary>
        /// <returns>Returns an int of the hours that the lesson models will expire.</returns>
        public static int GetLessonModelsExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_lesson_model_expiration"]);
        }

        /// <summary>
        /// To get the lesson's pdf expiration hours.
        /// </summary>
        /// <returns>Returns an int of the hours that the lesson models will expire.</returns>
        public static int GetLessonPDFExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_lesson_pdf_expiration"]);
        }

        /// <summary>
        /// To get the user handle user expiration hours.
        /// </summary>
        /// <returns>Returns an int of the hours that the user-handle-user will expire.</returns>
        public static int GetUserCanHandleUserExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_user_handle_user_expiration"]);
        }

        /// <summary>
        /// To get the subject list expiration hours.
        /// </summary>
        /// <returns>Returns an int of the hours that the subject list will expire.</returns>
        public static int GetAthenasSubjectsExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_subjects_list_expiration"]);
        }

        /// <summary>
        /// To get the user general information expiration hours.
        /// </summary>
        /// <returns>Returns an int of hours. </returns>
        public static int GetUserGeneralInfoExpiration()
        {
            return int.Parse(AppConfiguration.Configuration["AppSettings:redis_cache_user_general_info_expiration"]);
        }
    }
}