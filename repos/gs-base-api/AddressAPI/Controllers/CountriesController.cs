using AddressAPI.AddressAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using AddressAPI;

namespace AddressAPI.Controllers
{
    /// <summary>
    /// Countries controller to handle countries details and methods.
    /// </summary>
    [ApiController]
    [Route("addressapi/addresses/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly IConfiguration _config = AppConfiguration.Configuration;


        // Helpers -------------------------------------------------------------

        private string ResolveLangFromHeader()
        {
            // default from config
            var fallback = _config["AppSettings:default_language_code"];

            if (Request?.Headers.TryGetValue("Accept-Language", out var values) == true)
            {
                // Take first token (before comma/semicolon), e.g. "es-PR;q=0.9" -> "es-PR"
                var token = values.ToString().Split(',', ';').FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(token))
                    return token.Trim();
            }
            return fallback;
        }

        private AddressService CreateAddressManager(string? langCode)
        {
            var writeConn = _config.GetConnectionString("AddressDBConnection");
            var readConn = _config.GetConnectionString("AddressDBConnectionReader");
            var defaultLang = _config["AppSettings:default_language_code"];
            var lang = string.IsNullOrWhiteSpace(langCode) ? defaultLang : langCode;

            return new AddressService(writeConn, readConn, lang);
        }

        // Endpoints -----------------------------------------------------------

        /// <summary>
        /// Get countries’ details by ISO Alpha 3 list.
        /// </summary>
        [HttpPost("by-iso-list")]
        public IActionResult GetCountriesByIsoList([FromBody] List<string> isoList)
        {
            try
            {
                var langCode = ResolveLangFromHeader();
                var addressManager = CreateAddressManager(langCode);
                var result = addressManager.GetCountryNamesAndIsosByIsoList(isoList);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get countries’ details, optionally filtering by language and availability.
        /// </summary>
        [HttpGet("filtering/{filterByLanguage:bool}/{onlyAvailable:bool}")]
        public IActionResult GetCountries(bool filterByLanguage, bool onlyAvailable)
        {
            try
            {
                var langCode = ResolveLangFromHeader();
                var addressManager = CreateAddressManager(langCode);
                var result = addressManager.GetCountryFullDetails(filterByLanguage, onlyAvailables: onlyAvailable);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        /// <summary>
        /// Get a country’s name by ISO Alpha 3 (e.g., USA).
        /// </summary>
        [HttpGet("iso/{countryIso}")]
        public IActionResult GetCountryNameByISO(string countryIso)
        {
            try
            {
                var addressManager = CreateAddressManager(langCode: null);
                var result = addressManager.GetCountryStandardName(countryIso);
                return Ok(result);
            }
            catch
            {
                return BadRequest(string.Empty);
            }
        }

        /// <summary>
        /// Get ISO Alpha 1 (US) by ISO Alpha 3 (USA).
        /// </summary>
        [HttpGet("iso1-by-iso2/{countryIso}")]
        public IActionResult GetCountryISO1ByISO2(string countryIso)
        {
            try
            {
                var addressManager = CreateAddressManager(langCode: null);
                var result = addressManager.GetIso1FromIso2Country(countryIso);
                return Ok(result);
            }
            catch
            {
                return BadRequest(string.Empty);
            }
        }
    }
}
