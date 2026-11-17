using System;
using System.Security.Cryptography;
using System.Text;

namespace TeachersAPI.TeachersAPI.Services.UserQuizzes.CustomClasses
{
    /// <summary>
    /// Used to generate unique tokens.
    /// </summary>
    public static class TokenGenerator
    {

        /// <summary>
        /// Returns an alphanumeric string with security standards.
        /// </summary>
        /// <param name="maxSize">Max length of the token.</param>
        /// <returns>Returns the generated token.</returns>
        public static string GenerateAlphanumericToken(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }

            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % chars.Length]);
            }

            return result.ToString();
        }
    }
}