using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Teachers
{
    /// <summary>
    /// To handle the response for teacher groups.
    /// </summary>
    public class TeacherGroupResponseModel : ResponseModel
    {
        /// <summary>
        /// Gets or sets teacher groups list.
        /// </summary>
        public List<TeacherGroupExtraDetailModel> TeacherGroupModelList { get; set; }

        /// <summary>
        /// Gets or sets the total results
        /// </summary>
        public int TotalResults { get; set; }
    }

    /// <summary>
    /// To get the extra details.
    /// </summary>
    public class TeacherGroupExtraDetailModel : TeacherGroupModel
    {
        /// <summary>
        /// The total students in the group.
        /// </summary>
        public int TotalStudents { get; set; }

        public List<TeacherGroupStudentModel> TeacherGroupStudentModelList { get; set; }
    }
}
