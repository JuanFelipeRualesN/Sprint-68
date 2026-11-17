using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using UsersAPI.UsersAPI.Domain.Models.Roles;
using UsersAPI.UsersAPI.Services.Roles;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// To handle the permissions. 
    /// </summary>
    [ApiController]
    [Route("usersapi/permissions")]
    public class PermissionController : ControllerBase
    {
        /// <summary>
        /// To get the permissions. The permissions are related to a role. A role is related to a user. 
        /// The permissions are used in the majority of the endpoints to check if the user has a permission to call the request.
        /// </summary>
        /// <returns>Returns the permissions' details list.</returns>
        [HttpGet]
        public async Task<IActionResult> ReadPermissions()
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RolesRead");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var permissions = new PermissionManager(
                    "", // Connection string placeholder
                    "", // Connection string placeholder
                    sessionResponseModel.LangCode).GetPermissions(new List<PermissionModel>());
                return Ok(permissions);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To call specific permission by id.
        /// </summary>
        /// <param name="id">The permission’s id.</param>
        /// <returns>Returns the permissions list. Use the first element.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> ReadPermissions(string id)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RolesRead");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var permissions = new PermissionManager(
                    "", // Connection string placeholder
                    "", // Connection string placeholder
                    sessionResponseModel.LangCode).GetPermissions(new List<PermissionModel> { new PermissionModel { Id = id } });
                return Ok(permissions);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To create a new permission.
        /// </summary>
        /// <param name="permissionModel">Permission model to create.</param>
        /// <returns>Returns the response with transaction details. Includes messages with the error details if any.</returns>
        [HttpPost("create")]
       public async Task<IActionResult> CreatePermission([FromBody] PermissionModel permissionModel)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RolesPermissionHandle");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var permissionResponseModel = new PermissionManager(
                    "", // Connection string placeholder
                    "", // Connection string placeholder
                    sessionResponseModel.LangCode).CreatePermission(permissionModel, sessionResponseModel.UserId);
                if (!permissionResponseModel.Valid)
                {
                    return BadRequest(permissionResponseModel);
                }
                return Created(string.Empty, permissionResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To modify permission.
        /// </summary>
        /// <param name="permissionModel">Permission model to modify.</param>
        /// <returns>Returns the permissions’ details with transaction details.</returns>
        [HttpPost("modify")]
       public async Task<IActionResult> UpdatePermission([FromBody] PermissionModel permissionModel)
        {
            try
            {
                var sessionResponseModel = CustomClasses.ValidateGenericCall.ValidateFullCall(HttpContext, "RolesPermissionHandle");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var permissionResponseModel = new PermissionManager(
                    "", // Connection string placeholder
                    "", // Connection string placeholder
                    sessionResponseModel.LangCode).UpdatePermission(permissionModel, sessionResponseModel.UserId);
                if (!permissionResponseModel.Valid)
                {
                    return BadRequest(permissionResponseModel);
                }
                return Ok(permissionResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
