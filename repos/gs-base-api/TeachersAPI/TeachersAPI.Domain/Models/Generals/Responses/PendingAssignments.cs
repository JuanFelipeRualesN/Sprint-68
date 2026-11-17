using TeachersAPI.Domain.Models.Generals.Responses.APIs;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// pending assignmet details
    /// </summary>
    public class PendingAssignments
    {
        /// <summary>
        /// Gets or sets the unique identifier for the plan.
        /// </summary>
        public string PlanId { get; set; }
        
        /// <summary>
        /// Gets or sets the unique identifier for the plan quiz.
        /// </summary>
        public string PlanQuizId { get; set; }
        
        /// <summary>
        /// Gets or sets the unique identifier for the teacher.
        /// </summary>
        public string TeacherId { get; set; }
        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        public string PlanName { get; set; }
        /// <summary>
        /// Gets or sets the name of the assignment.
        /// </summary>
        public string AssignmentName { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the assignment.
        /// </summary>
        public string AssignmentId { get; set; }
        /// <summary>
        /// Gets or sets the submitted date.
        /// </summary>
        public string DateSubmitted { get; set; }
        /// <summary>
        /// Gets or sets the student.
        /// </summary>
        public UserResponse Student { get; set; }
    }
}