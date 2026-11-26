using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and material types.
    /// </summary>
    public class TeacherPlanMaterialType
    {
        /// <summary>
        /// Unique identifier of the teacher plan material type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan material id.
        /// </summary>
        public int TeacherPlanMaterialId { get; set; }

        /// <summary>
        /// The material type&apos;s id.
        /// </summary>
        public int MaterialTypeId { get; set; }

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

        public virtual MaterialType MaterialType { get; set; }

        public virtual TeacherPlanMaterial TeacherPlanMaterial { get; set; }
    }
}
