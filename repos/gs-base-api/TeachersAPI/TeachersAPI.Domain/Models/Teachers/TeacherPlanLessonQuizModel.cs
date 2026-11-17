using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class TeacherPlanLessonQuizModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string PlanLessonId { get; set; }
        public string QuizId { get; set; }
        public string Required { get; set; }
        public string ForGrading { get; set; }
        public string DueDate { get; set; }
        public string DueDateTimezone { get; set; }
        public string Duration { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}