namespace TeachersAPI.Domain.Models.Lessons
{
    public class TemplateInstructionModel
    {
        public string Id { get; set; }
        public string TemplateId { get; set; }
        public string InstructionDesc { get; set; }
        public string LangCode { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}