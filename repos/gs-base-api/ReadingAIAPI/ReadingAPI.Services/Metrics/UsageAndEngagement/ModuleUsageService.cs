using Dapper;
using Microsoft.CognitiveServices.Speech.Transcription;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using ReadingAPI.Domain.DTOs;
using ReadingAPI.ReadingAPI.Domain.DTOs.Metrics;
using ReadingAPI.ReadingAPI.Domain.Entities.UsageAndEngagement;
using ReadingAPI.ReadingAPI.Infrastructure;

namespace ReadingAPI.ReadingAPI.Services.Metrics.UsageAndEngagement
{
    /// <summary>
    /// Reports who has used the reading module, scoped to the requesting user's role
    /// </summary>
    public class ModuleUsageService : MetricsBaseService<UserSessionEntity>
    {
        private readonly ILogger<ModuleUsageService> _logger;

        public ModuleUsageService(ReadingAIDbContext dbContext, ILogger<ModuleUsageService> logger)
            : base(dbContext)
        {
            _logger = logger;
        }

        private static readonly string UsersConnectionString = AppConfiguration.Configuration["ConnectionStrings:GenialSkillsUsersAPIDBConnection"];

        private static readonly string SchoolsConnectionString = AppConfiguration.Configuration["ConnectionStrings:SchoolDBConnectionReader"];

        private static readonly string TeacherWebConnectionString = AppConfiguration.Configuration["ConnectionStrings:TeacherWebAPIConnectionStringReader"];

        private static readonly int StudentRoleId = int.Parse(AppConfiguration.Configuration["AppSettings:ur_role_client_child"]);

        private static readonly int TeacherRoleId = int.Parse(AppConfiguration.Configuration["AppSettings:ur_role_teacher"]);

        protected override IQueryable<UserSessionEntity> GetBaseQuery()  => _dbContext.UserSessions.AsQueryable();

        protected override IQueryable<UserSessionEntity> ApplyFilters(IQueryable<UserSessionEntity> query, UserSessionMetricsRequestDto request, int? regionId, int? schoolId, List<int>? studentIds, bool applyDateFilters)
        {
            if (regionId.HasValue)
            {
                query = query.Where(s => s.RegionId == regionId.Value);
            }

            if (schoolId.HasValue)
            {
                query = query.Where(s => s.SchoolId == schoolId.Value);
            }

            if (studentIds != null)
            {
                query = query.Where(s => studentIds.Contains(s.UserId));
            }

            return query;
        }

