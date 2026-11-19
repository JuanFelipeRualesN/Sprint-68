using System.Collections.Generic;
using TeachersAPI.Domain.Models.Events.Admin;

namespace TeachersAPI.Domain.Models.Generals.Responses.Admin
{
    /// <summary>
    /// To handle the single event response.
    /// </summary>
    /// <summary>
    /// To handle the single event response.
    /// </summary>
    public class EventResponseModel
    {
        /// <summary>
        /// Sets the event response.
        /// </summary>
        public EventResponseModel()
        {
            EventAttachmentModelList = new List<EventAttachmentModel>();
        }

        /// <summary>
        /// Gets or sets the event model.
        /// </summary>
        public EventModel EventModel { get; set; }

        /// <summary>
        /// Gets or sets the event attachments.
        /// </summary>
        public List<EventAttachmentModel> EventAttachmentModelList { get; set; }
    }
}
