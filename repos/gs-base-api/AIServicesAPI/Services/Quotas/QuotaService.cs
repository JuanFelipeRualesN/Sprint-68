using GSAIServicesAPI.FeignAPIs;
using GSAIServicesAPI.Models.DTOs.Quotas;
using GSAIServicesAPI.Models.Quotas;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GSAIServicesAPI.Services.Quotas
{
    /// <summary>
    /// Enforces quota consumption, resolves which limit applies to a user, keeps their
    /// per-period balance up to date, and decides whether a request may proceed.
    /// Callers use a two-phase flow, because AI operations only reveal their true cost after they have run
    /// <br></br>
    /// ReserveAsync holds an estimated amount before the call.
    /// CommitAsync settles the hold against the real usage.
    /// ReleaseAsync gives the hold back if the call never completed.
    /// <br></br>
    /// Reserving up-front keeps concurrent requests safe: the balance is debited by a
    /// single conditional UPDATE, so two simultaneous requests can never both pass a
    /// check against the same remaining balance.
    /// </summary>
    public class QuotaService
    {
        private readonly AppDbContext _db;

        public QuotaService(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Number of times a partial hold is retried when a concurrent request moves the
        /// balance between reading it and debiting it.
        /// </summary>
        private const int MaxHoldAttempts = 3;

        /// <summary>
        /// Attempts to hold an estimated amount of a resource for a user.
        /// Returns a reservation describing whether the call may proceed, when no
        /// quota applies the reservation is allowed and marked as not enforced.
        /// </summary>
        /// <param name="userId">The user id</param>
        /// <param name="roleId">The user's role id</param>
        /// <param name="token">The caller's session token, used to look up their schools when a school-scoped limit exists</param>
        /// <param name="resourceTypeCode">The resource to consume</param>
        /// <param name="estimatedAmount">Units to hold</param>
        /// <param name="reference">Module/endpoint the usage is attributed to</param>
        public async Task<QuotaReservation> ReserveAsync(string userId, string? roleId, string token, string resourceTypeCode, long estimatedAmount, string? reference = null)
        {
            var limit = await ResolveLimitAsync(resourceTypeCode, userId, roleId, token);

            if (limit is null)
            {
                // No quota governs this call, but the consumption is still recorded at
                // settle time so usage stays auditable with enforcement off.
                return QuotaReservation.NotEnforced(userId, resourceTypeCode, reference);
            }

            var now = DateTime.UtcNow;
            var period = await GetOrCreatePeriodAsync(userId, resourceTypeCode, limit, now);

            var requested = Math.Max(estimatedAmount, 0);
            var reservationId = Guid.NewGuid().ToString();

            // The debit and its audit row go together: a balance moved without a log entry
            // cannot be reconciled afterwards, so every consumption is recorded. The period
            // row is resolved before this point, since its insert can fail on the unique
            // index and that must not damage the transaction.
            await using var transaction = await _db.Database.BeginTransactionAsync();

            var reserved = requested == 0 ? 0 : await TryHoldAsync(period.Id, requested, now);

            // Nothing at all could be held, so the allowance is spent.
            if (requested > 0 && reserved == 0)
            {
                // A reject moves no balance, so its logged amount is 0
                await WriteLogAsync(userId, resourceTypeCode, period.Id, reservationId, QuotaUsageAction.Reject, 0, reference, now);
                await transaction.CommitAsync();

                return new QuotaReservation
                {
                    Allowed = false,
                    Enforced = true,
                    ReservationId = reservationId,
                    ReservedAmount = 0,
                    RequestedAmount = requested,
                    UsagePeriodId = period.Id,
                    ResourceTypeCode = resourceTypeCode,
                    UserId = userId,
                    Reference = reference,
                    RemainingAmount = await GetRemainingAsync(period.Id),
                    PeriodEnd = period.PeriodEnd,
                    DenialMessageKey = "QuotaExceeded"
                };
            }

            await WriteLogAsync(userId, resourceTypeCode, period.Id, reservationId, QuotaUsageAction.Reserve, reserved, reference, now);
            await transaction.CommitAsync();

            return new QuotaReservation
            {
                Allowed = true,
                Enforced = true,
                ReservationId = reservationId,
                ReservedAmount = reserved,
                RequestedAmount = requested,
                UsagePeriodId = period.Id,
                ResourceTypeCode = resourceTypeCode,
                UserId = userId,
                Reference = reference,
                RemainingAmount = await GetRemainingAsync(period.Id),
                PeriodEnd = period.PeriodEnd
            };
        }

        /// <summary>
        /// Debits as much of the requested amount as the balance allows and returns what
        /// was actually held, or zero when nothing was left.
        /// <br/><br/>
        /// A partial hold keeps a user from being denied while they still have balance that a
        /// full-amount hold would leave stranded: because the reservation is an over-estimate,
        /// insisting on the full amount would waste whatever is smaller than one request's estimate.
        /// The debit stays a single conditional UPDATE, so concurrency safety is unchanged;
        /// if a rival request moves the balance in between, this simply retries with the
        /// new value.
        /// </summary>
        private async Task<long> TryHoldAsync(long periodId, long requested, DateTime now)
        {
            for (var attempt = 0; attempt < MaxHoldAttempts; attempt++)
            {
                var remaining = await GetRemainingAsync(periodId);

                if (remaining <= 0)
                {
                    return 0;
                }

                var take = Math.Min(requested, remaining);

                var updated = await _db.Database.ExecuteSqlInterpolatedAsync($@"
                    UPDATE quota_usage_periods
                    SET consumed_amount = consumed_amount + {take},
                        updated_at = {now}
                    WHERE id = {periodId}
                      AND allocated_amount - consumed_amount >= {take}");

                if (updated > 0)
                {
                    return take;
                }
            }

            return 0;
        }

        /// <summary>
        /// Settles a reservation against what was actually consumed, applying only the difference. 
        /// Safe to call with an amount larger than the reservation: the
        /// operation has already happened and its real cost must be recorded, so the
        /// balance may end up fully spent and the user's next request is what gets denied.
        /// </summary>
        public async Task CommitAsync(QuotaReservation reservation, long actualAmount)
        {
            // A rejected reservation never ran, so there is nothing to settle or record.
            if (!reservation.Allowed)
                return;

            if (!reservation.TryClaimSettlement())
                return;

            try
            {
                var now = DateTime.UtcNow;

                // Not enforced (quotas off, or no limit for this user): no balance to settle,
                // but the real consumption is still logged with a null period and the Record action,
                // so usage stays auditable with enforcement disabled.
                if (!reservation.Enforced || reservation.UsagePeriodId is null)
                {
                    if (actualAmount > 0)
                    {
                        await WriteLogAsync(reservation.UserId, reservation.ResourceTypeCode,
                            usagePeriodId: null, reservation.ReservationId,
                            QuotaUsageAction.Record, actualAmount, reservation.Reference, now);
                    }
                    return;
                }

                var delta = Math.Max(actualAmount, 0) - reservation.ReservedAmount;

                await using var transaction = await _db.Database.BeginTransactionAsync();

                if (delta != 0)
                {
                    await ApplyDeltaAsync(reservation.UsagePeriodId.Value, delta, now);
                }

                await WriteLogAsync(reservation.UserId, reservation.ResourceTypeCode,
                    reservation.UsagePeriodId, reservation.ReservationId,
                    QuotaUsageAction.Commit, delta, reservation.Reference, now);

                await transaction.CommitAsync();
            }
            catch
            {
                reservation.ResetSettlement();
                throw;
            }
        }

        /// <summary>
        /// Returns a reservation to the balance because the operation did not complete.
        /// </summary>
        public async Task ReleaseAsync(QuotaReservation reservation)
        {
            if (!reservation.Enforced || !reservation.Allowed || reservation.UsagePeriodId is null)
                return;

            if (reservation.ReservedAmount == 0)
                return;

            if (!reservation.TryClaimSettlement())
                return;

            try
            {
                var now = DateTime.UtcNow;

                await using var transaction = await _db.Database.BeginTransactionAsync();

                await ApplyDeltaAsync(reservation.UsagePeriodId.Value, -reservation.ReservedAmount, now);

                await WriteLogAsync(reservation.UserId, reservation.ResourceTypeCode,
                    reservation.UsagePeriodId, reservation.ReservationId,
                    QuotaUsageAction.Release, -reservation.ReservedAmount, reservation.Reference, now);

                await transaction.CommitAsync();
            }
            catch
            {
                reservation.ResetSettlement();
                throw;
            }
        }

        /// <summary>
        /// Returns the user's current balance for every active resource, for display in the frontend.
        /// Reading a balance never creates a period row
        /// </summary>
        public async Task<List<QuotaBalanceDto>> GetBalancesAsync(string userId, string? roleId, string token)
        {
            var resourceTypes = await _db.QuotaResourceTypes
                .AsNoTracking()
                .Where(r => r.IsActive)
                .OrderBy(r => r.Code)
                .ToListAsync();

            var now = DateTime.UtcNow;
            var balances = new List<QuotaBalanceDto>();

            // Read the switch and every candidate limit once, up front, rather than once per
            // resource inside the loop below.
            var quotasEnabled = await AreQuotasEnabledAsync();

            var allLimits = quotasEnabled
                ? await _db.QuotaResourceLimits.AsNoTracking().Where(l => l.Enabled).ToListAsync()
                : [];

            var schoolIds = await ResolveSchoolIdsAsync(allLimits, userId, token);

            foreach (var resourceType in resourceTypes)
            {
                var dto = new QuotaBalanceDto
                {
                    ResourceTypeCode = resourceType.Code,
                    Name = resourceType.Name,
                    Unit = resourceType.Unit
                };

                var limit = MatchLimit(
                    allLimits.Where(l => l.ResourceTypeCode == resourceType.Code).ToList(),
                    userId, roleId, schoolIds);

                if (limit is not null)
                {
                    var (periodStart, periodEnd) = CalculatePeriod(limit, now);
                    // Scoped to the governing limit, so the balance shown is the one the next
                    // request will actually draw from
                    var period = await _db.QuotaUsagePeriods
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.UserId == userId &&
                                                  p.ResourceTypeCode == resourceType.Code &&
                                                  p.PeriodStart == periodStart &&
                                                  p.LimitId == limit.Id);

                    // Derived from the limit as it stands, exactly as the enforcement path
                    // does, so the figure shown is the one the next request will get.
                    // Taking the stored allocation instead would report a stale number after a limit
                    // change, and omitting the rollover would understate the allowance of a
                    // user whose period row does not exist yet.
                    var carriedIn = period?.RolloverCarriedIn ?? await CalculateRolloverAsync(userId, resourceType.Code, limit, periodStart);

                    dto.Enforced = true;
                    dto.PeriodStart = periodStart;
                    dto.PeriodEnd = periodEnd;
                    dto.AllocatedAmount = limit.MaxAmount + carriedIn;
                    dto.ConsumedAmount = period?.ConsumedAmount ?? 0;
                    dto.RolloverCarriedIn = carriedIn;
                    dto.RemainingAmount = Math.Max(dto.AllocatedAmount - dto.ConsumedAmount, 0);
                }

                balances.Add(dto);
            }

            return balances;
        }

        /// <summary>
        /// Finds the limit that governs a user, from the most specific scope to the least:
        /// user, then school, then role, then global.
        /// Returns null when quotas are switched off globally or no enabled limit matches, meaning "do not enforce".
        /// </summary>
        private async Task<QuotaResourceLimit?> ResolveLimitAsync(string resourceTypeCode, string userId, string? roleId, string token)
        {
            if (!await AreQuotasEnabledAsync())
                return null;

            var candidates = await _db.QuotaResourceLimits
                .AsNoTracking()
                .Where(l => l.ResourceTypeCode == resourceTypeCode && l.Enabled)
                .ToListAsync();

            var schoolIds = await ResolveSchoolIdsAsync(candidates, userId, token);

            return MatchLimit(candidates, userId, roleId, schoolIds);
        }

        /// <summary>
        /// Looks up the user's schools, but only when a school-scoped limit is actually among the candidates.
        /// This keeps the extra API call off the request path for the common case where no per-school limit is configured.
        /// </summary>
        private static async Task<IReadOnlyCollection<string>> ResolveSchoolIdsAsync(List<QuotaResourceLimit> candidates, string userId, string token)
        {
            if (!candidates.Any(l => l.Scope == QuotaScope.School))
                return Array.Empty<string>();

            return await UserFeignAPI.GetUserSchoolIdsAsync(userId, token);
        }

        /// <summary>
        /// Reads the master switch. A missing row means an unmigrated DB, and is
        /// treated as quotas off so a configuration problem cannot block generation.
        /// </summary>
        private async Task<bool> AreQuotasEnabledAsync()
        {
            var settings = await _db.QuotaSettings
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == QuotaSetting.SingletonId);

            return settings is not null && settings.QuotasEnabled;
        }

        /// <summary>
        /// Picks the limit that applies to a user out of the candidates for one resource,
        /// from the most specific scope to the least. Kept separate from the query so a
        /// caller that already holds the rows can resolve without hitting the DB.
        /// </summary>
        private static QuotaResourceLimit? MatchLimit(List<QuotaResourceLimit> candidates, string userId, string? roleId, IReadOnlyCollection<string>? schoolIds)
        {
            if (candidates.Count == 0)
                return null;

            return Match(QuotaScope.User, userId)
                ?? MatchSchool()
                ?? Match(QuotaScope.Role, roleId)
                ?? candidates.FirstOrDefault(l => l.Scope == QuotaScope.Global);

            QuotaResourceLimit? Match(QuotaScope scope, string? scopeId) =>
                string.IsNullOrEmpty(scopeId)
                    ? null
                    : candidates.FirstOrDefault(l => l.Scope == scope && l.ScopeId == scopeId);

            // A user can belong to several schools. When more than one has a limit for this
            // resource, the most generous wins, so a user is never held to
            // their strictest school while another grants more.
            QuotaResourceLimit? MatchSchool() =>
                schoolIds is null or { Count: 0 }
                    ? null
                    : candidates
                        .Where(l => l.Scope == QuotaScope.School && schoolIds.Contains(l.ScopeId))
                        .OrderByDescending(l => l.MaxAmount)
                        .ThenBy(l => l.ScopeId)
                        .FirstOrDefault();
        }

        /// <summary>
        /// Returns the user's balance row for the period covering the current time, creating it if this is their first request of the period.
        /// Because the row is created on first use rather than by a job, the reset is automatic,
        /// a new period is just a new row with a new allocation.
        /// <br/><br/>
        /// The row is scoped to the governing limit, so a user who moves from one limit to another
        /// mid-period opens a fresh balance starting at zero rather than inheriting the spend from
        /// the previous one
        /// </summary>
        private async Task<QuotaUsagePeriod> GetOrCreatePeriodAsync(string userId, string resourceTypeCode, QuotaResourceLimit limit, DateTime now)
        {
            var (periodStart, periodEnd) = CalculatePeriod(limit, now);

            var existing = await _db.QuotaUsagePeriods
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId &&
                                          p.ResourceTypeCode == resourceTypeCode &&
                                          p.PeriodStart == periodStart &&
                                          p.LimitId == limit.Id);
            if (existing is not null)
            {
                // Re-derive the allocation from the limit as it stands now. Without this the
                // amount is frozen at whatever it was when the row was created, so raising
                // or lowering a limit would only reach users who had not yet made a request
                // in the current period.

                // The carried-in rollover is kept: it was settled when the period opened and
                // does not change with the limit.
                var allocated = limit.MaxAmount + existing.RolloverCarriedIn;

                if (existing.AllocatedAmount != allocated)
                {
                    await _db.Database.ExecuteSqlInterpolatedAsync($@"
                        UPDATE quota_usage_periods
                        SET allocated_amount = {allocated},
                            updated_at = {now}
                        WHERE id = {existing.Id}");

                    existing.AllocatedAmount = allocated;
                }

                return existing;
            }

            var carriedIn = await CalculateRolloverAsync(userId, resourceTypeCode, limit, periodStart);

            var period = new QuotaUsagePeriod
            {
                UserId = userId,
                ResourceTypeCode = resourceTypeCode,
                LimitId = limit.Id,
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                AllocatedAmount = limit.MaxAmount + carriedIn,
                ConsumedAmount = 0,
                RolloverCarriedIn = carriedIn,
                CreatedAt = now,
                UpdatedAt = now
            };

            try
            {
                _db.QuotaUsagePeriods.Add(period);
                await _db.SaveChangesAsync();
                return period;
            }
            catch (DbUpdateException ex) when (ex.InnerException is MySqlException { Number: 1062 })
            {
                // Another concurrent request created the same period first; the unique
                // index rejected this insert, so adopt the row that came first.
                _db.Entry(period).State = EntityState.Detached;

                return await _db.QuotaUsagePeriods
                    .AsNoTracking()
                    .FirstAsync(p => p.UserId == userId &&
                                     p.ResourceTypeCode == resourceTypeCode &&
                                     p.PeriodStart == periodStart &&
                                     p.LimitId == limit.Id);
            }
        }

        /// <summary>
        /// Calculates how much unused balance carries into a new period.
        /// Only the immediately preceding period rolls over: a user who
        /// was inactive for several periods does not accumulate allowance for time they never used the service.
        /// <br/><br/>
        /// Only a period of the same limit rolls over.
        /// </summary>
        private async Task<long> CalculateRolloverAsync(string userId, string resourceTypeCode, QuotaResourceLimit limit, DateTime periodStart)
        {
            if (!limit.RolloverEnabled)
            {
                return 0;
            }

            var previous = await _db.QuotaUsagePeriods
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.UserId == userId &&
                                          p.ResourceTypeCode == resourceTypeCode &&
                                          p.PeriodEnd == periodStart &&
                                          p.LimitId == limit.Id);

            if (previous is null)
            {
                return 0;
            }

            var leftover = Math.Max(previous.AllocatedAmount - previous.ConsumedAmount, 0);

            return limit.RolloverMaxAmount is { } cap
                ? Math.Min(leftover, cap)
                : leftover;
        }

        /// <summary>
        /// Returns the start and end of the limit's period containing the current time.
        /// </summary>
        internal static (DateTime start, DateTime end) CalculatePeriod(QuotaResourceLimit limit, DateTime now)
        {
            var today = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);

            switch (limit.PeriodType)
            {
                case PeriodType.Daily:
                    return (today, today.AddDays(1));

                case PeriodType.Weekly:
                {
                    var daysSinceMonday = ((int)today.DayOfWeek + 6) % 7;
                    var start = today.AddDays(-daysSinceMonday);

                    return (start, start.AddDays(7));
                }

                case PeriodType.Custom:
                {
                    // Fixed length windows counted from when the limit was configured
                    var days = limit.CustomPeriodDays is > 0 ? limit.CustomPeriodDays.Value : 30;

                    var anchor = limit.CreatedAt == default
                        ? today
                        : new DateTime(limit.CreatedAt.Year, limit.CreatedAt.Month, limit.CreatedAt.Day,
                                       0, 0, 0, DateTimeKind.Utc);

                    if (anchor > today)
                        anchor = today;

                    var elapsed = (today - anchor).Days;
                    var start = anchor.AddDays(elapsed / days * days);

                    return (start, start.AddDays(days));
                }

                case PeriodType.Monthly:
                default:
                {
                    var start = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                    return (start, start.AddMonths(1));
                }
            }
        }

        /// <summary>
        /// Applies a signed change to a period's consumed amount in one statement
        /// Never lets the consumed amount fall below zero
        /// </summary>
        private Task ApplyDeltaAsync(long periodId, long delta, DateTime now) =>
            _db.Database.ExecuteSqlInterpolatedAsync($@"
                UPDATE quota_usage_periods
                SET consumed_amount = GREATEST(consumed_amount + {delta}, 0),
                    updated_at = {now}
                WHERE id = {periodId}");

        /// <summary>
        /// Reads the balance left on a period
        /// </summary>
        private async Task<long> GetRemainingAsync(long periodId)
        {
            var period = await _db.QuotaUsagePeriods
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == periodId);

            return period is null ? 0 : Math.Max(period.AllocatedAmount - period.ConsumedAmount, 0);
        }

        /// <summary>
        /// Appends one row to the audit trail
        /// </summary>
        private async Task WriteLogAsync(string userId, string resourceTypeCode, long? usagePeriodId, string reservationId, QuotaUsageAction action, long amount, string? reference, DateTime now)
        {
            _db.QuotaUsageLogs.Add(new QuotaUsageLog
            {
                UserId = userId,
                ResourceTypeCode = resourceTypeCode,
                UsagePeriodId = usagePeriodId,
                ReservationId = reservationId,
                Action = action,
                Amount = amount,
                Reference = reference,
                CreatedAt = now,
                UpdatedAt = now
            });

            await _db.SaveChangesAsync();
        }
    }
}
