using System;
using System.Collections.Generic;
using GSWebAPI.CustomClasses;
using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using GSWebAPI.GSWebAPI.Services.Generals;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GSWebAPI.Controllers
{
    /// <summary>
    ///  To handle the countries and states where the APP is available.
    /// </summary>
    [ApiController]
    [Route("gswebapi")]
    public class AddressesController : ControllerBase
    {
        /// <summary>
        /// Get the coutries’ isos that the app is available.
        /// </summary>
        /// <param name="countryIsoAlpha3">The country’s iso-code Alpha code 3. Example: USA, PRI, etc.</param>
        /// <returns>Includes a list of countries where the APP is available.</returns>
        [HttpGet]
        [Route("addresses/app-available")]
        public async Task<IActionResult> CheckIfPlanAvailableForCountry(string countryIsoAlpha3 = null)
        {
            try
            {
                var countryAbleISOModelList = new AppAvailabilityManager(
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:GSWebDBConnectionReader"]).GetAvailableCountryIsos();
                var isoList = new List<string>();
                if (countryIsoAlpha3 != null)
                {
                    isoList.Add(countryIsoAlpha3);
                }
                else
                {
                    for (int i = 0; i < countryAbleISOModelList.Count; i++)
                    {
                        isoList.Add(countryAbleISOModelList[i].CountryIsoCode2);
                    }
                }
                var result = new AddressDetailsHandler().GetCountriesFullDetails(HttpContext, isoList, RequestHandler.GetLanguage(HttpContext));
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
