using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// To handle the competition's results.
    /// </summary>
    public class CompetitionResultsResponseModel
    {
        /// <summary>
        /// Gets or sets the competition results.
        /// </summary>
        public CompetitionResultsModel CompetitionResultsModel { get; set; }

        /// <summary>
        /// Gets or sets the use reward.
        /// </summary>
        public UserRewardModel UserRewardModel { get; set; }
    }
}