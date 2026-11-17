using TeachersAPI.Domain.Models.Lessons.Request;

namespace TeachersAPI.Domain.Models.Lessons.Response
{
    /// <summary>
    /// To handle Athenas JSON requests.
    /// </summary>
    public class AthenasJSONQuizResponseModel
    {
        /// <summary>
        /// Gets or sets the quiz request result details.
        /// </summary>
        public QuizRequestModel QuizRequestModel { get; set; }
    }
}