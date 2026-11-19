using System.Collections.Generic;
using TeachersAPI.Domain.Models.Events;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the events list response.
    /// </summary>
    public class EventsResponseModel
    {
        /// <summary>
        /// Sets the event list.
        /// </summary>
        public EventsResponseModel()
        {
            EventModelList = new List<EventModel>();
        }

        /// <summary>
        /// Gets or sets the event model list.
        /// </summary>
        public List<EventModel> EventModelList { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        public int TotalItems { get; set; }
    }
}
