using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and accommodation dates.
    /// </summary>
    public class TeacherPlanAccommodationDate
    {
        /// <summary>
        /// Unique identifier of the teacher plan accommodation date.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan accommodation id.
        /// </summary>
        public int TeacherPlanAccommodationId { get; set; }

        /// <summary>
        /// Date on which the accommodation will be used.
        /// </summary>
        public DateTime ImplementationDate { get; set; }

        /// <summary>
        /// The user who modified the accommodation date.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the accommodation date.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the accommodation date was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlanAccommodation TeacherPlanAccommodation { get; set; }
    }
}
