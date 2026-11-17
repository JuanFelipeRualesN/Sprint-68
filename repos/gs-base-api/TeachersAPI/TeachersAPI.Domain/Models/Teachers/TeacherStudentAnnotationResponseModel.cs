using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class TeacherStudentAnnotationResponseModel : ResponseModel
    {
        /// <summary>
        /// Gets or sets the total results.
        /// </summary>
        public int TotalResults { get; set; }

        /// <summary>
        /// Gets or sets the teachers student annotation.
        /// </summary>
        public List<TeacherStudentAnnotationModel> TeacherStudentAnnotationModelList { get; set; }
    }
}
