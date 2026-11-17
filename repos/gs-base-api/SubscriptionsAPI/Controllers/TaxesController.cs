using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    /// To handle the taxes. This is intended to be used only for commercial purposes.
    /// </summary>
    [ApiController]
    [Route("subscriptionsapi/taxes")]
    public class TaxesController : ControllerBase
    {
        /// <summary>
        /// To get the taxes that could apply to the plans. This is intended to be used only for commercial purposes.
        /// </summary>
        /// <returns>Returns a response with taxes details.</returns>
        [HttpGet("read")]
        public async Task<ActionResult> GetTaxes()
        {
            try
            {
                var result = new PlansReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext)).GetTaxes();
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
