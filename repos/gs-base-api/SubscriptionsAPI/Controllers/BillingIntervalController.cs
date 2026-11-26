using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    /// To handle the billing intervals. This is used only for commercial version. However, we do not recommend to remove it, since 
    /// we do not know if the frontend calls it in the non-commercial version.
    /// This applies only to commercial products.
    /// </summary>
    [ApiController]
    [Route("subscriptionsapi/billing-interval")]
    public class BillingIntervalController : ControllerBase
    {
        /// <summary>
        /// Reads the billing intervals for plans (month, year, etc). 
        /// This is used only for commercial version. However, we do not recommend to remove it, since 
        /// we do not know if the frontend calls it in the non-commercial version.
        /// </summary>
        /// <returns>Returns a response with billing details.</returns>
        [HttpGet("read")]
        public async Task<ActionResult> GetBillingIntervals()
        {
            try
            {
                var result = new PlansReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext)).GetBillingIntervals(true, true);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
