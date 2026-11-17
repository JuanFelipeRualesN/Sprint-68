namespace MessagesAPI.MessageAPI.Domain.Models.General
{
    /// <summary>
    /// To handle messages requests.
    /// </summary>
    public class MessageRequestModel
    {
        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the email id.
        /// </summary>
        public EmailSystem.EmailMessageFullDetailModel EmailMessageFullDetailModel { get; set; }
    }
}