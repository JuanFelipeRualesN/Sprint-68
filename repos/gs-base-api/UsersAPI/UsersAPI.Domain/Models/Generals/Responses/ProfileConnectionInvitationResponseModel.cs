using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals.Responses
{
    public class ProfileConnectionInvitationResponseModel
    {

        /// <summary>
        /// Gets or sets total Pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        public List<UsersAPI.Domain.Models.Relations.ProfileConnectionInvitationByEmailModel> ProfileConnectionInvitationByEmailModelList { get; set; }
    }
}