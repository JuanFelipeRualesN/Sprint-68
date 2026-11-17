using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Pending assignments response model
    /// </summary>
    public class PendingAssignmentsResponseModel
    {
        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// Gets or sets pending assignments.
        /// </summary>
        public List<PendingAssignments> PendingAssignments { get; set; }
    }
}