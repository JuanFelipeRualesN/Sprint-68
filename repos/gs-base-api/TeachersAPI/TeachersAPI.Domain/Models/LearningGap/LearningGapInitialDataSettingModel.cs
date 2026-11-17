namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapInitialDataSettingModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string TeacherId { get; set; }
        public string StudentId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}