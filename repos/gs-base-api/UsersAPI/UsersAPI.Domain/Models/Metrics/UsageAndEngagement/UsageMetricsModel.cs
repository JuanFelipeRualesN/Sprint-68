using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.UsersAPI.Domain.Models.Metrics.UsageAndEngagement
{
    public class UsageMetricsModel
    {
        public int TotalSessions { get; set; }
        public int ActiveDays { get; set; }
        public double AverageTimeMinutes { get; set; }

        public List<FrequencyUsageData> FrequencyUsage { get; set; } = new List<FrequencyUsageData>();

        public LocationUsageData OffVsOnSchoolUsage { get; set; } = new LocationUsageData();

        public DeviceUsageData MostUsedDevices { get; set; } = new DeviceUsageData();
    }

    public class FrequencyUsageData
    {
        public string Day { get; set; }
        public int SessionCount { get; set; }
        public double AverageTimeMinutes { get; set; }
    }

    public class LocationUsageData
    {
        public double OffSchoolPercentage { get; set; }
        public double OnSchoolPercentage { get; set; }
    }

    public class DeviceUsageData
    {
        public double TabletPercentage { get; set; }
        public double DesktopPercentage { get; set; }
        public double MobilePercentage { get; set; }
    }
}
