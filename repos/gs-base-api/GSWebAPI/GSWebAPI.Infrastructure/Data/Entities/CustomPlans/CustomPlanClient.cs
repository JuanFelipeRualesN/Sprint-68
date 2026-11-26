using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>
    /// Plan header: custom_plan_client. A study plan for one student, owned by the paying client. Soft-deleted.
    /// </summary>
    public class CustomPlanClient : ISoftDeletableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to custom_plan_types (RESTRICT).</summary>
        public byte CustomPlanTypeId { get; set; }

        /// <summary>ul_user_accounts.id of the paying account holder (logical reference to the users DB).</summary>
        public ulong ClientId { get; set; }

        /// <summary>ul_user_accounts.id of the student (logical reference to the users DB). Must be a profile the client manages.</summary>
        public ulong StudentId { get; set; }

        /// <summary>Athenas level id (logical reference).</summary>
        public ulong LevelId { get; set; }

        /// <summary>FK to curriculum_frameworks (RESTRICT). Null = no framework, so every lesson's standard must be null.</summary>
        public ushort? CurriculumFrameworkId { get; set; }

        /// <summary>Planned length of the plan in weeks (not 0).</summary>
        public ushort PlanDurationWeeks { get; set; }

        /// <summary>First day of the plan.</summary>
        public DateOnly StartDate { get; set; }

        /// <summary>Last day of the plan. The service fills it with <see cref="StartDate"/> + <see cref="PlanDurationWeeks"/> when not sent.</summary>
        public DateOnly? EndDate { get; set; }

        /// <summary>Plan status (DB default <c>draft</c>). The structure can only be edited while in draft.</summary>
        public CustomPlanStatus Status { get; set; }

        /// <summary>
        /// Minutes the student has studied in this plan so far: the sum of the lessons' <see cref="CustomPlanClientLesson.TotalStudyMinutes"/>,
        /// kept by the service in the same transaction. DB default 0.
        /// </summary>
        public uint TotalStudyMinutes { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>UTC soft-delete time; null = not deleted. Rows with a value are hidden by the global query filter.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Type of the plan.</summary>
        public CustomPlanType CustomPlanType { get; set; } = null!;

        /// <summary>Curriculum framework of the plan, if any.</summary>
        public CurriculumFramework? CurriculumFramework { get; set; }

        /// <summary>Athenas subjects included in the plan.</summary>
        public List<CustomPlanClientSubject> Subjects { get; set; } = new();

        /// <summary>Days of the week the student studies.</summary>
        public List<CustomPlanClientDay> Days { get; set; } = new();

        /// <summary>Lessons of the plan.</summary>
        public List<CustomPlanClientLesson> Lessons { get; set; } = new();

        /// <summary>Evidences and projects attached to the plan.</summary>
        public List<CustomPlanEvidence> Evidences { get; set; } = new();
    }
}
