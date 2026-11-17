using System;
using System.Collections.Generic;
using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Codes
{
    public class CodeSubjectModel : IResponse
    {
        public bool Valid { get; set; }
        public List<string> Messages { get; set; }
        public string Id { get; set; }
        public string CodeId { get; set; }
        public string SubjectCode { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}