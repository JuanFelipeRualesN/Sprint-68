using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Lessons.Response
{
    public class QuizResponseModel : Request.QuizRequestModel
    {
        /// <summary>
        /// If was valid or not.
        /// </summary>
        public bool Valid { get; set; }
        /// <summary>
        /// Messages list
        /// </summary>
        public List<string> Messages { get; set; }
    }
}
