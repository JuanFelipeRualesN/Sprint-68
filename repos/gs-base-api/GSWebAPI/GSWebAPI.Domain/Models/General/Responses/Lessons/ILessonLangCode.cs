using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.Lessons
{
    /// <summary>
    /// To implement lesson lang code as required.
    /// </summary>
    public interface ILessonLangCode
    {
        /// <summary>
        /// Gets or sets the lesson language code.
        /// </summary>
        string LessonLangCode { get; set; }
    }
}
