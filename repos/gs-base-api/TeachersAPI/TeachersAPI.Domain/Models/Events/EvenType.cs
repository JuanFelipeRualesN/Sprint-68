namespace TeachersAPI.Domain.Models.Events.Admin
{
    /// <summary>
    /// Distinguishes the source of an event so the client can tell
    /// a homework from a lesson from a lesson-level quiz.
    /// </summary>
    public enum EventType
    {
        /// <summary>A quiz attached directly to a plan (homework).</summary>
        Homework = 1,

        /// <summary>A lesson within a plan.</summary>
        Lesson = 2,

        /// <summary>A quiz attached to a specific lesson.</summary>
        LessonQuiz = 3,

        /// <summary>
        /// To handle events in calendar (events module).
        /// </summary>
        CalendarEvent = 4,
    }
}
