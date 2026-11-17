using System;
using System.Collections.Generic;
using TeachersAPI.Model;

namespace TeachersAPI.Domain.Models.Codes
{
    public class CodeLevelModel : IResponse
    {
        public bool Valid { get; set; }
        public List<string> Messages { get; set; }
        public string Id { get; set; }
        public string CodeId { get; set; }
        public string LevelCode { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}