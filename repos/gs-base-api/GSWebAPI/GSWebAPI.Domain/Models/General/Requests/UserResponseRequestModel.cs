using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Requests
{
    /// <summary>
    /// To handle User submitted quiz.
    /// </summary>
    public class UserResponseRequestModel : GeneralRequestModel
    {
        /// <summary>
        /// Gets or sets the user response model (with all user response details).
        /// </summary>
        public List<UserResponseModel> UserResponseModelList { get; set; }
    }
}