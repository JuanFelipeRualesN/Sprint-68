using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>custom_plan_client_days: a day of the week the student studies in a plan. Unique per plan and day.</summary>
    public class CustomPlanClientDay : IAuditableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to custom_plan_client (CASCADE).</summary>
        public ulong CustomPlanClientId { get; set; }

        /// <summary>ISO 8601: 1 = Monday ... 7 = Sunday (CHECK 1–7).</summary>
        public byte DayOfWeek { get; set; }

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>Plan the day belongs to.</summary>
        public CustomPlanClient CustomPlanClient { get; set; } = null!;

        /// <summary>Study sessions on this day.</summary>
        public List<CustomPlanClientDayTime> Times { get; set; } = new();
    }
}
