using GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Subscriptions
{
    /// <summary>
    /// To handle the subscription responses.
    /// </summary>
    public class SubscriptionAcademicFullResponseModel
    {
        /// <summary>
        /// Gets or sets the subscription's model.
        /// </summary>
        public SubscriptionModel SubscriptionModel { get; set; }

        /// <summary>
        /// Gets or sets the plan's model.
        /// </summary>
        public List<SubscriptionProductModel> SubscriptionProductModelList { get; set; }

        /// <summary>
        /// Gets or sets the item responses.
        /// </summary>
        public EntityAcademicDetailFullResponseModel EntityAcademicDetailFullResponseModel { get; set; }

        /// <summary>
        /// Gets or sets the exam tries max.
        /// </summary>
        public ExamTryModel ExamTryModel { get; set; }
    }
}