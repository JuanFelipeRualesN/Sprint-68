using System.Collections.Generic;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools
{
    public class ResponseModel
    {
        public bool Valid { get; set; }
        public List<string> Messages = new List<string>();
        public List<string> MessageCodes = new List<string>();
    }
}
