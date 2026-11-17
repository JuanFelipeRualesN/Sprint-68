using TeachersAPI.Domain.Models.Teachers;
using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class TeacherStudentAnnotationResponseModel
    {
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Gets or sets the teacher student annotation.
        /// </summary>
        public List<TeacherStudentAnnotationModel> TeacherStudentAnnotationModelList { get; set; }
    }
}