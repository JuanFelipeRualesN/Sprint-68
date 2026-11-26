using GSWebAPI.GSWebAPI.Domain.Models.General;
using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Services.Community;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSWebAPI.Controllers.Community
{
    /// <summary>
    /// Community groups, members, events, resources and forums.
    /// Permissions: CommunityRead (read) and CommunityHandler (write).
    /// </summary>
    [ApiController]
    [Route("gswebapi/community")]
    public class CommunityController : ServiceResultControllerBase
    {
        private const string ReadPermission = "CommunityRead";
        private const string WritePermission = "CommunityHandler";

        private readonly ICommunityGroupService _groups;
        private readonly ICommunityEventService _events;
        private readonly ICommunityResourceService _resources;
        private readonly ICommunityForumService _forums;

        /// <summary>Creates the controller.</summary>
        /// <param name="groups">Groups and members.</param>
        /// <param name="events">Events and registrations.</param>
        /// <param name="resources">Resources.</param>
        /// <param name="forums">Forums, categories and posts.</param>
        public CommunityController(
            ICommunityGroupService groups,
            ICommunityEventService events,
            ICommunityResourceService resources,
            ICommunityForumService forums)
        {
            _groups = groups;
            _events = events;
            _resources = resources;
            _forums = forums;
        }

        // ---------- Groups ----------

        /// <summary>Lists community groups (public and private), newest first, paged.</summary>
        /// <param name="onlyMine">True to list only the groups where the user is an active or pending member.</param>
        /// <param name="page">Zero-based page.</param>
        /// <param name="pageSize">Page size (1-100, default 50).</param>
        /// <returns>200 with the page: { page, pageSize, totalItems, totalPages, items }; 401 if the session or the CommunityRead permission is not valid; it does not fail otherwise.</returns>
        [HttpGet("groups")]
        public async Task<IActionResult> ListGroups([FromQuery] bool onlyMine = false, [FromQuery] int page = 0, [FromQuery] int pageSize = 50)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _groups.ListAsync(user, onlyMine, page, pageSize, HttpContext.RequestAborted));
        }

        /// <summary>Reads one group, with the membership of the user in session.</summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>200 with the group; 401 if the session or the CommunityRead permission is not valid; 404 if the group does not exist.</returns>
        [HttpGet("groups/{groupId}")]
        public async Task<IActionResult> GetGroup(ulong groupId)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _groups.GetAsync(user, groupId, HttpContext.RequestAborted));
        }

        /// <summary>Creates a group; the user in session becomes its owner.</summary>
        /// <param name="request">Group data.</param>
        /// <returns>201 with the group; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid.</returns>
        [HttpPost("groups")]
        public async Task<IActionResult> CreateGroup([FromBody] CommunityGroupRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _groups.CreateAsync(user, request, HttpContext.RequestAborted), created: true);
        }

        /// <summary>Updates a group (owners only).</summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="request">New group data.</param>
        /// <returns>200 with the group; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid; 403 if not an owner; 404 if the group does not exist.</returns>
        [HttpPut("groups/{groupId}")]
        public async Task<IActionResult> UpdateGroup(ulong groupId, [FromBody] CommunityGroupRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _groups.UpdateAsync(user, groupId, request, HttpContext.RequestAborted));
        }

        /// <summary>Soft-deletes a group with its members, forums, events and resources (owners only).</summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>200 with true; 401 if the session or the CommunityHandler permission is not valid; 403 if not an owner; 404 if the group does not exist.</returns>
        [HttpDelete("groups/{groupId}")]
        public async Task<IActionResult> DeleteGroup(ulong groupId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _groups.DeleteAsync(user, groupId, HttpContext.RequestAborted));
        }

        // ---------- Members ----------

        /// <summary>Lists the members of a group (active members only; owners/moderators also see pending and banned).</summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>200 with the members; 401 if the session or the CommunityRead permission is not valid; 403 if not an active member; 404 if the group does not exist.</returns>
        [HttpGet("groups/{groupId}/members")]
        public async Task<IActionResult> ListMembers(ulong groupId)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _groups.ListMembersAsync(user, groupId, HttpContext.RequestAborted));
        }

        /// <summary>Joins the group: active in a public group, pending approval in a private one.</summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>200 with the membership; 401 if the session or the CommunityHandler permission is not valid; 403 if banned; 404 if the group does not exist; 409 if already a member or pending.</returns>
        [HttpPost("groups/{groupId}/join")]
        public async Task<IActionResult> Join(ulong groupId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _groups.JoinAsync(user, groupId, HttpContext.RequestAborted));
        }

        /// <summary>Leaves the group (or withdraws a pending request).</summary>
        /// <param name="groupId">Group id.</param>
        /// <returns>200 with true; 401 if the session or the CommunityHandler permission is not valid; 403 if banned; 404 if not a member; 409 if the user is the last owner.</returns>
        [HttpPost("groups/{groupId}/leave")]
        public async Task<IActionResult> Leave(ulong groupId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _groups.LeaveAsync(user, groupId, HttpContext.RequestAborted));
        }

        /// <summary>Approves a pending join request (owners and moderators).</summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="memberUserId">User id of the pending member.</param>
        /// <returns>200 with the membership; 401 if the session or the CommunityHandler permission is not valid; 403 if not an owner/moderator; 404 if the member does not exist; 409 if not pending.</returns>
        [HttpPost("groups/{groupId}/members/{memberUserId}/approve")]
        public async Task<IActionResult> ApproveMember(ulong groupId, ulong memberUserId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _groups.ApproveMemberAsync(user, groupId, memberUserId, HttpContext.RequestAborted));
        }

        /// <summary>Rejects a pending join request (owners and moderators).</summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="memberUserId">User id of the pending member.</param>
        /// <returns>200 with true; 401 if the session or the CommunityHandler permission is not valid; 403 if not an owner/moderator; 404 if the member does not exist; 409 if not pending.</returns>
        [HttpPost("groups/{groupId}/members/{memberUserId}/reject")]
        public async Task<IActionResult> RejectMember(ulong groupId, ulong memberUserId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _groups.RejectMemberAsync(user, groupId, memberUserId, HttpContext.RequestAborted));
        }

        /// <summary>Bans a member (owners and moderators; owners and oneself cannot be banned).</summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="memberUserId">User id of the member.</param>
        /// <returns>200 with the membership; 401 if the session or the CommunityHandler permission is not valid; 403 if not an owner/moderator or the target is an owner or oneself; 404 if the member does not exist.</returns>
        [HttpPost("groups/{groupId}/members/{memberUserId}/ban")]
        public async Task<IActionResult> BanMember(ulong groupId, ulong memberUserId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _groups.BanMemberAsync(user, groupId, memberUserId, HttpContext.RequestAborted));
        }

        /// <summary>Changes the role of an active member (owners only; the last owner cannot be demoted).</summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="memberUserId">User id of the member.</param>
        /// <param name="request">New role.</param>
        /// <returns>200 with the membership; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing; 403 if not an owner; 404 if not an active member; 409 if it would leave no owner.</returns>
        [HttpPut("groups/{groupId}/members/{memberUserId}/role")]
        public async Task<IActionResult> ChangeMemberRole(ulong groupId, ulong memberUserId, [FromBody] ChangeMemberRoleRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _groups.ChangeMemberRoleAsync(user, groupId, memberUserId, request.Role, HttpContext.RequestAborted));
        }

        // ---------- Events ----------

        /// <summary>Lists the events of a group, most recent first (latest start date first), paged (active members only).</summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="fromUtc">If sent, only events starting at or after this UTC date.</param>
        /// <param name="page">Zero-based page.</param>
        /// <param name="pageSize">Page size (1-100, default 20).</param>
        /// <returns>200 with the page of events: { page, pageSize, totalItems, totalPages, items }; 401 if the session or the CommunityRead permission is not valid; 403 if not an active member; 404 if the group does not exist.</returns>
        [HttpGet("groups/{groupId}/events")]
        public async Task<IActionResult> ListEvents(ulong groupId, [FromQuery] DateTime? fromUtc = null, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _events.ListAsync(user, groupId, fromUtc, page, pageSize, HttpContext.RequestAborted));
        }

        /// <summary>Creates an open event in a group (owners and moderators); the user in session is the organizer.</summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="request">Event data.</param>
        /// <returns>201 with the event; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid; 403 if not an owner/moderator; 404 if the group does not exist.</returns>
        [HttpPost("groups/{groupId}/events")]
        public async Task<IActionResult> CreateEvent(ulong groupId, [FromBody] CommunityEventRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _events.CreateAsync(user, groupId, request, HttpContext.RequestAborted), created: true);
        }

        /// <summary>
        /// Lists events, most recent first (latest start date first), paged. groupId is an optional filter: with it, that group's events (active members
        /// only); without it, every event the user can see (platform-wide plus the user's groups).
        /// </summary>
        /// <param name="groupId">Optional group filter.</param>
        /// <param name="fromUtc">If sent, only events starting at or after this UTC date.</param>
        /// <param name="page">Zero-based page.</param>
        /// <param name="pageSize">Page size (1-100, default 20).</param>
        /// <param name="communityGroupId">Same as groupId (the name used in the request bodies); groupId wins when both are sent.</param>
        /// <returns>200 with the page of events: { page, pageSize, totalItems, totalPages, items }; 401 if the session or the CommunityRead permission is not valid; with groupId, 403 if not an active member and 404 if the group does not exist.</returns>
        [HttpGet("events")]
        public async Task<IActionResult> ListAllEvents([FromQuery] ulong? groupId = null, [FromQuery] DateTime? fromUtc = null, [FromQuery] int page = 0, [FromQuery] int pageSize = 20, [FromQuery] ulong? communityGroupId = null)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _events.ListAsync(user, groupId ?? communityGroupId, fromUtc, page, pageSize, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Creates an open event; the user in session is the organizer. communityGroupId in the body is optional: without it
        /// the event is platform-wide (any user can create it, everyone can see it and register); with it, only the group's
        /// owners and moderators can create it.
        /// </summary>
        /// <param name="request">Event data with the optional communityGroupId.</param>
        /// <returns>201 with the event; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid; with a group, 403 if not an owner/moderator and 404 if the group does not exist.</returns>
        [HttpPost("events")]
        public async Task<IActionResult> CreateAnyEvent([FromBody] CommunityEventRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _events.CreateAsync(user, request.CommunityGroupId, request, HttpContext.RequestAborted), created: true);
        }

        /// <summary>Reads one event with all its registrations (active members of its group only).</summary>
        /// <param name="eventId">Event id.</param>
        /// <returns>200 with the event; 401 if the session or the CommunityRead permission is not valid; 403 if not an active member; 404 if the event does not exist.</returns>
        [HttpGet("events/{eventId}")]
        public async Task<IActionResult> GetEvent(ulong eventId)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _events.GetAsync(user, eventId, HttpContext.RequestAborted));
        }

        /// <summary>Updates an event (organizer, owners and moderators).</summary>
        /// <param name="eventId">Event id.</param>
        /// <param name="request">New event data.</param>
        /// <returns>200 with the event; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid; 403 if not allowed; 404 if the event does not exist; 409 if the capacity is below the people going.</returns>
        [HttpPut("events/{eventId}")]
        public async Task<IActionResult> UpdateEvent(ulong eventId, [FromBody] CommunityEventRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _events.UpdateAsync(user, eventId, request, HttpContext.RequestAborted));
        }

        /// <summary>Soft-deletes an event with its registrations (organizer, owners and moderators).</summary>
        /// <param name="eventId">Event id.</param>
        /// <returns>200 with true; 401 if the session or the CommunityHandler permission is not valid; 403 if not allowed; 404 if the event does not exist.</returns>
        [HttpDelete("events/{eventId}")]
        public async Task<IActionResult> DeleteEvent(ulong eventId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _events.DeleteAsync(user, eventId, HttpContext.RequestAborted));
        }

        /// <summary>Creates or replaces the registration of the user in session (going, interested, not_going).</summary>
        /// <param name="eventId">Event id.</param>
        /// <param name="request">Response and number of people.</param>
        /// <returns>200 with the registration; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or the quantity is invalid; 403 if not an active member; 404 if the event does not exist; 409 if the event is not open or is full.</returns>
        [HttpPut("events/{eventId}/registration")]
        public async Task<IActionResult> Register(ulong eventId, [FromBody] EventRegistrationRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _events.RegisterAsync(user, eventId, request, HttpContext.RequestAborted));
        }

        /// <summary>Cancels the registration of the user in session.</summary>
        /// <param name="eventId">Event id.</param>
        /// <returns>200 with true; 401 if the session or the CommunityHandler permission is not valid; 404 if there is no registration.</returns>
        [HttpDelete("events/{eventId}/registration")]
        public async Task<IActionResult> CancelRegistration(ulong eventId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _events.CancelRegistrationAsync(user, eventId, HttpContext.RequestAborted));
        }

        // ---------- Resources ----------

        /// <summary>
        /// Lists resources. groupId is an optional filter: with it, that group's resources (active members only); without it,
        /// every resource the user can see (platform-wide plus the user's groups). Newest first, paged. Drafts only for those
        /// allowed to see them.
        /// </summary>
        /// <param name="groupId">Optional group filter.</param>
        /// <param name="page">Zero-based page.</param>
        /// <param name="pageSize">Page size (1-100, default 20).</param>
        /// <param name="communityGroupId">Same as groupId (the name used in the request bodies); groupId wins when both are sent.</param>
        /// <returns>200 with the page of resources: { page, pageSize, totalItems, totalPages, items }; 401 if the session or the CommunityRead permission is not valid; with groupId, 403 if not an active member and 404 if the group does not exist.</returns>
        [HttpGet("resources")]
        public async Task<IActionResult> ListResources([FromQuery] ulong? groupId = null, [FromQuery] int page = 0, [FromQuery] int pageSize = 20, [FromQuery] ulong? communityGroupId = null)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _resources.ListAsync(user, groupId ?? communityGroupId, page, pageSize, HttpContext.RequestAborted));
        }

        /// <summary>Reads one resource.</summary>
        /// <param name="resourceId">Resource id.</param>
        /// <returns>200 with the resource; 401 if the session or the CommunityRead permission is not valid; 404 if it does not exist or the user cannot see it.</returns>
        [HttpGet("resources/{resourceId}")]
        public async Task<IActionResult> GetResource(ulong resourceId)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _resources.GetAsync(user, resourceId, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Creates a resource with an external link (application/json), in a group (active members) or platform-wide (any
        /// user). To upload a file, use <c>POST resources/upload</c> (<see cref="CreateResourceWithFile"/>).
        /// </summary>
        /// <param name="request">Resource data; Link is required.</param>
        /// <returns>201 with the resource; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid; 403 if not an active member of the group; 404 if the group does not exist.</returns>
        [HttpPost("resources")]
        public async Task<IActionResult> CreateResource([FromBody] CommunityResourceRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _resources.CreateAsync(user, request, null, HttpContext.RequestAborted), created: true);
        }

        /// <summary>
        /// Creates a resource from a form (multipart/form-data): CommunityGroupId, Title, Description, Publish and either a
        /// <c>file</c> or a Link. The file is uploaded to Blob Storage (office documents, pdf, txt, mp4, mp3; max 100 MB); its
        /// type comes from the file and the Link is ignored. Reads return a signed link to the file, valid for one hour.
        /// </summary>
        /// <param name="request">Resource fields from the form.</param>
        /// <param name="file">Optional uploaded file; without it the Link is required.</param>
        /// <returns>201 with the resource; 401 if the session or the CommunityHandler permission is not valid; 400 if invalid (file type, size, content, link); 403 if not an active member of the group; 404 if the group does not exist; 503 if Blob Storage is unavailable.</returns>
        [HttpPost("resources/upload")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(CommunityResourceFiles.MaxFileBytes + 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = CommunityResourceFiles.MaxFileBytes + 1024 * 1024)]
        public async Task<IActionResult> CreateResourceWithFile([FromForm] CommunityResourceRequestModel request, IFormFile? file)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            if (request == null)
            {
                return BadRequest();
            }

            if (file == null)
            {
                return FromResult(await _resources.CreateAsync(user, request, null, HttpContext.RequestAborted), created: true);
            }

            await using var stream = file.OpenReadStream();
            var upload = new UploadedFile(stream, file.FileName, file.ContentType, file.Length);
            return FromResult(await _resources.CreateAsync(user, request, upload, HttpContext.RequestAborted), created: true);
        }

        /// <summary>Updates a resource (author only).</summary>
        /// <param name="resourceId">Resource id.</param>
        /// <param name="request">New resource data (the group is ignored).</param>
        /// <returns>200 with the resource; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid; 403 if not the author; 404 if it does not exist or the user cannot see it.</returns>
        [HttpPut("resources/{resourceId}")]
        public async Task<IActionResult> UpdateResource(ulong resourceId, [FromBody] CommunityResourceRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _resources.UpdateAsync(user, resourceId, request, HttpContext.RequestAborted));
        }

        /// <summary>Soft-deletes a resource (author, group owners/moderators, or staff for platform-wide resources).</summary>
        /// <param name="resourceId">Resource id.</param>
        /// <returns>200 with true; 401 if the session or the CommunityHandler permission is not valid; 403 if not allowed; 404 if it does not exist or the user cannot see it.</returns>
        [HttpDelete("resources/{resourceId}")]
        public async Task<IActionResult> DeleteResource(ulong resourceId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _resources.DeleteAsync(user, resourceId, HttpContext.RequestAborted));
        }

        // ---------- Forums ----------

        /// <summary>Lists the forum categories.</summary>
        /// <returns>200 with the categories; 401 if the session or the CommunityRead permission is not valid; it does not fail otherwise.</returns>
        [HttpGet("forum-categories")]
        public async Task<IActionResult> ListForumCategories()
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _forums.ListCategoriesAsync(user.LangCode, HttpContext.RequestAborted));
        }

        /// <summary>Lists the forums of a group, pinned first and then by latest activity (active members only).</summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="categoryId">If sent, only forums of this category.</param>
        /// <param name="page">Zero-based page.</param>
        /// <param name="pageSize">Page size (1-100, default 20).</param>
        /// <returns>200 with the page of forums: { page, pageSize, totalItems, totalPages, items }; 401 if the session or the CommunityRead permission is not valid; 403 if not an active member; 404 if the group does not exist.</returns>
        [HttpGet("groups/{groupId}/forums")]
        public async Task<IActionResult> ListForums(ulong groupId, [FromQuery] byte? categoryId = null, [FromQuery] int page = 0, [FromQuery] int pageSize = 20)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _forums.ListAsync(user, groupId, categoryId, page, pageSize, HttpContext.RequestAborted));
        }

        /// <summary>Creates a forum in a group (active members; moderators-only categories need an owner/moderator).</summary>
        /// <param name="groupId">Group id.</param>
        /// <param name="request">Forum data.</param>
        /// <returns>201 with the forum; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid (including foul language); 403 if not an active member or the category is moderators-only; 404 if the group does not exist.</returns>
        [HttpPost("groups/{groupId}/forums")]
        public async Task<IActionResult> CreateForum(ulong groupId, [FromBody] CommunityForumRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _forums.CreateAsync(user, groupId, request, HttpContext.RequestAborted), created: true);
        }

        /// <summary>
        /// Lists forums, pinned first and then by latest activity. groupId is an optional filter: with it, that group's forums
        /// (active members only); without it, every forum the user can see (platform-wide plus the user's groups).
        /// </summary>
        /// <param name="groupId">Optional group filter.</param>
        /// <param name="categoryId">If sent, only forums of this category.</param>
        /// <param name="page">Zero-based page.</param>
        /// <param name="pageSize">Page size (1-100, default 20).</param>
        /// <param name="communityGroupId">Same as groupId (the name used in the request bodies); groupId wins when both are sent.</param>
        /// <returns>200 with the page of forums: { page, pageSize, totalItems, totalPages, items }; 401 if the session or the CommunityRead permission is not valid; with groupId, 403 if not an active member and 404 if the group does not exist.</returns>
        [HttpGet("forums")]
        public async Task<IActionResult> ListAllForums([FromQuery] ulong? groupId = null, [FromQuery] byte? categoryId = null, [FromQuery] int page = 0, [FromQuery] int pageSize = 20, [FromQuery] ulong? communityGroupId = null)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _forums.ListAsync(user, groupId ?? communityGroupId, categoryId, page, pageSize, HttpContext.RequestAborted));
        }

        /// <summary>
        /// Creates a forum; the user in session is the author. communityGroupId in the body is optional: without it the forum
        /// is platform-wide (any user; staff moderate it and post announcements); with it, the group's active members.
        /// </summary>
        /// <param name="request">Forum data with the optional communityGroupId.</param>
        /// <returns>201 with the forum; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid (including foul language); 403 if not an active member of the group or the category is moderators-only; 404 if the group does not exist.</returns>
        [HttpPost("forums")]
        public async Task<IActionResult> CreateAnyForum([FromBody] CommunityForumRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _forums.CreateAsync(user, request.CommunityGroupId, request, HttpContext.RequestAborted), created: true);
        }

        /// <summary>Reads one forum (active members of its group only).</summary>
        /// <param name="forumId">Forum id.</param>
        /// <returns>200 with the forum; 401 if the session or the CommunityRead permission is not valid; 403 if not an active member; 404 if the forum does not exist.</returns>
        [HttpGet("forums/{forumId}")]
        public async Task<IActionResult> GetForum(ulong forumId)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _forums.GetAsync(user, forumId, HttpContext.RequestAborted));
        }

        /// <summary>Updates a forum (author, owners and moderators; locked forums only owners/moderators).</summary>
        /// <param name="forumId">Forum id.</param>
        /// <param name="request">New forum data.</param>
        /// <returns>200 with the forum; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid; 403 if not allowed; 404 if the forum does not exist; 409 if the forum is locked.</returns>
        [HttpPut("forums/{forumId}")]
        public async Task<IActionResult> UpdateForum(ulong forumId, [FromBody] CommunityForumRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _forums.UpdateAsync(user, forumId, request, HttpContext.RequestAborted));
        }

        /// <summary>Pin/unpin and lock/unlock (group owners and moderators).</summary>
        /// <param name="forumId">Forum id.</param>
        /// <param name="request">New flags; null fields are left unchanged.</param>
        /// <returns>200 with the forum; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing; 403 if not an owner/moderator; 404 if the forum does not exist.</returns>
        [HttpPatch("forums/{forumId}/moderation")]
        public async Task<IActionResult> ModerateForum(ulong forumId, [FromBody] CommunityForumModerationRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _forums.ModerateAsync(user, forumId, request, HttpContext.RequestAborted));
        }

        /// <summary>Soft-deletes a forum with its posts (author, owners and moderators).</summary>
        /// <param name="forumId">Forum id.</param>
        /// <returns>200 with true; 401 if the session or the CommunityHandler permission is not valid; 403 if not allowed; 404 if the forum does not exist.</returns>
        [HttpDelete("forums/{forumId}")]
        public async Task<IActionResult> DeleteForum(ulong forumId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _forums.DeleteAsync(user, forumId, HttpContext.RequestAborted));
        }

        // ---------- Forum posts ----------

        /// <summary>Lists the posts of a forum in creation order; deleted posts with replies come as placeholders (active members only).</summary>
        /// <param name="forumId">Forum id.</param>
        /// <param name="page">Zero-based page.</param>
        /// <param name="pageSize">Page size (1-100, default 50).</param>
        /// <returns>200 with the page of posts: { page, pageSize, totalItems, totalPages, items }; 401 if the session or the CommunityRead permission is not valid; 403 if not an active member; 404 if the forum does not exist.</returns>
        [HttpGet("forums/{forumId}/posts")]
        public async Task<IActionResult> ListForumPosts(ulong forumId, [FromQuery] int page = 0, [FromQuery] int pageSize = 50)
        {
            var user = await GetRequestUserAsync(ReadPermission);
            return user == null ? Unauthorized() : FromResult(await _forums.ListPostsAsync(user, forumId, page, pageSize, HttpContext.RequestAborted));
        }

        /// <summary>Adds a post or a reply to a forum (active members; locked forums only owners/moderators).</summary>
        /// <param name="forumId">Forum id.</param>
        /// <param name="request">Text and optional parent post.</param>
        /// <returns>201 with the post; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid (including foul language or a bad parent); 403 if not an active member; 404 if the forum does not exist; 409 if the forum is locked.</returns>
        [HttpPost("forums/{forumId}/posts")]
        public async Task<IActionResult> CreateForumPost(ulong forumId, [FromBody] CommunityForumPostRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _forums.CreatePostAsync(user, forumId, request, HttpContext.RequestAborted), created: true);
        }

        /// <summary>Edits the text of a post (author only).</summary>
        /// <param name="postId">Post id.</param>
        /// <param name="request">New text.</param>
        /// <returns>200 with the post; 401 if the session or the CommunityHandler permission is not valid; 400 if the body is missing or invalid; 403 if not the author or not an active member; 404 if the post does not exist; 409 if the forum is locked.</returns>
        [HttpPut("forum-posts/{postId}")]
        public async Task<IActionResult> UpdateForumPost(ulong postId, [FromBody] CommunityForumPostRequestModel request)
        {
            var user = await GetRequestUserAsync(WritePermission);
            if (user == null)
            {
                return Unauthorized();
            }

            return request == null ? BadRequest() : FromResult(await _forums.UpdatePostAsync(user, postId, request, HttpContext.RequestAborted));
        }

        /// <summary>Soft-deletes a post; its replies are kept (author, owners and moderators).</summary>
        /// <param name="postId">Post id.</param>
        /// <returns>200 with true; 401 if the session or the CommunityHandler permission is not valid; 403 if not allowed; 404 if the post does not exist.</returns>
        [HttpDelete("forum-posts/{postId}")]
        public async Task<IActionResult> DeleteForumPost(ulong postId)
        {
            var user = await GetRequestUserAsync(WritePermission);
            return user == null ? Unauthorized() : FromResult(await _forums.DeletePostAsync(user, postId, HttpContext.RequestAborted));
        }
    }
}
