using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagesAPI.MessageAPI.Domain.Models.General.APIs
{
    /// <summary>
    /// To handle the user email request details.
    /// </summary>
    public class UserEmailModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Validated { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}