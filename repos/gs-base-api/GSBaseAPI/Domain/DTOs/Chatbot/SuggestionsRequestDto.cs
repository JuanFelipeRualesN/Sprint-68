namespace GSBaseAPI.Domain.DTOs.Chatbot
{
    public class SuggestionsRequestDto
    {
        /// <summary>
        /// The field name to filter in the plannings
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// The user input being used for the suggestion
        /// </summary>
        public string UserInput { get; set; }

        /// <summary>
        /// The planning context to improve the suggestions
        /// </summary>
        public PlanningContextDto PlanningContext { get; set; }
    }

}
