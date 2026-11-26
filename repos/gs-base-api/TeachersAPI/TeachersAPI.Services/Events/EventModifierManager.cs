using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeachersAPI;
using TeachersAPI.Domain.Models.Events;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Infrastructure.Repositories.Events;
using TeachersAPI.TeachersAPI.Services.Generals;

namespace TeachersAPI.TeachersAPI.Services.Events
{
    public class EventModifierManager
    {
        private readonly string _connectionString, _connectionStringRead, _langCode;

        public EventModifierManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        public async Task<EventResponseModel> CreateEvent(EventModifierRequestModel request, string userSessionId, EventOrganizerModel organizer)
        {
            try
            {
                EventResponseModel responseModel = new EventResponseModel
                {
                    Valid = true,
                    Messages = new List<string>()
                };

                MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);

                if (string.IsNullOrWhiteSpace(request.EventModel.Title) || request.EventModel.Title.Trim().Length < 3)
                {
                    return new EventResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_title_required"])
                        }
                    };
                }

                if (string.IsNullOrWhiteSpace(request.EventModel.StartDate))
                {
                    return new EventResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"])
                        }
                    };
                }

                // EndDate is optional (recommended, not required) — only validated when provided.
                if (!string.IsNullOrWhiteSpace(request.EventModel.EndDate) &&
                    DateTime.TryParse(request.EventModel.StartDate, out DateTime startDateToValidate) &&
                    DateTime.TryParse(request.EventModel.EndDate, out DateTime endDateToValidate) &&
                    startDateToValidate >= endDateToValidate)
                {
                    return new EventResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"])
                        }
                    };
                }

                // New events only — edits to already-past events (e.g. adding attachments later) must stay allowed.
                // Compared in the event's own timezone, since StartDate is naive local time in that zone.
                if (DateTime.TryParse(request.EventModel.StartDate, out DateTime startDateForPastCheck) &&
                    startDateForPastCheck < EventTimeProvider.NowInZone(request.EventModel.TimeZoneCode))
                {
                    return new EventResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"])
                        }
                    };
                }

                // `timezone_code` is NOT NULL in the DB with no default. Without this check, an
                // empty value reaches EventDAO.Create, which silently omits the column from the
                // INSERT, the DB rejects it, and the caller ends up indexing an empty result list —
                // surfacing a confusing "Index was out of range" instead of a real validation error.
                // No dedicated message code exists for this yet (would require a new messages-table
                // row), so this returns a literal, language-aware string instead.
                if (string.IsNullOrWhiteSpace(request.EventModel.TimeZoneCode))
                {
                    return new EventResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            _langCode == "en" ? "Time zone is required." : "La zona horaria es obligatoria."
                        }
                    };
                }

                EventDAO eventDAO = new EventDAO();

                request.EventModel.Id = Guid.NewGuid().ToString();
                request.EventModel.CreatedBy = userSessionId;
                request.EventModel.ModifiedBy = userSessionId;
                request.EventModel.Available = "1";
                // Set synchronously; the actual Teams sync runs in the background (queued below).
                request.EventModel.TeamsSyncStatus = EventTeamsSyncStatuses.Pending;
                request.EventModel.CreatedAt = null; // This is set on DB
                request.EventModel.UpdatedAt = null; // This is set on DB

                if (request.EventAttachmentModelList != null)
                {
                    foreach (var attachment in request.EventAttachmentModelList)
                    {
                        attachment.ModifiedBy = userSessionId;
                        attachment.CreatedAt = null; // This is set on DB
                        attachment.UpdatedAt = null; // This is set on DB
                    }
                }

                if (request.EventRoleModelList != null)
                {
                    foreach (var role in request.EventRoleModelList)
                    {
                        role.ModifiedBy = userSessionId;
                        role.CreatedAt = null; // This is set on DB
                        role.UpdatedAt = null; // This is set on DB
                    }
                }

                if (request.EventSchoolModelList != null)
                {
                    foreach (var school in request.EventSchoolModelList)
                    {
                        school.ModifiedBy = userSessionId;
                        school.CreatedAt = null; // This is set on DB
                        school.UpdatedAt = null; // This is set on DB
                    }
                }

                if (request.EventRegionModelList != null)
                {
                    foreach (var region in request.EventRegionModelList)
                    {
                        region.ModifiedBy = userSessionId;
                        region.CreatedAt = null; // This is set on DB
                        region.UpdatedAt = null; // This is set on DB
                    }
                }

                if (request.EventUserModelList != null)
                {
                    foreach (var user in request.EventUserModelList)
                    {
                        user.ModifiedBy = userSessionId;
                        user.CreatedAt = null; // This is set on DB
                        user.UpdatedAt = null; // This is set on DB
                    }
                }

                request.EventAttachmentModelList ??= new List<EventAttachmentModel>();
                request.EventRoleModelList ??= new List<EventRoleModel>();
                request.EventSchoolModelList ??= new List<EventSchoolModel>();
                request.EventRegionModelList ??= new List<EventRegionModel>();
                request.EventUserModelList ??= new List<EventUserModel>();

                responseModel.EventModel = eventDAO.Create(_connectionString, request.EventModel)[0];
                responseModel.EventModel.Valid = true;
                responseModel.EventModel.Status = EventStatusCalculator.Calculate(responseModel.EventModel);
                responseModel.EventModel.AudienceSummary = EventAudienceSummaryCalculator.Calculate(
                    request.EventRoleModelList.Select(role => role.RoleId),
                    request.EventSchoolModelList.Count > 0,
                    request.EventRegionModelList.Count > 0,
                    request.EventUserModelList.Count > 0);
                eventDAO.UpsertEventRelations(_connectionString, request.EventModel.Id, request.EventAttachmentModelList, request.EventRoleModelList, request.EventSchoolModelList, request.EventRegionModelList, request.EventUserModelList);

                responseModel.EventAttachmentModelList = request.EventAttachmentModelList ?? new List<EventAttachmentModel>();
                responseModel.EventRoleModelList = request.EventRoleModelList ?? new List<EventRoleModel>();
                responseModel.EventSchoolModelList = request.EventSchoolModelList ?? new List<EventSchoolModel>();
                responseModel.EventRegionModelList = request.EventRegionModelList ?? new List<EventRegionModel>();
                responseModel.EventUserModelList = request.EventUserModelList ?? new List<EventUserModel>();

                responseModel.Messages.Add(
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]));

                try
                {
                    // Queued to run in the background (EventTeamsSyncWorker) so audience
                    // resolution and Graph sync never block this response.
                    string connectionString = _connectionString;
                    List<EventRoleModel> roles = request.EventRoleModelList;
                    List<EventSchoolModel> schools = request.EventSchoolModelList;
                    List<EventRegionModel> regions = request.EventRegionModelList;
                    List<EventUserModel> users = request.EventUserModelList;
                    EventModel eventModelForSync = responseModel.EventModel;

                    await EventTeamsSyncQueueAccessor.Instance.EnqueueAsync((CancellationToken ct) =>
                        new EventTeamsManager(connectionString).SyncEventCreation(
                            eventModelForSync, roles, schools, regions, users, organizer));
                }
                catch { }

                return responseModel;
            }
            catch (Exception ex)
            {
                return new EventResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<EventResponseModel> UpdateEvent(EventModifierRequestModel request, string userSessionId, EventOrganizerModel organizer)
        {
            try
            {
                EventResponseModel responseModel = new EventResponseModel
                {
                    Valid = true,
                    Messages = new List<string>()
                };

                MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);

                if (string.IsNullOrWhiteSpace(request.EventModel.Title) || request.EventModel.Title.Trim().Length < 3)
                {
                    return new EventResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_title_required"])
                        }
                    };
                }

                if (string.IsNullOrWhiteSpace(request.EventModel.StartDate))
                {
                    return new EventResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"])
                        }
                    };
                }

                // EndDate is optional (recommended, not required) — only validated when provided.
                if (!string.IsNullOrWhiteSpace(request.EventModel.EndDate) &&
                    DateTime.TryParse(request.EventModel.StartDate, out DateTime startDateToValidate) &&
                    DateTime.TryParse(request.EventModel.EndDate, out DateTime endDateToValidate) &&
                    startDateToValidate >= endDateToValidate)
                {
                    return new EventResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_error_invalid_date"])
                        }
                    };
                }

                // Same guard as CreateEvent — see the comment there for why this matters.
                if (string.IsNullOrWhiteSpace(request.EventModel.TimeZoneCode))
                {
                    return new EventResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>()
                        {
                            _langCode == "en" ? "Time zone is required." : "La zona horaria es obligatoria."
                        }
                    };
                }

                EventDAO eventDAO = new EventDAO();

                // Update()'s response echoes back the object used to build the SET clause, which
                // never carries TeamsEventId (the request from the client never sends it). Read
                // the currently-persisted value first so EventTeamsManager can tell apart "never
                // synced" (create) from "already synced" (update) further down. Also used to
                // preserve CreatedBy, which is immutable and never accepted from the client.
                List<EventModel> existingEvents = eventDAO.Read(_connectionStringRead, new EventModel { Id = request.EventModel.Id }, true, false);
                string existingTeamsEventId = existingEvents.Count > 0 ? existingEvents[0].TeamsEventId : null;
                string existingCreatedBy = existingEvents.Count > 0 ? existingEvents[0].CreatedBy : null;

                request.EventModel.CreatedBy = null; // Immutable — never taken from the client on edit.
                request.EventModel.ModifiedBy = userSessionId;
                // Set synchronously; the actual re-sync runs in the background (queued below).
                request.EventModel.TeamsSyncStatus = EventTeamsSyncStatuses.Pending;
                request.EventModel.CreatedAt = null; // This is set on DB
                request.EventModel.UpdatedAt = null; // This is set on DB

                if (request.EventAttachmentModelList != null)
                {
                    foreach (var attachment in request.EventAttachmentModelList)
                    {
                        attachment.ModifiedBy = userSessionId;
                        attachment.CreatedAt = null; // This is set on DB
                        attachment.UpdatedAt = null; // This is set on DB
                    }
                }

                if (request.EventRoleModelList != null)
                {
                    foreach (var role in request.EventRoleModelList)
                    {
                        role.ModifiedBy = userSessionId;
                        role.CreatedAt = null; // This is set on DB
                        role.UpdatedAt = null; // This is set on DB
                    }
                }

                if (request.EventSchoolModelList != null)
                {
                    foreach (var school in request.EventSchoolModelList)
                    {
                        school.ModifiedBy = userSessionId;
                        school.CreatedAt = null; // This is set on DB
                        school.UpdatedAt = null; // This is set on DB
                    }
                }

                if (request.EventRegionModelList != null)
                {
                    foreach (var region in request.EventRegionModelList)
                    {
                        region.ModifiedBy = userSessionId;
                        region.CreatedAt = null; // This is set on DB
                        region.UpdatedAt = null; // This is set on DB
                    }
                }

                if (request.EventUserModelList != null)
                {
                    foreach (var user in request.EventUserModelList)
                    {
                        user.ModifiedBy = userSessionId;
                        user.CreatedAt = null; // This is set on DB
                        user.UpdatedAt = null; // This is set on DB
                    }
                }

                EventModel eventModelWhere = new EventModel()
                {
                    Id = request.EventModel.Id
                };

                request.EventAttachmentModelList ??= new List<EventAttachmentModel>();
                request.EventRoleModelList ??= new List<EventRoleModel>();
                request.EventSchoolModelList ??= new List<EventSchoolModel>();
                request.EventRegionModelList ??= new List<EventRegionModel>();
                request.EventUserModelList ??= new List<EventUserModel>();

                responseModel.EventModel = eventDAO.Update(_connectionString, request.EventModel, eventModelWhere)[0];
                responseModel.EventModel.TeamsEventId = existingTeamsEventId;
                responseModel.EventModel.CreatedBy = existingCreatedBy;
                responseModel.EventModel.Valid = true;
                responseModel.EventModel.Status = EventStatusCalculator.Calculate(responseModel.EventModel);
                responseModel.EventModel.AudienceSummary = EventAudienceSummaryCalculator.Calculate(
                    request.EventRoleModelList.Select(role => role.RoleId),
                    request.EventSchoolModelList.Count > 0,
                    request.EventRegionModelList.Count > 0,
                    request.EventUserModelList.Count > 0);
                eventDAO.UpsertEventRelations(_connectionString, request.EventModel.Id, request.EventAttachmentModelList, request.EventRoleModelList, request.EventSchoolModelList, request.EventRegionModelList, request.EventUserModelList);

                responseModel.EventAttachmentModelList = request.EventAttachmentModelList ?? new List<EventAttachmentModel>();
                responseModel.EventRoleModelList = request.EventRoleModelList ?? new List<EventRoleModel>();
                responseModel.EventSchoolModelList = request.EventSchoolModelList ?? new List<EventSchoolModel>();
                responseModel.EventRegionModelList = request.EventRegionModelList ?? new List<EventRegionModel>();
                responseModel.EventUserModelList = request.EventUserModelList ?? new List<EventUserModel>();

                responseModel.Messages.Add(
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]));

                try
                {
                    // Same reasoning as CreateEvent
                    string connectionString = _connectionString;
                    List<EventRoleModel> roles = request.EventRoleModelList;
                    List<EventSchoolModel> schools = request.EventSchoolModelList;
                    List<EventRegionModel> regions = request.EventRegionModelList;
                    List<EventUserModel> users = request.EventUserModelList;
                    EventModel eventModelForSync = responseModel.EventModel;

                    await EventTeamsSyncQueueAccessor.Instance.EnqueueAsync((CancellationToken ct) =>
                        new EventTeamsManager(connectionString).SyncEventUpdate(
                            eventModelForSync, roles, schools, regions, users, organizer));
                }
                catch { }

                return responseModel;
            }
            catch (Exception ex)
            {
                return new EventResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        ex.Message
                    }
                };
            }
        }

        public async Task<EventResponseModel> DeleteEvent(string eventId, string userSessionId, EventOrganizerModel organizer)
        {
            try
            {
                EventResponseModel responseModel = new EventResponseModel
                {
                    Valid = true,
                    Messages = new List<string>()
                };

                MessageManager messageManager = new MessageManager(_connectionStringRead, _langCode);
                EventDAO eventDAO = new EventDAO();

                // Same reasoning as UpdateEvent: Update()'s response never carries TeamsEventId,
                // so it must be read before the soft delete to know whether Graph must be called.
                List<EventModel> existingEvents = eventDAO.Read(_connectionStringRead, new EventModel { Id = eventId }, true, false);
                string existingTeamsEventId = existingEvents.Count > 0 ? existingEvents[0].TeamsEventId : null;

                EventModel eventModelSet = new EventModel()
                {
                    Available = "0",
                    ModifiedBy = userSessionId,
                    CreatedAt = null, // This is set on DB
                    UpdatedAt = null, // This is set on DB
                };

                EventModel eventModelWhere = new EventModel()
                {
                    Id = eventId,
                };

                responseModel.EventModel = eventDAO.Update(_connectionString, eventModelSet, eventModelWhere)[0];
                responseModel.EventModel.Id = eventId;
                responseModel.EventModel.TeamsEventId = existingTeamsEventId;

                responseModel.Messages.Add(
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]));

                try
                {
                    await new EventTeamsManager(_connectionString).SyncEventDeletion(responseModel.EventModel, organizer);
                }
                catch { }

                return responseModel;
            }
            catch (Exception ex)
            {
                return new EventResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}
