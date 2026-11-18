using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the materials types.
    /// </summary>
    public class MaterialType
    {
        public const int IsCustom = 1;
        public const int IsDefault = 0;
        
        /// <summary>
        /// Unique identifier of the material type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the material type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A friendly text used to describe the material type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Descriminator used to identify the material type: default (0) , custom (1).
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// The user who modified the material type.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the material type.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the material type was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<TeacherPlanMaterialType> TeacherPlanMaterialTypes { get; set; } = new List<TeacherPlanMaterialType>();
    }
}
