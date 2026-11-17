namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapRequiredSubjectModel
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string SubjectCode { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public string SubjectName { get; set; }

    }
}