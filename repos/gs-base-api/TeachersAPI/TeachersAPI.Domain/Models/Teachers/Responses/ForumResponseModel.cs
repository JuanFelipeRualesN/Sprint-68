using TeachersAPI.Domain.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Teachers.Responses
{
    /// <summary>
    /// To handle forum creation.
    /// </summary>
    public class ForumResponseModel : ResponseModel
    {
        /// <summary>
        /// Gets or sets the Forum.
        /// </summary>
        public ForumModel ForumModel { get; set; }

        /// <summary>
        /// Gets or sets the forum post. 
        /// </summary>
        public ForumPostModel ForumPostModel { get; set; }
    }
}
