using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GSWebAPI.GSWebAPI.Services.Langs;

namespace GSWebAPI.Controllers
{
    /// <summary>
    /// To handle interface's languages.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class LanguageController : ControllerBase
    {
        /// <summary>
        /// Returns the available languages for the APP interface. This should be a get. But first, check if any client uses it.
        /// </summary>
        /// <returns>Returns the language’s models.</returns>
        [HttpPost]
        [Route("language")]
        public async Task<IActionResult> ReadLanguage()
        {
            try
            {
                var langs = new LangManager(AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"]).GetLangs();
                return Ok(langs);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
