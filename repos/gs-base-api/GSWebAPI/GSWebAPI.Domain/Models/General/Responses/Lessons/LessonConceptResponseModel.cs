using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.Lessons
{
    /// <summary>
    /// To handle the lesson concept response.
    /// </summary>
    public class LessonConceptResponseModel : ILessonLangCode
    {
        /// <summary>
        /// Gets or sets the lesson's language code.
        /// </summary>
        public string LessonLangCode { get; set; }

        /// <summary>
        /// Gets or sets the concept data.
        /// </summary>
        public string Concept { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the lesson is a classic format or not.
        /// </summary>
        /// [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool ClassicFormat { get; set; }
    }
}