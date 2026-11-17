using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Services.Currencies;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    ///  To handle the currencies for commercial plan prices. This is intended to be used for commercial purposes only.
    ///  We do not recommend removing it, since we do not know if the frontend calls it even for the non-commercial version.
    /// </summary>
    [ApiController]
    [Route("subscriptionsapi/currency")]
    public class CurrencyController : ControllerBase
    {
        /// <summary>
        /// To read the currencies. The currencies are to be used for plan prices.
        /// This is only to be used for commercial purposes.
        /// </summary>
        /// <returns>Returns a HTTP Response Message.</returns>
        [HttpGet("read")]
        public async Task<ActionResult> GetCurrencies()
        {
            try
            {
                var currencyManager = new CurrencyManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var result = currencyManager.GetAvailableCurrencies();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
