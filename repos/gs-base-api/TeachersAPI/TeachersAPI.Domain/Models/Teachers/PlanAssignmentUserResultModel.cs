using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class PlanAssignmentUserResultModel : IResponse
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string PlanQuizId { get; set; }
        public string UserId { get; set; }
        public string ManuallyCorrectedByTeacher { get; set; }
        public string Score { get; set; }
        public string TotalPercent { get; set; }
        public string UsquiUserResultId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}