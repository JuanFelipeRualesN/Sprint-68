using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.UsersAPI.Domain.Models.Metrics.UsageAndEngagement
{
    public class WeeklySesionsModel
    {
        public int Week { get; set; }
        public int TotalSessions { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
