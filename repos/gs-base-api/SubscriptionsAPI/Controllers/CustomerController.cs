using Microsoft.AspNetCore.Mvc;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests.APIs;
using System.Threading.Tasks;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    /// Handles customer details. This is intended to be used for commercial purposes. However, to not break the flow 
    /// between commercial and non-commercial, we decided to create a dummy customer id. 
    /// </summary>
   [ApiController]
    [Route("subscriptionsapi/customers/stripe")]
    public class CustomerController : ControllerBase
    {
        /// <summary>
        /// Creates the customer for Stripe. However, for non-commercial purposes, 
        /// we still create the customer with a dummy id, since we have to maintain the compatibility 
        /// between commercial and non-commercial purposes. So this is stills being used even for non-commercial.
        /// Since this is a guest call (for first timer subscribers), the token is not evaluated.
        /// </summary>
        /// <param name="customerRequest">The customer model to create.</param>
        /// <param name="stripeRequired">To check if stripe is required, so it will be created on Stripe.</param>
        /// <returns>Returns the customer's platform id only.</returns>
        [HttpPost("create_customer")]
        public IActionResult CreateCustomerForStripe([FromBody] CustomerRequestModel customerRequest, [FromQuery] bool stripeRequired = false)
        {
            if (string.IsNullOrWhiteSpace(customerRequest.Email)
                || string.IsNullOrWhiteSpace(customerRequest.UserId))
            {
                return BadRequest();
            }

            var httpRequest = HttpContext;
            var customerModel = new SubscriptionManager(
                 AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                 AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                 RequestHandler.GetLanguage(HttpContext))
                .CreateCustomer(customerRequest.Email, customerRequest.UserId,
                AppConfiguration.Configuration["AppSettings:subs_stripe_key"], stripeRequired);

            return Ok(customerModel.PlatformId);
        }

        /// <summary>
        /// Updates the payment method in Stripe. The StripeToken property, Token, UserId, PermissionName, IpAddress are required. This is intended to be used in an API to API call.
        /// </summary>
        /// <param name="customerRequest">The customer request with customer details. The StripeToken property, Token, UserId, PermissionName, IpAddress are required. This is intended to be used in an API to API call.</param>
        /// <returns>Returns the response model with transaction details.</returns>
        [HttpPost("update_payment_method")]
        public async Task<IActionResult> UpdatePaymentMethod([FromBody] CustomerRequestModel customerRequest)
        {
            var httpRequest = HttpContext;
            var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                RequestHandler.GetToken(HttpContext),
                IPAddressDetector.GetClientIpAddress(HttpContext),
                "PaymentMethodUpdate", null);
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var langCode = RequestHandler.GetLanguage(HttpContext);
            var subscriptionManager = new SubscriptionManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], langCode);

            // If was sent by the client
            if (string.IsNullOrWhiteSpace(customerRequest.UserId))
            {
                customerRequest.UserId = responseSession.UserId;
                // Retrieve the user email from the user details API
                var userEmail = GetUserDetails.GetUserEmail(responseSession.Token, responseSession.IpAddress, langCode, responseSession.UserId);
                customerRequest.Email = userEmail;
            }

            // Get the platform id of the user and send the new payment token to stripe.
            var response =
            await subscriptionManager.StoreCustomerPaymentToken(
                customerRequest.UserId,
                customerRequest.Email,
                customerRequest.StripeToken,
                AppConfiguration.Configuration["AppSettings:subs_stripe_key"],
                responseSession.UserId);

            if (!response.TransactionValid)
            {
                return BadRequest(new SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel()
                {
                    Messages = response.Messages,
                });
            }

            return Ok(new SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel()
            {
                TransactionValid = true,
                Messages = response.Messages,
            });
        }
    }
}
