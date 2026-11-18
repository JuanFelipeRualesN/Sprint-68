using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// The users&apos; groups. 
    /// </summary>
    public class UserGroup
    {
        public int Id { get; set; }

        /// <summary>
        /// The group interface id from Power DE. 
        /// </summary>
        public int? PdeGroupId { get; set; }

        /// <summary>
        /// The user&apos;s id (the student&apos;s id or the teacher&apos;s id). 
        /// </summary>
        public int? UserId { get; set; }

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

        public virtual Group PdeGroup { get; set; }
    }
}
