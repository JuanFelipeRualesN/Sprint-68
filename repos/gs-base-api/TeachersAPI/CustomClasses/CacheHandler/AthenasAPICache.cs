using System;
using System.Collections.Generic;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses.APIs.Athenas;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Lessons.Response;
using TeachersAPI.Utils;
using Newtonsoft.Json;
using CacheHandlerLibrary;

namespace TeachersAPI.CustomClasses.CacheHandler
{
    /// <summary>
    /// To handle cache related to the system that handles the lessons and subjects.
    /// </summary>
    public class AthenasAPICache
    {
        private const string LESSON_STRING = "lessons";
        private const string COLON_STRING = ":";
        private const string MODEL_STRING = "model";
        private const string PDF_STRING = "pdf";
        private const string PDF_STUDENT = "student";
        private const string SUBJECTS_STRING = "subjects";
        private const string LIST_STRING = "list";
        private const string QUIZZES_STRING = "quizzess";
        private const string LESSON_SINGLE_STRING = "lesson";
        private const string LESSON_HTML_STRING = "html";
      
        /// <summary>
        /// To get the lesson's quizzess.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the lesson object.</returns>
        public LessonQuizzesResponseModel GetLessonQuizzess(string lessonId)
        {
            // TODO: Enable again when needed
           
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return null;
                }

                var value = RedisCacheHandler.Read($"{LESSON_STRING}{COLON_STRING}{lessonId}{COLON_STRING}{QUIZZES_STRING}");
                if (value is not null)
                {
                    return JsonConvert.DeserializeObject<LessonQuizzesResponseModel>(value.ToString());
                }
                else
                {
                    return null;
                } 
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        /// <summary>
        /// To store the quizzess in the cache.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="quizzessToStore">The quizzes to store.</param>
        public void SetLessonQuizzess(string lessonId, LessonQuizzesResponseModel quizzessToStore)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }
                RedisCacheHandler.Write($"{LESSON_STRING}{COLON_STRING}{lessonId}{COLON_STRING}{QUIZZES_STRING}",
                    JsonConvert.SerializeObject(quizzessToStore),
                    ConfigCache.GetQuizExpiration());
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// To get the lesson models from the cache by lessons' models that should include the lesson's id.
        /// </summary>
        /// <param name="lessonIds">The lesson model with the ids.</param>
        /// <returns>Return the lesson models that are in the cache.</returns>
        public List<LessonModel> GetLessonModels(List<LessonModel> lessonIds)
        {
            return new List<LessonModel>();
          
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return new List<LessonModel>();
                }

                List<LessonModel> lessonsInCache = new List<LessonModel>();

                foreach (LessonModel lesson in lessonIds)
                {
                    var value = RedisCacheHandler.Read($"{LESSON_STRING}{COLON_STRING}{MODEL_STRING}{COLON_STRING}{lesson.Id}");
                    if (value is not null)
                    {
                        lessonsInCache.Add(JsonConvert.DeserializeObject<LessonModel>(value.ToString()));
                    }
                }

