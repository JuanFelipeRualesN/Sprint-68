using Newtonsoft.Json;
using ReadingAPI.Domain.DTOs;

namespace ReadingAPI.ReadingAPI.Domain.DTOs.Metrics
{
    /// <summary>
    /// Module-usage response. 
    /// Only the section relevant to the user's role is populated
    /// </summary>
    public class ModuleUsageResponseDto
    {
        /// <summary>
        /// Populated for admin (all regions) and admin-ore (admin-ore region only)
        /// </summary>
        public List<RegionUsageDto> ByRegion { get; set; } = new();

        /// <summary>
        /// Populated for teachers, their connected students with a used/not-used flag.
        /// </summary>
        public List<StudentUsageDto> Students { get; set; } = new();

        /// <summary>
        /// Populated for directors, their school's teachers, each with their groups and usage.
        /// </summary>
        public List<DirectorTeacherUsageDto>? Teachers { get; set; }
    }

    /// <summary>
    /// A director's teacher together with the usage of each group they created
    /// </summary>
    public class DirectorTeacherUsageDto
    {
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public List<GroupUsageDto> Groups { get; set; } = new();
    }

    /// <summary>
    /// Usage percentage for a single group
    /// </summary>
    public class GroupUsageDto
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int UsedStudents { get; set; }
        public double UsagePercent { get; set; }
    }

    /// <summary>
    /// Usage percentage for a region and, when drilling in, the breakdown of its schools
    /// </summary>
    public class RegionUsageDto
    {
        public int RegionId { get; set; }
        public string RegionName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int UsedStudents { get; set; }
        public double UsagePercent { get; set; }

        /// <summary>
        /// Omitted in the region-summary view (no region/school filter). 
        /// Present when drilling into a specific region.
        /// </summary>
        public PaginationModel<SchoolUsageDto>? Schools { get; set; }
    }

    /// <summary>
    /// Usage percentage for a single school
    /// </summary>
    public class SchoolUsageDto
    {
        public int SchoolId { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public int TotalStudents { get; set; }
        public int UsedStudents { get; set; }
        public double UsagePercent { get; set; }
    }

    /// <summary>
    /// A teacher's connected student and whether they have used the module
    /// </summary>
    public class StudentUsageDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool HasUsed { get; set; }
    }
}
