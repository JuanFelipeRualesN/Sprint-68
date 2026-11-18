using System;
using System.Collections.Generic;
using System.Linq;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and accommodation.
    /// </summary>
    public class TeacherPlanAccommodation
    {
        /// <summary>
        /// Unique identifier of the teacher plan accommodation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// The user who modified the accommodation.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the accommodation.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the accommodation was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual string Description
        {
            get
            {
                if (TeacherPlanAccommodationTypes.Any())
                {
                    return string.Join(", ",
                        TeacherPlanAccommodationTypes.Select(x => x.AccommodationType?.Description).ToArray());
                }

                return string.Empty;
            }
        }

        public virtual TeacherPlan TeacherPlan { get; set; }

        public virtual ICollection<TeacherPlanAccommodationDate> TeacherPlanAccommodationDates { get; set; } = new List<TeacherPlanAccommodationDate>();

        public virtual ICollection<TeacherPlanAccommodationType> TeacherPlanAccommodationTypes { get; set; } = new List<TeacherPlanAccommodationType>();
    }
}
