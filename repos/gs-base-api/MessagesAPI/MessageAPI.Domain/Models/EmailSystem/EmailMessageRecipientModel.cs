namespace MessagesAPI.MessageAPI.Domain.Models.EmailSystem
{
    public class EmailMessageRecipientModel
    {
        public string Id { get; set; }
        public string RecipientId { get; set; }
        public string MessageId { get; set; }
        public string IsRead { get; set; }
        public string DateRead { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        // Extension
        /// <summary>
        /// Gets or sets the recipient's name.
        /// </summary>
        public string RecipientName { get; set; }

    }
}