using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesAPI.MessageAPI.Domain.Models.EmailSystem
{
    /// <summary>
    /// To return the email details.
    /// </summary>
    public class EmailMessageFullDetailModel
    {
        /// <summary>
        /// Gets or sets email generics.
        /// </summary>
        public EmailMessageModel EmailMessageModel { get; set; }

        /// <summary>
        /// Gets or sets email attachments.
        /// </summary>
        public List<EmailAttachmentModel> EmailAttachmentModelList { get; set; }

        /// <summary>
        /// Gets or sets email recipients.
        /// </summary>
        public List<EmailMessageRecipientModel> EmailMessageRecipientModelList { get; set; }

        /// <summary>
        /// Gets or sets the email message parent (if anything)
        /// </summary>
        public EmailMessageModel EmailMessageParentModel { get; set; }
    }
}
