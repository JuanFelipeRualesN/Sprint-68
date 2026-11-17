using GSWebAPI.CustomClasses.CacheHandler;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Services.Generals;
using Microsoft.AspNetCore.Mvc;


namespace GSWebAPI.Controllers
{

    /// <summary>
    /// Handle calls related to game version.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class VersionGameController : ControllerBase
    {
        private readonly VersionCache _versionCache;

        /// <summary>
        /// To read the current version of Unity's APP.
        /// </summary>
        /// <returns>Returns the HTTP Response with the current version for Unity's APP.</returns>
        [HttpGet]
        [Route("version/app")]
        public async Task<IActionResult> ReadVersion()
        {
            try
            {
                var result = new VersionManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"],
                    null).GetAppVersions(new GameVersionModel());
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To evaluate if user can enter to Unity APP.
        /// </summary>
        /// <param name="gameVersion">Game version model.</param>
        /// <returns>Returns a boolean with OK http response.</returns>
        [HttpPost]
        [Route("version/access/")]
        public async Task<IActionResult> EvaluateIfUserCanEnterToApp(GameVersionModel gameVersion)
        {
            try
            {
                // First, try it with cache
                List<GameVersionModel> gameVersions = _versionCache.GetVersions();
                if (gameVersions != null && gameVersions.Count > 0 && gameVersions.Find(x => x.Platform == gameVersion.Platform && x.Version == gameVersion.Version && DateTime.Parse(x.EndDate) > DateTime.Now) != null)
                {
                    return Ok(true);
                }

                // Else, check with DB
                List<GameVersionModel> gameVersionsDB = new VersionManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"], null).GetAppVersions(new GameVersionModel());
                _versionCache.SetVersions(gameVersionsDB);

                // Check if APP can enter
                if (gameVersionsDB != null && gameVersionsDB.Count > 0 && gameVersionsDB.Find(x => x.Platform == gameVersion.Platform && x.Version == gameVersion.Version && DateTime.Parse(x.EndDate) > DateTime.Now) != null)
                {
                    return Ok(true);
                }
                return Ok(false);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
