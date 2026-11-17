using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagesAPI.MessageAPI.Domain.Models.General
{
    public class ResponseModel
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets whether the session is valid or not.
        /// </summary>
        public bool SessionValid { get; set; }

        /// <summary>
        /// Gets or sets whether the transaction is valid or not.
        /// </summary>
        public bool TransactionValid { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        public List<string> Messages { get; set; }
    }
}