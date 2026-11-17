using GSWebAPI.GSWebAPI.Domain.Models.Avatar;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;
using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// To handle the user rival details for quiz competitions.
    /// </summary>
    public class UserRivalModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRivalModel"/> class.
        /// </summary>
        public UserRivalModel()
        {
            UserResponseModelList = new List<UserResponseModel>();
            UserAvatarModel = new UserAvatarModel();
        }

        /// <summary>
        /// Gets or sets the rival username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the rival City.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the rival country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the rival school.
        /// </summary>
        public string School { get; set; }

        /// <summary>
        /// Gets or sets user rival avatar.
        /// </summary>
        public UserAvatarModel UserAvatarModel { get; set; }

        /// <summary>
        /// Gets or sets user rival (3d) version.
        /// </summary>
        public AvatarV2JsonModel UserAvatar3dModel { get; set; }


        /// <summary>
        /// Gets or Sets the user rival responses.
        /// </summary>
        public List<UserResponseModel> UserResponseModelList { get; set; }
    }
}