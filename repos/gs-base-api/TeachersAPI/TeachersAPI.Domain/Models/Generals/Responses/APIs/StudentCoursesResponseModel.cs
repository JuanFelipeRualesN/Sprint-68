using TeachersAPI.Domain.Models.Generals.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Generals.Responses.APIs
{
    public class StudentCoursesResponseModel : ResponseModel
    {
        public List<StudentCourseModel> UserSubscribedLevelsSubjectsModelList { get; set; }
    }

    public class StudentCourseModel
    {
        /// <summary>
        /// Gets or sets the name of student courses.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the product id.
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the modified by.
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        public string UpdatedAt { get; set; }
    }
}