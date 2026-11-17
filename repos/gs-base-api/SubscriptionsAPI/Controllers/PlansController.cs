using Microsoft.AspNetCore.Mvc;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Requests;
using SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;
using System;
using System.Collections.Generic;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests;

namespace SubscriptionsAPI.Controllers
{
    [ApiController]
    [Route("subscriptionsapi/plans")]
    public class PlansController : ControllerBase
    {
        [HttpPost("advanced/read")]
        public IActionResult PlansAdvanceRead([FromBody] PlanRequestQueryModel requestModel)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "PlansAdvanceRead", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var result = new PlansReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext)).GetPlans(
                    requestModel.PlanRequest,
                    requestModel.FilterByLanguage,
                    requestModel.OrderByDate,
                    requestModel.PageFrom,
                    requestModel.Limiter,
                    requestModel.DetailsNeeded);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("create")]
        public IActionResult CreatePlan([FromBody] PlanRequestModel requestModel, [FromQuery] bool isCommercial = false)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "PlanCreate", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var generalResponseModel = new SubscriptionsAPI.Domain.Models.Generals.Responses.PlanResponseModel();
                var subscriptionManager = new PlanCreatorManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var planResponseModel = subscriptionManager.CreatePlan(requestModel, AppConfiguration.Configuration["AppSettings:subs_stripe_key"], responseSession.UserId, isCommercial);
                generalResponseModel.Plans = new List<SubscriptionsAPI.Domain.Models.Subscriptions.Responses.PlanResponseModel> { planResponseModel };
                generalResponseModel.Messages = planResponseModel.Messages;
                if (!planResponseModel.TransactionValid)
                {
                    return BadRequest(generalResponseModel);
                }
                return Created(string.Empty, generalResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("update")]
        public IActionResult UpdatePlan([FromBody] PlanRequestModel requestModel)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "PlanUpdate", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var subscriptionManager = new PlansUpdaterManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var responseModel = subscriptionManager.UpdatePlan(requestModel, responseSession.UserId);
                var generalResponseModel = new SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel() { Messages = responseModel.Messages };
                if (!responseModel.TransactionValid)
                {
                    return BadRequest(generalResponseModel);
                }
                return Ok(generalResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("calculate")]
        public IActionResult PlanCalculateCost([FromQuery] string planId, [FromQuery] int subscriptionQuantity, [FromQuery] string couponPlatformId = null, [FromQuery] string stateIso = null, [FromQuery] string countryIso = null)
        {
            var httpRequest = HttpContext;
            try
            {
                var subscriptionReaders = new PlansReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var result = subscriptionReaders.CalculatePlanCost(planId, true, true, subscriptionQuantity, couponPlatformId, stateIso, countryIso);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("admin/calculate")]
        public IActionResult PlanCalculateCostAdmin([FromQuery] string planId, [FromQuery] int subscriptionQuantity, [FromQuery] string userId, [FromQuery] string couponPlatformId = null)
        {
            var httpRequest = HttpContext;
            try
            {
                var subscriptionReaders = new PlansReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var userAddressIso = GetUserDetails.GetUserAddressByEmployee(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), userId);
                var result = subscriptionReaders.CalculatePlanCost(planId, false, true, subscriptionQuantity, couponPlatformId, userAddressIso.StateIsoCode, userAddressIso.CountryIsoCode2);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("guests")]
        public IActionResult GetPlanGuests([FromQuery] bool isSchool = false, [FromQuery] bool onlyAvailables = true, [FromQuery] bool isPrivate = false, [FromQuery] string currencyCode = "USD", [FromQuery] bool filterByLanguage = true, [FromQuery] string planId = null, [FromQuery] bool specialPromo = false)
        {
            var httpRequest = HttpContext;
            try
            {
                var result = new PlansReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext)).GetPlans(currencyCode, isSchool, isPrivate, onlyAvailables, filterByLanguage, planId, specialPromo);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("available-iso/{planId}/{countryIso}")]
        public IActionResult ValidateIfPlanAvailableForCountryIso(string planId, string countryIso)
        {
            var httpRequest = HttpContext;
            string langCode = RequestHandler.GetLanguage(HttpContext);
            PlansReaderManager plansReaderManager = new PlansReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], langCode);
            var planResponseModel = plansReaderManager.GetPlanDetails(planId, true, true);
            if (!planResponseModel.PlanAvailableForIso)
            {
                var subscriptionMessageManager = new MessageManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], langCode);
                var responseModel = new SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel { Messages = new List<string>() };
                responseModel.Messages.Add(subscriptionMessageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_plan_not_available_for_region"]));
                return BadRequest(responseModel);
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet("available-iso/{planId}/{countryIso}/{stateIso}")]
        public IActionResult ValidateIfPlanAvailableForIso(string planId, string countryIso, string stateIso)
        {
            var httpRequest = HttpContext;
            string langCode = RequestHandler.GetLanguage(HttpContext);
            PlansReaderManager plansReaderManager = new PlansReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], langCode);
            var planResponseModel = plansReaderManager.GetPlanDetails(planId, true, true);
            if (!planResponseModel.PlanAvailableForIso)
            {
                var subscriptionMessageManager = new MessageManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], langCode);
                return BadRequest(new SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel
                {
                    Messages = new List<string>()
                    {
                        subscriptionMessageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_plan_not_available_for_region"])
                    }
                });
            }
            else
            {
                return Ok();
            }
        }
    }
}
