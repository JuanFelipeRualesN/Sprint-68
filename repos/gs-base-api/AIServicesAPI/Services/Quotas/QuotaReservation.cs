namespace GSAIServicesAPI.Services.Quotas
{
    /// <summary>
    /// Outcome of asking the quota system for permission to run an AI operation.
    /// <br/><br/>
    /// When Allowed is true the balance has already been debited by
    /// ReservedAmount, so the caller must finish by calling
    /// CommitAsync with the real consumption or ReleaseAsync if the operation never ran.
    /// </summary>
    public class QuotaReservation
    {
        /// <summary>
        /// Whether the operation may proceed
        /// </summary>
        public bool Allowed { get; init; }

        /// <summary>
        /// False when no quota governs this call (quotas switched off, resource not limited)
        /// The caller proceeds, release is a no-op, and commit only records the real usage.
        /// </summary>
        public bool Enforced { get; init; }

        /// <summary>
        /// Correlation id tying this reservation to its audit log rows
        /// </summary>
        public string ReservationId { get; init; } = string.Empty;

        /// <summary>
        /// Amount held up-front, settled against the real usage on commit
        /// </summary>
        public long ReservedAmount { get; init; }

        /// <summary>
        /// Amount the caller asked to hold, before any trimming to the balance left
        /// </summary>
        public long RequestedAmount { get; init; }

        /// <summary>
        /// Balance row that was debited, when one exists
        /// </summary>
        public long? UsagePeriodId { get; init; }

        /// <summary>
        /// Resource being consumed
        /// </summary>
        public string ResourceTypeCode { get; init; } = string.Empty;

        /// <summary>
        /// User the balance belongs to
        /// </summary>
        public string UserId { get; init; } = string.Empty;

        /// <summary>
        /// Module or endpoint the consumption is attributed to
        /// </summary>
        public string? Reference { get; init; }

        /// <summary>
        /// Balance left after the reservation, null when not enforced
        /// </summary>
        public long? RemainingAmount { get; init; }

        /// <summary>
        /// When the quota resets, null when not enforced
        /// </summary>
        public DateTime? PeriodEnd { get; init; }

        /// <summary>
        /// Message key explaining a denial, or null when allowed.
        /// </summary>
        public string? DenialMessageKey { get; init; }

        private int _settled;

        /// <summary>
        /// Claims the right to settle this reservation, returning false when it was
        /// settled already. Commit and release both move the balance, so letting either
        /// run twice would apply the adjustment twice.
        /// </summary>
        internal bool TryClaimSettlement() => Interlocked.Exchange(ref _settled, 1) == 0;

        /// <summary>
        /// Gives up a settlement claim that failed to persist, so a compensating release (or a
        /// retry) can settle the reservation instead of leaving the hold stranded. Called only
        /// when the DB work inside a claimed commit or release threw and rolled back.
        /// </summary>
        internal void ResetSettlement() => Interlocked.Exchange(ref _settled, 0);

        /// <summary>
        /// A reservation for a call that no quota restricts. It still carries a reservation
        /// id and reference so the settle step can record the consumption for auditing,
        /// even though no balance is held.
        /// </summary>
        public static QuotaReservation NotEnforced(string userId, string resourceTypeCode, string? reference) => new()
        {
            Allowed = true,
            Enforced = false,
            ReservationId = Guid.NewGuid().ToString(),
            UserId = userId,
            ResourceTypeCode = resourceTypeCode,
            Reference = reference
        };
    }
}
