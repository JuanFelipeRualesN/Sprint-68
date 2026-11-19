using Microsoft.AspNetCore.Mvc;
using GSWebAPI.GSWebAPI.Domain.Models.Athenas.Requests;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Services.Athenas;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;
using GSWebAPI.GSWebAPI.Domain.Models.Athenas;

namespace GSWebAPI.Controllers.AthenasControllers
{
    /// <summary>
    /// Controller to handle Athenas Lessons related endpoints.
    /// </summary>
    [ApiController]
    [Route("athenasapi")]
    public class AthenasLessonsController : ControllerBase
    {
        /// <summary>
        /// To get only the lessons’ models. True if calling only the published ones.
        /// </summary>
        /// <param name="lessonModelSpecificsRequestModel">Details to request.</param>
        /// <param name="filteredByPublished">True to call only the published lessons.</param>
        /// <returns>Returns the lesson’s details.</returns>
        [HttpPost]
        [Route("lessons/by-lesson-models/{filteredByPublished}")]
        public async Task<IActionResult> GetLessonsModelsSpecifics(LessonModelSpecificsRequestModel lessonModelSpecificsRequestModel, bool filteredByPublished)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var lessonBylessonModel = new AthenasLessonsService();
                AthenasLessonResponseByLessonModel lessonsResponseModel = await lessonBylessonModel.GetLessonByLessonModels(lessonModelSpecificsRequestModel, filteredByPublished);
                return Ok(lessonsResponseModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
