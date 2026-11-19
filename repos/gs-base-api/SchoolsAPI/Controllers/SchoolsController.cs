using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Responses;
using SchoolsAPI.SchoolsAPI.Services.Schools;
using SchoolsAPI.CustomClasses;
using SchoolsAPI.SchoolsAPI.Services.FeignAPIs;

namespace SchoolsAPI.Controllers
{
    [ApiController]
    [Route("schoolsapi")]
    public class SchoolsController : ControllerBase
    {
        /// <summary>
        /// Returns the entities list by filters. Example of entities: schools, programs, corporations, etc.
        /// </summary>
        [HttpGet("entities")]
        public async Task<IActionResult> ReadEntities(
            int pageFrom = 0,
            int limiter = 10,
            string? entityTypeId = null,
            string? name = null,
            string? phone = null,
            string? state = null,
            string? city = null,
            string? entityId = null,
            string? countryIsoCode = null,
            string? privatePublicSchool = null)
        {
            var generalResponseModel = new SchoolEntityModel();

            try
            {
                // This is a free-access endpoint; if a valid session is present, filter by ORE for that user.
                string? oreId = null;
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesRead");

                if (responseSession.SessionValid)
                {
                    oreId = responseSession.OreId;
                }

                generalResponseModel.SchoolEntities =
                    SchoolsAPI.Infrastructure.Repositories.DapperHandler.EntityDetails.GetEntities(
                        entityId,
                        entityTypeId,
                        name,
                        oreId, // filter by user's ORE if present
                        phone,
                        state,
                        city,
                        countryIsoCode,
                        privatePublicSchool
                    );

                // Pagination and ordering
                var total = generalResponseModel.SchoolEntities.Count;
                generalResponseModel.PageTotals = total == 0 || limiter <= 0
                    ? 0
                    : (int)Math.Ceiling(total / (double)limiter);

                generalResponseModel.SchoolEntities = generalResponseModel.SchoolEntities
                    .OrderBy(x => x.EntityName)
                    .Skip(pageFrom * Math.Max(limiter, 1))
                    .Take(Math.Min(limiter, total))
                    .ToList();

                return Ok(generalResponseModel);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns the entities' models. Open endpoint (no token check). Avoid huge unfiltered calls.
        /// </summary>
        [HttpPost("entities/entity")]
        public async Task<IActionResult> ReadEntity(SchoolsAPI.Domain.Models.Generals.Requests.SchoolEntitiesRequestModel schoolEntityRequest)
        {
            try
            {
                var reader = new SchoolsReader(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var result = reader.GetEntityModels(schoolEntityRequest.EntityModifier?.Entities, true);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Reads the entity details, including relations, name, etc.
        /// </summary>
        [HttpGet("entities/entity/{entityId}/details")]
        public async Task<IActionResult> ReadEntityDetails(string entityId)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesRead");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                // If the user belongs to an ORE, ensure it can handle this school
                if (!string.IsNullOrWhiteSpace(responseSession.OreId))
                {
                    var canHandle = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails
                        .CheckIfOreCanHandleSchool(responseSession.OreId, entityId);

                    if (!canHandle)
                        return Unauthorized();
                }

                var reader = new SchoolsReader(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var result = reader.GetEntityWithDetails(
                    new SchoolsAPI.Domain.Models.Schools.EntityModel { Id = entityId },
                    AppConfiguration.Configuration["AppSettings:sch_program_type_id"],
                    AppConfiguration.Configuration["AppSettings:sch_institution_type_id"],
                    AppConfiguration.Configuration["AppSettings:sch_company_type_id"],
                    AppConfiguration.Configuration["AppSettings:sch_school_type_id"]);

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Creates or updates an entity. Send Id when updating; omit Id when creating.
        /// </summary>
        [HttpPost("entity/modify")]
        public async Task<IActionResult> EntityModify(SchoolsAPI.Domain.Models.Generals.Requests.SchoolEntitiesRequestModel entityRequest)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesUpdate");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var entityId = entityRequest?.EntityModifier?.Entities?.FirstOrDefault()?.Id;

                if (!string.IsNullOrWhiteSpace(responseSession.OreId) && !string.IsNullOrWhiteSpace(entityId))
                {
                    var canHandle = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails
                        .CheckIfOreCanHandleSchool(responseSession.OreId, entityId);

                    if (!canHandle)
                        return Unauthorized();
                }

                var modifier = new SchoolModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var response = modifier.ModifyEntity(entityRequest.EntityModifier, responseSession.UserId);

                if (!response.Valid)
                    return BadRequest(new GeneralResponseModel { Messages = response.Messages });

                return Ok(response.Entities[0].Entities);
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Creates or updates an entity address.
        /// </summary>
        [HttpPost("addresses/modify")]
        public async Task<IActionResult> AddressModify(SchoolsAPI.Domain.Models.Generals.Requests.SchoolEntitiesRequestModel addressRequest)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesUpdate");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var address = addressRequest?.EntityModifier?.Addresses?.FirstOrDefault();
                if (address is null)
                    return BadRequest();

                if (!string.IsNullOrWhiteSpace(responseSession.OreId) &&
                    !string.Equals(responseSession.OreId, address.RegionId, StringComparison.Ordinal))
                {
                    return Unauthorized();
                }

                var modifier = new SchoolModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var resp = modifier.ModifyAddress(address, responseSession.UserId);

                if (!resp.Valid)
                    return BadRequest(new GeneralResponseModel { Messages = resp.Messages });

                return Ok(new GeneralResponseModel { Messages = resp.Messages });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes an entity address.
        /// </summary>
        [HttpPost("addresses/delete")]
        public async Task<IActionResult> AddressDelete(SchoolsAPI.Domain.Models.Generals.Requests.SchoolEntitiesRequestModel addressRequest)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesUpdate");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var address = addressRequest?.EntityModifier?.Addresses?.FirstOrDefault();
                if (address is null)
                    return BadRequest();

                if (!string.IsNullOrWhiteSpace(responseSession.OreId))
                {
                    var canHandle = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails
                        .CheckIfOreCanHandleSchool(responseSession.OreId, address.EntityId);

                    if (!canHandle)
                        return Unauthorized();
                }

                var modifier = new SchoolModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var resp = modifier.DeleteAddress(address);

                if (!resp.Valid)
                    return BadRequest(new GeneralResponseModel { Messages = resp.Messages });

                return Ok(new GeneralResponseModel { Messages = resp.Messages });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Creates or updates an entity phone.
        /// </summary>
        [HttpPost("phones/modify")]
        public async Task<IActionResult> PhonesModify(SchoolsAPI.Domain.Models.Generals.Requests.SchoolEntitiesRequestModel phoneRequest)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesUpdate");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var phone = phoneRequest?.EntityModifier?.Phones?.FirstOrDefault();
                if (phone is null)
                    return BadRequest();

                if (!string.IsNullOrWhiteSpace(responseSession.OreId))
                {
                    var canHandle = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails
                        .CheckIfOreCanHandleSchool(responseSession.OreId, phone.EntityId);

                    if (!canHandle)
                        return Unauthorized();
                }

                var modifier = new SchoolModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var resp = modifier.ModifyPhone(phone, responseSession.UserId);

                if (!resp.Valid)
                    return BadRequest(new GeneralResponseModel { Messages = resp.Messages });

                return Ok(new GeneralResponseModel { Messages = resp.Messages });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes an entity phone.
        /// </summary>
        [HttpPost("phones/delete")]
        public async Task<IActionResult> PhonesDelete(SchoolsAPI.Domain.Models.Generals.Requests.SchoolEntitiesRequestModel phoneRequest)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesUpdate");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var phone = phoneRequest?.EntityModifier?.Phones?.FirstOrDefault();
                if (phone is null)
                    return BadRequest();

                if (!string.IsNullOrWhiteSpace(responseSession.OreId))
                {
                    var canHandle = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails
                        .CheckIfOreCanHandleSchool(responseSession.OreId, phone.EntityId);

                    if (!canHandle)
                        return Unauthorized();
                }

                var modifier = new SchoolModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var resp = modifier.DeletePhone(phone);

                if (!resp.Valid)
                    return BadRequest(new GeneralResponseModel { Messages = resp.Messages });

                return Ok(new GeneralResponseModel { Messages = resp.Messages });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Adds a relation between entities (e.g., school to institution).
        /// </summary>
        [HttpPost("relations/add")]
        public async Task<IActionResult> RelationsAdd(SchoolsAPI.Domain.Models.Generals.Requests.SchoolEntitiesRequestModel relationRequest)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesUpdate");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var relation = relationRequest?.EntityModifier?.Relations?.FirstOrDefault();
                if (relation is null)
                    return BadRequest();

                if (!string.IsNullOrWhiteSpace(responseSession.OreId))
                {
                    var canHandleA = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails
                        .CheckIfOreCanHandleSchool(responseSession.OreId, relation.EntityId);
                    var canHandleB = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails
                        .CheckIfOreCanHandleSchool(responseSession.OreId, relation.RelationEntityId);

                    if (!canHandleA || !canHandleB)
                        return Unauthorized();
                }

                var modifier = new SchoolModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var resp = modifier.AddRelation(relation, responseSession.UserId);

                if (!resp.Valid)
                    return BadRequest(new GeneralResponseModel { Messages = resp.Messages });

                return StatusCode(StatusCodes.Status201Created, new GeneralResponseModel { Messages = resp.Messages });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Deletes a relation between entities.
        /// </summary>
        [HttpPost("relations/delete")]
        public async Task<IActionResult>  RelationsDelete(SchoolsAPI.Domain.Models.Generals.Requests.SchoolEntitiesRequestModel relationRequest)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(
                    HttpContext,
                    "SchoolEntitiesUpdate");

                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                    return Unauthorized();

                var relation = relationRequest?.EntityModifier?.Relations?.FirstOrDefault();
                if (relation is null)
                    return BadRequest();

                if (!string.IsNullOrWhiteSpace(responseSession.OreId))
                {
                    var canHandleA = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails
                        .CheckIfOreCanHandleSchool(responseSession.OreId, relation.EntityId);
                    var canHandleB = SchoolsAPI.Infrastructure.Repositories.DapperHandler.OreDetails
                        .CheckIfOreCanHandleSchool(responseSession.OreId, relation.RelationEntityId);

                    if (!canHandleA || !canHandleB)
                        return Unauthorized();
                }

                var modifier = new SchoolModifierManager(
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnection"],
                    AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                    RequestHandler.GetLanguage(HttpContext));

                var resp = modifier.DeleteRelation(relation);

                if (!resp.Valid)
                    return BadRequest(new GeneralResponseModel { Messages = resp.Messages });

                return Ok(new GeneralResponseModel { Messages = resp.Messages });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Returns generic details for a list of entities. Validates an API key (not user token).
        /// </summary>
        [HttpPost("schools/generic-details/by-list")]
        public async Task<IActionResult> GetSchoolGenericDetails(List<string> schoolIds)
        {
            var apiKey = RequestHandler.GetAPIKey(HttpContext);
            var langCode = RequestHandler.GetLanguage(HttpContext);

            if (string.IsNullOrWhiteSpace(apiKey))
                return Unauthorized();

            var apiKeyResponse = SchoolsAPI.Services.Generals.ValidateAPIKey.ValidateKey(apiKey, langCode);
            if (apiKeyResponse == null || !apiKeyResponse.Valid)
                return Unauthorized();

            var entitySchoolsToFind = new List<SchoolsAPI.Domain.Models.Schools.EntityModel>();
            var addressSchoolsToFind = new List<SchoolsAPI.Domain.Models.Schools.AddressModel>();

            foreach (var id in schoolIds ?? new List<string>())
            {
                entitySchoolsToFind.Add(new SchoolsAPI.Domain.Models.Schools.EntityModel { Id = id });
                addressSchoolsToFind.Add(new SchoolsAPI.Domain.Models.Schools.AddressModel { EntityId = id });
            }

            var reader = new SchoolsReader(
                AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"],
                langCode);

            var entitySchools = reader.GetEntityModels(entitySchoolsToFind, true) ?? new List<SchoolsAPI.Domain.Models.Schools.EntityModel>();
            var addressSchools = reader.GetAddressModels(addressSchoolsToFind) ?? new List<SchoolsAPI.Domain.Models.Schools.AddressModel>();
            var regions = reader.ReadRegions(new SchoolsAPI.Domain.Models.Schools.RegionModel()) ?? new List<SchoolsAPI.Domain.Models.Schools.RegionModel>();

            var result = new List<EntityGeneralDetailsResponseModel>();

            foreach (var entity in entitySchools)
            {
                var dto = new EntityGeneralDetailsResponseModel
                {
                    EntityId = entity.Id,
                    EntityIdentifier = entity.Token,
                    EntityName = entity.Name,
                    IsPrivate = entity.Private
                };

                var addr = addressSchools.Find(x => x.EntityId == entity.Id);
                if (addr != null)
                {
                    dto.AddressLine1 = addr.AddressLine1;
                    dto.AddressLine2 = addr.AddressLine2;
                    dto.City = addr.City;
                    dto.Country = addr.Country;
                    dto.ZipCode = addr.Zip;
                    dto.State = addr.State;
                    dto.Region = regions.Find(x => x.Id == addr.RegionId)?.Name;
                }

                result.Add(dto);
            }

            return Ok(result);
        }
    }
}
