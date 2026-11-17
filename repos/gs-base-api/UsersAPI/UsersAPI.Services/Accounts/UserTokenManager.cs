using System;

namespace UsersAPI.UsersAPI.Services.Accounts
{
    /// <summary>
    /// To handle the user's token. This is not the access token. 
	/// This is a token that functions like a friend's code, so the teacher can add the students, the adults can send 
	/// "friend" requests and so on.
    /// </summary>
    public class UserTokenManager
    {
		private readonly Random _random = new Random();

		/// <summary>
		/// Constructor.
		/// </summary>
		public UserTokenManager()
        {
			this._random = new Random(); 	
        }

		/// <summary>
		/// Generates a Token (connection code/"friend" code).
		/// </summary>
		/// <returns>Returns the generated token.</returns>
        public string GenerateTokenManager()
        {
			// Must be unique, so we will work with the date and some random letters
			// No 0s or Os or Ls or Is or Zs to avoid confusion between characters.
			// "A" will be used to replace 0s
			const string chars = "BCDEFGHJKMNPQRSTUVWXY";
			var stringChars = new char[5];
			string randomStr = "";
			for (int i = 0; i < stringChars.Length; i++)
			{
				randomStr += chars[this._random.Next(chars.Length)];
			}

			DateTime dateTimeNow = DateTime.Now;
			string dateStr = dateTimeNow.ToString("ddMMyyHHss");
			dateStr = dateStr.Replace("0", "A");

			string returnString = "";
			for (int i = 0; i < dateStr.Length; i++)
			{
				if (i == 0)
				{
					returnString += randomStr[0];
				}
				else if (i == 2)
				{
					returnString += randomStr[2];
				}
				else if (i == 5)
				{
					returnString += randomStr[3];
				}
				else
				{
					returnString += dateStr[i];
				}
			}
			return returnString;
		}
    }
}
