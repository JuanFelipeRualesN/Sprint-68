namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests.UnityAPP
{
    /// <summary>
    /// To handle if mini game is unlocked or not.
    /// </summary>
    public class MiniGameUnlockRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the mini game code.
        /// </summary>
        public string MiniGameCode { get; set; }
    }
}