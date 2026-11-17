using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.APIs
{
    /// <summary>
    /// To handle learning gap quiz submit request model.
    /// </summary>
    public class LearningGapQuizSubmitRequestModel
    {
        /// <summary>
        /// Get or sets the student id.
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// Gets or sets the lesson id.
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Gets or sets the quiz value.
        /// </summary>
        public string QuizValue { get; set; }

        /// <summary>
        /// Gets or sets the quiz id.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets the usqui quiz result id.
        /// </summary>
        public string UsquiQuizResultId { get; set; }

        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public string Score { get; set; }

        /// <summary>
        /// Gets or sets the learning sheet ids.
        /// </summary>
        public List<string> LearningSheetIds { get; set; }
    }
}