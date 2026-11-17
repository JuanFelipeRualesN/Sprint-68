using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachersAPI.Domain.Models.Lessons;

namespace TeachersAPI.Domain.Models.Codes
{
    public class ConstructedCodeCodeModel : IResponse
    {
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }
        public string Id { get; set; }
        public string LessonConstructedCodeId { get; set; }
        public string CodeId { get; set; }
        public string DotAfter { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        // Extended for extended queries
        /// <summary>
        /// Gets or sets the code text.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the code descriptions.
        /// </summary>
        public string CodeDescription { get; set; }

        /// <summary>
        /// Gets or sets the code type.
        /// </summary>
        public string CodeTypeName { get; set; }

        /// <summary>
        /// Gets or sets the code type id.
        /// </summary>
        public string CodeTypeId { get; set; }

    }
}