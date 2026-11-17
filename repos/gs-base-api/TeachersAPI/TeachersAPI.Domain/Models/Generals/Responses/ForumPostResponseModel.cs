using TeachersAPI.Domain.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle post responses.
    /// </summary>
    public class ForumPostResponseModel
    {
        /// <summary>
        /// Gets or sets the forum post model.
        /// </summary>
        public ForumPostModel ForumPostModel { get; set; }

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
    }
}