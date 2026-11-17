using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle general responses that include the Messages' details. 
    /// </summary>
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
        /// Gets or sets a value indicating the messages of the transaction.
        /// </summary>
        public List<string> Messages { get; set; }
    }
}