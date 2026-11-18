using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the groups created by a teacher.
    /// </summary>
    public class TeacherGroup
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s id (the user&apos;s id).
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// The subject code. Example: MAT-EN, MAT-SP, etc.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// The level code. Example: K, 1, 2, 3....
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// The group&apos;s name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If the group is available or not.
        /// </summary>
        public int? Available { get; set; }

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

        public virtual ICollection<TeacherGroupStudent> TeacherGroupStudents { get; set; } = new List<TeacherGroupStudent>();

        public virtual ICollection<TeacherPlanGroup> TeacherPlanGroups { get; set; } = new List<TeacherPlanGroup>();
    }
}
