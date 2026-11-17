using Microsoft.AspNetCore.Mvc;
using SubscriptionsAPI.CustomClasses;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses.APIs;
using SubscriptionsAPI.SubscriptionsAPI.Services.FeignAPIs;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Services.Subscriptions;
using SubscriptionsAPI.SubscriptionsAPI.Infrastructure.Repositories.Generals;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses;
using System;
using System.Collections.Generic;

namespace SubscriptionsAPI.Controllers
{
    /// <summary>
    /// To handle the item products. For these APIs, the item products are the combination of a subject and a grade/level.
    /// </summary>
    [ApiController]
    [Route("subscriptionsapi/item-products")]
    public class ItemProductsController : ControllerBase
    {
        [HttpGet("interface/{itemProductId}")]
        public IActionResult GetInterfaceProducts(string itemProductId)
        {
            try
            {
                var result = new SubscriptionProductDAO().Read(
                    AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], new SubscriptionProductModel() { ProductId = itemProductId }, true, true);
                return Ok(result.Count > 0 ? result[0] : null);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetItemProducts()
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ProductRead", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                ProductsReaderManager subscriptionManager = new ProductsReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                List<SubscriptionProductModel> itemProductInterfaces = new SubscriptionProductDAO().Read(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], new List<SubscriptionProductModel>());
                List<ItemProductModel> itemProductModelList = subscriptionManager.GetItemProducts(new ItemProductModel());
                List<ItemProductResponseModel> itemProductResponseModels = new List<ItemProductResponseModel>();
                foreach (ItemProductModel itemProduct in itemProductModelList)
                {
                    SubscriptionProductModel subscriptionItemProduct = itemProductInterfaces.Find(x => x.ProductId == itemProduct.Id);
                    itemProductResponseModels.Add(new ItemProductResponseModel()
                    {
                        Id = itemProduct.Id,
                        SubjectCode = subscriptionItemProduct?.Subject,
                        LevelCode = subscriptionItemProduct?.Level,
                        Available = itemProduct.Available,
                        CreatedAt = itemProduct.CreatedAt,
                        ModifiedBy = itemProduct.ModifiedBy,
                        Name = itemProduct.Name,
                        UpdatedAt = itemProduct.UpdatedAt
                    });
                }
                return Ok(itemProductResponseModels);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetItemProductsSpecificById(string id)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ProductRead", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                ProductsReaderManager subscriptionManager = new ProductsReaderManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                List<SubscriptionProductModel> itemProductInterfaces = new SubscriptionProductDAO().Read(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], new SubscriptionProductModel(), false, false);
                List<ItemProductModel> itemProductModelList = subscriptionManager.GetItemProducts(new ItemProductModel() { Id = id });
                List<ItemProductResponseModel> itemProductResponseModels = new List<ItemProductResponseModel>();
                foreach (ItemProductModel itemProduct in itemProductModelList)
                {
                    SubscriptionProductModel subscriptionItemProduct = itemProductInterfaces.Find(x => x.ProductId == itemProduct.Id);
                    itemProductResponseModels.Add(new ItemProductResponseModel()
                    {
                        Id = itemProduct.Id,
                        SubjectCode = subscriptionItemProduct?.Subject,
                        LevelCode = subscriptionItemProduct?.Level,
                        Available = itemProduct.Available,
                        CreatedAt = itemProduct.CreatedAt,
                        ModifiedBy = itemProduct.ModifiedBy,
                        Name = itemProduct.Name,
                        UpdatedAt = itemProduct.UpdatedAt
                    });
                }
                return Ok(itemProductResponseModels);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("create")]
        public IActionResult CreateItemProduct([FromBody] ItemProductResponseModel requestModel)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ProductCreate", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                var responseModel = new ProductsCreatorManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext)).CreateItemProduct(new ItemProductModel()
                {
                    Available = requestModel.Available,
                    Name = requestModel.Name,
                }, responseSession.UserId);
                if (!responseModel.TransactionValid)
                {
                    return BadRequest(new ResponseModel() { Messages = responseModel.Messages });
                }
                var generalResponseModel = new ItemProductResponseModel
                {
                    Id = responseModel.ItemProductModel.Id,
                    CreatedAt = responseModel.ItemProductModel.CreatedAt,
                    Name = responseModel.ItemProductModel.Name,
                    SubjectCode = requestModel.SubjectCode,
                    LevelCode = requestModel.LevelCode
                };
                new SubscriptionProductDAO().Create(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], new SubscriptionProductModel()
                {
                    ProductId = generalResponseModel.Id,
                    Subject = requestModel.SubjectCode,
                    SubjectName = requestModel.Name,
                    Level = requestModel.LevelCode,
                    ModifiedBy = responseSession.UserId
                });
                return Created(string.Empty, generalResponseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("modify")]
        public IActionResult ModifyItemProduct([FromBody] ItemProductResponseModel requestModel)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ProductCreate", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                ProductsCreatorManager subscriptionManager = new ProductsCreatorManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var responseModel = subscriptionManager.UpdateItemProduct(requestModel, responseSession.UserId);
                SubscriptionProductDAO subscriptionProductDAO = new SubscriptionProductDAO();
                List<SubscriptionProductModel> subscriptionProductModelToCheckIfExists = subscriptionProductDAO.Read(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], new SubscriptionProductModel() { Subject = requestModel.SubjectCode, Level = requestModel.LevelCode }, true, true);
                if (subscriptionProductModelToCheckIfExists.Count > 0)
                {
                    subscriptionProductDAO.Update(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], new SubscriptionProductModel()
                    {
                        ProductId = requestModel.Id,
                        Subject = requestModel.SubjectCode,
                        SubjectName = requestModel.Name,
                        Level = requestModel.LevelCode,
                        ModifiedBy = responseSession.UserId
                    }, new SubscriptionProductModel() { Id = subscriptionProductModelToCheckIfExists[0].Id });
                }
                else
                {
                    subscriptionProductDAO.Create(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnection"], new SubscriptionProductModel()
                    {
                        ProductId = requestModel.Id,
                        Subject = requestModel.SubjectCode,
                        SubjectName = requestModel.Name,
                        Level = requestModel.LevelCode,
                        ModifiedBy = responseSession.UserId
                    });
                }
                if (!responseModel.TransactionValid)
                {
                    return BadRequest(responseModel);
                }
                var generalResponseModel = new ItemProductResponseModel()
                {
                    Id = responseModel.ItemProductModel.Id,
                    CreatedAt = responseModel.ItemProductModel.CreatedAt,
                    Name = responseModel.ItemProductModel.Name,
                    SubjectCode = requestModel.SubjectCode,
                    LevelCode = requestModel.LevelCode
                };
                return Ok(responseModel);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("main-account")]
        public IActionResult GetItemsProductsofMainAccount()
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ItemsMainAccount", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                SubscriptionReadersManager subscriptionManager = new SubscriptionReadersManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var result = subscriptionManager.GetAvailableItemsBySubscriptions(true, responseSession.UserId);
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("profiles/{profileId}/items")]
        public IActionResult ReadProfileItems(string profileId)
        {
            var httpRequest = HttpContext;
            try
            {
                var responseSession = GetUserDetails.GetUserIdRequestOnOtherAPI(RequestHandler.GetToken(HttpContext), IPAddressDetector.GetClientIpAddress(HttpContext), "ProfileItemsRead", null);
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                SubscriptionReadersManager subscriptionManager = new SubscriptionReadersManager(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], RequestHandler.GetLanguage(HttpContext));
                var subscriptionGeneralDetailResponseModelList = subscriptionManager.GetAvailableItemsBySubscriptions(false, profileId);
                List<SubscriptionProductModel> subscriptionsProductsToFind = new List<SubscriptionProductModel>();
                foreach (var subscriptionItem in subscriptionGeneralDetailResponseModelList)
                {
                    subscriptionsProductsToFind.Add(new SubscriptionProductModel() { ProductId = subscriptionItem.ItemId });
                }
                List<SubscriptionProductModel> subscriptionProductModelResults = new SubscriptionProductDAO().Read(AppConfiguration.Configuration["ConnectionStrings:SubscriptionsStripeDBConnectionReader"], subscriptionsProductsToFind);
                List<SubscriptionProfileSelectedItemResponseModel> subscriptionResponseListToReturn = new List<SubscriptionProfileSelectedItemResponseModel>();
                foreach (var subscriptionItem in subscriptionGeneralDetailResponseModelList)
                {
                    subscriptionResponseListToReturn.Add(new SubscriptionProfileSelectedItemResponseModel()
                    {
                        ItemId = subscriptionItem.ItemId,
                        Name = subscriptionItem.Name,
                        SubscriptionProductModel = subscriptionProductModelResults.Find(x => x.ProductId == subscriptionItem.ItemId)
                    });
                }
                return Ok(subscriptionResponseListToReturn);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}