namespace GSWebAPI.GSWebAPI.Infrastructure.Data.Entities.CustomPlans
{
    /// <summary>
    /// A scheduled activity of a plan lesson: a practice, an exam, or a pre/post test. All share the same status rules.
    /// </summary>
    public interface IPlanActivity
    {
        /// <summary>FK to custom_plan_client_lessons (CASCADE): the lesson the activity is scheduled for.</summary>
        ulong CustomPlanClientLessonId { get; }

        /// <summary>Day the activity opens, local to <see cref="Timezone"/>.</summary>
        DateOnly AssignedDate { get; }

        /// <summary>UTC deadline. Shown to the student in <see cref="Timezone"/>.</summary>
        DateTime DueDateTime { get; }

        /// <summary>IANA time zone of the activity (max 64).</summary>
        string Timezone { get; }

        /// <summary>Activity status (DB default <c>not_started</c>).</summary>
        PlanActivityStatus Status { get; set; }

        /// <summary>UTC. Set when the status becomes completed; cleared when it moves away.</summary>
        DateTime? CompletedAt { get; set; }
    }
}
