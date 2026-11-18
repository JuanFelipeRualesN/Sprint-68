using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and material dates.
    /// </summary>
    public class TeacherPlanMaterialDate
    {
        /// <summary>
        /// Unique identifier of the teacher plan material date.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan material id.
        /// </summary>
        public int TeacherPlanMaterialId { get; set; }

        /// <summary>
        /// Date on which the material will be used.
        /// </summary>
        public DateTime ImplementationDate { get; set; }

        /// <summary>
        /// The user who modified the material date.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the material date.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the material date was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlanMaterial TeacherPlanMaterial { get; set; }
    }
}
