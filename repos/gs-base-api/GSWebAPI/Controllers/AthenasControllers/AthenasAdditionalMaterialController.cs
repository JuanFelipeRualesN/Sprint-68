using Microsoft.AspNetCore.Mvc;
using GSWebAPI.GSWebAPI.Services.Athenas;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using GSWebAPI.GSWebAPI.Domain.Models.Athenas.Requests;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas;

namespace GSWebAPI.Controllers.AthenasControllers
{
    [ApiController]
    [Route("athenasapi")]
    public class AthenasAdditionalMaterialController : ControllerBase
    {
        /// <summary>
        /// To read the additional material list.
        /// </summary>
        /// <param name="additionalMaterialRequestModel">Additional material details to filter such SubjectCodes, LevelCodes, FromPage and Limit.</param>
        /// <returns>Returns the additional materials’ details.</returns>
        [HttpPost]
        [Route("additional-material/read/")]
        public async Task<IActionResult> GetAdditionalMaterial(AdditionalMaterialRequestModel additionalMaterialRequestModel)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var additionalMaterial = new AthenasAdditionalMaterialService();
                AthenasAdditionalMaterialResponseModel additionaMaterialResponseModel = await additionalMaterial.GetAdditionalMaterialService(additionalMaterialRequestModel);
                return Ok(additionaMaterialResponseModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// To read the additional material by lesson's id.
        /// </summary>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <param name="fromPage">Current page from pagination. Example: 1</param>
        /// <param name="limit">How many items per page. Example: 10</param>
        /// <returns>Returns the additional materials response model.</returns>
        [HttpGet]
        [Route("additional-material/lesson/read/{lessonId}/{fromPage}/{limit}")]
        public async Task<IActionResult> GetLessonAditionalMaterial(string lessonId, int fromPage, int limit)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var additionalMaterial = new AthenasAdditionalMaterialService();
                AthenasAdditionalMaterialResponseModel additionaMaterialResponseModel = await additionalMaterial.GetLessonAditionalMaterialService(lessonId, fromPage, limit);
                return Ok(additionaMaterialResponseModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// To read a specific additional material by id.
        /// </summary>
        /// <param name="additionalMaterialId">The Id of additional material.</param>
        /// <returns>Returns the additional material model.</returns>
        [HttpGet]
        [Route("additional-material/read/{additionalMaterialId}")]
        public async Task<IActionResult> GetAdditionalMaterialByAddtionalMaterialId(string additionalMaterialId)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var additionalMaterial = new AthenasAdditionalMaterialService();
                List<AdditionalMaterialModel> additionaMaterialResponseModel = await additionalMaterial.GetAdditionalMaterialByAddtionalMaterialIdService(additionalMaterialId);
                return Ok(additionaMaterialResponseModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        /// <summary>
        /// To generate and get the lesson’s PDF for students (which does not include the correct responses).
        /// </summary>
        /// <param name="lessonId">The lesson’s id to get the PDF.</param>
        /// <param name="quizId">= 0, full document
        ///                     > 0, Document with the cover and specific Quiz
        ///                     =-1, Document without Quiz</param>
        ///  <param name="outPutFormat"> pdf = Export to pdf
        ///                             doc =Export to Document   </param>       
        /// <returns>Returns a string with the document’s location.</returns>
        [HttpGet]
        [Route("lessons/students/print/{lessonId}/{quizId}/{outPutFormat}")]
        public async Task<IActionResult> PrintStudentLessons(string lessonId, string quizId, string outPutFormat = "pdf")
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var offlineMaterial = new AthenasAdditionalMaterialService();
                string offlineMaterialResponseModel = await offlineMaterial.PrintStudentLessonsService(lessonId, quizId, outPutFormat);
                return Ok(offlineMaterialResponseModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
