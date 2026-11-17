namespace MessagesAPI.MessageAPI.Domain.Models.EmailSystem
{
    public class EmailMessageModel
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string DateCreated { get; set; }
        public string ParentMessageId { get; set; }
        public string AuthorId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        // Extensions 
        /// <summary>
        /// Gets or sets wheter a message was read or not.
        /// </summary>
        public bool Read { get; set; }

        /// <summary>
        /// Gets or sets the user full name.
        /// </summary>
        public string UserFullName { get; set; }

    }
}