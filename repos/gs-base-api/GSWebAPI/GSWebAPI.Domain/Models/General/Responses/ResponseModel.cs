using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    public class ResponseModel : IResponse
    {
        public List<string> Messages { get; set; }
        public bool Valid { get; set; }
    }
}