using Microsoft.AspNetCore.Mvc;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Requests;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs;
using SubscriptionsAPI.SubscriptionsAPI.Services.Generals;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;
using System;
using System.Collections.Generic;
using ResponseModel = SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.ResponseModel;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    /// To handle commercial coupons.
    /// </summary>
    [ApiController]
    [Route("subscriptionsapi/coupons")]
    public class CouponsController : ControllerBase
    {
        [HttpPost("read")]
        public IActionResult GetCoupons([FromBody] CouponRequestModel requestModel)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    RequestHandler.GetToken(HttpContext),
                    IPAddressDetector.GetClientIpAddress(HttpContext),
                    "CouponsRead", RequestHandler.GetLanguage(HttpContext));

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var couponReaderManager =
                    new CouponReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], RequestHandler.GetLanguage(HttpContext));

                var couponSearchResponseModels = couponReaderManager.GetCouponsAdvanced(requestModel.Coupon,
                      requestModel.PageFrom,
                      requestModel.Limiter,
                      requestModel.OrderByDate);
                CouponAdvanceResponseModel generalResponseModel = new CouponAdvanceResponseModel
                {
                    Coupons = couponSearchResponseModels.Coupons,
                    PageTotals = couponSearchResponseModels.PageTotals
                };

                return Ok(generalResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("create")]
        public IActionResult CreateCoupon([FromBody] CouponRequestModel requestModel)
        {
            var httpRequest = HttpContext;
            var generalResponseModel = new ResponseModel();
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(
                    RequestHandler.GetToken(HttpContext),
                    IPAddressDetector.GetClientIpAddress(HttpContext),
                    "CouponCreate", RequestHandler.GetLanguage(HttpContext));
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                var couponCreatorManager =
                   new CouponCreatorManager(
                   AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"],
                   AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                   RequestHandler.GetLanguage(HttpContext));
                var response =
                    couponCreatorManager.CreateCoupon(
                        requestModel.Coupon,
                        AppConfiguration.Configuration["AppSettings:subs_stripe_key"]);
                generalResponseModel.Messages = response.Messages;
                if (!response.TransactionValid)
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

        [HttpGet("validate/{couponCode}/{interval}")]
        public IActionResult ValidateCoupon(string couponCode, string interval)
        {
            var httpRequest = HttpContext;
            string langCode = RequestHandler.GetLanguage(HttpContext);
            var couponManager = new CouponReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], langCode);
            var couponModel = couponManager.ValidateCouponByCode(
                new CouponModel()
                {
                    Code = couponCode,
                    IntervalIntended = interval,
                },
                langCode);

            if (!couponModel.TransactionValid)
            {
                return Ok(new CouponResponseModel()
                {
                    CouponValid = false,
                    CouponReason = couponModel.Messages != null && couponModel.Messages.Count > 0 ? couponModel.Messages[0] : null
                });
            }
            else
            {
                if (string.IsNullOrWhiteSpace(RequestHandler.GetToken(HttpContext)))
                {
                    return Ok(new CouponResponseModel()
                    {
                        CouponPlatformId = couponModel.CouponModel.PlatformId,
                        CouponValid = true,
                    });
                }
                else
                {
                    // If the user is not a guest, then check if the user can use the coupon for a second time. 
                    /* Token validation in Users API to get the user id. */
                    var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "CouponsRead", null);

                    if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    {
                        return Ok(new CouponResponseModel()
                        {
                            CouponPlatformId = couponModel.CouponModel.PlatformId,
                            CouponValid = true,
                        });
                    }

                    var subscriptionReadersManager = new SubscriptionReadersManager(
                            AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], langCode);

                    var couponCanBeUsedModel = couponManager.ValidateIfCustomerCanUserCouponAgain(
                        subscriptionReadersManager.GetCustomerPlatformId(responseSession.UserId),
                        couponModel.CouponModel.PlatformId, langCode);
                    // If cannot be used again, so return the details
                    if (!couponCanBeUsedModel.TransactionValid)
                    {
                        return Ok(new CouponResponseModel()
                        {
                            CouponValid = false,
                            CouponReason = couponModel.Messages != null && couponModel.Messages.Count > 0 ? couponModel.Messages[0] : null
                        });
                    }
                    else
                    {
                        return Ok(new CouponResponseModel()
                        {
                            CouponPlatformId = couponModel.CouponModel.PlatformId,
                            CouponValid = true,
                        });
                    }
                }
            }
        }

        [HttpGet("validate/platform/{couponPlatformId}")]
        public IActionResult ValidateCouponByPlatform(string couponPlatformId)
        {
            var httpRequest = HttpContext;
            string langCode = RequestHandler.GetLanguage(HttpContext);
            var couponManager = new CouponReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], langCode);
            var couponModel = couponManager.ValidateCoupon(couponPlatformId);

            if (couponModel == null)
            {
                var messageManager = new MessageManager(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"],
                    langCode);
                return BadRequest(new ResponseModel()
                {
                    Messages = new List<string>()
                    {
                        messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:subr_message_coupon_invalid_code"])
                    }
                });
            }

            return Ok(new CouponResponseModel() { CouponValid = true, CouponModel = couponModel });
        }
    }
}