using System;
using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using CacheHandlerLibrary;

namespace GSWebAPI.CustomClasses.CacheHandler
{
    public class AthenasAPICache : DefaultCache
    {
        private const string LESSON_STRING = "lessons";
        private const string COLON_STRING = ":";
        private const string QUIZZES_STRING = "quizzess"; // kept as-is
        private const string QUIZ_STRING = "quiz";
        private const string SUBJECTS_STRING = "subjects";

        private readonly IMemoryCache _memoryCache;

        // Inject IMemoryCache via DI
        public AthenasAPICache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>To get the lesson.</summary>
        public AthenasResponseLessonModel? GetLesson(string lessonId)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return null;

            string key = $"{LESSON_STRING}{COLON_STRING}{lessonId}";

            return _memoryCache.TryGetValue(key, out AthenasResponseLessonModel model) ? model : null;
        }

        /// <summary>To get Athenas' subjects from the cache.</summary>
        public List<SubjectModel>? GetAthenasSubjects()
        {
            try
            {
                if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                    return null;

                string key = SUBJECTS_STRING;


                return _memoryCache.TryGetValue(key, out List<SubjectModel> list) ? list : null;

            }
            catch
            {
                return null;
            }
        }

        /// <summary>To set Athenas' subjects in the cache.</summary>
        public void SetAthenasSubjects(List<SubjectModel> subjectsList)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            int expirationHours = ConfigCache.GetLessonExpiration();
            string key = SUBJECTS_STRING;

            _memoryCache.Set(
                key,
                subjectsList,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                });
        }

        /// <summary>To store the lesson in the cache.</summary>
        public void SetLesson(string lessonId, AthenasResponseLessonModel lessonToStore)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            int expirationHours = ConfigCache.GetLessonExpiration();
            string key = $"{LESSON_STRING}{COLON_STRING}{lessonId}";


            _memoryCache.Set(
                key,
                lessonToStore,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                });
        }


        /// <summary>To get the lesson's quizzess.</summary>
        public LessonQuizzesResponseModel? GetLessonQuizzess(string lessonId)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return null;

            string key = $"{LESSON_STRING}{COLON_STRING}{lessonId}{COLON_STRING}{QUIZZES_STRING}";


            return _memoryCache.TryGetValue(key, out LessonQuizzesResponseModel model) ? model : null;

        }

        /// <summary>To store the quizzess in the cache.</summary>
        public void SetLessonQuizzess(string lessonId, LessonQuizzesResponseModel quizzessToStore)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            int expirationHours = ConfigCache.GetQuizExpiration();
            string key = $"{LESSON_STRING}{COLON_STRING}{lessonId}{COLON_STRING}{QUIZZES_STRING}";

            _memoryCache.Set(
                key,
                quizzessToStore,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                });

        }

        /// <summary>To get the quiz.</summary>
        public AthenasJSONQuizResponseModel? GetQuiz(string quizId)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return null;

            string key = $"{QUIZ_STRING}{COLON_STRING}{quizId}";


            return _memoryCache.TryGetValue(key, out AthenasJSONQuizResponseModel model) ? model : null;

        }

        /// <summary>To store a quiz in the cache.</summary>
        public void SetQuiz(string quizId, AthenasJSONQuizResponseModel quizzessToStore)
        {
            if (AppConfiguration.Configuration["AppSettings:cache_on_off"] == "0")
                return;

            int expirationHours = ConfigCache.GetQuizExpiration();
            string key = $"{QUIZ_STRING}{COLON_STRING}{quizId}";


            _memoryCache.Set(
                key,
                quizzessToStore,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(expirationHours)
                });

        }
    }

}