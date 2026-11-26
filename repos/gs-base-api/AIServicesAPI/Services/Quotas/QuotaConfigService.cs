using GSAIServicesAPI.FeignAPIs.Models;
using GSAIServicesAPI.Models.DTOs.Quotas;
using GSAIServicesAPI.Models.Quotas;
using Microsoft.EntityFrameworkCore;

namespace GSAIServicesAPI.Services.Quotas
{
    /// <summary>
    /// Reads and writes the quota configuration for the admin panel. 
    /// </summary>
    public class QuotaConfigService
    {
        private readonly AppDbContext _db;

        public QuotaConfigService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns the full quota configuration, the master switch and one entry per
        /// active resource type
        /// </summary>
        public async Task<QuotaConfigDto> GetConfigAsync()
        {
            var settings = await GetSettingsAsync();

            var resourceTypes = await _db.QuotaResourceTypes
                .Where(resource => resource.IsActive)
                .OrderBy(resource => resource.Code)
                .ToListAsync();

            // Only the Global-scope limits are surfaced to the admin panel
            var limits = await _db.QuotaResourceLimits
                .Where(c => c.Scope == QuotaScope.Global)
                .ToListAsync();

            var result = new QuotaConfigDto
            {
                QuotasEnabled = settings.QuotasEnabled,
                AiEnabled = settings.AiEnabled,
                DefaultScope = settings.DefaultScope,
                Resources = resourceTypes.Select(resourceType =>
                {
                    var limit = limits.FirstOrDefault(c => c.ResourceTypeCode == resourceType.Code);
                    return new QuotaResourceConfigDto
                    {
                        ResourceTypeCode = resourceType.Code,
                        Name = resourceType.Name,
                        Unit = resourceType.Unit,
                        Enabled = limit?.Enabled ?? false,
                        MaxAmount = limit?.MaxAmount ?? 0,
                        PeriodType = (limit?.PeriodType ?? PeriodType.Monthly).ToString(),
                        CustomPeriodDays = limit?.CustomPeriodDays,
                        RolloverEnabled = limit?.RolloverEnabled ?? false,
                        RolloverMaxAmount = limit?.RolloverMaxAmount
                    };
                }).ToList()
            };

            return result;
        }

        /// <summary>
        /// Validates and persists the supplied configuration, updates the master switch row
        /// and upserts each resource's quota row.
        /// </summary>
        /// <returns>
        /// (true, null) on success, or (false, errorKey) naming the failed validation.
        /// </returns>
        public async Task<(bool ok, string? errorKey)> SaveConfigAsync(QuotaConfigDto request)
        {
            if (request?.Resources == null)
            {
                return (false, "InvalidSettings");
            }

            var knownCodes = await _db.QuotaResourceTypes
                .Where(r => r.IsActive)
                .Select(r => r.Code)
                .ToListAsync();

            // Validate every resource entry before touching the DB so a bad
            // payload never persists a partial configuration.
            foreach (var resource in request.Resources)
            {
                if (!knownCodes.Contains(resource.ResourceTypeCode))
                {
                    return (false, "UnknownResourceType");
                }

                var (valid, errorKey, _) = ValidateLimitRules(
                    resource.PeriodType, resource.MaxAmount, resource.CustomPeriodDays,
                    resource.RolloverEnabled, resource.RolloverMaxAmount);

                if (!valid)
                {
                    return (false, errorKey);
                }
            }

            // DefaultScope is optional, but when supplied it must name a real scope
            if (!string.IsNullOrWhiteSpace(request.DefaultScope)
                && !Enum.TryParse<QuotaScope>(request.DefaultScope, ignoreCase: true, out _))
            {
                return (false, "InvalidDefaultScope");
            }

            var now = DateTime.UtcNow;

            await UpdateSettingsAsync(request, now);

            foreach (var resource in request.Resources)
            {
                var periodType = Enum.Parse<PeriodType>(resource.PeriodType, ignoreCase: true);
                await UpsertResourceLimitAsync(resource, periodType, now);
            }

            await _db.SaveChangesAsync();

            return (true, null);
        }

        /// <summary>
        /// Applies the master switches and default scope to the settings row.
        /// </summary>
        private async Task UpdateSettingsAsync(QuotaConfigDto request, DateTime now)
        {
            var settings = await GetSettingsAsync();

            var scope = Enum.TryParse<QuotaScope>(request.DefaultScope, ignoreCase: true, out var parsed)
                ? parsed
                : QuotaScope.Global;

            settings.QuotasEnabled = request.QuotasEnabled;
            settings.AiEnabled = request.AiEnabled;
            settings.DefaultScope = scope.ToString();
            settings.UpdatedAt = now;
        }

