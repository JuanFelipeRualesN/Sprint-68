using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.FeignAPI
{
    /// <summary>
    /// To handle the subscription response model.
    /// </summary>
    public class SubscriptionResponseModel
    {
        /// <summary>
        /// Gets or sets the susbcription type.
        /// </summary>
        public string SubscriptionType { get; set; }

        /// <summary>
        /// Gets or sets the courses.
        /// </summary>
        public List<UserSubscribedLevelsSubjectsModel> UserSubscribedLevelsSubjectsModelList { get; set; }
    }

    /// <summary>
    /// To handle the user subscription levels and subjects model.
    /// </summary>
    public class UserSubscribedLevelsSubjectsModel
    {
        public string Name { get; set; }
        public string ProductId { get; set; }
        public string SubjectName { get; set; }
        public string SubscriptionType { get; set; }
        public bool Valid { get; set; }
        public string Messages { get; set; }
        public string Id { get; set; }
        public bool Available { get; set; }
        public string Subject { get; set; }
        public string Level { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}