using System;
using System.Linq;
using BCrypt.Net;

namespace UsersAPI.UsersAPI.Services.Accounts.CustomClasses
{
    /// <summary>
    /// This manages everything related to passwords.
    /// It uses BCrypt because it is a secure and widely accepted standard.
    /// </summary>
    public static class PasswordManager
    {
        /// <summary>
        /// Verifies if the password is too short.
        /// </summary>
        /// <param name="password">The password as a normal string.</param>
        /// <returns>Returns true if too short, else false.</returns>
        public static bool IsPasswordTooShort(string password)
        {
            return password.Length < 5;
        }

        /// <summary>
        /// Verifies if password contains an upper, lower and a digit.
        /// </summary>
        /// <param name="password">The password as a normal string.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool DoesPasswordContainsUpperLowerAndDigit(string password)
        {
            bool valid = true;

            if (!password.Any(char.IsUpper))
                valid = false;

            if (!password.Any(char.IsLower))
                valid = false;

            if (!password.Any(char.IsDigit))
                valid = false;

            return valid;
        }

        /// <summary>
        /// Verify if password and the password retype are equal (as normal strings).
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="passwordRetype">The password retype.</param>
        /// <returns>Returns a boolean.</returns>
        public static bool IsPasswordAndRetypeEqual(string password, string passwordRetype)
        {
            return String.Equals(password, passwordRetype);
        }

        /// <summary>
        /// Check if passwords match.
        /// </summary>
        /// <param name="dbPassword">The DB (hashed) password.</param>
        /// <param name="userPassword">The user password (plain text).</param>
        /// <returns>Return true or false if the password matches.</returns>
        public static bool PasswordMatch(string dbPassword, string userPassword)
        {
            return BCrypt.Net.BCrypt.Verify(userPassword, dbPassword);
        }

        /// <summary>
        /// Returns the hashed password.
        /// </summary>
        /// <param name="password">The user password to be hashed.</param>
        /// <returns>Returns the hashed password.</returns>
        public static string HashedPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt());
        }
    }
}
