using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace TeachersAPI.Domain.Models.LearningGap
{
    public class LearningGapStudentHasNotificationsModel
    {
        public string StudentId { get; set; }
        public bool HasNotifications { get; set; }
    }
}