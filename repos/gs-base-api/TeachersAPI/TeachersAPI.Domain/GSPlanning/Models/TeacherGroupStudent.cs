using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the students attached to a group created by a teacher (Student/group relationship).
    /// </summary>
    public class TeacherGroupStudent
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher group&apos;s id.
        /// </summary>
        public int TeacherGroupId { get; set; }

        /// <summary>
        /// The student&apos;s id (user&apos;s id of the student).
        /// </summary>
        public int StudentId { get; set; }

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

        public virtual TeacherGroup TeacherGroup { get; set; }
    }
}
