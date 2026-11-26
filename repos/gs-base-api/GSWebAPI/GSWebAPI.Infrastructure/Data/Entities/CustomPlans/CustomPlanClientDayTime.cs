using GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.Common;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>custom_plan_client_day_times: a study session (start time and duration) on a plan day. Unique per day and start time.</summary>
    public class CustomPlanClientDayTime : IAuditableEntity
    {
        /// <summary>Primary key.</summary>
        public ulong Id { get; set; }

        /// <summary>FK to custom_plan_client_days (CASCADE).</summary>
        public ulong CustomPlanClientDayId { get; set; }

        /// <summary>Local time in <see cref="Timezone"/>.</summary>
        public TimeOnly StartTime { get; set; }

        /// <summary>Length of the session in minutes (not 0).</summary>
        public ushort DurationMinutes { get; set; }

        /// <summary>IANA time zone name (max 64).</summary>
        public string Timezone { get; set; } = null!;

        /// <summary>UTC creation time; set by the audit interceptor.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>UTC time of the last update; set by the audit interceptor.</summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>Day the session belongs to.</summary>
        public CustomPlanClientDay Day { get; set; } = null!;
    }
}
