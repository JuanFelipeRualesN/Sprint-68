using System;
using Microsoft.AspNetCore.Mvc;
using DataAnalysisAPI.DataAnalysisAPI.Domain.Models.FeignAPIs;
using DataAnalysisAPI.DataAnalysisAPI.Services.FeignAPIs;
using DataAnalysisAPI.Utils;

namespace DataAnalysisAPI.Controllers
{
    /// <summary>
    /// Generic lookups: roles, schools, cities, countries, etc.
    /// </summary>
    [ApiController]
    [Route("dataanalysisapi")]
    public class GenericsController : ControllerBase
    {
        /// <summary>Get schools (optionally filtered by ORE).</summary>
        [HttpGet("school")]
        public IActionResult GetSchools([FromQuery] int? oreId = null)
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "school");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                if (!string.IsNullOrWhiteSpace(session.OreId) && int.TryParse(session.OreId, out var oreParsed))
                    oreId = oreParsed;

                var result = DataAnalysisAPI.Services.Generics.GetSchools(
                    int.Parse(session.UserId),
                    session.IsStaff,
                    oreId);

                return Ok(result);
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }

        /// <summary>Get countries.</summary>
        [HttpGet("country")]
        public IActionResult GetCountries()
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "country");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                return Ok(DataAnalysisAPI.Services.Generics.GetCountries());
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }

        /// <summary>Get OREs (regions).</summary>
        [HttpGet("ore")]
        public IActionResult GetOres([FromQuery] int? oreId = null, [FromQuery] string? countryIso = null)
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "ore");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                if (!string.IsNullOrWhiteSpace(session.OreId) && int.TryParse(session.OreId, out var oreParsed))
                    oreId = oreParsed;

                return Ok(DataAnalysisAPI.Services.Generics.GetOres(oreId, countryIso));
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }

        /// <summary>Get states by country.</summary>
        [HttpGet("state")]
        public IActionResult GetStates([FromQuery] string? countryIso = null)
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "state");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                return Ok(DataAnalysisAPI.Services.Generics.GetStates(countryIso));
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }

        /// <summary>Get cities.</summary>
        [HttpGet("city")]
        public IActionResult GetCities(
            [FromQuery] int? oreId = null,
            [FromQuery] string? countryIso = null,
            [FromQuery] string? stateIso = null)
        {
            try
            {
                // NOTE: corregido a "city" (en tu código original estaba "ore")
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "city");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                if (!string.IsNullOrWhiteSpace(session.OreId) && int.TryParse(session.OreId, out var oreParsed))
                    oreId = oreParsed;

                return Ok(DataAnalysisAPI.Services.Generics.GetCities(oreId, countryIso, stateIso));
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }

        /// <summary>Get subjects.</summary>
        [HttpGet("subject")]
        public IActionResult GetSubjects([FromQuery] string? subjectCode = null)
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "subject");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                return Ok(DataAnalysisAPI.Services.Generics.GetSubjects(subjectCode));
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }

        /// <summary>Get levels.</summary>
        [HttpGet("level")]
        public IActionResult GetLevels([FromQuery] string? levelCode = null)
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "level");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                return Ok(DataAnalysisAPI.Services.Generics.GetLevels(levelCode));
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }

        /// <summary>Get roles.</summary>
        [HttpGet("role")]
        public IActionResult GetRoles()
        {
            try
            {
                var session = GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "role");
                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                return Ok(DataAnalysisAPI.Services.Generics.GetRoles());
            }
            catch
            {
                var lang = RequestHandler.GetLanguage(HttpContext);
                return BadRequest(Configurations.GetGenericMessageOnFail(lang));
            }
        }
    }
}
