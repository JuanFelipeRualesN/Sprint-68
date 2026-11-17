using Microsoft.AspNetCore.Mvc;
using TeachersAPI.CustomClasses.CacheHandler;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using System;

namespace TeachersAPI.Controllers
{
    /// <summary>
    /// To handle the cache maintenance.
    /// </summary>
    [ApiController]
    [Route("teachersapi/cache")]
    public class CacheController : ControllerBase
    {
        /// <summary>
        /// To delete a lesson from cache.
        /// </summary>
        /// <param name="lessonId">The id of the lesson.</param>
        /// <returns>Returns an OK if deleted.</returns>
        [HttpPost("lesson/{lessonId}/delete")]
        public async Task<IActionResult> DeleteLessonFromCache(string lessonId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "Planning");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                // Delete the lesson from cache
                new AthenasAPICache().DeleteAthenasLesson(lessonId);

                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Return error about invalid request
                return BadRequest();
            }
        }
    }
}
