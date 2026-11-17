using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Codes
{
    public class ExpectationCodeModel
    {
        /// <summary>
        /// The expectation code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The expectation description
        /// </summary>
        public string CodeId { get; set; }
    }
}