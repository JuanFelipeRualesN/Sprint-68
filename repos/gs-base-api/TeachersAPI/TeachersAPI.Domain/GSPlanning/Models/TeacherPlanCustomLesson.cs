using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the custom lessons related to the instruction plans.
    /// </summary>
    public class TeacherPlanCustomLesson
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher’s plan id.
        /// </summary>
        public int TeacherPlanId { get; set; }

        /// <summary>
        /// The lesson title/name.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual TeacherPlan TeacherPlan { get; set; }

        public virtual ICollection<TeacherPlanCustomLessonEducationalCriteria> TeacherPlanCustomLessonEducationalCriterias { get; set; } = new List<TeacherPlanCustomLessonEducationalCriteria>();
        
        public virtual ICollection<TeacherPlanCustomLessonDate> TeacherPlanCustomLessonDates { get; set; } = new List<TeacherPlanCustomLessonDate>();
    }
}