        /// <summary>
        /// Returns the settings row
        /// </summary>
        private async Task<QuotaSetting> GetSettingsAsync() =>
            await _db.QuotaSettings.FirstOrDefaultAsync(s => s.Id == QuotaSetting.SingletonId)
            ?? throw new InvalidOperationException(
                "quota_settings has no row. It is seeded by the AddQuotaSystem migration, so the DB is most likely not up to date."
            );

        /// <summary>
        /// Upserts the Global-scope quota limit row for a single resource
        /// </summary>
        private async Task UpsertResourceLimitAsync(QuotaResourceConfigDto resource, PeriodType periodType, DateTime now)
        {
            var limit = await _db.QuotaResourceLimits.FirstOrDefaultAsync(c =>
                c.ResourceTypeCode == resource.ResourceTypeCode &&
                c.Scope == QuotaScope.Global &&
                c.ScopeId == "");

            if (limit is null)
            {
                limit = new QuotaResourceLimit
                {
                    ResourceTypeCode = resource.ResourceTypeCode,
                    Scope = QuotaScope.Global,
                    ScopeId = string.Empty,
                    CreatedAt = now
                };

                _db.QuotaResourceLimits.Add(limit);
            }

            limit.Enabled = resource.Enabled;
            limit.MaxAmount = resource.MaxAmount;
            limit.PeriodType = periodType;
            limit.CustomPeriodDays = periodType == PeriodType.Custom ? resource.CustomPeriodDays : null;
            limit.RolloverEnabled = resource.RolloverEnabled;
            limit.RolloverMaxAmount = resource.RolloverEnabled ? resource.RolloverMaxAmount : null;
            limit.UpdatedAt = now;
        }

        /// <summary>
        /// Validates the rules shared by every limit, whatever scope it applies at.
        /// </summary>
        private static (bool ok, string? errorKey, PeriodType periodType) ValidateLimitRules(string periodTypeRaw, long maxAmount, int? customPeriodDays, bool rolloverEnabled, long? rolloverMaxAmount)
        {
            if (!Enum.TryParse<PeriodType>(periodTypeRaw, ignoreCase: true, out var periodType))
            {
                return (false, "InvalidPeriodType", default);
            }

            if (periodType == PeriodType.Custom && (customPeriodDays is null or <= 0))
            {
                return (false, "InvalidCustomPeriodDays", default);
            }

            if (maxAmount < 0)
            {
                return (false, "InvalidMaxAmount", default);
            }

            if (rolloverEnabled && rolloverMaxAmount is < 0)
            {
                return (false, "InvalidRolloverMax", default);
            }

            return (true, null, periodType);
        }

        /// <summary>
        /// Returns the user, school and role overrides, every limit that replaces the
        /// global default for one specific target. A user's own limit wins over their
        /// school's, a school's over their role's, and a role's over the global one, so a
        /// row listed here is what actually applies to its target.
        /// The global defaults are served by the settings endpoint
        /// </summary>
        /// <param name="scope">Optional scope filter (Role, User, School).</param>
        /// <param name="scopeId">Optional target filter (a specific role, user or school id)</param>
        /// <param name="page">The current page index.</param>
        /// <param name="pageSize">Rows per page</param>
        public async Task<PaginationModel<QuotaScopedLimitDto>> GetScopedLimitsAsync(string? scope, string? scopeId, int page, int pageSize)
        {
            page = Math.Max(page, 0);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var resourceTypes = await _db.QuotaResourceTypes
                .AsNoTracking()
                .ToListAsync();

            var query = BuildScopedLimitQuery(scope, scopeId);

            var total = await query.CountAsync();

            var limits = await query
                .OrderBy(l => l.Scope)
                .ThenBy(l => l.ScopeId)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var consumption = await GetUserConsumptionAsync(limits);

            var items = limits.Select(limit =>
            {
                var resourceType = resourceTypes.FirstOrDefault(r => r.Code == limit.ResourceTypeCode);
                var dto = ToScopedDto(limit, resourceType);

                if (consumption.TryGetValue(limit.Id, out var usage))
                {
                    dto.ConsumedAmount = usage.Consumed;
                    dto.PeriodStart = usage.PeriodStart;
                }

                return dto;
            }).ToList();

            return new PaginationModel<QuotaScopedLimitDto>(total, pageSize, items);
        }

