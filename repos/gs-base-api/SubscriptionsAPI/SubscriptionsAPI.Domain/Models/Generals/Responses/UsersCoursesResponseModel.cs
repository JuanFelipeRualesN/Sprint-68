using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// User courses response. Should be removed and use only the list. It used to inherits from GeneralResponseModel, which is not needed anymore.
    /// </summary>
    public class UserCoursesResponseModel
    {
        /// <summary>
        /// Gets or sets the type of subscription. H for Homeschooler, S for School, T for Tutoring.
        /// </summary>
        public string SubscriptionType { get; set; }
        /// <summary>
        /// Gets or sets the user subscribed levels subjects.
        /// </summary>
        public List<SubsSubjectLevelCodesResponseModel> UserSubscribedLevelsSubjectsModelList { get; set; }
    }

    /// <summary>
    /// To handle the user subject and levels.
    /// </summary>
    public class SubsSubjectLevelCodesResponseModel : SubscriptionProductModel
    {
        /// <summary>
        /// Gets or sets the name of the subject and grade.
        /// </summary>
        public string Name { get; set; }
    }
}