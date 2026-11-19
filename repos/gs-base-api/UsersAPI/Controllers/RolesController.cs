using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Domain.Models.Roles.Request;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// To handle roles.
    /// </summary>
    [ApiController]
    [Route("usersapi/roles")]
    public class RolesController : ControllerBase
    {
        /// <summary>
        /// To read the roles.
        /// </summary>
        /// <returns>Returns a list of roles.</returns>
        [HttpGet]
        public async Task<IActionResult> ReadRoles()
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RolesRead");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var rolesManager = new UsersAPI.Services.Roles.RolesManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    sessionResponseModel.LangCode);
                var result = rolesManager.GetRolesByRoleTypeModel(new List<UsersAPI.Domain.Models.Roles.RoleTypeModel>());
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To read the details of a specific role.
        /// </summary>
        /// <param name="id">The id of the role.</param>
        /// <returns>Returns the roles.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadRoles(string id)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RolesRead");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var rolesManager = new UsersAPI.Services.Roles.RolesManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    sessionResponseModel.LangCode);
                var result = rolesManager.GetRoleDetails(new UsersAPI.Domain.Models.Roles.Request.RoleRequestModel { Id = id });
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRoles([FromBody] RoleRequestModel roleRequest)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RolesPermissionHandle");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var rolesManager = new UsersAPI.Services.Roles.RolesManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    sessionResponseModel.LangCode);
                var roleResponseModel = rolesManager.CreateRole(roleRequest, sessionResponseModel.UserId);
                if (!roleResponseModel.Valid)
                {
                    return BadRequest(roleResponseModel);
                }
                return Created(string.Empty, roleResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("modify")]
        public async Task<IActionResult> UpdatesRoles([FromBody] RoleRequestModel roleRequest)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RolesPermissionHandle");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var rolesManager = new UsersAPI.Services.Roles.RolesManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    sessionResponseModel.LangCode);
                var roleResponseModel = rolesManager.UpdateRole(roleRequest, sessionResponseModel.UserId);
                if (!roleResponseModel.Valid)
                {
                    return BadRequest(roleResponseModel);
                }
                return Ok(roleResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("users/id-list")]
        public async Task<IActionResult> ReadUserRoles([FromBody] UsersAPI.Domain.Models.Generals.Requests.APIs.UserReportsFilterRequestModel userResponseFilters)
        {
            var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RolesRead", 
                string.IsNullOrWhiteSpace(userResponseFilters.IpAddress) ? IPAddressDetector.GetClientIpAddress(HttpContext) : userResponseFilters.IpAddress);
            if (!sessionResponseModel.SessionValid)
            {
                return Unauthorized();
            }
            var result = Users.SearchUsersRolesByUserIds(userResponseFilters.UserIds);
            return Ok(result);
        }
    }
}
