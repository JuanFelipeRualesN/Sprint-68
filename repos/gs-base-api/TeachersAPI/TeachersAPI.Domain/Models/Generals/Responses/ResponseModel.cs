using System;
using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class ResponseModel
    {
        /// <summary>
        /// Gets or sets whether transaction is valid or not.
        /// </summary>
        public bool TransactionValid { get; set; }

        /// <summary>
        /// Gets or sets whether session is valid or not.
        /// </summary>
        public bool SessionValid { get; set; }

        /// <summary>
        /// Gets or sets messages.
        /// </summary>
        public List<string> Messages { get; set; }

    }
}