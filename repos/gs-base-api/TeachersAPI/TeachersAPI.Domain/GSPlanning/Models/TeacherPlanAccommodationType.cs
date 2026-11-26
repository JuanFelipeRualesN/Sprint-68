using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and accommodation types.
    /// </summary>
    public class TeacherPlanAccommodationType
    {
        /// <summary>
        /// Unique identifier of the teacher plan accommodation type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan accommodation id.
        /// </summary>
        public int TeacherPlanAccommodationId { get; set; }

        /// <summary>
        /// The accommodation type&apos;s id.
        /// </summary>
        public int AccommodationTypeId { get; set; }

        /// <summary>
        /// The user who modified the accommodation type.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the accommodation type.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the accommodation type was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual AccommodationType AccommodationType { get; set; }

        public virtual TeacherPlanAccommodation TeacherPlanAccommodation { get; set; }
    }
}
