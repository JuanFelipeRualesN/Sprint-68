using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace GSWebAPI.GSWebAPI.Services.Common
{
    internal static class DbUpdateExceptionExtensions
    {
        /// <summary>True if the insert/update failed on a UNIQUE index (MySQL error 1062).</summary>
        public static bool IsUniqueViolation(this DbUpdateException exception)
        {
            return exception.InnerException is MySqlException { ErrorCode: MySqlErrorCode.DuplicateKeyEntry };
        }
    }
}
