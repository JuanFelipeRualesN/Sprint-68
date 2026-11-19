using AddressAPI.AddressAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressAPI;

namespace AddressAPI.Controllers
{
    [ApiController]
    [Route("addressapi/addresses/states")]
    public class StatesController : ControllerBase
    {
        /// <summary>
        /// Get the states of a country by ISO alpha-3 (e.g., USA).
        /// </summary>
        [HttpGet("{countryIsoAlpha3}")]
        public IActionResult GetStates(string countryIsoAlpha3)
        {
            try
            {
                // Resolve language (Accept-Language header -> fallback to config)
                var langCode = AppConfiguration.Configuration["AppSettings:default_language_code"];
                if (Request?.Headers.TryGetValue("Accept-Language", out var values) == true)
                {
                    var token = values.ToString().Split(',', ';').FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(token)) langCode = token.Trim();
                }

                var writeConn = AppConfiguration.Configuration["ConnectionStrings:AddressDBConnection"];
                var readConn = AppConfiguration.Configuration["ConnectionStrings:AddressDBConnectionReader"];

                var addressManager = new AddressService(writeConn, readConn, langCode);
                var result = addressManager.GetStatesByCountryIso2(countryIsoAlpha3, onlyAvailables: true);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
