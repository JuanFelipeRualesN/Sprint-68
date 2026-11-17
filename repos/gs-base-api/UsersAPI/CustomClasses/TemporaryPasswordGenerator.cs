using System;
using System.Text;

namespace UsersAPI.CustomClasses
{
    /// <summary>
    /// The temporary password generator.
    /// </summary>
    public static class TemporaryPasswordGenerator
    {
        /// <summary>
        /// Creates a random string to be used as temporary password.
        /// </summary>
        /// <param name="length">Max Length.</param>
        /// <returns>Returns a random string.</returns>
        public static string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }

            return res.ToString();
        }
    }
}