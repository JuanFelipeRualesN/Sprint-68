using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle Power DE groups
    /// </summary>
    public class Group
    {
        public int Id { get; set; }

        /// <summary>
        /// The group id in Power DE.
        /// </summary>
        public int? GroupInterfaceId { get; set; }

        /// <summary>
        /// The group code (example: 10-5). 
        /// 
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<UserGroup> UserGroups { get; set; } = new List<UserGroup>();
    }
}
