using Microsoft.EntityFrameworkCore;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data
{
    /// <summary>Provider settings shared by the runtime registration, the design-time factory and the tests.</summary>
    public static class GSWebDbContextOptions
    {
        /// <summary>Production server version (Azure Database for MySQL 8.0.46). Explicit on purpose: no AutoDetect.</summary>
        public static readonly MySqlServerVersion ServerVersion = new(new Version(8, 0, 46));

        /// <summary>
        /// Configures Pomelo MySQL with the fixed <see cref="ServerVersion"/>, this assembly for migrations and
        /// retries on transient failures (3 retries, up to 5 seconds apart). Because of the retries, explicit
        /// transactions must run inside <c>Database.CreateExecutionStrategy().ExecuteAsync(...)</c>.
        /// </summary>
        /// <param name="builder">The options builder to configure.</param>
        /// <param name="connectionString">MySQL connection string.</param>
        /// <returns>The same <paramref name="builder"/>.</returns>
        public static DbContextOptionsBuilder Configure(DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseMySql(connectionString, ServerVersion, mySql =>
            {
                mySql.MigrationsAssembly(typeof(GSWebDbContext).Assembly.GetName().Name);

                // Transient-fault retries. Every explicit transaction must therefore run inside
                // Database.CreateExecutionStrategy().ExecuteAsync(...).
                mySql.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
            });
        }
    }
}
