using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// Handles mini game lock request details.
    /// </summary>
    public class MiniGameLockRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether verifies if the mini games are locked.
        /// </summary>
        public bool MiniGamesLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is came from parents app.
        /// </summary>
        public bool ParentsApp { get; set; }
    }
}