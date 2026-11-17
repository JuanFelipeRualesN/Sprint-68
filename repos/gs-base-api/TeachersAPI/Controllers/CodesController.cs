using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using TeachersAPI.Domain.Models.Codes;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Services.Codes;
using TeachersAPI.TeachersAPI.Services.FeignAPI;

namespace TeachersAPI.Controllers
{
    /// <summary>
    /// Handle the codes that are read from Athenas API (academic content API).
    /// </summary>
    [ApiController]
    [Route("teachersapi/codes")]
    public class CodesController : ControllerBase
    {
        private static class CodeTypes
        {
            public const string Standards = "1";
            public const string Expectations = "7";
        }

        [HttpGet("{type}")]
        public async Task<IActionResult> GetCodes(string type, int fromPage = 0, int limiter = 10)
        {
            try
            {
                ValidationResponseModel responseSession =
                    await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var codeHandleModel = new CodeHandleModel();
                bool isExpectationType = false;

                switch (type.ToLower())
                {
                    case "standards":
                        codeHandleModel.TypeId = CodeTypes.Standards;
                        break;
                    case "expectations":
                        codeHandleModel.TypeId = CodeTypes.Expectations;
                        isExpectationType = true;
                        break;
                    default:
                        return BadRequest("Invalid code type specified");
                }

                var athenasCodes = await AthenasHandler.GetAthenasCodesAsync(codeHandleModel, fromPage, limiter, isExpectationType);
                return Ok(athenasCodes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("constructed-codes/lesson/{lessonId}")]
        public async Task<IActionResult> ConstructedCodeLesson(string lessonId)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var constructedCodeService = new ConstructedCodeService();
                List<ConstructedCodeHandleModel> lessonsResponseModel = await constructedCodeService.ConstructedCodesServiceAsync(lessonId);
                return Ok(lessonsResponseModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}