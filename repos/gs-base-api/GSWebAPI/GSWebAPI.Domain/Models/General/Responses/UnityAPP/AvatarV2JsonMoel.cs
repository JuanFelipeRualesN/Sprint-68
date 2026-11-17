using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP
{
    /// <summary>
    /// To handle the response for the user avatar. 
    /// </summary>
    public class AvatarV2JsonModel
    {
        // // {"Gender":"M","Glasses":false,"Skin":0,"Parts":[30,27,22,11,8,5,2]}
        /// <summary>
        /// Gets or sets the Avatar Gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the glasses.
        /// </summary>
        public bool Glasses { get; set; }

        /// <summary>
        /// Gets or sets the skin.
        /// </summary>
        public int Skin { get; set; }

        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        public List<int> Parts { get; set; }
    }
}