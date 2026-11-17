using System;
using Newtonsoft.Json;

namespace GSWebAPI.AccessLogs.Domain.Models
{

    /// <summary>
    /// Represents a model for event logs
    /// </summary>
    public class EventLogsModel
    {
        [JsonProperty("user-name")]
        public string UserName { get; set; } = null;

        [JsonProperty("invitation-code")]
        public string InvitationCode { get; set; } = null;

        [JsonProperty("role")]
        public string Role { get; set; } = null;

        [JsonProperty("area-id")]
        public string AreaId { get; set; } = null;

        /// <summary>
        /// Identifier for the lesson
        /// </summary>
        [JsonProperty("lesson-id")]
        public string LessonId { get; set; } = null;

        /// <summary>
        /// Title of the lesson
        /// </summary>
        [JsonProperty("lesson-title")]
        public string LessonTitle { get; set; } = null;

        /// <summary>
        /// Subject of the lesson
        /// </summary>
        [JsonProperty("subject")]
        public string Subject { get; set; } = null;

        /// <summary>
        /// Grade level of the lesson
        /// </summary>
        [JsonProperty("grade")]
        public string Grade { get; set; } = null;

        /// <summary>
        /// Additional details
        /// </summary>
        [JsonProperty("details")]
        public string Details { get; set; } = null;

        /// <summary>
        /// Identifier for the plan
        /// </summary>
        [JsonProperty("plan-id")]
        public string PlanId { get; set; } = null;

        /// <summary>
        /// Name of the plan
        /// </summary>
        [JsonProperty("plan-name")]
        public string PlanName { get; set; } = null;

        /// <summary>
        /// Identifier for the assignment
        /// </summary>
        [JsonProperty("assignment-id")]
        public string AssignmentId { get; set; } = null;

        /// <summary>
        /// Title of the assignment
        /// </summary>
        [JsonProperty("assignment-title")]
        public string AssignmentTitle { get; set; } = null;

        /// <summary>
        /// Identifier for the forum
        /// </summary>
        [JsonProperty("forum-id")]
        public string ForumId { get; set; } = null;

        /// <summary>
        /// Title of the forum
        /// </summary>
        [JsonProperty("forum-title")]
        public string ForumTitle { get; set; } = null;

        /// <summary>
        /// Indicates if the event is related to Unity
        /// </summary>
        [JsonProperty("isUnity")]
        public bool IsUnity { get; set; } = false;

        /// <summary>
        /// Project enviroment origin (private-beta, private, msj-beta, msj)
        /// </summary>
        [JsonProperty("project-environment")]
        public string ProjectEnvironment { get; set; } = null;

        /// <summary>
        /// Id of subscription
        /// </summary>
        [JsonProperty("subscription-id")]
        public string SubscriptionId { get; set; } = null;

        /// <summary>
        /// Id of the plan subscription.
        /// </summary>
        [JsonProperty("plan-subscription-id")]
        public string PlanSubscriptionId { get; set; } = null;

        /// <summary>
        /// Name of the plan subscription.
        /// </summary>
        [JsonProperty("plan-subscription-name")]
        public string PlanSubscriptionName { get; set; } = null;

        /// <summary>
        /// Quantity of enrollments for the plan.
        /// </summary>
        [JsonProperty("quantity-enrollment")]
        public string QuantityEnrollment { get; set; } = null;

        /// <summary>
        /// Information about student profile like UserId, Role, InvitationCode, UserName, GiveName, SurName.
        /// </summary>
        [JsonProperty("profile-information")]
        public ProfileInformationModel ProfileInformation { get; set; } = null;

        /// <summary>
        /// Information when the new users try to register and payment like GiveName, SurName, Email, Country, State (if applicable).
        /// </summary>
        [JsonProperty("client-information")]
        public ClientInformationModel ClientInformation { get; set; } = null;

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }


        [JsonProperty("correlation-id")]
        public string CorrelationId { get; set; }

    }
}
