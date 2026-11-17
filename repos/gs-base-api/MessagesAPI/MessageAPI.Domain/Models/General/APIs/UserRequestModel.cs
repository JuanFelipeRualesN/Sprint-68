using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagesAPI.MessageAPI.Domain.Models.General.APIs
{
    public class UserRequestModel
    {
        public string RoleId { get; set; }
        public int PageFrom { get; set; }
        public int Limiter { get; set; }
    }
}