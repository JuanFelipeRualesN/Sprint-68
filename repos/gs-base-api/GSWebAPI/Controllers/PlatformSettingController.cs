using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.PlatformSettings;
using System;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle platform settings.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class PlatformSettingController : ControllerBase
    {
        /// <summary>
        /// To get the platform settings.
        /// </summary>
        /// <returns>Returns the plaform settings.</returns>
        [HttpGet]
        [Route("platform/settings")]
        public async Task<IActionResult> GetPlatformSettings()
        {
            try
            {
                var result = new PlatformSettingsManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"]).GetPlatformSettings();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To check the platform settings status.
        /// </summary>
        /// <param name="platformSetting">The platform setting to check. </param>
        /// <returns></returns>
        [HttpGet]
        [Route("platform/settings/status")]
        public async Task<IActionResult> GetPlatformSettingsStatus(string platformSetting)
        {
            try
            {
                var result = new PlatformSettingsManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"]).
                    CheckPlatformSettingStatus(platformSetting);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To create or update platform settings.
        /// </summary>
        /// <returns>Returns the platform settings.</returns>
        [HttpPost]
        [Route("platform/settings")]
        public async Task<IActionResult> PostPlatformSettings(GSWebAPI.Domain.Models.PlatformSetting.Request.PlatformSettingModel platformSettingModel)
        {
            try
            {
                /* Token validation in Users API */
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlatformSettings");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var plaformSettings = new PlatformSettingsManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"]).ModifyPlatformSetting(platformSettingModel);

                if (plaformSettings.Valid)
                {
                    return Ok(plaformSettings);
                }
                else
                {
                    return BadRequest(plaformSettings);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
