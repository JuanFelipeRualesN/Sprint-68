using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeachersAPI;
using TeachersAPI.CustomClasses;
using TeachersAPI.Domain.Models.Events;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Generals.Responses.APIs;
using TeachersAPI.TeachersAPI.Services.Events;
using TeachersAPI.TeachersAPI.Services.FeignAPI;
using TeachersAPI.TeachersAPI.Infrastructure.DapperHandler;

namespace TeachersAPI.Controllers
{
    /// <summary>
    /// To handle events. Events are calendar entries that can be assigned to roles, schools, regions and users.
    /// </summary>
    [ApiController]
    [Route("teachersapi/events")]
    public class EventController : ControllerBase
    {
        /// <summary>
        /// Gets the lesson, lesson-quiz, and homework events for the authenticated
        /// teacher within the given month and year. 
        /// For lessons, Athenas is called, including the lessons with the units.
        /// </summary>
        /// <param name="month">The month (1-12).</param>
        /// <param name="year">The four-digit year.</param>
        /// <returns>Returns the event response models for the given month and year.</returns>
        [HttpGet]
        [Route("teacher")]
        public async Task<IActionResult> GetEventsByMonthAndYear([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "EventRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }


                if (month < 1 || month > 12)
                {
                    return BadRequest("Month must be between 1 and 12.");
                }

                if (year < 1)
                {
                    return BadRequest("Year must be a valid four-digit year.");
                }

                if (!int.TryParse(responseSession.UserId, out int userId))
                {
                    return Unauthorized();
                }

                var oreIds = new List<string>();
                var userSchoolIds = new List<int>();
                bool isAdmin = responseSession.RoleId == EventAudienceRoles.Admin;
                bool isAdminOre = responseSession.RoleId == EventAudienceRoles.AdminOre;

                // Central Admin has global event access and does not need school/region lookup.
                // Admin ORE is identified by role, not by the absence of schools. Teachers and
                // students can also legitimately have no school assigned.
                if (isAdmin)
                {
                    // No audience identifiers are needed; the data query grants global scope.
                }
                else if (isAdminOre)
                {
                    if (string.IsNullOrWhiteSpace(responseSession.OreId))
                    {
                        return Unauthorized();
                    }

                    oreIds.Add(responseSession.OreId);
                }
                else
                {
                    var userSchools = await GetUserDetails.GetUserSchoolsAsync(responseSession.Token);
                    if (userSchools != null)
                    {
                        foreach (var school in userSchools)
                        {
                            if (int.TryParse(school.InstitutionId, out int schoolId) &&
                                !userSchoolIds.Contains(schoolId))
                            {
                                userSchoolIds.Add(schoolId);
                            }
                        }

                        if (userSchoolIds.Count > 0)
                        {
                            var schoolRegions = await SchoolsDetailsHandler.GetSchoolOresAsync(
                                userSchoolIds,
                                responseSession.Token);

                            foreach (var region in schoolRegions)
                            {
                                if (!string.IsNullOrWhiteSpace(region.RegionId) &&
                                    !oreIds.Contains(region.RegionId))
                                {
                                    oreIds.Add(region.RegionId);
                                }
                            }
                        }
                    }
                }

                var service = new EventsService();
                var events = service.GetEventsByMonthAndYear(month, year, userId, responseSession.RoleId, userSchoolIds, oreIds, RequestHandler.GetLanguage(HttpContext));
                return Ok(events);
            }
            catch (Exception e)
            {
                // Return error about invalid request
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// Gets the calendar events (events module) assigned to the authenticated student
        /// within the given month and year. Lessons, exams and homework are not included here -
        /// students already have their own dedicated calendar endpoints for those.
        /// </summary>
        /// <param name="month">The month (1-12).</param>
        /// <param name="year">The four-digit year.</param>
        /// <returns>Returns the calendar-event response models for the given month and year.</returns>
        [HttpGet]
        [Route("student")]
        public async Task<IActionResult> GetStudentEventsByMonthAndYear([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningStudentRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                if (month < 1 || month > 12)
                {
                    return BadRequest("Month must be between 1 and 12.");
                }

                if (year < 1)
                {
                    return BadRequest("Year must be a valid four-digit year.");
                }

                if (!int.TryParse(responseSession.UserId, out int userId))
                {
                    return Unauthorized();
                }

                // Call the user schools and their regions, same pattern as the teacher calendar.
                var userSchools = await GetUserDetails.GetUserSchoolsAsync(responseSession.Token);
                var oreIds = new List<string>();
                var userSchoolIds = new List<int>();

                if (userSchools != null)
                {
                    foreach (var school in userSchools)
                    {
                        if (int.TryParse(school.InstitutionId, out int schoolId) &&
                            !userSchoolIds.Contains(schoolId))
                        {
                            userSchoolIds.Add(schoolId);
                        }
                    }

                    if (userSchoolIds.Count > 0)
                    {
                        var schoolRegions = await SchoolsDetailsHandler.GetSchoolOresAsync(
                            userSchoolIds,
                            responseSession.Token);

                        foreach (var region in schoolRegions)
                        {
                            if (!string.IsNullOrWhiteSpace(region.RegionId) &&
                                !oreIds.Contains(region.RegionId))
                            {
                                oreIds.Add(region.RegionId);
                            }
                        }
                    }
                }

                var events = EventsDapperHandler.GetCalendarEventsOnly(
                    month, year, EventAudienceRoles.Student, userId, userSchoolIds, oreIds);

                return Ok(events);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        /// <summary>
        /// Returns a paginated list of events.
        /// </summary>
        /// <param name="title">Text to filter by title.</param>
        /// <param name="available">Filter by availability. Example: 1</param>
        /// <param name="status">Filter by the computed functional status. Example: ongoing</param>
        /// <param name="role">Filter by audience RoleId (Director/Teacher/Student, see EventAudienceRoles). Example: 5</param>
        /// <param name="fromPage">The current page (0-based). Example: 0</param>
        /// <param name="limit">How many items per page. Example: 10</param>
        /// <param name="sortDescending">Legacy parameter retained for client compatibility. Events are ordered by creation date, newest first.</param>
        /// <returns>Returns the events list with pagination details.</returns>
        [HttpGet]
        public async Task<IActionResult> GetEvents(string title = null, string available = null, string status = null, string role = null, int fromPage = 0, int limit = 10, bool sortDescending = false)
        {
            try
            {
                
                ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "EventRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                EventModel filter = new EventModel()
                {
                    Title = title,
                    Available = available,
                    Status = status,
                    AudienceRoleId = role,
                };

                return Ok(new EventReaderManager(
                    AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                    responseSession.LangCode).GetEvents(
                        filter,
                        fromPage,
                        limit,
                        responseSession.OreId));
            }
            catch(Exception)
            {
                return Ok(new EventsResponseModel());
            }
        }

        /// <summary>
        /// Returns the full details of a single event including all related entities.
        /// </summary>
        /// <param name="eventId">The id of the event.</param>
        /// <returns>Returns the event with its relations assembled.</returns>
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEvent(string eventId)
        {
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "EventRead");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            EventModifierRequestModel response = new EventReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).GetEvent(eventId, responseSession.OreId);

            if (!string.IsNullOrWhiteSpace(responseSession.OreId) && response.EventModel == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        /// <summary>
        /// Creates a new event with its related entities.
        /// </summary>
        /// <param name="request">The event and its relations to create.</param>
        /// <returns>Returns the created event details.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] EventModifierRequestModel request)
        {
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "EventHandler");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            if (request == null || request.EventModel == null)
            {
                return BadRequest();
            }

            ScopeRequestToSessionRegion(request, responseSession.OreId);

            EventOrganizerModel organizer = new EventOrganizerModel()
            {
                Email = responseSession.Email,
                Token = responseSession.Token,
                FullName = responseSession.FullName,
            };

            var response = await new EventModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).CreateEvent(request, responseSession.UserId, organizer);

            return !response.Valid
                ? BadRequest(response)
                : StatusCode(StatusCodes.Status201Created, response);
        }

        /// <summary>
        /// Updates an existing event and its related entities.
        /// </summary>
        /// <param name="eventId">The id of the event to update.</param>
        /// <param name="request">The event and its relations to update.</param>
        /// <returns>Returns the updated event details.</returns>
        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateEvent(string eventId, EventModifierRequestModel request)
        {
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "EventHandler");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            if (request == null || request.EventModel == null)
            {
                return BadRequest();
            }

            if (!CanAccessEvent(eventId, responseSession))
            {
                return NotFound();
            }

            request.EventModel.Id = eventId;
            ScopeRequestToSessionRegion(request, responseSession.OreId);

            EventOrganizerModel organizer = new EventOrganizerModel()
            {
                Email = responseSession.Email,
                Token = responseSession.Token,
                FullName = responseSession.FullName,
            };

            var response = await new EventModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).UpdateEvent(request, responseSession.UserId, organizer);

            return !response.Valid
                ? BadRequest(response)
                : Ok(response);
        }

