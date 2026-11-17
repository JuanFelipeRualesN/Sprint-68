using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To return the subscription academic full response.
    /// </summary>
    public class SubscriptionAcademicFullResponseModel
    {
        /// <summary>
        /// Gets or sets the subscription's model.
        /// </summary>
        public SubscriptionsAPI.Domain.Models.Subscriptions.SubscriptionModel SubscriptionModel { get; set; }

        /// <summary>
        /// Gets or sets the plan's model.
        /// </summary>
        public SubscriptionsAPI.Domain.Models.Subscriptions.PlanModel PlanModel { get; set; }

        /// <summary>
        /// Gets or sets the item-products.
        /// </summary>
        public List<SubscriptionProductModel> SubscriptionProductModelList { get; set; }

        /// <summary>
        /// Gets or sets the entity academic details.
        /// </summary>
        public SubscriptionsAPI.Domain.Models.Generals.Responses.APIs.Schools.EntityAcademicDetailFullResponseModel EntityAcademicDetailFullResponseModel { get; set; }

        /// <summary>
        /// Gets or sets the exam try max models for subscription. This is overwritter by teacher's plan.
        /// </summary>
        public ExamTryMaxModel ExamTryMaxModel { get; set; }
    }
}