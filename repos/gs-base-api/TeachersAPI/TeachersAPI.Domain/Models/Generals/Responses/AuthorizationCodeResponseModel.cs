using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Details of authorization codes.
    /// </summary>
    public class AuthorizationCodeResponseModel : QuizTriesLeftModel
    {
        //  Remove after Unity updates to version 2 calls.
        public AuthorizationCodeResponseModel()
        {
            SessionValid = true; //  Remove after Unity updates to version 2 calls.
            TransactionValid = true;  // Remove after Unity updates to version 2 calls.
        }
        /// <summary>
        /// Gets or sets the authorization code.
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// Gets or sets the quiz attachments urls.
        /// </summary>
        public List<string> QuizAttachmentsURLs { get; set; }

        /// <summary>
        /// Gets or sets the session valid for Unity APP version 1. Remove after Unity updates to version 2 calls.
        /// </summary>
        public bool SessionValid { get; set; }

        /// <summary>
        /// Gets or sets the transaction valid for Unity APP version 1. Remove after Unity updates to version 2 calls.
        /// </summary>
        public bool TransactionValid { get; set; }
    }
}