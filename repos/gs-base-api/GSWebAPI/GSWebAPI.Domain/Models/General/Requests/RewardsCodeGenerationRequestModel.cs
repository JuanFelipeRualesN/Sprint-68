using GSWebAPI.GSWebAPI.Domain.Models.GameUserReward;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// Handles the game rewards code.
    /// </summary>
    public class RewardsCodeGenerationRequestModel
    {
        /// <summary>
        /// Gets or sets the quantity of the rewards.
        /// </summary>
        public int QuantityRewardCodes { get; set; }

        /// <summary>
        /// Gets or sets the reward code.
        /// </summary>
        public RewardCodeModel RewardCodeModel { get; set; }
    }
}