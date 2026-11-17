using System;

namespace GSWebAPI.AccessLogs.Domain.Models
{

    /// <summary>
    /// Request model for access event logs.
    /// </summary>
    public class EventLogsRequestModel
    {
        /// <summary>
        /// User id
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// User invitation code.
        /// </summary>
        public string InvitationCode { get; set; }

        /// <summary>
        /// Role of the user (Student, Teacher, Director).
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Id of the access area.
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// Id of the lesson.
        /// </summary>
        public string LessonId { get; set; } 

        /// <summary>
        /// Title of the lesson.
        /// </summary>
        public string LessonTitle { get; set; } 

        /// <summary>
        /// Subject code or name.
        /// </summary>
        public string Subject { get; set; } 

        /// <summary>
        /// Grade or level.
        /// </summary>
        public string Grade { get; set; } 

        /// <summary>
        /// Additional details or context.
        /// </summary>
        public string Details { get; set; } 

        /// <summary>
        /// Id of the plan.
        /// </summary>
        public string PlanId { get; set; } 

        /// <summary>
        /// Name of the plan.
        /// </summary>
        public string PlanName { get; set; } 

        /// <summary>
        /// Id of the assignment.
        /// </summary>
        public string AssignmentId { get; set; } 

        /// <summary>
        /// Title of the assignment.
        /// </summary>
        public string AssignmentTitle { get; set; } 

        /// <summary>
        /// Id of the forum.
        /// </summary>
        public string ForumId { get; set; } 

        /// <summary>
        /// Title of the forum.
        /// </summary>
        public string ForumTitle { get; set; }

        /// <summary>
        /// Indicates if the event is related to Unity
        /// </summary>
        public bool IsUnity { get; set; }

        /// <summary>
        /// Project enviroment origin (private-beta, private, msj-beta, msj)
        /// </summary>
        public string ProjectEnvironment { get; set; }

        /// <summary>
        /// Id of subscription
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Id of the plan subscription.
        /// </summary>
        public string PlanSubscriptionId { get; set; }

        /// <summary>
        /// Name of the plan subscription.
        /// </summary>
        public string PlanSubscriptionName { get; set; }

        /// <summary>
        /// Quantity of enrollments for the plan.
        /// </summary>
        public string QuantityEnrollment { get; set; }

        /// <summary>
        /// Information about student profile like UserId, Role, InvitationCode, UserName, GiveName, SurName.
        /// </summary>
        public ProfileInformationModel ProfileInformation { get; set; }

        /// <summary>
        /// Information when the new users try to register and payment like GiveName, SurName, Email, Country, State (if applicable).
        /// </summary>
        public ClientInformationModel ClientInformation { get; set; }

        /// <summary>
        /// Date and time when the record was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }


    public class ProfileInformationModel
    {
        /// <summary>
        /// User ID of the student profile.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Role of the user (Student, Teacher, Director).
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Invitation code of the user.
        /// </summary>
        public string InvitationCode { get; set; }

        /// <summary>
        /// User name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Given name of the user.
        /// </summary>
        public string GiveName { get; set; }

        /// <summary>
        /// Surname of the user.
        /// </summary>
        public string SurName { get; set; }
    }

    public class ClientInformationModel
    {
        /// <summary>
        /// Given name of the client.
        /// </summary>
        public string GiveName { get; set; }

        /// <summary>
        /// Surname of the client.
        /// </summary>
        public string SurName { get; set; }

        /// <summary>
        /// Email of the client.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Country of the client.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// State of the client (if applicable).
        /// </summary>
        public string State { get; set; }
    }

}