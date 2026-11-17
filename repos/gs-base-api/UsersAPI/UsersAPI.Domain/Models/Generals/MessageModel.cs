using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals
{
    /// <summary>
    /// Handles the message model.
    /// </summary>
    public class MessageModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string LangCode { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}