        /// <summary>
        /// Archives an event (soft delete).
        /// </summary>
        /// <param name="eventId">The id of the event to archive.</param>
        /// <returns>Returns the archived event details.</returns>
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEvent(string eventId)
        {
            ValidationResponseModel responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "EventHandler");
            if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
            {
                return Unauthorized();
            }

            if (!CanAccessEvent(eventId, responseSession))
            {
                return NotFound();
            }

            EventOrganizerModel organizer = new EventOrganizerModel()
            {
                Email = responseSession.Email,
                Token = responseSession.Token,
                FullName = responseSession.FullName,
            };

            var response = await new EventModifierManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionString"],
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).DeleteEvent(eventId, responseSession.UserId, organizer);

            return !response.Valid
                ? BadRequest(response)
                : Ok(response);
        }

        private static void ScopeRequestToSessionRegion(EventModifierRequestModel request, string oreId)
        {
            if (string.IsNullOrWhiteSpace(oreId))
            {
                return;
            }

            request.EventRegionModelList = new List<EventRegionModel>
            {
                new EventRegionModel { RegionId = oreId }
            };
        }

        private static bool CanAccessEvent(string eventId, ValidationResponseModel responseSession)
        {
            if (string.IsNullOrWhiteSpace(responseSession.OreId))
            {
                return true;
            }

            EventModifierRequestModel eventDetails = new EventReaderManager(
                AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"],
                responseSession.LangCode).GetEvent(eventId, responseSession.OreId);

            return eventDetails.EventModel != null;
        }

        /// <summary>
        /// Gets the lesson, lesson-quiz, and homework events for the authenticated
        /// teacher within the given month and year.
        /// </summary>
        /// <param name="month">The month (1-12).</param>
        /// <param name="year">The four-digit year.</param>
        /// <returns>Returns the event response models for the given month and year.</returns>
        /*[HttpGet("teacher")]
        public async Task<IActionResult> GetEventsByMonthAndYear([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var responseSession = await GetUserDetails.GetUserIdRequestOnOtherAPI(HttpContext, "PlanningRead");
                if (!responseSession.SessionValid || string.IsNullOrWhiteSpace(responseSession.UserId))
                {
                    return Unauthorized();
                }

                if (month < 1 || month > 12)
                {
                    return BadRequest("Month must be between 1 and 12.");
                }

                if (year < 1)
                {
                    return BadRequest("Year must be a valid four-digit year.");
                }

                if (!int.TryParse(responseSession.UserId, out int userId))
                {
                    return Unauthorized();
                }

                // Call the user school and user ORE according to schools. 
                var userSchoolIds = new List<int>();
                var oreIds = new List<string>();
                var userSchools = await GetUserDetails.GetUserSchoolsAsync(responseSession.Token);
                // Call the school region ids
                if (userSchools != null)
                {
                    // Prepare the ids.
                    foreach (var school in userSchools)
                    {
                        if (int.TryParse(school.InstitutionId, out int institutionId))
                        {
                            userSchoolIds.Add(institutionId);
                        }
                    }

                    // Call the regions of those schools. 
                    var schoolRegions = await SchoolsDetailsHandler.GetSchoolOresAsync(userSchoolIds, responseSession.Token);
                    foreach (var region in schoolRegions)
                    {
                        if (!string.IsNullOrWhiteSpace(region.RegionId))
                        {
                            oreIds.Add(region.RegionId);
                        }
                    }
                }

                var service = new EventsService();
                var events = service.GetEventsByMonthAndYear(month, year, userId, responseSession.RoleId, userSchoolIds, oreIds, RequestHandler.GetLanguage(HttpContext));
                return Ok(events);
            }
            catch (Exception)
            {
                // Return error about invalid request
                return BadRequest();
            }
        } */
    }
}
