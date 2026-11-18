using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Response for a director listing the groups created by their teachers
    /// </summary>
    public class DirectorTeacherGroupsResponseModel
    {
        /// <summary>
        /// Gets or sets the teachers, each with their own groups.
        /// </summary>
        public List<DirectorTeacherModel> Teachers { get; set; } = new List<DirectorTeacherModel>();

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// A teacher belonging to the director, together with the groups they created.
    /// </summary>
    public class DirectorTeacherModel
    {
        /// <summary>
        /// Gets or sets the teacher's id.
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the teacher's full name.
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// Gets or sets the groups created by the teacher.
        /// </summary>
        public List<DirectorTeacherGroupModel> Groups { get; set; } = new List<DirectorTeacherGroupModel>();
    }

    /// <summary>
    /// A teacher group with the ids of the students that belong to it.
    /// </summary>
    public class DirectorTeacherGroupModel
    {
        /// <summary>
        /// Gets or sets the group's id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the group's name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the group's subject code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the group's level code.
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// Gets or sets the number of students in the group.
        /// </summary>
        public int TotalStudents { get; set; }

        /// <summary>
        /// Gets or sets the ids of the students that belong to the group.
        /// </summary>
        public List<string> StudentIds { get; set; } = new List<string>();
    }
}
