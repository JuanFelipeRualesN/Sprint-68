using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.UnityAPP
{
    public class MobileResponseModel
    {
        public bool LockedGame { get; set; }

        public bool SessionValid { get; set; }

        public bool TransactionValid { get; set; }
    }
}