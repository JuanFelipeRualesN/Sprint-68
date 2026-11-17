using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Services.PersonalDetails;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// To handle the user's addresses.
    /// </summary>
    [ApiController]
    [Route("usersapi/addresses")]
    public class AddressesController : ControllerBase
    {
        /// <summary>
        /// To get the user’s main address.
        /// </summary>
        [HttpPost("user/main")]
        public async Task<IActionResult> ReadUserMainAddress([FromBody] PermissionSessionRequestModel requestModel = null)
        {
            try
            {
                // Replace legacy token validation with HttpContext
                var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "MainAccountAddressRead",
                   requestModel == null || string.IsNullOrWhiteSpace(requestModel.IpAddress) ? IPAddressDetector.GetClientIpAddress(HttpContext) :
                    requestModel.IpAddress);
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var address = new AddressManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    sessionResponseModel.LangCode).GetUsersMainAddress(new List<string> { sessionResponseModel.UserId })[0];
                return Ok(address);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the address of a user (by the staff).
        /// </summary>
        [HttpPost("user/main/by-employee")]
        public async Task<IActionResult> ReadUserMainAddressByEmployee([FromBody] PermissionSessionRequestModel requestModel = null)
        {
            try
            {
                var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "UserAddressReadByEmployee", requestModel == null || string.IsNullOrWhiteSpace(requestModel.IpAddress) ? IPAddressDetector.GetClientIpAddress(HttpContext) : requestModel.IpAddress);
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                if (sessionResponseModel.IsStaff && !string.IsNullOrWhiteSpace(sessionResponseModel.OreId))
                {
                    if (!ORESchoolUserCheck.CheckIfStaffOreCanHandleUser(sessionResponseModel.OreId, requestModel.UserId))
                    {
                        return Unauthorized();
                    }
                }
                var address = new AddressManager(
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnectionReader"],
                    sessionResponseModel.LangCode).GetUsersMainAddress(new List<string> { requestModel.UserId })[0];
                return Ok(address);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To modify a user address (by the user in session).
        /// </summary>
        [HttpPost("user/modify")]
        public async Task<IActionResult> UpdateAddress([FromBody] MainAddressUpdateRequestModel requestModel)
        {
            try
            {
                var sessionResponseModel = ValidateGenericCall.ValidateFullCall(HttpContext, "MainAccountAddressUpdate");
                if (!sessionResponseModel.SessionValid)
                {
                    return Unauthorized();
                }
                var userUpdateAddressModel = AddressUserManager.UpdateUserAddress(
                    requestModel.AddressLine1,
                    requestModel.AddressLine2,
                    requestModel.City,
                    requestModel.CountryIsoCode,
                    requestModel.Country,
                    requestModel.ZipCode,
                    requestModel.State ?? requestModel.StateIsoCode,
                    requestModel.StateIsoCode ?? requestModel.State,
                    sessionResponseModel.UserId,
                    sessionResponseModel.LangCode,
                    sessionResponseModel.UserId);
                if (!userUpdateAddressModel.TransactionValid)
                {
                    return BadRequest(new GeneralResponseModel { Messages = userUpdateAddressModel.Messages });
                }
                return Ok(new GeneralResponseModel { Messages = userUpdateAddressModel.Messages });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
