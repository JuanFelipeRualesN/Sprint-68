using System;

namespace TeachersAPI.Domain.DTOs.Events
{
    /// <summary>
    /// Query DTO holding a single lesson row (plan -&gt; lesson branch)
    /// as materialized by Dapper before mapping to an event.
    /// </summary>
    public class LessonEventRow
    {
        /// <summary>
        /// Gets or sets the plan's identifier.
        /// </summary>
        public string PlanId { get; set; }

        /// <summary>
        /// Gets or sets the plan's name.
        /// </summary>
        public string PlanName { get; set; }

        /// <summary>
        /// Gets or sets the lesson's identifier.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets the lesson's due date.
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}
