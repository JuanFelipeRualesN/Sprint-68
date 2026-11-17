using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagesAPI.MessageAPI.Domain.Models.General
{
    public class EmailMessageFullDetailModel
    {
        /// <summary>
        /// Gets or sets the email message model (with the user full name, that's why we did not use directly the model)
        /// </summary>
        public EmailMessageModel EmailMessageModel { get; set; }

        /// <summary>
        /// Get or sets the list of recipients (with the recipients name, that's why we did not use directly the model of the library.
        /// </summary>
        public List<EmailMessageRecipientModel> EmailRecipientModelList { get; set; }

        /// <summary>
        /// Gets or sets the attachment list.
        /// </summary>
        public List<EmailAttachmentModel> EmailAttachmentModelList { get; set; }
    }
}