using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// The relationship between the teacher&apos;s instructional plan and the groups. If there is a relationship, the student should see the instructional plan details as lessons, assignments, etc, but just if the student is attached to the group. 
    /// </summary>
    public class TeacherPlanGroup
    {
        /// <summary>
        /// An identifier.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The id of the teacher&apos;s plan.
        /// </summary>
        public int? TeacherPlanId { get; set; }

        /// <summary>
        /// The group&apos;s id.
        /// </summary>
        public int? GroupId { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The date when the row was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The date when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherGroup Group { get; set; }

        public virtual TeacherPlan TeacherPlan { get; set; }
    }
}