        /// <summary>
        /// Retrieves how much each User-scoped override on the page has spent in its current period,
        /// using the limit id.
        /// <br/><br/>
        /// Role and School overrides are skipped, usage is calculated per user, so there is no single
        /// counter for a whole role or school.
        /// Periods are calculated with the same routine the enforcement path uses, 
        /// so the figure matches what the next request would see.
        /// </summary>
        private async Task<Dictionary<int, (long Consumed, DateTime PeriodStart)>> GetUserConsumptionAsync(List<QuotaResourceLimit> limits)
        {
            var result = new Dictionary<int, (long, DateTime)>();

            var userLimits = limits.Where(l => l.Scope == QuotaScope.User).ToList();

            if (userLimits.Count == 0)
            {
                return result;
            }

            var now = DateTime.UtcNow;

            // Period start per limit, so the lookup below knows which row to match.
            var periodStarts = userLimits.ToDictionary(
                l => l.Id,
                l => QuotaService.CalculatePeriod(l, now).start);

            var userIds = userLimits.Select(l => l.ScopeId).Distinct().ToList();
            var resourceCodes = userLimits.Select(l => l.ResourceTypeCode).Distinct().ToList();
            var earliestStart = periodStarts.Values.Min();

            var limitIds = userLimits.Select(l => l.Id).ToList();

            var periods = await _db.QuotaUsagePeriods
                .AsNoTracking()
                .Where(p => userIds.Contains(p.UserId)
                            && resourceCodes.Contains(p.ResourceTypeCode)
                            && limitIds.Contains(p.LimitId)
                            && p.PeriodStart >= earliestStart)
                .Select(p => new { p.UserId, p.ResourceTypeCode, p.PeriodStart, p.LimitId, p.ConsumedAmount })
                .ToListAsync();

            foreach (var limit in userLimits)
            {
                var periodStart = periodStarts[limit.Id];

                // Matched on the limit too: a balance left behind by a different limit is not this
                // limit's spend, since consumption does not transfer between them.
                var period = periods.FirstOrDefault(p => p.UserId == limit.ScopeId
                                                         && p.ResourceTypeCode == limit.ResourceTypeCode
                                                         && p.LimitId == limit.Id
                                                         && p.PeriodStart == periodStart);

                // No row means the user has not spent anything in this period yet
                result[limit.Id] = (period?.ConsumedAmount ?? 0, periodStart);
            }

            return result;
        }

        /// <summary>
        /// Lists the users who have actually consumed a resource in the period running now, with
        /// the limit governing each one and the scope it came from.
        /// <br/><br/>
        /// This exists because the scoped-limit list only shows configured limits: a School or Role
        /// limit has no counter of its own, so there was no way to see who was spending under it.
        /// Rows are driven by recorded consumption
        /// <br/><br/>
        /// Each balance records the limit it was opened under, so the governing scope is read
        /// straight off the row.
        /// <br/><br/>
        /// Only balances of the period running now are listed. A balance left behind by a limit that
        /// no longer governs the user is skipped, since consumption does not transfer between limits
        /// </summary>
        /// <param name="resourceTypeCode">Optional resource filter</param>
        /// <param name="scope">Optional filter by the scope of the governing limit (Role, School, User, Global)</param>
        /// <param name="scopeId">Optional filter by the governing limit's target: a role or school id</param>
        /// <param name="userId">Optional filter by the consuming user</param>
        /// <param name="page">The current page index</param>
        /// <param name="pageSize">Rows per page</param>
        public async Task<PaginationModel<QuotaUserUsageDto>> GetUserUsageAsync(string? resourceTypeCode, string? scope, string? scopeId, string? userId, int page, int pageSize)
        {
            page = Math.Max(page, 0);
            pageSize = Math.Clamp(pageSize, 1, 50);

            var resourceTypes = await _db.QuotaResourceTypes
                .AsNoTracking()
                .ToListAsync();

            var limits = await _db.QuotaResourceLimits
                .AsNoTracking()
                .ToListAsync();

            var now = DateTime.UtcNow;

            // The period start each limit is currently on.
            // A balance is current when its own start matches the one its limit produces now,
            // anything older belongs to a closed period.
            var currentStarts = limits.ToDictionary(
                l => l.Id,
                l => QuotaService.CalculatePeriod(l, now).start);

            // Earliest start any active limit is currently on.
            // Used only to keep the query from dragging back the whole history,
            // the exact per-limit period is matched below, since limits of different cadences are on different windows at the same time.
            var earliestStart = currentStarts.Count > 0 ? currentStarts.Values.Min() : now.Date;

            var usageQuery = _db.QuotaUsagePeriods
                .AsNoTracking()
                .Where(p => p.PeriodStart >= earliestStart && p.ConsumedAmount > 0);

            if (!string.IsNullOrWhiteSpace(resourceTypeCode))
            {
                var trimmed = resourceTypeCode.Trim();
                usageQuery = usageQuery.Where(p => p.ResourceTypeCode == trimmed);
            }

            // Filtering by the consuming user is a plain column match: every balance belongs to one.
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var trimmedUserId = userId.Trim();
                usageQuery = usageQuery.Where(p => p.UserId == trimmedUserId);
            }

