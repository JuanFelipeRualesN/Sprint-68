using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.APIs
{
    public class SubjectModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LangCode { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}