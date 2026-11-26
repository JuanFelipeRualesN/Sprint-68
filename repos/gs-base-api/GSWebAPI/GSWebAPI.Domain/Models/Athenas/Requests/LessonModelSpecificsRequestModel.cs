using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSWebAPI.GSWebAPI.Domain.Models.Athenas.Requests
{
    /// <summary>
    /// JSON to ask lesson model specifics.
    /// </summary>
    public class LessonModelSpecificsRequestModel
    {
        /// <summary>
        /// Gets or sets the lesson models. 
        /// </summary>
        public List<LessonModel> LessonModelList { get; set; }
    }
}
