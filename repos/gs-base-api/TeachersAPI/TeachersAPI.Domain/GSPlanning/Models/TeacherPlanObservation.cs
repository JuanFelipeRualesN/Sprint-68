using GSPlanning.Domain;

namespace GSPlanning.Models
{
    public class TeacherPlanObservation
    {
        /// <summary>
        /// Unique identifier of the teacher plan observation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher plan id.
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// Indicates whether the observation was completed or not.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// More details about the observation.
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// The user who last modified the observation.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Datetime when the observation was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Datetime when the observation was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Navigation property to the related teacher plan (optional).
        /// </summary>
        public virtual TeacherPlan TeacherPlan { get; set; } = null!;

        /// <summary>
        /// Navigation property to the observation dates.
        /// </summary>
        public virtual ICollection<TeacherPlanObservationDate> TeacherPlanObservationDates { get; set; }
            = new List<TeacherPlanObservationDate>();
    }
}
