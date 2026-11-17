using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP
{
    /// <summary>
    /// Handle the competition results details.
    /// </summary>
    public class CompetitionResultsModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether the user won or not.
        /// </summary>
        public bool UserWon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the rival won or not.
        /// </summary>
        public bool RivalWon { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the match was a draw.
        /// </summary>
        public bool Draw { get; set; }

        /// <summary>
        /// Gets or sets the user total points.
        /// </summary>
        public int UserPoints { get; set; }

        /// <summary>
        /// Gets or sets the rival points.
        /// </summary>
        public int RivalPoints { get; set; }

        /// <summary>
        /// Gets or sets the reward type.
        /// </summary>
        public string RewardType { get; set; }

        /// <summary>
        /// Gets or sets the reward quantity.
        /// </summary>
        public int RewardQuantity { get; set; }
    }
}