                return lessonsInCache;
            }
            catch (Exception)
            {
                return new List<LessonModel>();
            }
        }

        /// <summary>
        /// Sets the lesson models in the cache.
        /// </summary>
        /// <param name="lessons">Lessons model list.</param>
        public void SetLessonModels(List<LessonModel> lessons)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }

                var expirationHours = ConfigCache.GetLessonModelsExpiration();

                foreach (LessonModel lesson in lessons)
                {
                    // Read first 
                    var value = RedisCacheHandler.Read($"{LESSON_STRING}{COLON_STRING}{MODEL_STRING}{COLON_STRING}{lesson.Id}");
                    if (value is null)
                    {
                        var obtToStore = JsonConvert.SerializeObject(lesson);
                        RedisCacheHandler.Write($"{LESSON_STRING}{COLON_STRING}{MODEL_STRING}{COLON_STRING}{lesson.Id}", obtToStore, expirationHours);
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// To get the lesson PDF location for teachers in the cache.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the location (a string) </returns>
        public string GetLessonPDFTeacherLocation(string lessonId)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return null;
                }

                var value = RedisCacheHandler.Read($"{LESSON_STRING}{COLON_STRING}{PDF_STRING}{COLON_STRING}{lessonId}");
                if (value is not null)
                    return JsonConvert.DeserializeObject<string>(value.ToString());
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// To store the lesson's PDF location in the cache.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="pdfLocation">The PDF's location.</param>
        public void SetLessonPDFTeacherLocation(string lessonId, string pdfLocation)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }

                RedisCacheHandler.Write($"{LESSON_STRING}{COLON_STRING}{PDF_STRING}{COLON_STRING}{lessonId}",
                    JsonConvert.SerializeObject(pdfLocation), ConfigCache.GetLessonPDFExpiration());
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// To get the lesson PDF location for students in the cache.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <returns>Returns the location (a string) </returns>
        public string GetLessonPDFStudentLocation(string lessonId)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return null;
                }
                var value = RedisCacheHandler.Read($"{LESSON_STRING}{COLON_STRING}{PDF_STRING}{COLON_STRING}{lessonId}{COLON_STRING}{PDF_STUDENT}");
                if (value is not null)
                    return JsonConvert.DeserializeObject<string>(value.ToString());
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// To store the lesson's PDF location for students in the cache.
        /// </summary>
        /// <param name="lessonId">The lesson's id.</param>
        /// <param name="pdfLocation">The PDF's location.</param>
        public void SetLessonPDFStudentLocation(string lessonId, string pdfLocation)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }

                RedisCacheHandler.Write($"{LESSON_STRING}{COLON_STRING}{PDF_STRING}{COLON_STRING}{lessonId}{COLON_STRING}{PDF_STUDENT}",
                    JsonConvert.SerializeObject(pdfLocation),
                    ConfigCache.GetLessonPDFExpiration());
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// To get Athenas' subjects from the cache.
        /// </summary>
        /// <returns>Returns the list of subjects. </returns>
        public List<SubjectModel> GetAthenasSubjects()
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return null;
                }

                var value = RedisCacheHandler.Read($"{SUBJECTS_STRING}{COLON_STRING}{LIST_STRING}");
                if (value is not null)
                    return JsonConvert.DeserializeObject<List<SubjectModel>>(value.ToString());

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// To set Athenas' subjects in the cache.
        /// </summary>
        /// <param name="subjectsList">Set the subject.</param>
        public void SetAthenasSubjects(List<SubjectModel> subjectsList)
        {
            try
            {
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }

                RedisCacheHandler.Write($"{SUBJECTS_STRING}{COLON_STRING}{LIST_STRING}",
                    JsonConvert.SerializeObject(subjectsList), ConfigCache.GetAthenasSubjectsExpiration());
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// To get Athenas lesson from the cache.
        /// </summary>
        /// <param name="lessonId">The id to find.</param>
        /// <returns>Returns the lesson details.</returns>
        public LessonResponseModel GetAthenasLesson(string lessonId)
        {
            try
            {
                var key = $"{LESSON_STRING}{COLON_STRING}{LESSON_SINGLE_STRING}{COLON_STRING}{lessonId}";
               

                if (!ConfigCache.IsRedisOn())
                {
                    return null;
                }

                var value = RedisCacheHandler.Read(key);
                if (value is not null)
                    return JsonConvert.DeserializeObject<LessonResponseModel>(value.ToString());

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// To set Athenas' subjects in the cache.
        /// </summary>
        /// <param name="lesson">Lesson store.</param>
        /// <param name="lessonId">The id of the lesson.</param>
        public void SetAthenasLesson(LessonResponseModel lesson, string lessonId)
        {
            try
            {
                var key = $"{LESSON_STRING}{COLON_STRING}{LESSON_SINGLE_STRING}{COLON_STRING}{lessonId}";
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }

                RedisCacheHandler.Write(key,
                    JsonConvert.SerializeObject(lesson), ConfigCache.GetLessonExpiration());
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// Deletes a lesson from the cache.
        /// </summary>
        /// <param name="lessonId">The lesson id to delete.</param>
        public void DeleteAthenasLesson(string lessonId)
        {
            try
            {
                var key = $"{LESSON_STRING}{COLON_STRING}{LESSON_SINGLE_STRING}{COLON_STRING}{lessonId}";
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }

                RedisCacheHandler.Delete(key);
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        /// <summary>
        /// To get Athenas lesson HTML from the cache.
        /// </summary>
        /// <param name="lessonId">The id to find.</param>
        /// <returns>Returns the lesson details.</returns>
        public HTMLViewerResponseModel GetAthenasLessonHtml(string lessonId)
        {
            try
            {
                var key = $"{LESSON_STRING}{COLON_STRING}{LESSON_HTML_STRING}{COLON_STRING}{lessonId}";
       

                if (!ConfigCache.IsRedisOn())
                {
                    return null;
                }

                var value = RedisCacheHandler.Read(key);
                if (value is not null)
                    return JsonConvert.DeserializeObject<HTMLViewerResponseModel>(value.ToString());

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// To set Athenas' subjects in the cache.
        /// </summary>
        /// <param name="lesson">Lesson store.</param>
        /// <param name="lessonId">The id of the lesson.</param>
        public void SetAthenasLessonHtml(HTMLViewerResponseModel lesson, string lessonId)
        {
            try
            {
                var key = $"{LESSON_STRING}{COLON_STRING}{LESSON_HTML_STRING}{COLON_STRING}{lessonId}";
                if (!ConfigCache.IsRedisOn())
                {
                    return;
                }

                RedisCacheHandler.Write(key,
                    JsonConvert.SerializeObject(lesson), ConfigCache.GetLessonExpiration());
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

    }
}