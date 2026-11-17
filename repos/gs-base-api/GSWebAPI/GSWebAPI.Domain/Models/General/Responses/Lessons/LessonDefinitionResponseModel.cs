using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.Lessons
{
    /// <summary>
    /// To handle the lesson definition.
    /// </summary>
    public class LessonDefinitionResponseModel : ILessonLangCode
    {
        /// <summary>
        /// Gets or sets The lesson language code.
        /// </summary>
        public string LessonLangCode { get; set; }

        /// <summary>
        /// Gets or sets the definition list.
        /// </summary>
        public List<LessonDefinitionModel> LessonDefinitionModelList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the definition is a link or not.
        /// </summary>
        public bool IsDefinitionALink { get; set; }
    }
}