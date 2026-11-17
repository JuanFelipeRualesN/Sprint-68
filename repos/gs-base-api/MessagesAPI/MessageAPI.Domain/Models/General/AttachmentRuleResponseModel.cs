using MessagesAPI.MessageAPI.Domain.Models.EmailSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MessagesAPI.MessageAPI.Domain.Models.General
{
    public class AttachmentRuleResponseModel : ResponseModel
    {
        public List<AttachmentRuleModel> AttachmentRuleModelList { get; set; }
    }
}