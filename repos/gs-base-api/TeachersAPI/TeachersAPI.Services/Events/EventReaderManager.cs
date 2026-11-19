using System;
using System.Collections.Generic;
using System.Linq;
using TeachersAPI.Domain.Models.Events;
using TeachersAPI.Infrastructure.Repositories.Events;
using TeachersAPI.Domain.Models.Generals.Responses;

namespace TeachersAPI.TeachersAPI.Services.Events
{
    /// <summary>
    /// To handle the reads associated directly to events.
    /// </summary>
    public class EventReaderManager
    {
        private readonly string _connectionStringRead, _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The user's language code.</param>
        public EventReaderManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Returns the event with all related details.
        /// </summary>
        /// <param name="eventId">The id of the event.</param>
        /// <param name="audienceRegionId">When present, the event must be explicitly assigned to this region.</param>
        /// <returns>Returns the event with its relations assembled.</returns>
        public EventModifierRequestModel GetEvent(string eventId, string audienceRegionId = null)
        {
            EventDAO eventDAO = new EventDAO();

            List<EventModel> eventModelList = eventDAO.Read(_connectionStringRead, new EventModel { Id = eventId }, true, false);

            if (eventModelList.Count == 0)
            {
                return new EventModifierRequestModel();
            }

            // Fetched once, reused both for AudienceSummary and for the response lists below —
            // no extra queries beyond what GetEvent already needed.
            List<EventRoleModel> eventRoleModelList = eventDAO.ReadRoles(_connectionStringRead, eventId);
            List<EventSchoolModel> eventSchoolModelList = eventDAO.ReadSchools(_connectionStringRead, eventId);
            List<EventRegionModel> eventRegionModelList = eventDAO.ReadRegions(_connectionStringRead, eventId);
            List<EventUserModel> eventUserModelList = eventDAO.ReadUsers(_connectionStringRead, eventId);

            // An Admin ORE can only read events explicitly assigned to their own region.
            // Events without a region remain global for calendars, but are not manageable by
            // a regional administrator through the event administration endpoints.
            if (!string.IsNullOrWhiteSpace(audienceRegionId) &&
                !eventRegionModelList.Any(region => region.RegionId == audienceRegionId))
            {
                return new EventModifierRequestModel();
            }

            eventModelList[0].Valid = true;
            eventModelList[0].Status = EventStatusCalculator.Calculate(eventModelList[0]);
            eventModelList[0].AudienceSummary = EventAudienceSummaryCalculator.Calculate(
                eventRoleModelList.Select(role => role.RoleId),
                eventSchoolModelList.Count > 0,
                eventRegionModelList.Count > 0,
                eventUserModelList.Count > 0);

            EventModifierRequestModel result = new EventModifierRequestModel
            {
                EventModel = eventModelList[0],
                EventAttachmentModelList = eventDAO.ReadAttachments(_connectionStringRead, eventId),
                EventRoleModelList = eventRoleModelList,
                EventSchoolModelList = eventSchoolModelList,
                EventRegionModelList = eventRegionModelList,
                EventUserModelList = eventUserModelList,
            };

            return result;
        }

        /// <summary>
        /// Returns a paginated list of events.
        /// </summary>
        /// <param name="filter">The event model used as filter.</param>
        /// <param name="from">The page number (0-based).</param>
        /// <param name="limit">How many items per page.</param>
        /// <param name="audienceRegionId">When present, only events explicitly assigned to this region are returned.</param>
        /// <returns>Returns the events response with pagination details.</returns>
        public EventsResponseModel GetEvents(EventModel filter, int from, int limit, string audienceRegionId = null)
        {
            EventDAO eventDAO = new EventDAO();

            // DAO.Read() only resolves real columns (Title/Available); Status/AudienceRoleId are never SQL filters.
            List<EventModel> events = eventDAO.Read(_connectionStringRead, filter, false, false);

            if (events.Count == 0)
            {
                return new EventsResponseModel();
            }

            // Aggregate query for the whole pre-pagination set — one call per page, never per event.
            // Reused below both to filter by role and to build AudienceSummary for the final page.
            Dictionary<string, HashSet<string>> roleMembership =
                eventDAO.ReadKnownRoleMembership(_connectionStringRead, events.Select(eventModel => eventModel.Id).ToList());

            // Calculate Status for every event first — EventStatusCalculator remains the only
            // place that knows the four status rules; nothing here duplicates that logic.
            events.ForEach(eventModel => eventModel.Status = EventStatusCalculator.Calculate(eventModel));

            if (!string.IsNullOrEmpty(filter.Status))
            {
                events = events.Where(eventModel => eventModel.Status == filter.Status).ToList();
            }

            if (!string.IsNullOrEmpty(filter.AudienceRoleId))
            {
                // Direct membership check against event_roles — never against AudienceSummary text.
                events = events.Where(eventModel =>
                    roleMembership.TryGetValue(eventModel.Id, out HashSet<string> roles) &&
                    roles.Contains(filter.AudienceRoleId)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(audienceRegionId))
            {
                Dictionary<string, HashSet<string>> regionMembership =
                    eventDAO.ReadKnownRegionMembership(
                        _connectionStringRead,
                        events.Select(eventModel => eventModel.Id).ToList());

                events = events.Where(eventModel =>
                    regionMembership.TryGetValue(eventModel.Id, out HashSet<string> regions) &&
                    regions.Contains(audienceRegionId)).ToList();
            }

            if (events.Count == 0)
            {
                return new EventsResponseModel();
            }

            EventsResponseModel result = new EventsResponseModel
            {
                TotalItems = events.Count,
                TotalPages = int.Parse(Math.Ceiling((double)events.Count / limit).ToString()),
            };

            result.EventModelList = events.Skip(from * limit).Take(limit).ToList();

            // AudienceSummary only needs Schools/Regions/Users presence for the events that made it
            // to the final page — not the whole pre-pagination set, since those three aren't filterable yet.
            List<string> pageEventIds = result.EventModelList.Select(eventModel => eventModel.Id).ToList();
            HashSet<string> eventIdsWithSchools = eventDAO.ReadEventIdsWithSchools(_connectionStringRead, pageEventIds);
            HashSet<string> eventIdsWithRegions = eventDAO.ReadEventIdsWithRegions(_connectionStringRead, pageEventIds);
            HashSet<string> eventIdsWithUsers = eventDAO.ReadEventIdsWithUsers(_connectionStringRead, pageEventIds);

            result.EventModelList.ForEach(eventModel =>
            {
                eventModel.Valid = true;
                HashSet<string> roles = roleMembership.TryGetValue(eventModel.Id, out HashSet<string> foundRoles)
                    ? foundRoles
                    : new HashSet<string>();
                eventModel.AudienceSummary = EventAudienceSummaryCalculator.Calculate(
                    roles,
                    eventIdsWithSchools.Contains(eventModel.Id),
                    eventIdsWithRegions.Contains(eventModel.Id),
                    eventIdsWithUsers.Contains(eventModel.Id));
            });

            return result;
        }
    }
}
