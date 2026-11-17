using System.Collections.Generic;
using SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Subscriptions.Responses;

namespace SubscriptionsAPI.SubscriptionsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Manages the plan response.
    /// </summary>
    public class PlanResponseModel
    {
        /// <summary>
        /// Gets or sets plan list.
        /// </summary>
        public List<SubscriptionsAPI.Domain.Models.Subscriptions.Responses.PlanResponseModel> Plans { get; set; }

        /// <summary>
        /// Gets or sets the messages list.
        /// </summary>
        public List<string> Messages { get; set; }
    }
}