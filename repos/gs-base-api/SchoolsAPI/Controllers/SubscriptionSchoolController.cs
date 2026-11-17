using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SchoolsAPI.SchoolsAPI.Services.Schools;
using SchoolsAPI.CustomClasses;
using SchoolsAPI.SchoolsAPI.Services.FeignAPIs;
using SchoolsAPI.SchoolsAPI.Services.Generals;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Requests;

namespace SchoolsAPI.Controllers
{
    [ApiController]
    [Route("schoolsapi/subscription")]
    public class SubscriptionSchoolController : ControllerBase
    {
        // The previous version used DI, which you did not include in your Terms example.
        // This version uses direct instantiation and your Settings class, as per your
        // provided code structure.

        /// <summary>
        /// Returns the school's id of the subscription if any. This is intended to use between APIs. It validates the API Key.
        /// </summary>
        /// <param name="subscriptionId">The subscription's id to retrieve the school.</param>
        /// <returns>Returns the id of the school if any.</returns>
        [HttpGet("get_school/{subscriptionId}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetSchoolIdOfSubscription(string subscriptionId)
        {
            string langCode = RequestHandler.GetLanguage(HttpContext);

            // Validate API Key using direct instantiation
            var responseValidateKey = ValidateAPIKey.ValidateKey(RequestHandler.GetAPIKey(HttpContext), langCode);
            if (!responseValidateKey.Valid)
            {
                return Unauthorized();
            }

            // Direct instantiation as per your TermsController example
            var schoolsReader = new SchoolsReader(Settings.CONNECTION_STRING_READ, langCode);

            var entitySubscriptionModels = schoolsReader.GetEntitySubcriptionModelList(new List<EntitySubscriptionModel>
        {
            new EntitySubscriptionModel { SubscriptionId = subscriptionId }
        });

            if (!entitySubscriptionModels.Any())
            {
                return BadRequest();
            }

            var entityModelsToFind = entitySubscriptionModels.Select(es => new EntityModel
            {
                Id = es.EntityId,
                EntityTypeId = Settings.SCH_SCHOOL_TYPE_ID // Using a static Settings class
            }).ToList();

            var entityModels = schoolsReader.GetEntityModels(entityModelsToFind, false);

            if (!entityModels.Any())
            {
                return BadRequest();
            }

            return Ok(entityModels.First().Id);
        }

        /// <summary>
        /// To relate a school and a subscription from Subscription API. The token and the API key are required.
        /// </summary>
        /// <param name="subscriptionRelateSchool">The subscription relate request model that should include the school id and the subscription id.</param>
        /// <returns>Returns a HTTP Response Message with transaction's details.</returns>
        [HttpPost("relate")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RelateSchoolToSubscription([FromBody] SubscriptionRelateRequestModel subscriptionRelateSchool)
        {
            string token = RequestHandler.GetToken(HttpContext);
            string langCode = RequestHandler.GetLanguage(HttpContext);
            string apiKey = RequestHandler.GetAPIKey(HttpContext);

            if (string.IsNullOrWhiteSpace(token) && string.IsNullOrWhiteSpace(apiKey))
            {
                return Unauthorized();
            }

            string userInSession = null;
            if (!string.IsNullOrWhiteSpace(token))
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "SubscriptionAdvanceRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }
                userInSession = responseSession.UserId;
            }
            else
            {
                var apiKeyResponse = ValidateAPIKey.ValidateKey(apiKey, langCode);
                if (apiKeyResponse == null || !apiKeyResponse.Valid)
                {
                    return Unauthorized();
                }
            }

            try
            {
                // Direct instantiation as per your example
                var schoolsReader = new SchoolsReader(Settings.CONNECTION_STRING_READ, langCode);
                var schoolsModifierManager = new SchoolModifierManager(Settings.CONNECTION_STRING_WRITE, Settings.CONNECTION_STRING_READ, langCode);

                var platforms = schoolsReader.GetPlatformModels(new List<PlatformModel>
            {
                new PlatformModel { Code = Settings.SCH_PLATFORM_ID }
            });

                if (!platforms.Any())
                {
                    return BadRequest("Platform not found.");
                }

                var platformId = platforms.First().Id;

                if (subscriptionRelateSchool.SchoolIds?.Any() == true)
                {
                    await AddSubscriptionAndAcademicDetailsToEntity(schoolsModifierManager, schoolsReader, langCode, subscriptionRelateSchool, userInSession, platformId);
                }

                if (subscriptionRelateSchool.ProgramIds?.Any() == true)
                {
                    await AddSubscriptionAndAcademicDetailsToEntity(schoolsModifierManager, schoolsReader, langCode, subscriptionRelateSchool, userInSession, platformId);
                }

                return Created("", null);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// To get the entities related to a subscription's id.
        /// </summary>
        /// <param name="subscriptionId">Subscription's id to find the entities related.</param>
        /// <returns>Returns a HTTP Response with school relations.</returns>
        [HttpGet("admin/schools-entities-related/{subscriptionId}")]
        [ProducesResponseType(typeof(List<EntityModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetSubscriptionRelatedSchoolsAndPrograms(string subscriptionId)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId))
            {
                return BadRequest();
            }

            var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "SubscriptionAdvanceRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            var langCode = RequestHandler.GetLanguage(HttpContext);
            // Direct instantiation
            var schoolsReader = new SchoolsReader(Settings.CONNECTION_STRING_READ, langCode);

            var entitySubscriptionModelList = schoolsReader.GetEntitySubcriptionModelList(new List<EntitySubscriptionModel>
        {
            new EntitySubscriptionModel { SubscriptionId = subscriptionId }
        });

            if (!entitySubscriptionModelList.Any())
            {
                return Ok(new List<EntityModel>());
            }

            var entitiesToFind = entitySubscriptionModelList.Select(es => new EntityModel { Id = es.EntityId }).ToList();
            var entitiesRelated = schoolsReader.GetEntityModels(entitiesToFind, false);

            return Ok(entitiesRelated);
        }

        /// <summary>
        /// To get the entity subscription model according to the list of entities list requested.
        /// This is used from API to API.
        /// </summary>
        /// <param name="entityIds">A string array including the entities' ids (the entities are schools, programs, etc).</param>
        /// <returns>Returns the entity models.</returns>
        [HttpPost("get_subscription_school_program")]
        [ProducesResponseType(typeof(List<EntitySubscriptionModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetEntitySubscriptionBySchoolAndProgramModelList([FromBody] List<string> entityIds)
        {
            if (entityIds == null || !entityIds.Any())
            {
                return BadRequest();
            }

            var langCode = RequestHandler.GetLanguage(HttpContext);
            var responseValidateKey = ValidateAPIKey.ValidateKey(RequestHandler.GetAPIKey(HttpContext), langCode);
            if (!responseValidateKey.Valid)
            {
                return Unauthorized();
            }

            // Direct instantiation
            var schoolsReader = new SchoolsReader(Settings.CONNECTION_STRING_READ, langCode);

            var entitySubscriptions = schoolsReader.GetEntitySubcriptionBySchoolAndProgramModelList(
                entityIds,
                Settings.SCH_SCHOOL_TYPE_ID,
                Settings.SCH_PROGRAM_TYPE_ID);

            return Ok(entitySubscriptions);
        }

        /// <summary>
        /// Private function to add academic details to an entity and to create the subscription relation to the entity.
        /// </summary>
        private async Task AddSubscriptionAndAcademicDetailsToEntity(SchoolModifierManager schoolModifierManager, SchoolsReader schoolsReader, string langCode, SubscriptionRelateRequestModel subscriptionRelateSchool, string userInSession, string platformId)
        {
            string entityId = subscriptionRelateSchool.SchoolIds?.FirstOrDefault() ?? subscriptionRelateSchool.ProgramIds?.FirstOrDefault();

            if (string.IsNullOrEmpty(entityId))
            {
                return;
            }

            var entitySubscriptionResponseModel = schoolModifierManager.CreateEntitySubscription(
                new EntitySubscriptionModel
                {
                    EntityId = entityId,
                    SubscriptionId = subscriptionRelateSchool.SubscriptionId,
                    PlatformId = platformId,
                }, subscriptionRelateSchool.UserId);

            if (!string.IsNullOrWhiteSpace(subscriptionRelateSchool.EntityAcademicDetailId))
            {
                SchoolsAPI.Infrastructure.Repositories.DapperHandler.AcademicDetails.RelateEntitySubscriptionWithAcademicDetails(entitySubscriptionResponseModel.Id, subscriptionRelateSchool.EntityAcademicDetailId, userInSession);
            }
        }
    }
}
