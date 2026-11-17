using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// To handle how many rewards coins or sapphires the user will use (and will be removed).
    /// </summary>
    public class RewardsCodeUseRequestModel
    {
        /// <summary>
        /// Gets or sets the coins quantity.
        /// </summary>
        public int CoinsQuantity { get; set; }

        /// <summary>
        /// Gets or sets the diamonds quantity.
        /// </summary>
        public int DiamondsQuantity { get; set; }
    }
}