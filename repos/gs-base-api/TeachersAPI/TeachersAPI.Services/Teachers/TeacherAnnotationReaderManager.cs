using System.Collections.Generic;
using System.Linq;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;

namespace TeachersAPI.TeachersAPI.Services.Teachers
{
    /// <summary>
    /// To read the teacher annotation.
    /// </summary>
    public class TeacherAnnotationReaderManager
    {
        private readonly string _connectionStringRead, _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The user's language code.</param>
        public TeacherAnnotationReaderManager(string connectionStringRead, string langCode)
        {
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// Get the teacher instructional plan student's annotations.
        /// </summary>
        /// <param name="teacherAnnotationToFilter">Teacher annotation to filter.</param>
        /// <param name="from">From Page.</param>
        /// <param name="limit">How many items per page. </param>
        /// <returns>Returns the annotations for the students (list).</returns>
        public TeacherStudentAnnotationResponseModel GetTeacherPlanStudentAnnotations(List<TeacherStudentAnnotationModel> teacherAnnotationToFilter, int from, int limit)
        {

            List<TeacherStudentAnnotationModel> teacherStudentAnnotationModelsOGResult = new TeacherStudentAnnotationDAO().Read(_connectionStringRead, teacherAnnotationToFilter);
            teacherStudentAnnotationModelsOGResult.Reverse();
            // If there are no groups, just return this
            if (teacherStudentAnnotationModelsOGResult.Count == 0)
            {
                return new TeacherStudentAnnotationResponseModel()
                {
                    Valid = true,
                    TotalResults = 0,
                    TeacherStudentAnnotationModelList = new List<TeacherStudentAnnotationModel>(),
                };
            }

            TeacherStudentAnnotationResponseModel teacherStudentAnnotationResponseModel = new TeacherStudentAnnotationResponseModel()
            {
                TotalResults = teacherStudentAnnotationModelsOGResult.Count,
            };

            teacherStudentAnnotationModelsOGResult = teacherStudentAnnotationModelsOGResult.Skip((from - 1) * limit).Take(limit).ToList();
            teacherStudentAnnotationResponseModel.TeacherStudentAnnotationModelList = teacherStudentAnnotationModelsOGResult;

            return teacherStudentAnnotationResponseModel;
        }
    }
}
