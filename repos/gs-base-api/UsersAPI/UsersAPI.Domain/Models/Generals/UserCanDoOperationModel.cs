using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals
{
    public class UserCanDoOperationModel :UsersAPI.Domain.Models.Roles.Responses.UserCanDoOperationModel
    {
        /// <summary>
        /// Gets or sets the ore id.
        /// </summary>
        public string OreId { get; set; }
    }
}