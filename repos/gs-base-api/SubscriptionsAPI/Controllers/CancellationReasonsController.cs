using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    /// To handle the cancellation reasons. The cancellation reasons are the explanation of why an enrollment/subscription was cancelled.
    /// </summary>
    [ApiController]
    [Route("subscriptionsapi/cancellations/reasons")]
    public class CancellationReasonsController : ControllerBase
    {
        /// <summary>
        /// To get the cancellation reasons.  
        /// The cancellation reasons are the explanation of why an enrollment/subscription was cancelled.
        /// </summary>
        /// <returns>Returns a HTTP response with cancellation reasons' models.</returns>
        [HttpGet]
        public async Task<ActionResult<List<CancellationReasonModel>>> GetCancellationReasons()
        {
            try
            {
                /* Token validation in Users API */
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext),
                    "CancellationReasons", RequestHandler.GetLanguage(HttpContext));

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var result = new SubscriptionReadersManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], null).GetCancellationResons(new List<CancellationReasonModel>());
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(new List<CancellationReasonModel>());
            }
        }

        /// <summary>
        /// To get an specific cancellation reason.  
        /// The cancellation reasons are the explanation of why an enrollment/subscription was cancelled.
        /// </summary>
        /// <param name="id">The id of the cancellation reason.</param>
        /// <returns>Returns a HTTP response with cancellation reason's model.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CancellationReasonModel>> GetCancellationReasonsSpecific(string id)
        {
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext),
                    "CancellationReasons", RequestHandler.GetLanguage(HttpContext));

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var result = new SubscriptionReadersManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], null).GetCancellationResons(
                    new List<CancellationReasonModel>()
                    {
                        new CancellationReasonModel()
                        {
                            Id = id,
                        }
                    });
                return Ok(result.Count > 0 ? result[0] : null);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}