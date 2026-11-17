using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP
{
    /// <summary>
    /// To handle the avatar parts and response. Example: 
    /// </summary>
    public class AvatarV2ItemsModel
    {
        /// <summary>
        /// To prepare the object.
        /// </summary>
        public AvatarV2ItemsModel()
        {
            Parts = new AvatarV2Parts
            {
                Chests = new List<AvatarV2PartDetails>(),
                Hairs = new List<AvatarV2PartDetails>(),
                Legs = new List<AvatarV2PartDetails>(),
                Feet = new List<AvatarV2PartDetails>()
            };
        }
        /// <summary>
        /// Gets or sets the user avatar's gender.
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the parts.
        /// </summary>
        public AvatarV2Parts Parts { get; set; }
    }

    /// <summary>
    /// To handle the part, cost, and if unlocked.
    /// </summary>
    public class AvatarV2PartDetails
    {
        public int PartIndex { get; set; }

        public int Cost { get; set; }

        public bool Unlocked { get; set; }
    }

    /// <summary>
    /// To handle the avatar parts
    /// </summary>
    public class AvatarV2Parts
    {
        public List<AvatarV2PartDetails> Hairs { get; set; }

        public List<AvatarV2PartDetails> Chests { get; set; }

        public List<AvatarV2PartDetails> Legs { get; set; }

        public List<AvatarV2PartDetails> Feet { get; set; }

    }
}