            // Filtering by scope goes through the limits, not the balances: a balance records which
            // limit opened it, so "everyone governed by a school limit" is "every balance whose
            // limit_id belongs to a School-scoped limit".
            if (Enum.TryParse<QuotaScope>(scope, ignoreCase: true, out var parsedScope))
            {
                var matchingLimits = limits.Where(l => l.Scope == parsedScope);

                // A target restricts it to one role, school or user; without it every limit of that scope counts
                if (!string.IsNullOrWhiteSpace(scopeId))
                {
                    var trimmedScopeId = scopeId.Trim();
                    matchingLimits = matchingLimits.Where(l => l.ScopeId == trimmedScopeId);
                }

                var matchingLimitIds = matchingLimits.Select(l => l.Id).ToList();

                usageQuery = usageQuery.Where(p => matchingLimitIds.Contains(p.LimitId));
            }

            // Closed periods are dropped before counting
            var liveRows = (await usageQuery.ToListAsync())
                .Where(p => currentStarts.TryGetValue(p.LimitId, out var start) && p.PeriodStart == start)
                .OrderByDescending(p => p.ConsumedAmount)
                .ThenBy(p => p.UserId)
                .ToList();

            var total = liveRows.Count;

            var usageRows = liveRows
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToList();

            var items = new List<QuotaUserUsageDto>();

            foreach (var row in usageRows)
            {
                var limit = limits.First(l => l.Id == row.LimitId);
                var resourceType = resourceTypes.FirstOrDefault(r => r.Code == row.ResourceTypeCode);

                items.Add(new QuotaUserUsageDto
                {
                    UserId = row.UserId,
                    ResourceTypeCode = row.ResourceTypeCode,
                    Name = resourceType?.Name,
                    Unit = resourceType?.Unit,
                    ConsumedAmount = row.ConsumedAmount,
                    AllocatedAmount = limit.MaxAmount + row.RolloverCarriedIn,
                    LimitScope = limit.Scope.ToString(),
                    LimitScopeId = limit.ScopeId,
                    PeriodStart = row.PeriodStart,
                    PeriodEnd = row.PeriodEnd
                });
            }

            return new PaginationModel<QuotaUserUsageDto>(total, pageSize, items);
        }

        /// <summary>
        /// Query for the scoped overrides: every limit except the global default, optionally filtered by
        /// scope and target. A blank, unknown or global scope means "no filter" and never returns the global row.
        /// </summary>
        private IQueryable<QuotaResourceLimit> BuildScopedLimitQuery(string? scope, string? scopeId)
        {
            var query = _db.QuotaResourceLimits
                .AsNoTracking()
                .Where(l => l.Scope != QuotaScope.Global);

            if (Enum.TryParse<QuotaScope>(scope, ignoreCase: true, out var parsed) && parsed != QuotaScope.Global)
            {
                query = query.Where(l => l.Scope == parsed);
            }

            if (!string.IsNullOrWhiteSpace(scopeId))
            {
                var trimmed = scopeId.Trim();
                query = query.Where(l => l.ScopeId == trimmed);
            }

            return query;
        }

        /// <summary>
        /// Creates or updates the override for one (resource, scope, target) combination.
        /// <br></br>
        /// The panel identifies a limit by what it applies to (resource, scope, target),
        /// not by its row id, which it does not know before the row exists. Saving an
        /// existing combination therefore updates it, since the unique index over those
        /// three columns would reject a second insert.
        /// </summary>

