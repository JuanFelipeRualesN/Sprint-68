using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// Handles the game quiz response details.
    /// </summary>
    public class GameQuizResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameQuizResponseModel"/> class.
        /// </summary>
        public GameQuizResponseModel()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Gets or sets the user quiz result.
        /// </summary>
        public UserQuizResultModel UserQuizResultModel { get; set; }

        /// <summary>
        /// Gets or sets the user reward details.
        /// </summary>
        public UserRewardModel UserRewardModel { get; set; }

        /// <summary>
        /// Gets or sets the messages' list.
        /// </summary>
        public List<string> Messages { get; set; }
    }
}