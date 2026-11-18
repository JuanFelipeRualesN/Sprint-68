using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To establish which lesson quizzes are required and what due date the quiz should have. 
    /// </summary>
    public class TeacherPlanLessonQuiz
    {
        public int Id { get; set; }

        /// <summary>
        /// The plan lesson&apos;s id (parent). 
        /// </summary>
        public int? PlanLessonId { get; set; }

        /// <summary>
        /// Athena&apos;s quiz id. (Do not confuse this with the quiz entity of this DB). 
        /// </summary>
        public int? QuizId { get; set; }

        /// <summary>
        /// If required (1), (0) if not.
        /// </summary>
        public int? Required { get; set; }

        /// <summary>
        /// If will be considered for grading (1), (0) if not.
        /// </summary>
        public int? ForGrading { get; set; }

        /// <summary>
        /// On this date, the quiz is going to be available.
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// This is meant to be used with the class FindSystemTimeZoneById which we need the full string of the TimeZone. For example: SA Western Standard Time
        /// </summary>
        public string DueDateTimezone { get; set; }

        /// <summary>
        /// The duration of the exam in minutes. 
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The date when the row was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The date when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlanLesson PlanLesson { get; set; }
    }
}
