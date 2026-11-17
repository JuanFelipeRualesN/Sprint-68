using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.Domain.Models.Lessons;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Services.Lessons.CustomClasses;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI;

namespace TeachersAPI.Controllers
{
    [ApiController]
    [Route("teachersapi/question-types")]
    public class QuestionTypeController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetQuestionTypes()
        {
            try
            {
                var questionCache = new CustomClasses.CacheHandler.QuestionTypesCache();
                List<QuizQuestionTypesModel> questionTypes = questionCache.GetQuestionTypes();
                if (questionTypes != null && questionTypes.Count > 0)
                {
                    return Ok(new QuestionTypesResponseModel
                    {
                        QuestionTypeModelList = questionTypes,
                        SessionValid = true,
                        TransactionValid = true
                    });
                }
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "QuizTypesRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var questionReaderManager = new QuestionReaderManager(AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"], null);
                questionTypes = questionReaderManager.GetQuestionType();
                questionCache.SetQuestionTypes(questionTypes);
                var questionTypesResponseModel = new QuestionTypesResponseModel
                {
                    QuestionTypeModelList = questionTypes,
                    SessionValid = true,
                    TransactionValid = true
                };
                return Ok(questionTypesResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}