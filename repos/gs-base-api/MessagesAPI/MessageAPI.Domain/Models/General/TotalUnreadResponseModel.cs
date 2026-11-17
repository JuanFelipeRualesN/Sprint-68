using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagesAPI.MessageAPI.Domain.Models.General
{
    public class TotalUnreadResponseModel : ResponseModel
    {
        /// <summary>
        /// Gets or sets the total unread messages.
        /// </summary>
        public int TotalUnread { get; set; }
    }
}