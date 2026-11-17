using System;
using System.Collections.Generic;
using System.Configuration;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.TeachersAPI.Infrastructure.Repositories.Teachers;
using TeachersAPI.TeachersAPI.Services.Generals;
using TeachersAPI;

namespace TeachersAPI.TeachersAPI.Services.Teachers
{
    /// <summary>
    /// To handle the annotations creation
    /// </summary>
    public class TeacherAnnotationModifierManager
    {
        private readonly string _connectionString, _connectionStringRead, _langCode;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="connectionString">The connection string (writer).</param>
        /// <param name="connectionStringRead">The connection string (reader).</param>
        /// <param name="langCode">The language code of the user.</param>
        public TeacherAnnotationModifierManager(string connectionString, string connectionStringRead, string langCode)
        {
            _connectionString = connectionString;
            _connectionStringRead = connectionStringRead;
            _langCode = langCode;
        }

        /// <summary>
        /// To create an annotation of the student for the plan.
        /// </summary>
        /// <param name="teacherStudentAnnotationModel">Teacher student model.</param>
        /// <param name="userSessionId">The user id of the user that is making the request.</param>
        /// <returns>Returns a respons emodel</returns>
        public ResponseModel MakeAnnotation(TeacherStudentAnnotationModel teacherStudentAnnotationModel, string userSessionId)
        {
            try
            {
                MessageManager messageManager = new MessageManager(_connectionStringRead,
                  _langCode);

                if (string.IsNullOrWhiteSpace(teacherStudentAnnotationModel.StudentId)
                    || string.IsNullOrWhiteSpace(teacherStudentAnnotationModel.TeacherId)
                    || string.IsNullOrWhiteSpace(teacherStudentAnnotationModel.Annotation))
                {
                    return new ResponseModel()
                    {
                        Valid = false,
                        Messages = new List<string>(),
                    };
                }

                teacherStudentAnnotationModel.ModifiedBy = userSessionId;
                new TeacherStudentAnnotationDAO().Create(_connectionString, teacherStudentAnnotationModel);

                return new ResponseModel()
                {
                    Valid = true,
                    Messages = new List<string>(){
                    messageManager.GetMessageDetails(AppConfiguration.Configuration["AppSettings:tm_message_code_success"]) }
                };
            }
            catch (Exception)
            {
                return new ResponseModel()
                {
                    Valid = false,
                    Messages = new List<string>()
                };
            }
        }
    }
}
