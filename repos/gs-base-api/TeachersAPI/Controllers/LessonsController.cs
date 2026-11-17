using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeachersAPI.CustomClasses;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using LessonsResponse = TeachersAPI.Domain.Models.Lessons.Response;
using FilterResponseModel = TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Generals.Responses.APIs.Athenas;
using TeachersAPI.TeachersAPI.Services.Lessons;
using TeachersAPI.Domain.Models.Lessons.LessonSearchModels;
using TeachersAPI.Domain.Models.Lessons.Request;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/lessons")]
    public class LessonsController : ControllerBase
    {
        private static class LessonStatus
        {
            public const string Published = "6";
        }

        [HttpGet]
        public async Task<IActionResult> GetLessons(string subjectCode, string levelCode, string isGapClosing = "0")
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                List<LessonModel> lessonModelsToCall = new List<LessonModel>()
                {
                    new LessonModel()
                    {
                        SubjectCode = subjectCode,
                        LevelCode = levelCode,
                        IsGapClosing = isGapClosing
                    }
                };
                LessonsResponseModel lessonsResponseModel = AthenasHandler.GetAthenasLessons(lessonModelsToCall, true);
                return Ok(lessonsResponseModel.LessonModelList);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("teachers/{lessonId}")]
        public async Task<IActionResult> GetLessonTeachers(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var cacheAthenas = new CustomClasses.CacheHandler.AthenasAPICache();
                var response = cacheAthenas.GetAthenasLesson(lessonId);
                if (response != null && response.LessonModifierRequestModel != null)
                {
                    return Ok(response);
                }
                var lessonTeachers = AthenasHandler.GetLessonsDetail(RequestHandler.GetToken(HttpContext), lessonId)[0];
                Task.Run(() => cacheAthenas.SetAthenasLesson(lessonTeachers, lessonId));
                return Ok(lessonTeachers);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("teachers/html/{lessonId}")]
        public async Task<IActionResult> GetLessonTeachersHtml(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var cacheAthenas = new CustomClasses.CacheHandler.AthenasAPICache();
                var responseCache = cacheAthenas.GetAthenasLessonHtml(lessonId);
                if (responseCache != null && responseCache.HTMLCode != null)
                {
                    return Ok(responseCache);
                }
                var lessonTeachers = AthenasHandler.GetHTMLResponse(lessonId, RequestHandler.GetLanguage(HttpContext));
                Task.Run(() => cacheAthenas.SetAthenasLessonHtml(lessonTeachers, lessonId));
                return Ok(lessonTeachers);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{lessonId}/exams")]
        public async Task<IActionResult> GetLessonExams(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                LessonQuizzesResponseModel lessonQuiz = AthenasHandler.GetLessonQuizzes(lessonId);
                if (lessonQuiz.Quizzes != null && lessonQuiz.Quizzes.Count > 0)
                {
                    lessonQuiz.Quizzes = lessonQuiz.Quizzes.FindAll(x => x.IsExam == "1");
                }
                return Ok(lessonQuiz.Quizzes);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("advance")]
        public async Task<IActionResult> GetAdvanceLessons([FromBody] LessonFilterRequestModel lessonFilterRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                   return Unauthorized();
                }
                lessonFilterRequestModel.StatusIds = new List<string> { LessonStatus.Published };
                var lessonReaderServices = new LessonsReaderServices();
                FilterResponseModel.LessonSearchResponseModel lessonsResponseModel = await lessonReaderServices.SearchLessonsAsync(lessonFilterRequestModel);
                return Ok(lessonsResponseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{lessonId}")]
        public async Task<IActionResult> LessonDetails(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                LessonModel lessonModel = await lessonReaderServices.LessonDetailsServiceAsync(lessonId);
                return Ok(lessonModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("description")]
        public async Task<IActionResult> LessonDescription([FromBody] LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                string lessonDescription = await lessonReaderServices.LessonDescriptionServiceAsync(lessonDetailsRequestModel);
                return Ok(lessonDescription);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("objective")]
        public async Task<IActionResult> LessonObjective([FromBody] LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                List<LessonObjectiveModel> lessonObjective = await lessonReaderServices.LessonObjectiveServiceAsync(lessonDetailsRequestModel);
                return Ok(lessonObjective);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("transversal-theme")]
        public async Task<IActionResult> LessonTransversalTheme([FromBody] LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                List<LessonTransversalThemeModel> lessonObjective = await lessonReaderServices.LessonTransversalThemeServiceAsync(lessonDetailsRequestModel);
                return Ok(lessonObjective);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("strategies")]
        public async Task<IActionResult> LessonStrategies([FromBody] LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                List<LessonStrategyModel> lessonStrategies = await lessonReaderServices.LessonStrategiesServiceAsync(lessonDetailsRequestModel);
                return Ok(lessonStrategies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("performance-task")]
        public async Task<IActionResult> LessonPerformanceTask([FromBody] LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                List<LessonPerformanceTaskModel> lessonPerformanceTask = await lessonReaderServices.LessonPerformanceTaskServiceAsync(lessonDetailsRequestModel);
                return Ok(lessonPerformanceTask);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("concept")]
        public async Task<IActionResult> LessonConcept([FromBody] LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                string lessonConcept = await lessonReaderServices.LessonConceptServiceAsync(lessonDetailsRequestModel);
                return Ok(lessonConcept);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("definition")]
        public async Task<IActionResult> LessonDefinition([FromBody] LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                List<LessonDefinitionModel> lessonDefinition = await lessonReaderServices.LessonDefinitionServiceAsync(lessonDetailsRequestModel);
                return Ok(lessonDefinition);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("example")]
        public async Task<IActionResult> LessonExample([FromBody] LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                List<LessonExampleModel> lessonExample = await lessonReaderServices.LessonExampleServiceAsync(lessonDetailsRequestModel);
                return Ok(lessonExample);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("quizzes-details")]
        public async Task<IActionResult> LessonQuizzesDetails([FromBody] LessonDetailsRequestModel lessonDetailsRequestModel)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonReaderServices = new LessonsReaderServices();
                var lessonQuizzesDetails = await lessonReaderServices.LessonQuizzesDetailsServiceAsync(lessonDetailsRequestModel);
                return Ok(lessonQuizzesDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}