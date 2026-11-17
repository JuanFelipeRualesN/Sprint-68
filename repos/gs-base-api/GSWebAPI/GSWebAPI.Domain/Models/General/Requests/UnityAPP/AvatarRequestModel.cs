using GSWebAPI.GSWebAPI.Domain.Models.Avatar;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP
{
    /// <summary>
    /// Handles the Avatar Request.
    /// </summary>
    public class AvatarRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the avatar items.
        /// </summary>
        public AvatarItemModel AvatarItemModel { get; set; }
    }
}