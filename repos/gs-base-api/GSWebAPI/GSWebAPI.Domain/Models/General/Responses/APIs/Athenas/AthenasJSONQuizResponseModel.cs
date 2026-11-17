using GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Athenas
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