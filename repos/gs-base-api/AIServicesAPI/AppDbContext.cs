using GSAIServicesAPI.Models;
using GSAIServicesAPI.Models.Quotas;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<TeacherDocument> TeacherDocuments => Set<TeacherDocument>();

    public DbSet<AiTutorSetting> AiTutorSettings => Set<AiTutorSetting>();

    public DbSet<QuotaResourceType> QuotaResourceTypes => Set<QuotaResourceType>();

    public DbSet<QuotaSetting> QuotaSettings => Set<QuotaSetting>();

    public DbSet<QuotaResourceLimit> QuotaResourceLimits => Set<QuotaResourceLimit>();

    public DbSet<QuotaUsagePeriod> QuotaUsagePeriods => Set<QuotaUsagePeriod>();

    public DbSet<QuotaUsageLog> QuotaUsageLogs => Set<QuotaUsageLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Store the quota enums as their string names so the DB stays self descriptive
        modelBuilder.Entity<QuotaResourceLimit>(entity =>
        {
            entity.Property(c => c.Scope).HasConversion<string>();
            entity.Property(c => c.PeriodType).HasConversion<string>();

            // At most one limit per (resource, scope, target).
            // ScopeId is empty rather than null on purpose because
            // MySQL counts nulls as distinct, so duplicate global limits would slip past the index
            entity.HasIndex(c => new { c.ResourceTypeCode, c.Scope, c.ScopeId })
                  .IsUnique();
        });

        modelBuilder.Entity<QuotaUsagePeriod>(entity =>
        {
            // Serves the per-request lookup, and its uniqueness makes the lazy "insert the period if missing" safe:
            // concurrent inserts collide here instead of creating two balances for the same period.
            // LimitId is part of it because a user holds one balance per limit
            entity.HasIndex(p => new { p.UserId, p.ResourceTypeCode, p.PeriodStart, p.LimitId })
                  .IsUnique();
        });

        modelBuilder.Entity<QuotaUsageLog>(entity =>
        {
            entity.Property(l => l.Action).HasConversion<string>();

            // Supports the per-user consumption
            entity.HasIndex(l => new { l.UserId, l.ResourceTypeCode, l.CreatedAt });
        });

        // Seed the current resource types. New resources are added as extra rows here
        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<QuotaResourceType>().HasData(
            new QuotaResourceType
            {
                Code = QuotaResourceType.TextTokens,
                Name = "Text generation (tokens)",
                Unit = "tokens",
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            },
            new QuotaResourceType
            {
                Code = QuotaResourceType.ImageCount,
                Name = "Image generation",
                Unit = "images",
                IsActive = true,
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            });

        // Seed the single settings row
        modelBuilder.Entity<QuotaSetting>().HasData(
            new QuotaSetting
            {
                Id = QuotaSetting.SingletonId,
                QuotasEnabled = false,
                AiEnabled = true,
                DefaultScope = QuotaScope.Global.ToString(),
                CreatedAt = seedDate,
                UpdatedAt = seedDate
            });
    }
}