using System;
using System.Collections.Generic;
using System.Linq;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the relationship between teacher plan and material dates.
    /// </summary>
    public class TeacherPlanMaterial
    {
        /// <summary>
        /// Unique identifier of the teacher plan material.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id.
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// The user who modified the material.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the material.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the material was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Description
        {
            get
            {
                if (TeacherPlanMaterialTypes.Any())
                {
                    return string.Join(", ", TeacherPlanMaterialTypes.Select(x => x.MaterialType?.Name).ToArray());
                }

                return string.Empty;
            }
        }

        public virtual TeacherPlan TeacherPlan { get; set; }

        public virtual ICollection<TeacherPlanMaterialDate> TeacherPlanMaterialDates { get; set; } = new List<TeacherPlanMaterialDate>();

        public virtual ICollection<TeacherPlanMaterialType> TeacherPlanMaterialTypes { get; set; } = new List<TeacherPlanMaterialType>();
    }
}
