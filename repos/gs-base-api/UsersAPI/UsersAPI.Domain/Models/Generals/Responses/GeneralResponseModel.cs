using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses
{
    public class GeneralResponseModel
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="GeneralResponseModel{T}"/> class.
        /// </summary>
        public GeneralResponseModel()
        {
            Messages = new List<string>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether transaction was valid.
        /// </summary>
        public bool TransactionValid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the messages of the transaction.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets a value for user session token.
        /// </summary>
        public string Token { get; set; }

    }
}