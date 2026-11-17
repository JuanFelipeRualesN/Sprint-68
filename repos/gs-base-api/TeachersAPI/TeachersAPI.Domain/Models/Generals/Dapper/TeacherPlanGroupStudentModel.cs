using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Dapper
{
    public class TeacherPlanGroupStudentModel : TeacherPlanModel
    {
        public string TeacherPlanGroupId { get; set; }

        public string GroupId { get; set; }

        public string GroupName { get; set; }

        public string TeacherGroupStudentId { get; set; }

        public string StudentId { get; set; }

        public string StudentFirstName { get; set; }

        public string StudentLastName { get; set; }

        public string UserSchoolRecord { get; set; }

        public string TeacherName { get; set; }

    }
}