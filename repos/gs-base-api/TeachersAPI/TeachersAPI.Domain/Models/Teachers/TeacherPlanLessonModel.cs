using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class TeacherPlanLessonModel : IResponse
    {
        public TeacherPlanLessonModel()
        {
            Messages = new System.Collections.Generic.List<string>();
        }
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string PlanId { get; set; }
        public string LessonId { get; set; }
        public string TriesQuantity { get; set; }
        public string ForGrading { get; set; }
        public string DueDate { get; set; }
        public string Available { get; set; }
        public string PrePostRequired { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string LessonTitle { get; set; }

    }
}