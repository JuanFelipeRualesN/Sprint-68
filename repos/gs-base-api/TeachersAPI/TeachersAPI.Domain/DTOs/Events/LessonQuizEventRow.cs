using System;

namespace TeachersAPI.Domain.DTOs.Events
{
    /// <summary>
    /// Query DTO holding a single lesson-quiz row (plan -&gt; lesson -&gt; lesson quiz branch)
    /// as materialized by Dapper before mapping to an event.
    /// </summary>
    public class LessonQuizEventRow
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
        /// Gets or sets the quiz's identifier.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets the quiz's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the quiz's due date.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the time zone code of the quiz's due date.
        /// </summary>
        public string DueDateTimezone { get; set; }

        /// <summary>
        /// Gets or sets the quiz's duration, in minutes.
        /// </summary>
        public int? Duration { get; set; }
    }
}
