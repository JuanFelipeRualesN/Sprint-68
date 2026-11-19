namespace GSBaseAPI.Domain.DTOs.Chatbot
{
    public class PlanningContextDto
    {
        /// <summary>
        /// The subject to filter the suggestion
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The grade code to filter the suggestion
        /// </summary>
        public string GradeCode { get; set; }
    }
}
