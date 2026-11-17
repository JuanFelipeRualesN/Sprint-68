using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachersAPI.Domain.Models.Generals.Responses;
using TeachersAPI.Domain.Models.Teachers;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the forum response.
    /// </summary>
    public class ForumResponseModel : ResponseModel
    {
        /// <summary>
        /// init the following
        /// </summary>
        public ForumResponseModel()
        {
            ForumPostModels = new List<ForumPostResponseModel>();
        }

        /// <summary>
        /// Gets or sets the forum model.
        /// </summary>
        public Teachers.ForumModel ForumModel { get; set; }

        /// <summary>
        /// Gets or sets the forum posts.
        /// </summary>
        public List<ForumPostResponseModel> ForumPostModels { get; set; }
        
        /// <summary>
        /// Gets or sets the participant groups.
        /// </summary>
        public List<TeacherGroupModel> TeacherGroupModels { get; set; }

        /// <summary>
        /// Gets or sets the author name.
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// Gets or sets whether the user can archive the forum or not.
        /// </summary>
        public bool ShowArchive { get; set; }

        /// <summary>
        /// Gets or sets whether the user can edit the forum or not.
        /// </summary>
        public bool ShowEdit { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the total items.
        /// </summary>
        public int TotalItems { get; set; }
    }
}