using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Models
{
    public class ForumModel : Teachers.ForumModel
    {
        public string SubjectCode { get; set; }
        public string LevelCode { get; set; }

    }
}