        /// <returns>
        /// (true, null) on success, or (false, errorKey) naming the failed validation.
        /// </returns>
        public async Task<(bool ok, string? errorKey, QuotaScopedLimitDto? saved)> SaveScopedLimitAsync(QuotaScopedLimitDto request)
        {
            if (request is null)
            {
                return (false, "InvalidSettings", null);
            }

            if (!Enum.TryParse<QuotaScope>(request.Scope, ignoreCase: true, out var scope))
            {
                return (false, "InvalidScope", null);
            }

            // The global limit is owned by the settings endpoint, which keeps exactly one row per resource.
            if (scope == QuotaScope.Global)
            {
                return (false, "GlobalScopeNotAllowedHere", null);
            }

            // Every non-global scope targets a specific entity, so its ScopeId (the user,
            // school or role id) is required.

            if (string.IsNullOrWhiteSpace(request.ScopeId))
            {
                return (false, "MissingScopeId", null);
            }

            var resourceType = await _db.QuotaResourceTypes.FirstOrDefaultAsync(r => r.Code == request.ResourceTypeCode && r.IsActive);

            if (resourceType is null)
            {
                return (false, "UnknownResourceType", null);
            }

            var (valid, errorKey, periodType) = ValidateLimitRules(
                request.PeriodType, request.MaxAmount, request.CustomPeriodDays,
                request.RolloverEnabled, request.RolloverMaxAmount);

            if (!valid)
            {
                return (false, errorKey, null);
            }

            var scopeId = request.ScopeId.Trim();
            var now = DateTime.UtcNow;

            var limit = await _db.QuotaResourceLimits.FirstOrDefaultAsync(limit =>
                limit.ResourceTypeCode == request.ResourceTypeCode &&
                limit.Scope == scope &&
                limit.ScopeId == scopeId);

            void ApplyFields(QuotaResourceLimit target)
            {
                target.Enabled = request.Enabled;
                target.MaxAmount = request.MaxAmount;
                target.PeriodType = periodType;
                target.CustomPeriodDays = periodType == PeriodType.Custom ? request.CustomPeriodDays : null;
                target.RolloverEnabled = request.RolloverEnabled;
                target.RolloverMaxAmount = request.RolloverEnabled ? request.RolloverMaxAmount : null;
                target.UpdatedAt = now;
            }

            if (limit is not null)
            {
                ApplyFields(limit);
                await _db.SaveChangesAsync();
                return (true, null, ToScopedDto(limit, resourceType));
            }

            limit = new QuotaResourceLimit
            {
                ResourceTypeCode = request.ResourceTypeCode,
                Scope = scope,
                ScopeId = scopeId,
                CreatedAt = now
            };
            ApplyFields(limit);
            _db.QuotaResourceLimits.Add(limit);

            await _db.SaveChangesAsync();

            return (true, null, ToScopedDto(limit, resourceType));
        }

        /// <summary>
        /// Removes an override so its target inherits from the level above it again
        /// (a user falls back to their school, a school to their role, a role to the global limit).
        /// </summary>
        /// <returns>
        /// (true, null) on success, or (false, errorKey) naming the failed validation.
        /// </returns>
        public async Task<(bool ok, string? errorKey)> DeleteScopedLimitAsync(int id)
        {
            var limit = await _db.QuotaResourceLimits.FirstOrDefaultAsync(l => l.Id == id);

            if (limit is null)
            {
                return (false, "LimitNotFound");
            }

            // Deleting the global row through this method would leave the resource with no default at all
            if (limit.Scope == QuotaScope.Global)
            {
                return (false, "GlobalScopeNotAllowedHere");
            }

            _db.QuotaResourceLimits.Remove(limit);
            await _db.SaveChangesAsync();

            return (true, null);
        }

        /// <summary>
        /// Converts a stored limit into the shape the frontend expects
        /// </summary>
        private static QuotaScopedLimitDto ToScopedDto(QuotaResourceLimit limit, QuotaResourceType? resourceType) => new()
        {
            Id = limit.Id,
            ResourceTypeCode = limit.ResourceTypeCode,
            Name = resourceType?.Name,
            Unit = resourceType?.Unit,
            Scope = limit.Scope.ToString(),
            ScopeId = limit.ScopeId,
            Enabled = limit.Enabled,
            MaxAmount = limit.MaxAmount,
            PeriodType = limit.PeriodType.ToString(),
            CustomPeriodDays = limit.CustomPeriodDays,
            RolloverEnabled = limit.RolloverEnabled,
            RolloverMaxAmount = limit.RolloverMaxAmount
        };
    }
}
