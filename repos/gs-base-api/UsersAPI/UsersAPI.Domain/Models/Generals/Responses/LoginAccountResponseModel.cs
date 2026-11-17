using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses
{
    public class LoginAccountResponseModel
    {
        public LoginAccountResponseModel()
        {
            Messages = new List<string>();
        }
        /// <summary>
        /// Gets or sets a value indicating whether the user has profiles or not.
        /// </summary>
        public bool HasProfiles { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if user has subscriptions (Flag asked by the person who developed the student app).
        /// </summary>
        public bool HasSubscriptions { get; set; }

        /// <summary>
        /// Gets or sets invitatin code.
        /// </summary>
        public string InvitationCode { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets whether the subscription is a main account or not.
        /// </summary>
        public bool IsSubscriptionMainAccount { get; set; }

        /// <summary>
        /// Gets or sets the roles of the user.
        /// </summary>
        public List<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets the messages list.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets whether the user is a student homeschool or not.
        /// </summary>
        public bool IsStudentHomeschool { get; set; }

        /// <summary>
        /// Gets or sets the recent subscription id if the user bought a subscription in the process.
        /// </summary>
        public string RecentSubscriptionId { get; set; }

    }
}