using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// To handle competition bonus response.
    /// </summary>
    public class CompetitionBonusResponseModel
    {
        /// <summary>
        /// Gets or sets the high score response model list.
        /// </summary>
        public List<HighScoreResponseModel> HighScoreResponseModelList { get; set; }

        /// <summary>
        /// Gets or sets the user bonus log.
        /// </summary>
        public UserBonusLogModel UserBonusLogModel { get; set; }
    }
}