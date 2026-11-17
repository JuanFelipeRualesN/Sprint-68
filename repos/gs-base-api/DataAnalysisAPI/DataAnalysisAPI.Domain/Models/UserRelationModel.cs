using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataAnalysisAPI.DataAnalysisAPI.Domain.Models
{
    public class UserRelationModel
    {
        public int Id { get; set; }
        public int? TeacherId { get; set; }
        public int? StudentId { get; set; }

    }
}