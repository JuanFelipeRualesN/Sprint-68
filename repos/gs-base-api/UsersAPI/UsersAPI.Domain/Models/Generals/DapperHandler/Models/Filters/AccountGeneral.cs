using System.Collections.Generic;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.DapperHandler.Models.Filters
{
    public class AccountGeneral
    {
        /// <summary>
        /// Gets or sets User Ids to call.
        /// </summary>
        public List<string> UserIds { get; set; }

        /// <summary>
        /// Gets or sets whether is a staff making the request.
        /// </summary>
        public bool IsStaff { get; set; }

        /// <summary>
        /// Gets or sets the Ore Id.
        /// </summary>
        public string OreId { get; set; }

        /// <summary>
        /// Gets or sets whether is the principal role or not.
        /// </summary>
        public bool IsDirector { get; set; }

        /// <summary>
        /// Gets or sets whether is a client account that is making the request.
        /// </summary>
        public bool IsClientAccount { get; set; }

        /// <summary>
        ///  Get or sets when student homeschool account is making the request
        /// </summary>
        public bool IsStudentHomeschool { get; set; }

        /// <summary>
        /// Gets or sets whether the same school rule required.
        /// </summary>
        public bool SameSchoolRequired { get; set; }

        /// <summary>
        /// Gets or sets the school id to filter.
        /// </summary>
        public List<string> SchoolIds { get; set; }

        /// <summary>
        /// Gets or sets the username to filter.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the first name to filter.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name to filter.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's role.
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Gets or sets the user's school record to filter.
        /// </summary>
        public string UserSchoolRecord { get; set; }

        /// <summary>
        /// Gets or sets the user making the request.
        /// </summary>
        public string UserAlphaId { get; set; }
    }
}