using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.Reports
{
    /// <summary>
    /// To handle the results by students.
    /// </summary>
    public class ReportGroupStudentResultsModel
    {
        /// <summary>
        /// Gets or sets the user's id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the student's given name.
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Gets or sets the student's surname.
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// Gets or sets the main group.
        /// </summary>
        public string MainGroupCode { get; set; }

        /// <summary>
        /// Gets or sets the teacher's groups.
        /// </summary>
        public List<string> TeacherGroups { get; set; }

        /// <summary>
        /// Gets or sets the report by quiz.
        /// </summary>
        public List<ReportQuizModel> ReportQuizModels { get; set; }
    }

}