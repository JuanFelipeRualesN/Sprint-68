using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>custom_plan_client_lessons: a lesson in the plan and the progress of the student on each part. Unique per plan and lesson.</summary>
    public class CustomPlanClientLesson : IAuditableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to custom_plan_client (CASCADE).</summary>
        public ulong CustomPlanClientId { get; set; }

        /// <summary>Athenas lesson id (logical reference).</summary>
        public ulong LessonId { get; set; }

        /// <summary>Athenas standard id (logical reference). Null when the plan has no framework.</summary>
        public ulong? StandardId { get; set; }

        /// <summary>Athenas subject id (logical reference). Must be one of the plan's subjects; lessons are grouped by it.</summary>
        public ulong SubjectId { get; set; }

        /// <summary>Order of the lesson within the plan (unique per plan, service rule).</summary>
        public ushort Sequence { get; set; }

        /// <summary>First day the learning part (concept, definitions and examples) is assigned, local to <see cref="LearningTimezone"/>.</summary>
        public DateOnly LearningAssignedDate { get; set; }

        /// <summary>Last day to finish the learning part (inclusive), local to <see cref="LearningTimezone"/>. CHECK: not before <see cref="LearningAssignedDate"/>.</summary>
        public DateOnly LearningDueDate { get; set; }

        /// <summary>IANA time zone the learning dates are read in (max 64).</summary>
        public string LearningTimezone { get; set; } = null!;

        /// <summary>UTC. Set when concept, definitions and examples are all completed; cleared if one is undone.</summary>
        public DateTime? LearningCompletedAt { get; set; }

        /// <summary>Lesson status (DB default <c>not_started</c>).</summary>
        public PlanLessonStatus LessonStatus { get; set; }

        /// <summary>The concept section is completed.</summary>
        public bool ConceptCompleted { get; set; }

        /// <summary>The definitions section is completed.</summary>
        public bool DefinitionsCompleted { get; set; }

        /// <summary>The examples section is completed.</summary>
        public bool ExamplesCompleted { get; set; }

        /// <summary>The pre-test is completed. When the lesson has scheduled pre-tests, kept by the service from them; otherwise set by hand.</summary>
        public bool PreCompleted { get; set; }

        /// <summary>When the lesson has scheduled practices, true once all of them are completed (kept by the service).</summary>
        public bool PracticesCompleted { get; set; }

        /// <summary>The exams are completed. When the lesson has scheduled exams, kept by the service (all completed); otherwise set by hand.</summary>
        public bool TestCompleted { get; set; }

        /// <summary>The post-test is completed. When the lesson has scheduled post-tests, kept by the service from them; otherwise set by hand.</summary>
        public bool PostCompleted { get; set; }

        /// <summary>UTC. Set when <see cref="LessonStatus"/> becomes completed, cleared when it moves away.</summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>Minutes the student has studied this lesson so far, as reported by the app. DB default 0.</summary>
        public uint TotalStudyMinutes { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>Plan the lesson belongs to.</summary>
        public CustomPlanClient CustomPlanClient { get; set; } = null!;

        /// <summary>Study-session dates of the lesson.</summary>
        public List<CustomPlanClientLessonDate> Dates { get; set; } = new();

        /// <summary>Exams scheduled for the lesson.</summary>
        public List<CustomPlanClientLessonExamDate> ExamDates { get; set; } = new();

        /// <summary>Practices scheduled for the lesson.</summary>
        public List<CustomPlanClientLessonPractice> Practices { get; set; } = new();

        /// <summary>Pre-tests and post-tests scheduled for the lesson.</summary>
        public List<CustomPlanClientLessonTest> Tests { get; set; } = new();

        /// <summary>Evidences and projects attached to this lesson.</summary>
        public List<CustomPlanEvidence> Evidences { get; set; } = new();
    }
}
