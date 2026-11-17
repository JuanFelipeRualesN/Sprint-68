using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP
{
    /// <summary>
    /// Handles the game rewards code. DELETE AFTER UPGRADE.
    /// </summary>
    public class GameRewardsCodeGenerationRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the quantity of the rewards.
        /// </summary>
        public int QuantityRewardCodes { get; set; }
    }
}