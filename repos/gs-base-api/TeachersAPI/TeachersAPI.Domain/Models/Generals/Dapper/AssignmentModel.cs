using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class AssignmentModel
    {
        public string Id { get; set; }

        public string PlanId { get; set; }

        public int TriesQuantity { get; set; }

        public bool Available { get; set; }

        public string DueDate { get; set; }

        public bool ForGrading { get; set; }

        public string Title { get; set; }

        public string PrePostRequired { get; set; }
    }
}