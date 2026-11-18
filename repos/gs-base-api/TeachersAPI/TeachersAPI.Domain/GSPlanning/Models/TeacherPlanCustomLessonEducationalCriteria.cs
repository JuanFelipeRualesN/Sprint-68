using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class TeacherPlanCustomLessonEducationalCriteria
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TeacherPlanCustomLessonId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EducationalCriteriaId { get; set; }
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

        public virtual EducationalCriteria EducationalCriteria { get; set; }
    }
}