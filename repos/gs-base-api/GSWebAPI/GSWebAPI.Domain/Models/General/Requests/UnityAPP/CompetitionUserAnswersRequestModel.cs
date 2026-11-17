using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;
using System.Collections.Generic;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP
{
    /// <summary>
    /// Handles the competition answers.
    /// </summary>
    public class CompetitionUserAnswersRequestModel
    {
        /// <summary>
        /// Gets or sets the user responses.
        /// </summary>
        public List<UserResponseModel> UserResponseModelList { get; set; }

        /// <summary>
        /// Gets or sets the rival responses.
        /// </summary>
        public List<UserResponseModel> RivalResponseModelList { get; set; }

        /// <summary>
        /// Gets or sets the Token. Delete when version upgrade.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the LangCode. Delete when version upgrade.
        /// </summary>
        public string LangCode { get; set; }
    }
}