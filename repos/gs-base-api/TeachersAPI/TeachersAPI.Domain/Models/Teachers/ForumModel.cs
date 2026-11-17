using TeachersAPI.Model;

using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class ForumModel
    {
        public ForumModel()
        {
            ImplementationDates = new List<ForumDatesModel>();
        }

        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
		
        public string Id { get; set; }
        public string TeacherPlanId { get; set; }
        public string Title { get; set; }
        public string Active { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string PlanName { get; set; }
        public List<ForumDatesModel> ImplementationDates { get; set; }
    }
}