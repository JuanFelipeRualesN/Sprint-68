using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the accommodations types.
    /// </summary>
    public class AccommodationType
    {
        public const int IsCustom = 1;
        public const int IsDefault = 0;
        /// <summary>
        /// Unique identifier of the accommodation type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the accommodation type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A friendly text used to describe the accommodation type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Descriminator used to identify the accommodation type: default (0), custom (1).
        /// </summary>
        public int Type { get; set; }

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

        public virtual ICollection<TeacherPlanAccommodationType> TeacherPlanAccommodationTypes { get; set; } = new List<TeacherPlanAccommodationType>();
    }
}
