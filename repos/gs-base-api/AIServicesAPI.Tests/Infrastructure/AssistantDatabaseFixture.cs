using GSAIServicesAPI.Models.Quotas;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Testcontainers.MySql;
using Xunit;

namespace AIServicesAPI.Tests.Infrastructure
{
    /// <summary>
    /// A real MySQL 8 with the AI services schema (EF migrations) for the quota, plus a fake UsersAPI that validates the session.
    /// The database comes from Testcontainers; when Docker is not available, set AI_SERVICES_TEST_CONNECTION_STRING to the connection string
    /// of an empty local MySQL 8 database and the tests use it.
    /// A token is "u{userId}-r{roleId}" (for example u2001-r2): the fake UsersAPI answers a valid session for that user and role.
    /// </summary>
    public sealed class AssistantDatabaseFixture : IAsyncLifetime
    {
        public const string ConnectionStringVariable = "AI_SERVICES_TEST_CONNECTION_STRING";

        private MySqlContainer? _container;
        private WebApplication? _fakeUsersApi;
        private int _nextUserId = Random.Shared.Next(1_000_000, 900_000_000);   // a long-lived test database keeps the limits of earlier runs

        public string ConnectionString { get; private set; } = string.Empty;

        public IConfigurationRoot Configuration { get; private set; } = null!;

        public async Task InitializeAsync()
        {
            ConnectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                _container = new MySqlBuilder().WithImage(Environment.GetEnvironmentVariable("AI_SERVICES_TEST_MYSQL_IMAGE") ?? "mysql:8.0.46")
                    .WithDatabase("gs_ai_services").WithUsername("root").WithPassword("test-password").Build();
                await _container.StartAsync();
                ConnectionString = _container.GetConnectionString();
            }

            await using (var db = CreateContext())
            {
                await db.Database.MigrateAsync();
                var settings = await db.QuotaSettings.FirstOrDefaultAsync(setting => setting.Id == QuotaSetting.SingletonId);
                if (settings is null)
                {
                    db.QuotaSettings.Add(new QuotaSetting { Id = QuotaSetting.SingletonId, QuotasEnabled = true, AiEnabled = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }
                else
                {
                    settings.QuotasEnabled = true;
                    settings.AiEnabled = true;
                }

                if (!await db.QuotaResourceTypes.AnyAsync(type => type.Code == QuotaResourceType.TextTokens))
                {
                    db.QuotaResourceTypes.Add(new QuotaResourceType { Code = QuotaResourceType.TextTokens, Name = "Text tokens", Unit = "tokens", IsActive = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
                }

                await db.SaveChangesAsync();
            }

            await StartFakeUsersApiAsync();
            Configuration = (IConfigurationRoot)TestConfiguration.Build(new Dictionary<string, string?>
            {
                ["AppSettings:gus_endpoint_web_service_url"] = UsersApiUrl,
                ["AppSettings:gus_endpoint_permission_session_api"] = "usersapi/accounts/get_user_by_token",
                ["AppSettings:development_mode_session_off"] = "0",
                ["AppSettings:data_assistant_allowed_role_ids"] = "2,8,6",
            });
            GSAIServicesAPI.AppConfiguration.Initialize(Configuration);
        }

        public string UsersApiUrl { get; private set; } = string.Empty;

        public async Task DisposeAsync()
        {
            if (_fakeUsersApi is not null)
            {
                await _fakeUsersApi.StopAsync();
                await _fakeUsersApi.DisposeAsync();
            }

            if (_container is not null)
            {
                await _container.DisposeAsync();
            }
        }

        public AppDbContext CreateContext() =>
            new(new DbContextOptionsBuilder<AppDbContext>().UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)).Options);

        /// <summary>
        /// Gives a user id nobody else uses, so every test has its own limit and balance.
        /// </summary>
        public string NewUserId() => Interlocked.Increment(ref _nextUserId).ToString();

        /// <summary>
        /// Creates a limit of text tokens for a user. The reservation is partial, so a user is only denied when the balance is spent.
        /// </summary>
        public async Task SetUserLimitAsync(string userId, long maxAmount)
        {
            await using var db = CreateContext();
            db.QuotaResourceLimits.Add(new QuotaResourceLimit
            {
                ResourceTypeCode = QuotaResourceType.TextTokens,
                Scope = QuotaScope.User,
                ScopeId = userId,
                Enabled = true,
                MaxAmount = maxAmount,
                PeriodType = PeriodType.Monthly,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            });
            await db.SaveChangesAsync();
        }

        public async Task<long> GetConsumedAsync(string userId)
        {
            await using var db = CreateContext();
            return await db.QuotaUsagePeriods.Where(period => period.UserId == userId).SumAsync(period => period.ConsumedAmount);
        }

        public async Task SetAiEnabledAsync(bool enabled)
        {
            await using var db = CreateContext();
            var settings = await db.QuotaSettings.SingleAsync(setting => setting.Id == QuotaSetting.SingletonId);
            settings.AiEnabled = enabled;
            await db.SaveChangesAsync();
        }

        private async Task StartFakeUsersApiAsync()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://127.0.0.1:0");
            builder.Logging.ClearProviders();
            _fakeUsersApi = builder.Build();
            _fakeUsersApi.MapPost("/usersapi/accounts/get_user_by_token", (HttpRequest request) =>
            {
                var token = request.Headers["Token"].ToString();
                var match = System.Text.RegularExpressions.Regex.Match(token, @"^u(\d+)-r(\d+)$");
                return match.Success
                    ? Results.Json(new { SessionValid = true, UserId = match.Groups[1].Value, RoleId = match.Groups[2].Value, Token = token })
                    : Results.Json(new { SessionValid = false });
            });
            await _fakeUsersApi.StartAsync();
            UsersApiUrl = _fakeUsersApi.Urls.First().TrimEnd('/') + "/";
        }
    }

    [CollectionDefinition(Name)]
    public sealed class AssistantDatabaseCollection : ICollectionFixture<AssistantDatabaseFixture>
    {
        public const string Name = "AssistantDatabase";
    }
}
