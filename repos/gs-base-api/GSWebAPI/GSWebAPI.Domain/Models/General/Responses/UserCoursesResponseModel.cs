using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses
{
    /// <summary>
    /// User courses response. Should be removed and use only the list. It used to inherits from GeneralResponseModel, which is not needed anymore.
    /// </summary>
    public class UserCoursesResponseModel
    {
        /// <summary>
        /// Gets or sets the user subscribed levels subjects.
        /// </summary>
        public List<SubsSubjectLevelCodesResponseModel> UserSubscribedLevelsSubjectsModelList { get; set; }
    }
}