        /// <summary>
        /// Returns the module-usage view
        /// </summary>
        /// <param name="request">The user request</param>
        /// <param name="userId">The requesting user Id</param>
        /// <param name="isStaff">Staff access</param>
        /// <param name="isDirector">Director flag</param>
        /// <param name="oreId">The admin-ore region Id, when present</param>
        /// <param name="fromPage">The "From" page value</param>
        /// <param name="limit">The page size</param>
        public async Task<ModuleUsageResponseDto> GetModuleUsage(UserSessionMetricsRequestDto request, string userId, bool isStaff, bool isDirector, string? oreId, int fromPage, int limit)
        {
            try
            {
                // Admin-ore and admin get percentage usage view
                if (!string.IsNullOrWhiteSpace(oreId) || isStaff)
                {
                    return await GetUsageByRegion(request, oreId, fromPage, limit);
                }

                // The groups of every teacher in their school, each group with its own usage percentage
                if (isDirector)
                {
                    var scope = await GetQueryScope(request, userId, isStaff: false, isDirector: true, oreId: null);
                    return new ModuleUsageResponseDto { Teachers = await GetDirectorGroupsUsage(scope?.SchoolId) };
                }

                // List of connected students with a used/not-used flag
                return new ModuleUsageResponseDto { Students = await GetTeacherStudents(request, userId) };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error building module usage for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Region-only summary
        /// Admin-ore is locked to its own region, admin can fetch different regions.
        /// </summary>
        private async Task<ModuleUsageResponseDto> GetUsageByRegion(UserSessionMetricsRequestDto request, string? oreId, int fromPage, int limit)
        {
            int? regionFilter = null;
            bool isAdminOre = false;

            if (!string.IsNullOrWhiteSpace(oreId) && int.TryParse(oreId, out var oreRegionId))
            {
                regionFilter = oreRegionId;
                isAdminOre = true;
            }
            else if (request.RegionId.HasValue)
            {
                regionFilter = request.RegionId;
            }

            int? schoolFilter = request.SchoolId;

            // Admin with no filters, retrieves a summary across every region
            if (!isAdminOre && !regionFilter.HasValue && !schoolFilter.HasValue)
            {
                return new ModuleUsageResponseDto { ByRegion = await GetRegionSummaries(null) };
            }

            // Admin-ore with no school filter, retrieves the same summary shape as admin but scoped to their own region.
            if (isAdminOre && !schoolFilter.HasValue)
            {
                return new ModuleUsageResponseDto { ByRegion = await GetRegionSummaries(regionFilter) };
            }

            // Admin drilled into a single school without a region, resolve the school's region.
            if (!regionFilter.HasValue && schoolFilter.HasValue)
            {
                regionFilter = await GetSchoolRegion(schoolFilter.Value);
            }

            return await GetRegionDetail(regionFilter, schoolFilter, fromPage, limit);
        }

        /// <summary>
        /// Region-only summary. 
        /// With no filter, retrieves every region every region, with a regionFilter,
        /// only that region scoped to its schools.
        /// </summary>
        private async Task<List<RegionUsageDto>> GetRegionSummaries(int? regionFilter)
        {
            // Admin-ore passes its region so the summary is scoped to that region's schools.
            // Admin passes null to summarize every region.
            List<int>? scopeSchoolIds = null;

            if (regionFilter.HasValue)
            {
                scopeSchoolIds = (await GetSchoolIdsInRegion(regionFilter.Value)).Distinct().ToList();
            }

            var totalPerSchool = await GetStudentsPerSchool(scopeSchoolIds);
            var usedPerSchool = await GetUsedStudentsPerSchool(regionFilter, null);
            var schoolToRegion = await GetSchoolRegionMap();
            var regionNames = await GetRegionNames();

            var regions = new Dictionary<int, RegionUsageDto>();

            foreach (var schoolId in totalPerSchool.Keys.Union(usedPerSchool.Keys))
            {
                // school without a region mapping, skip
                if (!schoolToRegion.TryGetValue(schoolId, out var regionId))
                    continue; 

                // keep only the scoped region
                if (regionFilter.HasValue && regionId != regionFilter.Value)
                    continue; 

                var total = totalPerSchool.GetValueOrDefault(schoolId);
                var used = Math.Min(usedPerSchool.GetValueOrDefault(schoolId), total);

                if (!regions.TryGetValue(regionId, out var region))
                {
                    region = new RegionUsageDto
                    {
                        RegionId = regionId,
                        RegionName = regionNames.GetValueOrDefault(regionId, string.Empty),
                    };
                    regions[regionId] = region;
                }

                region.TotalStudents += total;
                region.UsedStudents += used;
            }

            foreach (var region in regions.Values)
                region.UsagePercent = CalculatePercent(region.UsedStudents, region.TotalStudents);

            // Schools stays null so the object is omitted from the summary response.
            return regions.Values.OrderBy(r => r.RegionName).ToList();
        }

        /// <summary>
        /// Get one region with its schools
        /// </summary>
        private async Task<ModuleUsageResponseDto> GetRegionDetail(int? regionFilter, int? schoolFilter, int fromPage, int limit)
        {
            // The schools to show, a single school, or every school of the region
            List<int> schoolIds;

            if (schoolFilter.HasValue)
            {
                schoolIds = new List<int> { schoolFilter.Value };

            }
            else if (regionFilter.HasValue)
            {
                schoolIds = (await GetSchoolIdsInRegion(regionFilter.Value)).Distinct().ToList();
            }
            else
            {
                schoolIds = new List<int>();
            }

            if (schoolIds.Count == 0)
            {
                return new ModuleUsageResponseDto();
            }

            var totalPerSchool = await GetStudentsPerSchool(schoolIds);
            var usedPerSchool = await GetUsedStudentsPerSchool(regionFilter, schoolFilter);
            var schoolNames = await GetSchoolNames(schoolIds);
            var regionName = regionFilter.HasValue ? await GetRegionName(regionFilter.Value) : string.Empty;

            var allSchools = schoolIds
                .Select(id =>
                {
                    var total = totalPerSchool.GetValueOrDefault(id);
                    var used = Math.Min(usedPerSchool.GetValueOrDefault(id), total);
                    return new SchoolUsageDto
                    {
                        SchoolId = id,
                        SchoolName = schoolNames.GetValueOrDefault(id, string.Empty),
                        TotalStudents = total,
                        UsedStudents = used,
                        UsagePercent = CalculatePercent(used, total),
                    };
                })
                .OrderBy(s => s.SchoolName)
                .ToList();

            var regionTotal = allSchools.Sum(s => s.TotalStudents);
            var regionUsed = allSchools.Sum(s => s.UsedStudents);
            var pageIndex = Math.Max(0, fromPage);
            var pageSize = Math.Max(1, limit);

            var pagedSchools = allSchools.Skip(pageIndex * pageSize).Take(pageSize).ToList();

            var region = new RegionUsageDto
            {
                RegionId = regionFilter ?? 0,
                RegionName = regionName,
                TotalStudents = regionTotal,
                UsedStudents = regionUsed,
                UsagePercent = CalculatePercent(regionUsed, regionTotal),
                Schools = new PaginationModel<SchoolUsageDto>(allSchools.Count, pageSize, pagedSchools),
            };

            return new ModuleUsageResponseDto
            {
                ByRegion = new List<RegionUsageDto> { region },
            };
        }

        /// <summary>
        /// Used for teacher view.
        /// Retrieve every connected student with a flag telling whether they have used the module.
        /// </summary>
        private async Task<List<StudentUsageDto>> GetTeacherStudents(UserSessionMetricsRequestDto request, string userId)
        {
            var scope = await GetQueryScope(request, userId, isStaff: false, isDirector: false, oreId: null);
            return await GetStudentsUsage(scope?.StudentIds ?? new List<int>());
        }

        /// <summary>
        /// Builds the used/not-used list for a specific set of students. 
        /// Used means the student has at least one user_session record.
        /// </summary>
        private async Task<List<StudentUsageDto>> GetStudentsUsage(List<int> studentIds)
        {
            if (studentIds == null || studentIds.Count == 0)
            {
                return new List<StudentUsageDto>();
            }

            // Which of the students have any record in user_session.
            var usedIds = await _dbContext.UserSessions
                .Where(s => studentIds.Contains(s.UserId))
                .Select(s => s.UserId)
                .Distinct()
                .ToListAsync();

            var usedSet = usedIds.ToHashSet();

            var names = await GetUserNames(studentIds);

            return studentIds
                .Select(id => new StudentUsageDto
                {
                    UserId = id,
                    Name = names.GetValueOrDefault(id, string.Empty),
                    HasUsed = usedSet.Contains(id),
                })
                .OrderBy(s => s.Name)
                .ToList();
        }

        /// <summary>
        /// Used for director view.
        /// Retrieves every teacher of the director's school with their groups, each group carrying its
        /// own usage percentage
        /// </summary>
        private async Task<List<DirectorTeacherUsageDto>> GetDirectorGroupsUsage(int? schoolId)
        {
            if (!schoolId.HasValue)
            {
                return new List<DirectorTeacherUsageDto>();
            }

            // Teachers of the school, then their groups with the students
            var teacherIds = await GetSchoolTeacherIds(schoolId.Value);

            if (teacherIds.Count == 0)
            {
                return new List<DirectorTeacherUsageDto>();
            }

            var groupRows = await GetTeacherGroupRows(teacherIds);

            if (groupRows.Count == 0)
            {
                return new List<DirectorTeacherUsageDto>();
            }

            // Which of all the students across the groups have used the module
            var allStudentIds = groupRows
                .Where(row => row.StudentId.HasValue)
                .Select(row => row.StudentId.Value)
                .Distinct()
                .ToList();

            var usedSet = (await _dbContext.UserSessions
                .Where(s => allStudentIds.Contains(s.UserId))
                .Select(s => s.UserId)
                .Distinct()
                .ToListAsync())
                .ToHashSet();

            var teacherNames = await GetUserNames(teacherIds);

            var result = new List<DirectorTeacherUsageDto>();

            foreach (var teacherId in groupRows.Select(row => row.TeacherId).Distinct())
            {
                var teacherRows = groupRows.FindAll(row => row.TeacherId == teacherId);
                var teacherDto = new DirectorTeacherUsageDto
                {
                    TeacherId = teacherId,
                    TeacherName = teacherNames.GetValueOrDefault(teacherId, string.Empty),
                };

                foreach (var groupId in teacherRows.Select(row => row.GroupId).Distinct())
                {
                    var rowsForGroup = teacherRows.FindAll(row => row.GroupId == groupId);
                    var groupStudentIds = rowsForGroup
                        .Where(row => row.StudentId.HasValue)
                        .Select(row => row.StudentId.Value)
                        .Distinct()
                        .ToList();
                    var used = groupStudentIds.Count(id => usedSet.Contains(id));

                    teacherDto.Groups.Add(new GroupUsageDto
                    {
                        GroupId = groupId,
                        GroupName = rowsForGroup[0].GroupName,
                        TotalStudents = groupStudentIds.Count,
                        UsedStudents = used,
                        UsagePercent = CalculatePercent(used, groupStudentIds.Count),
                    });
                }

                result.Add(teacherDto);
            }

            return result;
        }

        /// <summary>
        /// Get Teacher ids assigned to the given school.
        /// </summary>
        private static async Task<List<int>> GetSchoolTeacherIds(int schoolId)
        {
            using var connection = new MySqlConnection(UsersConnectionString);

            var ids = await connection.QueryAsync<int>(
                @"SELECT DISTINCT us.user_id
                  FROM gs_users_db_prod.upd_user_schools us
                  INNER JOIN gs_users_db_prod.ur_user_roles ro ON ro.user_id = us.user_id
                  WHERE us.school_id = @SchoolId AND ro.role_id = @TeacherRoleId",
                new { SchoolId = schoolId, TeacherRoleId });

            return ids.ToList();
        }

        /// <summary>
        /// Get the available groups with their students created by the given teachers
        /// </summary>
        private static async Task<List<TeacherGroupRow>> GetTeacherGroupRows(List<int> teacherIds)
        {
            if (teacherIds.Count == 0)
            {
                return new List<TeacherGroupRow>();
            }

            using var connection = new MySqlConnection(TeacherWebConnectionString);

            var rows = await connection.QueryAsync<TeacherGroupRow>(
                @"SELECT gr.id AS GroupId, gr.teacher_id AS TeacherId, gr.name AS GroupName, st.student_id AS StudentId
                  FROM gs_teachers_web_db_prod.teacher_groups gr
                  LEFT JOIN gs_teachers_web_db_prod.teacher_group_students st ON st.teacher_group_id = gr.id
                  WHERE gr.teacher_id IN @TeacherIds AND gr.available = 1",
                new { TeacherIds = teacherIds });

            return rows.ToList();
        }

        /// <summary>
        /// Retrieves, for each school, the number of students who have used the module,
        /// read from the user_session records.
        /// </summary>
        private async Task<Dictionary<int, int>> GetUsedStudentsPerSchool(int? regionFilter, int? schoolFilter)
        {
            var query = _dbContext.UserSessions.AsQueryable();
            if (regionFilter.HasValue)
            {
                query = query.Where(s => s.RegionId == regionFilter.Value);
            }
            if (schoolFilter.HasValue)
            {
                query = query.Where(s => s.SchoolId == schoolFilter.Value);
            }

            var used = await query
                .GroupBy(s => s.SchoolId)
                .Select(g => new { SchoolId = g.Key, Used = g.Select(s => s.UserId).Distinct().Count() })
                .ToListAsync();

            return used.ToDictionary(x => x.SchoolId, x => x.Used);
        }

        /// <summary>
        /// Retrieves the total number of enrolled students in each school
        /// </summary>
        private static async Task<Dictionary<int, int>> GetStudentsPerSchool(List<int>? scopeSchoolIds)
        {
            // An empty scope means no schools in scope = no list
            if (scopeSchoolIds != null && scopeSchoolIds.Count == 0)
                return new Dictionary<int, int>();

            var query = @"SELECT us.school_id AS SchoolId, COUNT(DISTINCT us.user_id) AS Total
                        FROM gs_users_db_prod.upd_user_schools us
                        INNER JOIN gs_users_db_prod.ur_user_roles ro ON ro.user_id = us.user_id
                        WHERE ro.role_id = @StudentRoleId";

            if (scopeSchoolIds != null)
            {
                query += " AND us.school_id IN @SchoolIds";
            }

            query += " GROUP BY us.school_id";

            using var connection = new MySqlConnection(UsersConnectionString);
            var rows = await connection.QueryAsync<SchoolCount>(
                query, new { StudentRoleId, SchoolIds = scopeSchoolIds });

            return rows.ToDictionary(r => r.SchoolId, r => r.Total);
        }

        /// <summary>
        /// Gets the ids of every school that belongs to the given region
        /// </summary>
        private static async Task<List<int>> GetSchoolIdsInRegion(int regionId)
        {
            using var connection = new MySqlConnection(SchoolsConnectionString);

            var ids = await connection.QueryAsync<int>(
                @"SELECT entity_id FROM gs_schools_api_db_prod.sch_addresses WHERE region_id = @RegionId",
                new { RegionId = regionId });

            return ids.ToList();
        }

        /// <summary>
        /// Gets the region a school belongs to, taken from its first address that has a region
        /// </summary>
        private static async Task<int?> GetSchoolRegion(int schoolId)
        {
            using var connection = new MySqlConnection(SchoolsConnectionString);

            return await connection.QueryFirstOrDefaultAsync<int?>(
                @"SELECT region_id FROM gs_schools_api_db_prod.sch_addresses
                  WHERE entity_id = @SchoolId AND region_id IS NOT NULL LIMIT 1",
                new { SchoolId = schoolId });
        }

        /// <summary>
        /// Retrieves a map of school id to region id for every school that has a region
        /// </summary>
        private static async Task<Dictionary<int, int>> GetSchoolRegionMap()
        {
            using var connection = new MySqlConnection(SchoolsConnectionString);

            var rows = await connection.QueryAsync<SchoolRegionRow>(
                @"SELECT entity_id AS SchoolId, region_id AS RegionId
                  FROM gs_schools_api_db_prod.sch_addresses
                  WHERE region_id IS NOT NULL");

            return rows
                .GroupBy(r => r.SchoolId)
                .ToDictionary(g => g.Key, g => g.First().RegionId);
        }

        /// <summary>
        /// Retrieves the name of each school for the given ids
        /// </summary>
        private static async Task<Dictionary<int, string>> GetSchoolNames(List<int> schoolIds)
        {
            using var connection = new MySqlConnection(SchoolsConnectionString);

            var rows = await connection.QueryAsync<IdNameRow>(
                @"SELECT id AS Id, name AS Name
                  FROM gs_schools_api_db_prod.sch_entities
                  WHERE id IN @SchoolIds",
                new { SchoolIds = schoolIds });

            return rows
                .GroupBy(r => r.Id)
                .ToDictionary(g => g.Key, g => g.First().Name);
        }

        /// <summary>
        /// Gets the name of a single region
        /// </summary>
        private static async Task<string> GetRegionName(int regionId)
        {
            using var connection = new MySqlConnection(SchoolsConnectionString);
            var name = await connection.QueryFirstOrDefaultAsync<string>(
                @"SELECT name FROM gs_schools_api_db_prod.sch_regions WHERE id = @RegionId",
                new { RegionId = regionId });
            return name ?? string.Empty;
        }

        /// <summary>
        /// Retrieves a map of region id to name for every region
        /// </summary>
        private static async Task<Dictionary<int, string>> GetRegionNames()
        {
            using var connection = new MySqlConnection(SchoolsConnectionString);
            var rows = await connection.QueryAsync<IdNameRow>(
                @"SELECT id AS Id, name AS Name FROM gs_schools_api_db_prod.sch_regions");
            return rows
                .GroupBy(r => r.Id)
                .ToDictionary(g => g.Key, g => g.First().Name);
        }

        /// <summary>
        /// Retrieves the display name of each user for the given ids
        /// </summary>
        private static async Task<Dictionary<int, string>> GetUserNames(List<int> userIds)
        {
            using var connection = new MySqlConnection(UsersConnectionString);
            var rows = await connection.QueryAsync<UserName>(
                @"SELECT user_id AS UserId,
                         TRIM(CONCAT(given_name, ' ', surname)) AS Name
                  FROM gs_users_db_prod.upd_user_full_names
                  WHERE user_id IN @UserIds",
                new { UserIds = userIds });

            return rows
                .GroupBy(r => r.UserId)
                .ToDictionary(g => g.Key, g => g.First().Name);
        }

        /// <summary>
        /// Calculates the percentage of used values over the total.
        /// </summary>
        private static double CalculatePercent(int used, int total)
        {
            return total > 0 ? Math.Round(used * 100.0 / total, 1) : 0.0;
        }

        private sealed class TeacherGroupRow
        {
            public int GroupId { get; set; }
            public int TeacherId { get; set; }
            public string GroupName { get; set; } = string.Empty;
            public int? StudentId { get; set; }
        }

        private sealed class SchoolCount
        {
            public int SchoolId { get; set; }
            public int Total { get; set; }
        }

        private sealed class SchoolRegionRow
        {
            public int SchoolId { get; set; }
            public int RegionId { get; set; }
        }

        private sealed class IdNameRow
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private sealed class UserName
        {
            public int UserId { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }
}
