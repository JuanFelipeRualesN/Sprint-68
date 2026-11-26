using GSWebAPI.GSWebAPI.Infrastructure.Data;
using GSWebAPI.GSWebAPI.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Testcontainers.MySql;
using Xunit;

namespace GSWebAPI.Tests.Infrastructure
{
    /// <summary>
    /// One real MySQL 8.0.46 (same version as production) per test run. The schema is an empty copy that already
    /// contains the existing tables (Schema/existing_tables.sql); the EF migration is then applied on top of it.
    /// Tests share the database and isolate themselves by creating their own groups/plans.
    /// </summary>
    public sealed class MySqlDatabaseFixture : IAsyncLifetime
    {
        private readonly MySqlContainer _container = new MySqlBuilder()
            .WithImage(Environment.GetEnvironmentVariable("GSWEB_TEST_MYSQL_IMAGE") ?? "mysql:8.0.46")
            .WithDatabase("gs_web_api_db")
            .WithUsername("root")
            .WithPassword("test-password")
            .Build();

        public string ConnectionString { get; private set; } = null!;

        public TestTimeProvider Time { get; } = new();

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
            ConnectionString = new MySqlConnectionStringBuilder(_container.GetConnectionString())
            {
                AllowUserVariables = true,
            }.ConnectionString;

            await using (var connection = new MySqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var sql = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Schema", "existing_tables.sql"));
                await using var command = new MySqlCommand(sql, connection);
                await command.ExecuteNonQueryAsync();
            }

            await using var db = CreateContext();
            await db.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }

        public GSWebDbContext CreateContext()
        {
            var builder = new DbContextOptionsBuilder<GSWebDbContext>();
            GSWebDbContextOptions.Configure(builder, ConnectionString);
            builder.AddInterceptors(new AuditAndSoftDeleteInterceptor(Time));
            return new GSWebDbContext(builder.Options);
        }

        /// <summary>Runs raw SQL outside EF (to check MySQL behaviour such as CHECK and ENUM).</summary>
        public async Task<int> ExecuteSqlAsync(string sql)
        {
            await using var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand(sql, connection);
            return await command.ExecuteNonQueryAsync();
        }

        public async Task<object?> ScalarAsync(string sql)
        {
            await using var connection = new MySqlConnection(ConnectionString);
            await connection.OpenAsync();
            await using var command = new MySqlCommand(sql, connection);
            return await command.ExecuteScalarAsync();
        }
    }

    [CollectionDefinition(Name)]
    public sealed class MySqlCollection : ICollectionFixture<MySqlDatabaseFixture>
    {
        public const string Name = "MySQL";
    }
}
