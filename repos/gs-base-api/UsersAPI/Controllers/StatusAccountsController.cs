using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using UsersAPI.UsersAPI.Services.Accounts;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// To handle the statuses of the accounts.
    /// </summary>
    [ApiController]
    [Route("usersapi/status")]
    public class StatusAccountsController : ControllerBase
    {
        /// <summary>
        /// Return the possible status for an account.
        /// </summary>
        /// <returns>A HTTP Response with status account models' list.</returns>
        [HttpGet]
        public async Task<IActionResult> ReadStatuses()
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "StatusAccountRead");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var userDetailsManager = new UserDetailsManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    sessionResponseModel.LangCode);
                var result = userDetailsManager.GetStatus(
                    new List<UsersAPI.Domain.Models.Accounts.StatusAccountModel>());
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
