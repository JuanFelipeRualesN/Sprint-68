using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsersAPI.UsersAPI.Domain.Models.Generals;
using UsersAPI.UsersAPI.Services.Generals;
using UsersAPI.CustomClasses;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// To handle the user activity track. 
    /// </summary>
    [ApiController]
    [Route("usersapi/activity")]
    public class ActivityController : ControllerBase
    {
        /// <summary>
        /// To register that the user connected at least once per day. It is a Task process. 
        /// There is no data returned. To identify the user, it will use the token that is required in the header.
        /// </summary>
        [HttpPost("create")]
        public async Task<IActionResult> InsertActivity()
        {
            try
            {
                string token = RequestHandler.GetToken(HttpContext);
                if (string.IsNullOrWhiteSpace(token))
                {
                    return Ok();
                }

                await Task.Run(() =>
                {
                    List<ActivityDateReportModel> userActivityList = GUSRelated.GetUserActivity(token);
                    if (userActivityList != null && userActivityList.Count > 0)
                    {
                        if (string.IsNullOrWhiteSpace(userActivityList[userActivityList.Count - 1].CreatedAt))
                        {
                            GUSRelated.InserUserActivity(userActivityList[0].UserId);
                        }
                        else if (!string.IsNullOrWhiteSpace(userActivityList[userActivityList.Count - 1].CreatedAt) && DateTime.Parse(userActivityList[userActivityList.Count - 1].CreatedAt).Date < DateTime.Now.Date)
                        {
                            GUSRelated.InserUserActivity(userActivityList[0].UserId);
                        }
                    }
                });

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
