using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SchoolsAPI.CustomClasses;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Requests;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Responses;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using SchoolsAPI.SchoolsAPI.Infrastructure.Repositories.DapperHandler;
using SchoolsAPI.SchoolsAPI.Services.Schools;

using SchoolsAPI.SchoolsAPI.Services.FeignAPIs;
using SchoolsAPI.SchoolsAPI.Services.Generals;

namespace SchoolsAPI.Controllers
{
    /// <summary>
    /// Handles academic details related endpoints.
    /// </summary>
    [ApiController]
    [Route("schoolsapi/academics")]
    public class AcademicDetailsController : ControllerBase
    {
        /// <summary>
        /// To get the academic periods that include today's date, by school ids.
        /// </summary>
        /// <param name="schoolIds">The school ids, comma separated.</param>
        /// <returns>Returns the current academic periods.</returns>
        [HttpGet("periods/current")]
        public async Task<IActionResult> GetCurrentAcademicPeriods([FromQuery] string schoolIds)
        {
            try
            {
                // Validate permission
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext,
                    "AcademicYearDetails");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                if (string.IsNullOrWhiteSpace(schoolIds))
                {
                    return BadRequest();
                }

                // Parse the comma separated school ids.
                var schoolIdList = schoolIds.Split(',')
                    .Select(x => int.Parse(x.Trim()))
                    .ToList();

                return Ok(AcademicDetails.GetCurrentAcademicPeriods(schoolIdList));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Retrieve just the IDs of active academic years for the given schools.
        /// </summary>
        [HttpPost("entity/academic-year-ids/active")]
        public async Task<IActionResult> GetActiveAcademicYearsIdsOfSchools([FromBody] List<string> schoolIds)
        {
            // Validate permission
            var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext,
                "SchoolEntitiesRead");

            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            // Fetch and return only IDs
            var academics = AcademicDetails.GetEntityAcademicModelsActive(schoolIds);
            var ids = academics.Select(a => a.Id).ToList();
            return Ok(ids);
        }

        /// <summary>
        /// Get academic detail headers for an entity (paged).
        /// </summary>
        [HttpGet("details/entity/{entityId}")]
        public async Task<IActionResult> GetEntityAcademicDetailsByEntityId(string entityId, int fromPage = 0, int limit = 10)
        {
            // Validate permission
            var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                HttpContext,
                "SubscriptionAdvanceRead");

            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            // If user has an ORE, ensure they can access this entity
            if (!string.IsNullOrWhiteSpace(session.OreId) &&
                !OreDetails.CheckIfOreCanHandleSchool(session.OreId, entityId))
                return Unauthorized();

            var response = new EntityAcademicDetailResponseModel
            {
                EntityAcademicDetailModels = new List<SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel>()
            };

            var items = AcademicDetails.GetEntityAcademicModels(entityId);
            if (items == null || items.Count == 0)
                return Ok(response);

            var total = items.Count;
            response.PageTotals = (int)Math.Ceiling(total / (decimal)limit);
            response.EntityAcademicDetailModels = items
                .Skip(fromPage * limit)
                .Take(limit > total ? total : limit)
                .ToList();

            return Ok(response);
        }

