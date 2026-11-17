using GSWebAPI.GSWebAPI.Domain.Models.Lesson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.Lessons
{
    /// <summary>
    /// To handle the examples.
    /// </summary>
    public class LessonExampleRsponseModel : ILessonLangCode
    {
        /// <summary>
        /// Gets or sets The lesson language code.
        /// </summary>
        public string LessonLangCode { get; set; }

        /// <summary>
        /// Gets or sets the example list.
        /// </summary>
        public List<LessonExampleModel> LessonExampleModelList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the examples is a link or not.
        /// </summary>
        public bool IsExampleALink { get; set; }
    }
}