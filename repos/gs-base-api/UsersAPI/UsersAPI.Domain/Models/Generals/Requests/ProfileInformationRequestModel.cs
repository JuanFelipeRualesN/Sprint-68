using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Requests
{
    public class ProfileInformationRequestModel : RegisterRequestModel
    {
        /// <summary>
        /// Gets or sets the profile Id.
        /// </summary>
        public string ProfileId { get; set; }
    }
}