using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TeachersAPI.TeachersAPI.Services.Lessons;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/quiz-types")]
    public class QuizTypeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetQuizTypes()
        {
            try
            {
                var quizCache = new CustomClasses.CacheHandler.QuizTypeCache();
                List<QuizTypeModel> quizTypes = quizCache.GetQuizTypes();
                if (quizTypes != null && quizTypes.Count > 0)
                {
                    return Ok(new QuizTypesResponseModel
                    {
                        QuizTypeModelList = quizTypes,
                        SessionValid = true,
                        TransactionValid = true
                    });
                }
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "QuizTypesRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var quizReaderManager = new QuizReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                quizTypes = quizReaderManager.GetQuizType();
                quizCache.SetQuizTypes(quizTypes);
                var quizTypesResponseModel = new QuizTypesResponseModel
                {
                    QuizTypeModelList = quizTypes,
                    SessionValid = true,
                    TransactionValid = true
                };
                return Ok(quizTypesResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
