using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSWebAPI.GSWebAPI.Domain.Models.Avatar;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP
{
    /// <summary>
    /// Handles the User Avatar Requests.
    /// </summary>
    public class AvatarUserRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the user avatar.
        /// </summary>
        public UserAvatarModel UserAvatarModel { get; set; }
    }
}