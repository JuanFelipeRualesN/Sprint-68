using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    /// <summary>
    /// To handle the teacher-group-student for Dapper.
    /// </summary>
    public class TeacherGroupStudentModel
    {
        /// <summary>
        /// Gets or sets the group's id.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group's name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets whether the group is available or not.
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Gets or sets the teacher's id.
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the subject's code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the level's code.
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// Gets or sets the teacher groups student's id.
        /// </summary>
        public string TeacherGroupStudentId { get; set; }

        /// <summary>
        /// Gets or sets the student's id.
        /// </summary>
        public string StudentId { get; set; }

    }
}