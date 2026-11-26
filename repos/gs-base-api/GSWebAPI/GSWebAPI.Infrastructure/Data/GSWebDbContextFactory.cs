using GSWebAPI.GSWebAPI.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GSWebAPI.GSWebAPI.Infrastructure.Data
{
    /// <summary>
    /// Design-time factory for <c>dotnet ef</c>. Adding migrations and generating scripts does not connect to the
    /// server (the server version is explicit). To run <c>dotnet ef database update</c>, set the environment variable
    /// GSWEB_EF_CONNECTION; there is no hard-coded connection string.
    /// </summary>
    public class GSWebDbContextFactory : IDesignTimeDbContextFactory<GSWebDbContext>
    {
        /// <summary>
        /// Creates a context for the EF tools, with the connection string from GSWEB_EF_CONNECTION (falling back to a
        /// local placeholder without credentials) and the audit/soft-delete interceptor on the system clock.
        /// </summary>
        /// <param name="args">Arguments passed by the EF tools; not used.</param>
        /// <returns>A new <see cref="GSWebDbContext"/>.</returns>
        public GSWebDbContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("GSWEB_EF_CONNECTION")
                ?? "Server=localhost;Database=gs_web_api_db";

            var builder = new DbContextOptionsBuilder<GSWebDbContext>();
            GSWebDbContextOptions.Configure(builder, connectionString);
            builder.AddInterceptors(new AuditAndSoftDeleteInterceptor(TimeProvider.System));
            return new GSWebDbContext(builder.Options);
        }
    }
}
