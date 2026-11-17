using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesAPI.MessageAPI.Domain.Models.EmailSystem
{
    public class ResponseModel
    {
        /// <summary>
        /// Gets or sets whether if transaction was valid or not.
        /// </summary>
        public bool Valid { get; set; }

        /// <summary>
        /// Gets or sets the messages details.
        /// </summary>
        public List<string> Messages { get; set; }
        /// <summary>
        /// Gets or sets the messages codes details.
        /// </summary>
        public List<string> MessageCodes { get; set; }

        /// <summary>
        /// Gets or sets the email model list.
        /// </summary>
        public List<EmailMessageExtraDetail> EmailMessageModelList { get; set; }

        /// <summary>
        /// Gets or sets the result total
        /// </summary>
        public int Total { get; set; }
    }
}
