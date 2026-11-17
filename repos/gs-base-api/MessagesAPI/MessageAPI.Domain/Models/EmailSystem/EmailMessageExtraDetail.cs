using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesAPI.MessageAPI.Domain.Models.EmailSystem
{
    public class EmailMessageExtraDetail : EmailMessageModel
    {
        /// <summary>
        /// Gets or sets whether the message was read or not.
        /// </summary>
        public bool Read { get; set; }
    }
}
