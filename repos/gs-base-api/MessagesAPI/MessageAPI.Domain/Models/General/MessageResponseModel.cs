using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;

namespace MessagesAPI.MessageAPI.Domain.Models.General
{
    /// <summary>
    /// Message Response email details
    /// </summary>
    public class MessageResponseModel : ResponseModel
    {
        /// <summary>
        /// Gets or sets the email full detail.
        /// </summary>
        public EmailMessageFullDetailModel EmailMessageFullDetailModel { get; set; }

        /// <summary>
        /// Gets or sets email message model list.
        /// </summary>
        public List<EmailMessageModel> EmailMessageModelList { get; set; }

        /// <summary>
        /// Gets or sets the total pages of the result.
        /// </summary>
        public int TotalPages { get; set; }
    }
}