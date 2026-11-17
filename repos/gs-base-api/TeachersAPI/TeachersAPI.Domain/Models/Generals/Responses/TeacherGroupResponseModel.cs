using TeachersAPI.Domain.Models.Teachers;
using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Teacher group response model.
    /// </summary>
    public class TeacherGroupResponseModel
    {
        /// <summary>
        /// Gets or sets teacher group model list.
        /// </summary>
        public List<TeacherGroupExtraDetailModel> TeacherGroupModelList { get; set; }

        /// <summary>
        /// Gets or sets the student details.
        /// </summary>
        public List<StudentDetail> StudentDetailModelList { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }
    }

    public class StudentDetail
    {
        /// <summary>
        /// Gets or sets invitation code.
        /// </summary>
        public string InvitationCode { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the school record.
        /// </summary>
        public string UserSchoolRecord { get; set; }

        /// <summary>
        /// Gets or sets the User Id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

    }
}