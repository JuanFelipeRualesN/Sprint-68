using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace UsersAPI.UsersAPI.Services.Accounts.CustomClasses
{
    /// <summary>
    /// To handle details related to token, such as JWT.
    /// </summary>
    public class TokenManager
    {
        private readonly string _secretKey, _issuer;
        private readonly int _durationValidInHours;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="secretKey">The secret key. </param>
        /// <param name="issuer">The issuer (Ex. https://www.genialskills...").</param>
        /// <param name="durationValidInHours">The duration in hours of the access token.</param>
        public TokenManager(string secretKey, string issuer, int durationValidInHours)
        {
            this._secretKey = secretKey;
            this._issuer = issuer;
            this._durationValidInHours = durationValidInHours;
        }
        
        /// <summary>
        /// To generate the JWT token for the user's id.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <returns>Returns the generated token.</returns>
        public string GenerateToken(string userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, userId)
        };

            var token = new JwtSecurityToken(
                issuer: this._issuer,
                audience: this._issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(this._durationValidInHours * 60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// To validate the JWT Token.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <returns>Returns the claim principal related to the token (if any).</returns>
        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = this._issuer,
                    ValidateAudience = true,
                    ValidAudience = this._issuer,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                return principal;
            }
            catch(Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns an alphanumeric string. Note: This token is for simple tokens. Do not use this for scurity.
        /// </summary>
        /// <param name="maxSize">The max size for the token.</param>
        /// <returns>Returns the token as the string.</returns>
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
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}
