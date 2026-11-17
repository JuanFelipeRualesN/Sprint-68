using System.Collections.Generic;
using GSWebAPI.GSWebAPI.Domain.Models.Avatar;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;
using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;
using GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP;
using GSWebAPI.GSWebAPI.Domain.Models.MiniGame;
using GSWebAPI.GSWebAPI.Domain.Models.UserQuiz;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// To handle user game app response. The original resource of the Student APP (Carlos Orama), asked to everything be sent in the same response.
    /// </summary>
    public class UserGameAppResponseModel : GeneralResponseModel
    {
        /// <summary>
        /// Gets or sets the avatar items.
        /// </summary>
        public List<AvatarItemResponseModel> AvatarItemModelList { get; set; }

        /// <summary>
        /// Gets or sets the user avatar.
        /// </summary>
        public UserAvatarModel UserAvatarModel { get; set; }

        /// <summary>
        /// Gets or sets the user unlocked item list.
        /// </summary>
        public List<UserUnlockedItemModel> UserUnlockedItemList { get; set; }

        /// <summary>
        /// Gets or sets the use reward.
        /// </summary>
        public UserRewardModel UserRewardModel { get; set; }

        /// <summary>
        /// Gets or sets the game rewards code.
        /// </summary>
        public List<string> GameRewardsCodeGenerated { get; set; }

        /// <summary>
        /// Gets or sets game response model list.
        /// </summary>
        public List<GameResponseModel> GameResponseModelList { get; set; }

        /// <summary>
        /// Gets or sets the high score response model list.
        /// </summary>
        public List<HighScoreResponseModel> HighScoreResponseModelList { get; set; }

        /// <summary>
        /// Gets or sets the user bonus log.
        /// </summary>
        public UserBonusLogModel UserBonusLogModel { get; set; }

        /// <summary>
        /// Gets or sets the user response model list.
        /// </summary>
        public List<GameQuizCompetition.UserResponseModel> UserResponseModelList { get; set; }

        /// <summary>
        /// Gets or sets the user rival.
        /// </summary>
        public UserRivalModel UserRivalModel { get; set; }

        /// <summary>
        /// Gets or sets the competition results.
        /// </summary>
        public CompetitionResultsModel CompetitionResultsModel { get; set; }

        /// <summary>
        /// Gets or sets the user quiz result model.
        /// </summary>
        public UserQuizResultModel UserQuizResultModel { get; set; }

        /// <summary>
        /// Gets or sets the student average.
        /// </summary>
        public double StudentAverage { get; set; }

        /// <summary>
        /// Gets or sets the lesson modifier requests.
        /// </summary>
        public LessonModifierRequestModel LessonModifierRequestModel { get; set; }

        /// <summary>
        /// Gets or sets the user subscribed levels subjects.
        /// </summary>
        public List<SubsSubjectLevelCodesResponseModel> UserSubscribedLevelsSubjectsModelList { get; set; }

        /// <summary>
        /// Gets or sets the quiz Id.
        /// </summary>
        public string QuizId { get; set; }

        /// <summary>
        /// Gets or sets invitatin code.
        /// </summary>
        public string InvitationCode { get; set; }
    }
}