using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Handles the forum response.
    /// </summary>
    public class ForumsResponseModel
    {
        /// <summary>
        /// Set the forum details
        /// </summary>
        public ForumsResponseModel()
        {
            ForumModelList = new List<ForumModel>();
        }

        /// <summary>
        /// Gets or sets the forum model list.
        /// </summary>
        public List<ForumModel> ForumModelList { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        public int TotalItems { get; set; }
    }

    /// <summary>
    /// To handle the response with the subject name.
    /// </summary>
    public class ForumModel : Teachers.ForumModel
    {
        /// <summary>
        /// Gets or sets the subject code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// Gets or sets the level code.
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// Gets or sets the author name. 
        /// </summary>
        public string AuthorName { get; set; }


    }
}