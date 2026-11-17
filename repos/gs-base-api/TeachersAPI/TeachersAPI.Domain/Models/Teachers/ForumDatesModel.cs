using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Teachers
{
    public class ForumDatesModel
    {
        public string Id { get; set; }
        public string ForumId { get; set; }
        public string ImplementationDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}