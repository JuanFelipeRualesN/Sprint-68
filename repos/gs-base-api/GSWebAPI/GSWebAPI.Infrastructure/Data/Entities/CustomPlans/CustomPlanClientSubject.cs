using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>custom_plan_client_subjects: an Athenas subject included in a plan. Unique per plan and subject.</summary>
    public class CustomPlanClientSubject : IAuditableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to custom_plan_client (CASCADE).</summary>
        public ulong CustomPlanClientId { get; set; }

        /// <summary>Athenas subject id (logical reference).</summary>
        public ulong SubjectId { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>Plan the subject belongs to.</summary>
        public CustomPlanClient CustomPlanClient { get; set; } = null!;
    }
}
