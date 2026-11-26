using GSWebAPI.GSWebAPI.Domain.Models.General.Responses;
using GSWebAPI.GSWebAPI.Domain.Models.Community;
using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Conventions;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Community;
using GSWebAPI.GSWebAPI.Services.Common;
using GSWebAPI.GSWebAPI.Services.FeignAPIs;
using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Services.Community
{
    /// <summary>Community events and registrations (community_events_interests).</summary>
    public interface ICommunityEventService
    {
        /// <summary>
        /// Creates an open event; the caller becomes its organizer. In a group, only its active owners and moderators may call
        /// it. Without a group (platform-wide) any user may create it, and everyone can see it and register.
        /// </summary>
        /// <param name="user">User in session (the organizer).</param>
        /// <param name="groupId">Group id; null = platform-wide.</param>
        /// <param name="request">Event data. <see cref="CommunityEventRequestModel.EventAvailability"/> is ignored.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The created event (as <see cref="GetAsync"/> returns it). Errors: NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>,
        /// Forbidden <see cref="ServiceErrorCodes.NotModerator"/>, Validation <see cref="ServiceErrorCodes.EventTitleRequired"/>
        /// (title blank or over 150 characters, or location over 255), <see cref="ServiceErrorCodes.EventTimezoneInvalid"/>,
        /// <see cref="ServiceErrorCodes.EventEndBeforeStart"/>, <see cref="ServiceErrorCodes.EventCapacityInvalid"/> (capacity 0),
        /// <see cref="ServiceErrorCodes.EventMeetingFormatInvalid"/>.
        /// </returns>
        Task<ServiceResult<CommunityEventResponseModel>> CreateAsync(RequestUser user, ulong? groupId, CommunityEventRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Replaces the data of an event and, if sent, its availability. The organizer (while an active member) and active
        /// owners/moderators of the group may call it; for a platform-wide event, the organizer and staff. Runs with the event row locked, so the capacity cannot drop below the
        /// people already going.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="eventId">Event id.</param>
        /// <param name="request">New event data; a null <see cref="CommunityEventRequestModel.EventAvailability"/> keeps the current one.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The updated event. Errors: the validation errors of <see cref="CreateAsync"/> (checked first), NotFound
        /// <see cref="ServiceErrorCodes.EventNotFound"/>, Forbidden <see cref="ServiceErrorCodes.EventNotOrganizer"/>, Conflict
        /// <see cref="ServiceErrorCodes.EventCapacityInvalid"/> (capacity below the people going).
        /// </returns>
        Task<ServiceResult<CommunityEventResponseModel>> UpdateAsync(RequestUser user, ulong eventId, CommunityEventRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Soft-deletes an event with its registrations. The organizer (while an active member) and active owners/moderators of
        /// the group may call it; for a platform-wide event, the organizer and staff.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="eventId">Event id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>True when deleted. Errors: NotFound <see cref="ServiceErrorCodes.EventNotFound"/>, Forbidden <see cref="ServiceErrorCodes.EventNotOrganizer"/>.</returns>
        Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reads one event with all its registrations (<see cref="CommunityEventResponseModel.Attendees"/>) and names from the
        /// Users API. Only active members of the event's group may call it; anyone for a platform-wide event.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="eventId">Event id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>The event. Errors: NotFound <see cref="ServiceErrorCodes.EventNotFound"/>, Forbidden <see cref="ServiceErrorCodes.NotActiveMember"/>.</returns>
        Task<ServiceResult<CommunityEventResponseModel>> GetAsync(RequestUser user, ulong eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists events, most recent first (latest start date first), one page at a time
        /// (<see cref="CommunityEventResponseModel.Attendees"/> is not filled).
        /// With a group: that group's events, for its active members only. Without (optional filter): every event the user can
        /// see, platform-wide ones plus those of the groups where the user is an active member.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="groupId">Optional group filter.</param>
        /// <param name="fromUtc">If set, only events starting at or after this UTC date; null = all events.</param>
        /// <param name="page">Zero-based page; negative values become 0.</param>
        /// <param name="pageSize">Page size, clamped to 1-100.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>The page of events with the total items and pages. Errors (only with a group): NotFound <see cref="ServiceErrorCodes.GroupNotFound"/>, Forbidden <see cref="ServiceErrorCodes.NotActiveMember"/>.</returns>
        Task<ServiceResult<PagedResponseModel<CommunityEventResponseModel>>> ListAsync(RequestUser user, ulong? groupId, DateTime? fromUtc, int page, int pageSize, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates or replaces the registration of the user in session to an event (a cancelled one is restored). For a group
        /// event the user must be an active member of the group; a platform-wide event is open to everyone. going/interested need an open event; "going" cannot take the total over
        /// event_max_allowed_persons. not_going is always allowed. Runs with the event row locked.
        /// </summary>
        /// <param name="user">User in session (the one registering).</param>
        /// <param name="eventId">Event id.</param>
        /// <param name="request">Response and number of people (forced to 1 for not_going).</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>
        /// The registration (without the name). Errors: Validation <see cref="ServiceErrorCodes.EventQuantityInvalid"/> (quantity 0),
        /// NotFound <see cref="ServiceErrorCodes.EventNotFound"/>, Forbidden <see cref="ServiceErrorCodes.NotActiveMember"/>,
        /// Conflict <see cref="ServiceErrorCodes.EventNotOpen"/> or <see cref="ServiceErrorCodes.EventFull"/>.
        /// </returns>
        Task<ServiceResult<CommunityEventInterestResponseModel>> RegisterAsync(RequestUser user, ulong eventId, EventRegistrationRequestModel request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes (soft-deletes) the registration of the user in session to an event. It does not check the membership or the
        /// event state.
        /// </summary>
        /// <param name="user">User in session.</param>
        /// <param name="eventId">Event id.</param>
        /// <param name="cancellationToken">Cancels the operation.</param>
        /// <returns>True when cancelled, or NotFound <see cref="ServiceErrorCodes.RegistrationNotFound"/>.</returns>
        Task<ServiceResult<bool>> CancelRegistrationAsync(RequestUser user, ulong eventId, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Business rules:
    /// <list type="bullet">
    /// <item>Only active members see a group's events. Owners and moderators create events; the organizer, owners and
    /// moderators edit or delete them.</item>
    /// <item>The group is optional. A platform-wide event (no group) is created by any user, seen by everyone, and edited
    /// or deleted by its organizer or staff.</item>
    /// <item>Registration (going / interested): for a group event the user must be an active member of the group (also
    /// in public groups); the event must be open. "going" registrations cannot exceed event_max_allowed_persons in total.
    /// Answering not_going is always allowed, so people can free their place.</item>
    /// <item>All of the above runs in one transaction that first locks the event row (SELECT ... FOR UPDATE), so two
    /// simultaneous registrations cannot both take the last places.</item>
    /// </list>
    /// </summary>
    public sealed class CommunityEventService : ICommunityEventService
    {
        private readonly GSWebDbContext _db;
        private readonly IUserDirectoryClient _users;

        /// <summary>Creates the service.</summary>
        /// <param name="db">GSWebAPI database context.</param>
        /// <param name="users">Reads user full names from the Users API.</param>
        public CommunityEventService(GSWebDbContext db, IUserDirectoryClient users)
        {
            _db = db;
            _users = users;
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityEventResponseModel>> CreateAsync(RequestUser user, ulong? groupId, CommunityEventRequestModel request, CancellationToken cancellationToken = default)
        {
            if (groupId is { } id)
            {
                if (!await _db.CommunityGroups.AnyAsync(g => g.Id == id, cancellationToken))
                {
                    return ServiceResult<CommunityEventResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
                }

                if (!CommunityMembership.IsOwnerOrModerator(await CommunityMembership.GetActiveAsync(_db, id, user.UserId, cancellationToken)))
                {
                    return ServiceResult<CommunityEventResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotModerator);
                }
            }

            var errors = await ValidateAsync(request, cancellationToken);
            if (errors.Count > 0)
            {
                return ServiceResult<CommunityEventResponseModel>.Fail(ServiceErrorKind.Validation, errors);
            }

            var communityEvent = new CommunityEvent
            {
                CommunityGroupId = groupId,
                OrganizerUserId = user.UserId,
                EventAvailability = CommunityEventAvailability.Open,
            };
            Apply(communityEvent, request);
            _db.CommunityEvents.Add(communityEvent);
            await _db.SaveChangesAsync(cancellationToken);

            return await GetAsync(user, communityEvent.Id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityEventResponseModel>> UpdateAsync(RequestUser user, ulong eventId, CommunityEventRequestModel request, CancellationToken cancellationToken = default)
        {
            var errors = await ValidateAsync(request, cancellationToken);
            if (errors.Count > 0)
            {
                return ServiceResult<CommunityEventResponseModel>.Fail(ServiceErrorKind.Validation, errors);
            }

            // Locked like a registration, so the capacity cannot drop below the people already going.
            var result = await InLockedEventAsync(eventId, async communityEvent =>
            {
                if (!CanManage(await CommunityMembership.GetViewerAsync(_db, user, cancellationToken), communityEvent))
                {
                    return ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.EventNotOrganizer);
                }

                if (request.EventMaxAllowedPersons.HasValue
                    && await SumGoingAsync(communityEvent.Id, exceptUserId: null, cancellationToken) > request.EventMaxAllowedPersons.Value)
                {
                    return ServiceResult<bool>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.EventCapacityInvalid);
                }

                Apply(communityEvent, request);
                if (request.EventAvailability.HasValue)
                {
                    communityEvent.EventAvailability = request.EventAvailability.Value;
                }

                await _db.SaveChangesAsync(cancellationToken);
                return ServiceResult<bool>.Ok(true);
            }, cancellationToken);

            return result.Valid ? await GetAsync(user, eventId, cancellationToken) : result.As<CommunityEventResponseModel>();
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> DeleteAsync(RequestUser user, ulong eventId, CancellationToken cancellationToken = default)
        {
            var communityEvent = await _db.CommunityEvents.Include(e => e.Interests).FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
            if (communityEvent == null)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.EventNotFound);
            }

            if (!CanManage(await CommunityMembership.GetViewerAsync(_db, user, cancellationToken), communityEvent))
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.EventNotOrganizer);
            }

            // Soft-delete the event and its registrations together.
            _db.CommunityEventInterests.RemoveRange(communityEvent.Interests);
            _db.CommunityEvents.Remove(communityEvent);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Ok(true);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityEventResponseModel>> GetAsync(RequestUser user, ulong eventId, CancellationToken cancellationToken = default)
        {
            var communityEvent = await _db.CommunityEvents
                .AsNoTracking()
                .Include(e => e.MeetingFormat)
                .Include(e => e.Interests)
                .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);
            if (communityEvent == null)
            {
                return ServiceResult<CommunityEventResponseModel>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.EventNotFound);
            }

            if (!await CanSeeAsync(user, communityEvent.CommunityGroupId, cancellationToken))
            {
                return ServiceResult<CommunityEventResponseModel>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotActiveMember);
            }

            var names = await _users.GetFullNamesAsync(
                user,
                communityEvent.Interests.Select(i => i.UserId).Append(communityEvent.OrganizerUserId),
                cancellationToken);
            var model = Map(communityEvent, user, names);
            model.Attendees = communityEvent.Interests.OrderBy(i => i.Id).Select(i => MapInterest(i, names)).ToList();
            return ServiceResult<CommunityEventResponseModel>.Ok(model);
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<PagedResponseModel<CommunityEventResponseModel>>> ListAsync(RequestUser user, ulong? groupId, DateTime? fromUtc, int page, int pageSize, CancellationToken cancellationToken = default)
        {
            var query = _db.CommunityEvents
                .AsNoTracking()
                .Include(e => e.MeetingFormat)
                .Include(e => e.Interests)
                .AsQueryable();
            if (groupId is { } id)
            {
                if (await CommunityMembership.GetActiveAsync(_db, id, user.UserId, cancellationToken) == null)
                {
                    return await _db.CommunityGroups.AnyAsync(g => g.Id == id, cancellationToken)
                        ? ServiceResult<PagedResponseModel<CommunityEventResponseModel>>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotActiveMember)
                        : ServiceResult<PagedResponseModel<CommunityEventResponseModel>>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.GroupNotFound);
                }

                query = query.Where(e => e.CommunityGroupId == id);
            }
            else
            {
                // No filter: platform-wide events plus those of the user's groups.
                var myGroups = (await CommunityMembership.GetViewerAsync(_db, user, cancellationToken)).GroupIds;
                query = query.Where(e => e.CommunityGroupId == null || myGroups.Contains(e.CommunityGroupId.Value));
            }

            if (fromUtc.HasValue)
            {
                var from = UtcDateTimeConverter.ToUtc(fromUtc.Value);
                query = query.Where(e => e.EventDateTime >= from);
            }

            pageSize = Math.Clamp(pageSize, 1, 100);
            page = Math.Max(page, 0);
            var total = await query.CountAsync(cancellationToken);
            var events = await query
                .OrderByDescending(e => e.EventDateTime)
                .ThenByDescending(e => e.Id)
                .Skip(page * pageSize)
                .Take(pageSize)
                .AsSplitQuery()
                .ToListAsync(cancellationToken);
            var names = await _users.GetFullNamesAsync(user, events.Select(e => e.OrganizerUserId), cancellationToken);
            return ServiceResult<PagedResponseModel<CommunityEventResponseModel>>.Ok(
                PagedResponseModel<CommunityEventResponseModel>.Create(events.Select(e => Map(e, user, names)).ToList(), page, pageSize, total));
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<CommunityEventInterestResponseModel>> RegisterAsync(RequestUser user, ulong eventId, EventRegistrationRequestModel request, CancellationToken cancellationToken = default)
        {
            var quantity = request.Response == CommunityEventResponse.NotGoing ? (byte)1 : request.QuantityOfPersonsAssisting;
            if (quantity < 1)
            {
                return ServiceResult<CommunityEventInterestResponseModel>.Fail(ServiceErrorKind.Validation, ServiceErrorCodes.EventQuantityInvalid);
            }

            CommunityEventInterest? saved = null;
            var result = await InLockedEventAsync(eventId, async communityEvent =>
            {
                // Every check below runs after the event row is locked, in the same transaction as the write.
                if (!await CanSeeAsync(user, communityEvent.CommunityGroupId, cancellationToken))
                {
                    return ServiceResult<bool>.Fail(ServiceErrorKind.Forbidden, ServiceErrorCodes.NotActiveMember);
                }

                if (request.Response != CommunityEventResponse.NotGoing && communityEvent.EventAvailability != CommunityEventAvailability.Open)
                {
                    return ServiceResult<bool>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.EventNotOpen);
                }

                if (request.Response == CommunityEventResponse.Going && communityEvent.EventMaxAllowedPersons is { } maxAllowed)
                {
                    var goingWithoutMe = await SumGoingAsync(communityEvent.Id, user.UserId, cancellationToken);
                    if (goingWithoutMe + quantity > maxAllowed)
                    {
                        return ServiceResult<bool>.Fail(ServiceErrorKind.Conflict, ServiceErrorCodes.EventFull);
                    }
                }

                // IgnoreQueryFilters: a cancelled (soft-deleted) registration is restored, keeping the unique key.
                var interest = await _db.CommunityEventInterests
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(i => i.CommunityEventId == communityEvent.Id && i.UserId == user.UserId, cancellationToken);
                if (interest == null)
                {
                    interest = new CommunityEventInterest { CommunityEventId = communityEvent.Id, UserId = user.UserId };
                    _db.CommunityEventInterests.Add(interest);
                }

                interest.DeletedAt = null;
                interest.Response = request.Response;
                interest.QuantityOfPersonsAssisting = quantity;
                await _db.SaveChangesAsync(cancellationToken);
                saved = interest;
                return ServiceResult<bool>.Ok(true);
            }, cancellationToken);

            return result.Valid
                ? ServiceResult<CommunityEventInterestResponseModel>.Ok(MapInterest(saved!, null))
                : result.As<CommunityEventInterestResponseModel>();
        }

        /// <inheritdoc/>
        public async Task<ServiceResult<bool>> CancelRegistrationAsync(RequestUser user, ulong eventId, CancellationToken cancellationToken = default)
        {
            var interest = await _db.CommunityEventInterests
                .FirstOrDefaultAsync(i => i.CommunityEventId == eventId && i.UserId == user.UserId, cancellationToken);
            if (interest == null)
            {
                return ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.RegistrationNotFound);
            }

            _db.CommunityEventInterests.Remove(interest);
            await _db.SaveChangesAsync(cancellationToken);
            return ServiceResult<bool>.Ok(true);
        }

        /// <summary>
        /// Runs <paramref name="action"/> in a transaction that holds a row lock on the event
        /// (SELECT ... FOR UPDATE). Wrapped in the execution strategy because retry-on-failure is enabled; on a retry
        /// the whole unit (lock, checks, write) runs again. The transaction commits only if the action succeeds.
        /// </summary>
        private async Task<ServiceResult<bool>> InLockedEventAsync(
            ulong eventId,
            Func<CommunityEvent, Task<ServiceResult<bool>>> action,
            CancellationToken cancellationToken)
        {
            var strategy = _db.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                _db.ChangeTracker.Clear();
                await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);

                // Raw SQL with no LINQ composition, so EF sends it as is (FOR UPDATE cannot sit in a derived table).
                var locked = await _db.CommunityEvents
                    .FromSqlInterpolated($"SELECT * FROM `community_events` WHERE `id` = {eventId} AND `deleted_at` IS NULL FOR UPDATE")
                    .IgnoreQueryFilters()
                    .ToListAsync(cancellationToken);
                var communityEvent = locked.FirstOrDefault();
                if (communityEvent == null)
                {
                    return ServiceResult<bool>.Fail(ServiceErrorKind.NotFound, ServiceErrorCodes.EventNotFound);
                }

                var result = await action(communityEvent);
                if (result.Valid)
                {
                    await transaction.CommitAsync(cancellationToken);
                }

                return result;
            });
        }

        private async Task<int> SumGoingAsync(ulong eventId, ulong? exceptUserId, CancellationToken cancellationToken)
        {
            var query = _db.CommunityEventInterests.Where(i => i.CommunityEventId == eventId && i.Response == CommunityEventResponse.Going);
            if (exceptUserId.HasValue)
            {
                query = query.Where(i => i.UserId != exceptUserId.Value);
            }

            return await query.SumAsync(i => (int)i.QuantityOfPersonsAssisting, cancellationToken);
        }

        /// <summary>Owners/moderators of the group (staff for a platform-wide event), or the organizer while they can see it.</summary>
        private static bool CanManage(CommunityViewer viewer, CommunityEvent communityEvent)
        {
            var groupId = communityEvent.CommunityGroupId;
            return viewer.IsModerator(groupId) || (viewer.CanSee(groupId) && communityEvent.OrganizerUserId == viewer.UserId);
        }

        /// <summary>Platform-wide events are for everyone; group events for the group's active members.</summary>
        private async Task<bool> CanSeeAsync(RequestUser user, ulong? groupId, CancellationToken cancellationToken) =>
            groupId is not { } id || await CommunityMembership.GetActiveAsync(_db, id, user.UserId, cancellationToken) != null;

        private async Task<List<string>> ValidateAsync(CommunityEventRequestModel request, CancellationToken cancellationToken)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(request.EventTitle) || request.EventTitle.Trim().Length > 150
                || (request.EventLocation?.Trim().Length ?? 0) > 255)
            {
                errors.Add(ServiceErrorCodes.EventTitleRequired);
            }

            if (!TimeProviderExtensions.IsValidTimeZone(request.EventTimezone))
            {
                errors.Add(ServiceErrorCodes.EventTimezoneInvalid);
            }

            if (request.EventEndDateTime.HasValue
                && UtcDateTimeConverter.ToUtc(request.EventEndDateTime.Value) <= UtcDateTimeConverter.ToUtc(request.EventDateTime))
            {
                errors.Add(ServiceErrorCodes.EventEndBeforeStart);
            }

            if (request.EventMaxAllowedPersons == 0)
            {
                errors.Add(ServiceErrorCodes.EventCapacityInvalid);
            }

            if (!await _db.MeetingFormats.AnyAsync(f => f.Id == request.MeetingFormatId, cancellationToken))
            {
                errors.Add(ServiceErrorCodes.EventMeetingFormatInvalid);
            }

            return errors;
        }

        private static void Apply(CommunityEvent communityEvent, CommunityEventRequestModel request)
        {
            communityEvent.EventTitle = request.EventTitle.Trim();
            communityEvent.EventDescription = string.IsNullOrWhiteSpace(request.EventDescription) ? null : request.EventDescription.Trim();
            communityEvent.MeetingFormatId = request.MeetingFormatId;
            communityEvent.EventLocation = string.IsNullOrWhiteSpace(request.EventLocation) ? null : request.EventLocation.Trim();
            communityEvent.EventDateTime = TruncateToSeconds(UtcDateTimeConverter.ToUtc(request.EventDateTime));
            communityEvent.EventEndDateTime = request.EventEndDateTime.HasValue
                ? TruncateToSeconds(UtcDateTimeConverter.ToUtc(request.EventEndDateTime.Value))
                : null;
            communityEvent.EventTimezone = request.EventTimezone;
            communityEvent.EventMaxAllowedPersons = request.EventMaxAllowedPersons;
        }

        private static DateTime TruncateToSeconds(DateTime value) =>
            new(value.Ticks - (value.Ticks % TimeSpan.TicksPerSecond), DateTimeKind.Utc);

        private static CommunityEventResponseModel Map(CommunityEvent communityEvent, RequestUser user, IReadOnlyDictionary<ulong, string> names)
        {
            var mine = communityEvent.Interests.FirstOrDefault(i => i.UserId == user.UserId);
            return new CommunityEventResponseModel
            {
                Id = communityEvent.Id,
                CommunityGroupId = communityEvent.CommunityGroupId,
                OrganizerUserId = communityEvent.OrganizerUserId,
                OrganizerName = names.GetValueOrDefault(communityEvent.OrganizerUserId),
                EventTitle = communityEvent.EventTitle,
                EventDescription = communityEvent.EventDescription,
                MeetingFormatId = communityEvent.MeetingFormatId,
                MeetingFormatCode = communityEvent.MeetingFormat?.Code,
                EventLocation = communityEvent.EventLocation,
                EventDateTime = communityEvent.EventDateTime,
                EventEndDateTime = communityEvent.EventEndDateTime,
                EventTimezone = communityEvent.EventTimezone,
                EventAvailability = communityEvent.EventAvailability,
                EventMaxAllowedPersons = communityEvent.EventMaxAllowedPersons,
                GoingPersons = communityEvent.Interests.Where(i => i.Response == CommunityEventResponse.Going).Sum(i => i.QuantityOfPersonsAssisting),
                MyRegistration = mine == null ? null : MapInterest(mine, names),
            };
        }

        private static CommunityEventInterestResponseModel MapInterest(CommunityEventInterest interest, IReadOnlyDictionary<ulong, string>? names)
        {
            return new CommunityEventInterestResponseModel
            {
                Id = interest.Id,
                CommunityEventId = interest.CommunityEventId,
                UserId = interest.UserId,
                UserName = names?.GetValueOrDefault(interest.UserId),
                Response = interest.Response,
                QuantityOfPersonsAssisting = interest.QuantityOfPersonsAssisting,
            };
        }
    }
}
