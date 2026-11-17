namespace TeachersAPI.Domain.Models.Teachers
{
    public class TeacherPlanStudentModel
    {
        public string Id { get; set; }
        public string TeacherPlanId { get; set; }
        public string StudentId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}