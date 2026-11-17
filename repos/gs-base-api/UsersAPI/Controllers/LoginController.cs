using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;


using UsersAPI.CustomClasses;
using UsersAPI.UsersAPI.Domain.Models.Generals.Requests;
using UsersAPI.UsersAPI.Domain.Models.Generals.Responses;
using UsersAPI.UsersAPI.Services.Accounts;
using UsersAPI.UsersAPI.Domain.Models.Generals;
using UsersAPI.UsersAPI.Domain.Models.SSO;
using UsersAPI.UsersAPI.Services.SSO;

namespace UsersAPI.Controllers
{
    /// <summary>
    /// Handles the login.
    /// </summary>
    [ApiController]
    [Route("usersapi/login")]
    public class LoginController : ControllerBase
    {
        /// <summary>
        /// To log in to the account. It returns a token. The user should use this token to access to the other endpoints.
        /// </summary>
        [HttpPost("client")]
        public async Task<IActionResult> LoginClient([FromBody] LoginRequestModel loginRequestModel)
        {
            try
            {
                var loginAccountResponseModel = Authentication.LoginProcess(
                    loginRequestModel.Username,
                    loginRequestModel.Password,
                    loginRequestModel.Username != null && loginRequestModel.Username.Contains('@'),
                    IPAddressDetector.GetClientIpAddress(HttpContext),
                    RequestHandler.GetLanguage(HttpContext)
                );
                if (string.IsNullOrWhiteSpace(loginAccountResponseModel.Token))
                {
                    return Unauthorized();
                }
                return Ok(loginAccountResponseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// To sign in by SSO. (Azure support only).
        /// </summary>
        [HttpPost("sso")]
        public async Task<IActionResult> LoginSso([FromBody] LoginSsoModel loginSsoModel)
        {
            try
            {
                var ssoClientDetails = SsoManager.GetClientDetails(loginSsoModel.Domain, "AZURE");
                if (ssoClientDetails == null)
                {
                    return Unauthorized();
                }
                var loginAccountResponseModel = await Authentication.LoginProcessSSO(
                    loginSsoModel.Token,
                    IPAddressDetector.GetClientIpAddress(HttpContext),
                    ssoClientDetails.OpenIdConfigUrl,
                    ssoClientDetails.ClientId,
                    ssoClientDetails.ClientSecret,
                    ssoClientDetails.TenantId,
                    RequestHandler.GetLanguage(HttpContext)
                );
                if (string.IsNullOrWhiteSpace(loginAccountResponseModel.Token))
                {
                    return Unauthorized();
                }
                return Ok(loginAccountResponseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
