using System.Collections.Generic;
using TeachersAPI.Domain.Models.Events;
using TeachersResponseModel = TeachersAPI.Domain.Models.Teachers.ResponseModel;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the single event response.
    /// </summary>
    public class EventResponseModel : TeachersResponseModel
    {
        /// <summary>
        /// Sets the event response.
        /// </summary>
        public EventResponseModel()
        {
            EventAttachmentModelList = new List<EventAttachmentModel>();
            EventRoleModelList = new List<EventRoleModel>();
            EventSchoolModelList = new List<EventSchoolModel>();
            EventRegionModelList = new List<EventRegionModel>();
            EventUserModelList = new List<EventUserModel>();
        }

        /// <summary>
        /// Gets or sets the event model.
        /// </summary>
        public EventModel EventModel { get; set; }

        /// <summary>
        /// Gets or sets the event attachments.
        /// </summary>
        public List<EventAttachmentModel> EventAttachmentModelList { get; set; }

        /// <summary>
        /// Gets or sets the roles associated with this event.
        /// </summary>
        public List<EventRoleModel> EventRoleModelList { get; set; }

        /// <summary>
        /// Gets or sets the schools associated with this event.
        /// </summary>
        public List<EventSchoolModel> EventSchoolModelList { get; set; }

        /// <summary>
        /// Gets or sets the regions associated with this event.
        /// </summary>
        public List<EventRegionModel> EventRegionModelList { get; set; }

        /// <summary>
        /// Gets or sets the users associated with this event.
        /// </summary>
        public List<EventUserModel> EventUserModelList { get; set; }
    }
}
