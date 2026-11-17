using Microsoft.AspNetCore.Mvc;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests;
using SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;
using System;
using System.Collections.Generic;

namespace SubscriptionsAPI.Controllers
{
    [ApiController]
    [Route("subscriptionsapi/products")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("filter/{onlyAvailables}/{requireItemProducts}")]
        public IActionResult GetProducts(bool onlyAvailables, bool requireItemProducts, [FromQuery] bool isCommercial = false)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ProductRead", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var subscriptionManager = new ProductsReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var result = subscriptionManager.GetProducts(onlyAvailables, requireItemProducts, isCommercial);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("read/{productId}")]
        public IActionResult GetProduct(string productId)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ProductRead", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var subscriptionManager = new ProductsReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], RequestHandler.GetLanguage(HttpContext));
                var result = subscriptionManager.GetProductResponseModelListByProductId(productId, false, true);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("create")]
        public IActionResult CreateProduct([FromBody] ProductRequestModel requestModel)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ProductCreate", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var subscriptionManager = new ProductsCreatorManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var responseModel = subscriptionManager.CreateProduct(requestModel.ProductModel, requestModel.ProductItemProductModelList, AppConfiguration.Configuration["AppSettings:subs_stripe_key"], responseSession.UserId);
                if (requestModel.ProductModel.IsCommercial == "0" && responseModel.TransactionValid)
                {
                    new PlanCreatorManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext)).CreatePlan(
                        GetPlanDefaults(requestModel.ProductModel.Name, responseModel.Id, requestModel.ProductModel.Available, responseSession.UserId, null), null, responseSession.UserId, false);
                }
                if (!responseModel.TransactionValid)
                {
                    return BadRequest(responseModel);
                }
                return Created(string.Empty, responseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost("modify")]
        public IActionResult ModifyProduct([FromBody] ProductRequestModel requestModel)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ProductCreate", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var subscriptionManager = new ProductsCreatorManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var responseModel = subscriptionManager.UpdateProduct(requestModel.ProductModel, requestModel.ProductItemProductModelList, AppConfiguration.Configuration["AppSettings:subs_stripe_key"], responseSession.UserId);
                if ((string.IsNullOrWhiteSpace(requestModel.ProductModel.IsCommercial) || requestModel.ProductModel.IsCommercial == "0") && responseModel.TransactionValid)
                {
                    var planModels = new PlansReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext)).GetPlanModels(new List<SubscriptionsAPI.Domain.Models.Subscriptions.PlanModel>()
                    {
                        new PlanModel() { ProductId = requestModel.ProductModel.Id }
                    });
                    if (planModels.Count > 0)
                    {
                        try
                        {
                            new PlansUpdaterManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext)).UpdatePlan(
                                GetPlanDefaults(requestModel.ProductModel.Name, requestModel.ProductModel.Id, requestModel.ProductModel.Available, responseSession.UserId, planModels[0].Id), responseSession.UserId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                if (!responseModel.TransactionValid)
                {
                    return BadRequest(responseModel);
                }
                return Ok(responseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private SubscriptionsAPI.Domain.Models.Subscriptions.Requests.PlanRequestModel GetPlanDefaults(string productName, string productId, string available, string userInSession, string planId = null)
        {
            return new SubscriptionsAPI.Domain.Models.Subscriptions.Requests.PlanRequestModel()
            {
                Id = planId,
                PlanDescriptionsByLang = new List<PlanLangModel>()
                {
                    new PlanLangModel() { LangCode = "es", Name = productName, Description = productName, ModifiedBy = userInSession },
                    new PlanLangModel() { LangCode = "en", Name = productName, Description = productName, ModifiedBy = userInSession }
                },
                Amount = "199",
                SpecialPromoDays = "0",
                SpecialPromoLimit = "0",
                IsForSchool = "1",
                IsSpecialPromo = "0",
                Available = available,
                Currency = "USD",
                Interval = "year",
                IntervalCount = "1",
                IsPrivate = "1",
                Name = productName,
                ProductId = productId,
                TrialDays = 0,
                PlanTaxesIds = new List<string>(),
                PlanAvailableIsos = new List<SubscriptionsAPI.Domain.Models.Subscriptions.PlanAvailableIsoModel>()
                {
                    new PlanAvailableIsoModel() { CountryIso = "USA", StateIso = "PR" },
                    new PlanAvailableIsoModel() { CountryIso = "PRI" },
                    new PlanAvailableIsoModel() { CountryIso = "PR" }
                },
                ProfilesQuantity = "999999",
            };
        }
    }
}
