using System;
using System.Collections.Generic;
using CacheHandlerLibrary;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using Newtonsoft.Json;

namespace TeachersAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// To handle cache related to subscriptions.
    /// </summary>
    public class SubscriptionsAPICache : DefaultCache
    {
        private const string STUDENT_STRING = "student";
        private const string COLON_STRING = ":";
        private const string COURSES_STRING = "courses";

        /// <summary>
        /// To get the student courses from the cache.
        /// </summary>
        /// <param name="token">The student's token.</param>
        /// <returns>Returns the student courses response.</returns>
        public StudentCoursesResponseModel GetStudentCourses(string token)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
            {
                return null;
            }

            if (!ConfigCache.IsDebugMode())
            {
                var value = Read($"{STUDENT_STRING}{COLON_STRING}{token}{COLON_STRING}{COURSES_STRING}");
                if (!value.IsNullOrEmpty)
                    return JsonConvert.DeserializeObject<StudentCoursesResponseModel>(value.ToString());
                else
                    return null;
            }
            return null;
        }

        /// <summary>
        /// Saves the student courses in the cache.
        /// </summary>
        /// <param name="token">The student's token.</param>
        /// <param name="studentCourses">The student's courses.</param>
        public void SetStudentCourses(string token, StudentCoursesResponseModel studentCourses)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
            {
                return;
            }

            var expirationHours = ConfigCache.GetStudentCoursesExpiration();

            if (!ConfigCache.IsDebugMode())
            {
                var obtToStore = JsonConvert.SerializeObject(studentCourses);
                Write($"{STUDENT_STRING}{COLON_STRING}{token}{COLON_STRING}{COURSES_STRING}", obtToStore, expirationHours);
            }
           
        }
    }
}