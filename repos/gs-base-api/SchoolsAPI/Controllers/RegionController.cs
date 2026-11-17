using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SchoolsAPI.CustomClasses;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Responses;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using SchoolsAPI.SchoolsAPI.Services.Schools;
using SchoolsAPI.SchoolsAPI.Services.FeignAPIs;

namespace SchoolsAPI.Controllers
{
    /// <summary>
    /// Handles regions (OREs). Regions are the educational divisions of a country/state.
    /// </summary>
    [ApiController]
    [Route("schoolsapi")]
    public class RegionController : ControllerBase
    {
        private readonly IConfiguration _config;

        public RegionController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Returns the school's regions available by country (academic regions, not country admin regions).
        /// </summary>
        /// <param name="countryIsoCode">Optional country ISO filter.</param>
        [HttpGet("regions")]
        public async Task<IActionResult> ReadRegion(string? countryIsoCode = null)
        {
            try
            {
                // This endpoint is guest-accessible. If a valid session exists, use its ORE for filtering.
                var token = RequestHandler.GetToken(HttpContext);
                var ip = IPAddressDetector.GetClientIpAddress(HttpContext);

                var responseSession = await 
                    GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "SubscriptionAdvanceRead");

                var reader = new SchoolsReader(
                    _config.GetConnectionString("SchoolDBConnectionReader"),
                    langCode: null);

                List<RegionModel> regions;

                if (responseSession.SessionValid && !string.IsNullOrWhiteSpace(responseSession.OreId))
                {
                    regions = reader.ReadRegions(new RegionModel
                    {
                        Id = responseSession.OreId,
                        CountryIso = string.IsNullOrWhiteSpace(countryIsoCode) ? null : countryIsoCode
                    });
                }
                else
                {
                    regions = reader.ReadRegions(new RegionModel
                    {
                        CountryIso = string.IsNullOrWhiteSpace(countryIsoCode) ? null : countryIsoCode
                    });
                }

                regions ??= new List<RegionModel>();
                if (regions.Count > 0)
                {
                    regions = regions.OrderBy(x => x.Name).ToList();
                }

                return Ok(regions);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Reads the school IDs assigned to a specific ORE.
        /// </summary>
        /// <param name="oreId">The ORE ID.</param>
        [HttpGet("ore/{oreId}/schools_ids")]
        public IActionResult ReadOreSchools(string oreId)
        {
            try
            {
                var ids = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails.GetORESchoolsIds(oreId);
                return Ok(ids);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
