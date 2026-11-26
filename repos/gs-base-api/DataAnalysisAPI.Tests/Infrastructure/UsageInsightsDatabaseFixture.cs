using DataAnalysisAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Testcontainers.MySql;
using Xunit;

namespace DataAnalysisAPI.Tests.Infrastructure
{
    /// <summary>
    /// One real MySQL 8 with the synthetic gs_data_analysis schema and data (TestData/usage-insights-schema-and-data.sql), plus a fake UsersAPI
    /// that answers the session validation. The database comes from Testcontainers; when Docker is not available, set
    /// USAGE_INSIGHTS_TEST_CONNECTION_STRING to the connection string of an empty local MySQL 8 database and the tests use it
    /// (the tables are dropped and created by the script).
    /// </summary>
    public sealed class UsageInsightsDatabaseFixture : IAsyncLifetime
    {
        public const string ConnectionStringVariable = "USAGE_INSIGHTS_TEST_CONNECTION_STRING";

        private MySqlContainer? _container;
        private WebApplication? _fakeUsersApi;

        /// <summary>
        /// Gets the connection string of the test database.
        /// </summary>
        public string ConnectionString { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the base URL of the fake UsersAPI.
        /// </summary>
        public string UsersApiUrl { get; private set; } = string.Empty;

        /// <summary>
        /// Token sent by an administrator (role 2): sees every school.
        /// </summary>
        public const string AdminToken = "token-admin";

        /// <summary>
        /// Token of an ORE administrator (role 8, ORE 2).
        /// </summary>
        public const string OreAdminToken = "token-admin-ore";

        /// <summary>
        /// Token of a director (role 6, user 10 of school 1).
        /// </summary>
        public const string DirectorToken = "token-director";

        /// <summary>
        /// Token of a teacher (role 5): has no access to the usage insights.
        /// </summary>
        public const string TeacherToken = "token-teacher";

        /// <summary>
        /// Token of an ORE administrator without ORE: fails closed.
        /// </summary>
        public const string OreAdminWithoutOreToken = "token-admin-ore-without-ore";

        public async Task InitializeAsync()
        {
            ConnectionString = Environment.GetEnvironmentVariable(ConnectionStringVariable) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                _container = new MySqlBuilder()
                    .WithImage(Environment.GetEnvironmentVariable("USAGE_INSIGHTS_TEST_MYSQL_IMAGE") ?? "mysql:8.0.46")
                    .WithDatabase("gs_data_analysis")
                    .WithUsername("root")
                    .WithPassword("test-password")
                    .Build();
                await _container.StartAsync();
                ConnectionString = _container.GetConnectionString();
            }

            await using (var connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var sql = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "TestData", "usage-insights-schema-and-data.sql"));
                await using var command = new MySqlCommand(sql, connection);
                await command.ExecuteNonQueryAsync();
            }

            await StartFakeUsersApiAsync();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.dataanalysis.json"), optional: false)
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:AnalysisConnectionStringReader"] = ConnectionString,
                    ["AppSettings:default_language_code"] = "en",
                    ["AppSettings:gus_web_service_url"] = UsersApiUrl,
                    ["AppSettings:gus_endpoint_permission_session_api"] = "usersapi/accounts/get_user_by_token",
                    ["AppSettings:usage_insights_excluded_email_domains"] = "internal.example",
                    ["AppSettings:usage_insights_excluded_schools"] = "Facilitators School",
                })
                .Build();
            AppConfiguration.Initialize(configuration);
        }

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

        /// <summary>
        /// Starts a tiny HTTP server that plays UsersAPI: it answers the session validation for the known test tokens.
        /// </summary>
        private async Task StartFakeUsersApiAsync()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseUrls("http://127.0.0.1:0");
            builder.Logging.ClearProviders();
            _fakeUsersApi = builder.Build();
            _fakeUsersApi.MapPost("/usersapi/accounts/get_user_by_token", (HttpRequest request) =>
            {
                var token = request.Headers["Token"].ToString();
                return token switch
                {
                    AdminToken => Results.Json(new { SessionValid = true, UserId = "1", RoleId = "2", OreId = (string?)null }),
                    OreAdminToken => Results.Json(new { SessionValid = true, UserId = "2", RoleId = "8", OreId = "2" }),
                    OreAdminWithoutOreToken => Results.Json(new { SessionValid = true, UserId = "3", RoleId = "8", OreId = (string?)null }),
                    DirectorToken => Results.Json(new { SessionValid = true, UserId = "10", RoleId = "6", OreId = (string?)null }),
                    TeacherToken => Results.Json(new { SessionValid = true, UserId = "11", RoleId = "5", OreId = (string?)null }),
                    _ => Results.Json(new { SessionValid = false }),
                };
            });
            await _fakeUsersApi.StartAsync();
            UsersApiUrl = _fakeUsersApi.Urls.First().TrimEnd('/') + "/";
        }
    }

    [CollectionDefinition(Name)]
    public sealed class UsageInsightsCollection : ICollectionFixture<UsageInsightsDatabaseFixture>
    {
        public const string Name = "UsageInsightsDatabase";
    }
}