        /// <summary>
        /// Get a specific academic detail for an entity.
        /// </summary>
        [HttpGet("details/entity/{entityId}/{entityAcademicDetailId}")]
        public async Task<IActionResult> GetEntityAcademicDetailsByEntityIdDetail(string entityId, string entityAcademicDetailId)
        {
            // Validate permission
            var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                HttpContext,
                "SubscriptionAdvanceRead");

            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            return Ok(AcademicDetails.GetAcademicDetails(entityAcademicDetailId));
        }

        /// <summary>
        /// Get active academic period IDs. Accepts X-API-KEY or user token.
        /// </summary>
        [HttpGet("active/academic-years")]
        public async Task<IActionResult> GetActiveAcademicYears()
        {
            var apiKey = Request.Headers.TryGetValue("X-API-KEY", out var keyVals) ? keyVals.FirstOrDefault() : null;
            var internalKey = AppConfiguration.Configuration["AppSettings:InternalApiKey"];

            if (!string.IsNullOrWhiteSpace(apiKey) && apiKey == internalKey)
                return Ok(AcademicDetails.GetActiveAcademicPeriodIds());

            var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                HttpContext,
                "SubscriptionAdvanceRead");

            if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                return Unauthorized();

            return Ok(AcademicDetails.GetActiveAcademicPeriodIds());
        }

        /// <summary>
        /// Read academic periods (entity + periods) by subscription ID.
        /// </summary>
        [HttpGet("details/subscription/read/{subscriptionId}")]
        public async Task<IActionResult> GetAcademicDetails(string subscriptionId)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext, "SubscriptionAdvanceRead");
       

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var result = new SubscriptionRelateRequestModel();
                var periods = AcademicDetails.GetAcademicDetailsWithPeriodsBySubscriptionId(new List<string> { subscriptionId });

                if (periods == null || periods.Count == 0)
                {
                    result.AcademicPeriods = new List<AcademicPeriodModel>();
                    result.EntityAcademicDetail = new SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel();
                }
                else
                {
                    result.AcademicPeriods = periods[0].AcademicPeriodModelList;
                    result.EntityAcademicDetail = periods[0].EntityAcademicDetailModel;
                }

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Read academic periods for a list of subscription IDs. Accepts token or API key.
        /// </summary>
        [HttpPost("details/subscriptions/read")]
        public async Task<IActionResult> GetAcademicDetailsBySubscriptions([FromBody] List<string> subscriptionIds)
        {
            try
            {
                var token = RequestHandler.GetToken(HttpContext);
                var lang = RequestHandler.GetLanguage(HttpContext);
                var apiKey = RequestHandler.GetAPIKey(HttpContext);

                if (string.IsNullOrWhiteSpace(token) && string.IsNullOrWhiteSpace(apiKey))
                    return Unauthorized();

                if (!string.IsNullOrWhiteSpace(token))
                {
                    var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                        HttpContext,
                        "SubscriptionAdvanceRead");

                    if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                        return Unauthorized();
                }
                else
                {
                    var apiKeyValidation = SchoolsAPI.Services.Generals.ValidateAPIKey.ValidateKey(apiKey, lang);
                    if (apiKeyValidation == null || !apiKeyValidation.Valid)
                        return Unauthorized();
                }

                var payload = AcademicDetails.GetAcademicDetailsWithPeriodsBySubscriptionId(subscriptionIds);
                return Ok(payload);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Read academic periods by a list of period models.
        /// </summary>
        [HttpPost("periods/read/list")]
        public async Task<IActionResult> ReadPeriodsByPeriods([FromBody] List<AcademicPeriodModel> academicPeriodModels)
        {
            try
            {
                var reader = new SchoolsReader(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                return Ok(reader.GetAcademicPeriodModels(academicPeriodModels));
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update academic details (entity header + periods).
        /// </summary>
        [HttpPost("details/modify/{entityAcademicDetailId}")]
        public async Task<IActionResult> UpdateAcademicDetails(string entityAcademicDetailId, [FromBody] SubscriptionRelateRequestModel model)
        {
            try
            {
                var session =  await GetUserDetails.GetUserIdRequestOnOtherAPI(
                   HttpContext,
                    "SchoolEntitiesUpdate");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                // Ensure the academic detail exists
                var existing = AcademicDetails.GetEntityAcademicDetails(entityAcademicDetailId);
                if (existing == null || existing.Count == 0)
                    return Unauthorized();

                // Validate header
                var validateHeader = Validator.ValidateSchoolAcademicDetails(
                    new EntityAcademicDetailModel
                    {
                        FullTermEndDate = model.EntityAcademicDetail.FullTermEndDate,
                        FullTermStartDate = model.EntityAcademicDetail.FullTermStartDate,
                        DaysToClosePeriod = model.EntityAcademicDetail.DaysToClosePeriod,
                        Id = model.EntityAcademicDetail.Id,
                        PeriodQuantity = model.EntityAcademicDetail.PeriodQuantity,
                        TermId = model.EntityAcademicDetail.TermId
                    },
                    isEdit: true,
                    includeEntitySubscriptionId: false,
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                if (!validateHeader.Valid)
                    return BadRequest(validateHeader.Messages);

                // Split inserts/updates
                var toInsert = new List<AcademicPeriodModel>();
                var toUpdate = new List<AcademicPeriodModel>();
                foreach (var p in model.AcademicPeriods)
                {
                    p.EntityAcademicDetailId = entityAcademicDetailId;
                    if (string.IsNullOrWhiteSpace(p.Id)) toInsert.Add(p);
                    else toUpdate.Add(p);
                }

                // Period count must match header
                if (model.AcademicPeriods.Count != int.Parse(model.EntityAcademicDetail?.PeriodQuantity ?? "0"))
                    return BadRequest();

                if (toInsert.Count > 0)
                {
                    var v = Validator.ValidateSchoolPeriods(
                        toInsert,
                        false,
                         false,
                        AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                        RequestHandler.GetLanguage(HttpContext));
                    if (!v.Valid) return BadRequest(v.Messages);
                }

                if (toUpdate.Count > 0)
                {
                    var v = Validator.ValidateSchoolPeriods(
                        toUpdate,
                         true,
                         true,
                        AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                        RequestHandler.GetLanguage(HttpContext));
                    if (!v.Valid) return BadRequest(v.Messages);
                }

                // Persist
                var modifier = new SchoolModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                modifier.UpdateEntityAcademicDetails(new EntityAcademicDetailModel
                {
                    FullTermEndDate = model.EntityAcademicDetail?.FullTermEndDate,
                    FullTermStartDate = model.EntityAcademicDetail?.FullTermStartDate,
                    DaysToClosePeriod = model.EntityAcademicDetail?.DaysToClosePeriod,
                    PeriodQuantity = model.EntityAcademicDetail?.PeriodQuantity,
                    TermId = model.EntityAcademicDetail?.TermId,
                    Id = entityAcademicDetailId
                }, session.UserId);

                if (toInsert.Count > 0)
                {
                    foreach (var p in toInsert) p.EntityAcademicDetailId = entityAcademicDetailId;
                    modifier.CreateAcademicPeriods(toInsert, session.UserId);
                }

                if (toUpdate.Count > 0)
                    modifier.UpdateAcademicPeriods(toUpdate, session.UserId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new GeneralResponseModel
                {
                    Messages = new List<string> { ex.ToString() }
                });
            }
        }

        /// <summary>
        /// Create academic details (header + periods) for an entity.
        /// </summary>
        [HttpPost("details/{entityId}/create")]
        public async Task<IActionResult> CreateAcademicDetails(string entityId, [FromBody] SubscriptionRelateRequestModel model)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesUpdate");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                // If user has ORE, ensure they can access the entity
                if (!string.IsNullOrWhiteSpace(session.OreId) &&
                    !OreDetails.CheckIfOreCanHandleSchool(session.OreId, entityId))
                    return Unauthorized();

                var validateHeader = Validator.ValidateSchoolAcademicDetails(
                    new EntityAcademicDetailModel
                    {
                        FullTermEndDate = model?.EntityAcademicDetail?.FullTermEndDate,
                        FullTermStartDate = model?.EntityAcademicDetail?.FullTermStartDate,
                        EntityId = entityId,
                        DaysToClosePeriod = model?.EntityAcademicDetail?.DaysToClosePeriod,
                        PeriodQuantity = model?.EntityAcademicDetail?.PeriodQuantity,
                        TermId = model?.EntityAcademicDetail?.TermId
                    },
                     false,
                     false,
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                if (!validateHeader.Valid)
                    return BadRequest(validateHeader);

                // Validate periods to create
                var validatePeriods = Validator.ValidateSchoolPeriods(
                    model.AcademicPeriods,
                     false,
                     false,
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));
                if (!validatePeriods.Valid)
                    return BadRequest(validatePeriods);

                var modifier = new SchoolModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var created = modifier.CreateEntityAcademicDetails(new EntityAcademicDetailModel
                {
                    FullTermStartDate = model?.EntityAcademicDetail?.FullTermStartDate,
                    FullTermEndDate = model?.EntityAcademicDetail?.FullTermEndDate,
                    DaysToClosePeriod = model?.EntityAcademicDetail?.DaysToClosePeriod,
                    EntityId = entityId,
                    PeriodQuantity = model?.EntityAcademicDetail?.PeriodQuantity,
                    TermId = model?.EntityAcademicDetail?.TermId
                }, session.UserId);

                if (string.IsNullOrWhiteSpace(created.Id))
                    return BadRequest();

                if (model.AcademicPeriods != null && model.AcademicPeriods.Count > 0)
                {
                    foreach (var p in model.AcademicPeriods)
                        p.EntityAcademicDetailId = created.Id;

                    if (model.AcademicPeriods.Count == int.Parse(created.PeriodQuantity))
                    {
                        modifier.CreateAcademicPeriods(model.AcademicPeriods, session.UserId);
                    }
                }
                created.Valid = true;
                created.Messages = new List<string>()
                {
                    new MessageManager(
                        AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                        RequestHandler.GetLanguage(HttpContext))
                    .GetMessageDetails(AppConfiguration.Configuration["AppSettings:sch_entity_success"])
                };

                return StatusCode(201, created);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Get full period details by list of period IDs (optional entity filter).
        /// </summary>
        [HttpPost("details/periods-details")]
        public async Task<IActionResult>  GetAcademicDetailsByFullPeriodsList([FromBody] List<string> periodsIdsList, [FromQuery] string? entityId = null)
        {
            try
            {
                var session = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "ReadAcademicPeriodsFullDetails");

                if (!session.SessionValid || string.IsNullOrWhiteSpace(session.UserId))
                    return Unauthorized();

                var payload = AcademicDetails.GetFullDetailsPeriodsList(periodsIdsList, entityId);
                return Ok(payload);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
