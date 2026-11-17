using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.Domain.Models.Codes
{
    public class ConstructedCodeModel : IResponse
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string MainCode { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}