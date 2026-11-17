using System;
using System.Globalization;
using System.Text.RegularExpressions;


namespace UsersAPI.UsersAPI.Services.Accounts.CustomClasses
{
    /// <summary>
    /// To validate the fields.
    /// </summary>
    public class FieldValidator
    {
        bool invalidEmail = false;

        /// <summary>
        /// Validates if an email is valid or not.
        /// </summary>
        /// <param name="strIn">The email to validate.</param>
        /// <returns>Returns true if valid, else false.</returns>
        public bool IsValidEmail(string strIn)
        {
            invalidEmail = false;
            if (String.IsNullOrWhiteSpace(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalidEmail)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^([a-zA-Z0-9_\-\.ÑñÁáÉéÍíÓóÚú]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        ///  Part of the email validation function.
        /// </summary>
        /// <param name="match">Related to own function.</param>
        /// <returns>Returns the match value and the domain.</returns>
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalidEmail = true;
            }
            return match.Groups[1].Value + domainName;
        }

        /// <summary>
        /// Check if username is too short.
        /// </summary>
        /// <param name="username">The username input.</param>
        /// <returns>Return true if too short, else false.</returns>
        public bool IsUsernameTooShort(string username)
        {
            if (String.IsNullOrWhiteSpace(username) || username.Length < 5)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Validates if the username is empty.
        /// </summary>
        /// <param name="username">The username to validate.</param>
        /// <returns>Returns true if empty, else false.</returns>
        public bool IsUsernameEmpty(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verifies if password is empty.
        /// </summary>
        /// <param name="password">The password to validate.</param>
        /// <returns>Returns true if empty, else false.</returns>
        public bool IsPasswordEmpty(string password)
        {
            if (String.IsNullOrWhiteSpace(password))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verifies if email is empty.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns>Returns true if email is empty, else false.</returns>
        public bool IsEmailEmpty(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if username is too long.
        /// </summary>
        /// <param name="username">The username input.</param>
        /// <returns>Returns true if too long.</returns>
        public bool IsUserNameTooLong(string username)
        {
            if (!String.IsNullOrWhiteSpace(username) && username.Length > 50)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks if username has valid characters (underscore, numbers and characters, and now, dots).
        /// </summary>
        /// <param name="username">The username to enter.</param>
        /// <returns>Returns true if valid characters, else false.</returns>
        public bool IsUsernameValidCharacters(string username)
        {
            Regex r = new Regex("^[a-zA-Z0-9_.]*$");
            if (!r.IsMatch(username))
                return false;
            else
                return true;
        }
    }
}
