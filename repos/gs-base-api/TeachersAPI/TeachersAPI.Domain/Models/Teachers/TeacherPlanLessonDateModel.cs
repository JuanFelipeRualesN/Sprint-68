using System.Collections.Generic;
using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class TeacherPlanLessonDateModel : IResponse
    {
        public TeacherPlanLessonDateModel()
        {
            Messages = new List<string>();
        }

        public List<string> Messages { get; set; }
        public bool Valid { get; set; }
        public int Id { get; set; }
        public int TeacherPlanLessonId { get; set; }
        public int TeacherPlanCustomLessonId { get; set; }
        public string ImplementationDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}