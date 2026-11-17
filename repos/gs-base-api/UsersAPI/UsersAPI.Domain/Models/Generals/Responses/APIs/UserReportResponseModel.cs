using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UsersAPI.UsersAPI.Domain.Models.Accounts;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses.APIs
{
    public class UserReportResponseModel
    {
        /// <summary>
        /// Gets or sets the user id list.
        /// </summary>
        public List<string> UserIdList { get; set; }

        /// <summary>
        /// Gets or sets the user account.
        /// </summary>
        public List<UsersAPI.Domain.Models.Accounts.UserAccountModel> UserAccountModelList { get; set; }

        /// <summary>
        /// Gets or sets the user full names.
        /// </summary>
        public List<UsersAPI.Domain.Models.PersonalDetails.UserFullNameModel> UserFullNameModelList { get; set; }

        /// <summary>
        /// Gets or sets the user emails.
        /// </summary>
        public List<UserEmailModel> UserEmailModelList { get; set; }
    }
}