using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Courses
{
    public class LevelModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}