using UsersAPI.UsersAPI.Domain.Models.Accounts;
using UsersAPI.UsersAPI.Infrastructure.Repositories.Accounts;

namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To handle the user names validation.
    /// </summary>
    public class UsernameValidationManager
    {

        private readonly string _connectionStringRead, _langCode; 

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="connectionStringRead">The connection string to read.</param>
        /// <param name="langCode">The language's code.</param>
        public UsernameValidationManager(string connectionStringRead, string langCode)
        {
            this._connectionStringRead = connectionStringRead;
            this._langCode = langCode; 
        }

        /// <summary>
        /// Verifies if username is available.
        /// </summary>
        /// <param name="username">Username to check.</param>
        /// <returns>true if user name is available, else false.</returns>
        public bool IsUsernameAvailable(string username)
        {
            return new UserAccountDAO().Read(this._connectionStringRead, new UserAccountModel
            {
                Username = username
            }, true, true).Count == 0;
        }

    }
}
