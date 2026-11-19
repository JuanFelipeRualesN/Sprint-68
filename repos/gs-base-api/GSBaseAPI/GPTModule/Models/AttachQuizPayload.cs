namespace GSBaseAPI.GPTModule.Models
{
    public sealed class AttachQuizPayload
    {
        public int Id { get; set; } = 1;
        public string PlanningId { get; set; } = "";
        public string TriesQuantity { get; set; } = "1";
        public int ForGrading { get; set; } = 1;
        public string LangCode { get; set; } = "es";
        public string DueDate { get; set; } = "2026-03-04 00:00:00";
        public string QuizId { get; set; } = "";
    }

}
