namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP
{
    /// <summary>
    /// Handles the avatar buy part requests.
    /// </summary>
    public class AvatarBuyPartRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the part.
        /// </summary>
        public string Part { get; set; }

        /// <summary>
        /// Gets or sets the Id of the item according to that part.
        /// </summary>
        public string ItemId { get; set; }
    }
}