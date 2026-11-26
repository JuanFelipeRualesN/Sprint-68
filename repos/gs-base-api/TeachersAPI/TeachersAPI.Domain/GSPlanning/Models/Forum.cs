using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the forum&apos;s general details.
    /// </summary>
    public class Forum
    {
        public int Id { get; set; }

        /// <summary>
        /// The teacher&apos;s plan id where the forum is attached.
        /// </summary>
        public int? TeacherPlanId { get; set; }

        /// <summary>
        /// The forum&apos;s title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// If active (1) or not (0).
        /// </summary>
        public int? Active { get; set; }

        /// <summary>
        /// The author (user id).
        /// </summary>
        public int? Author { get; set; }

        /// <summary>
        /// The description of the forum. 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<ForumDate> ForumDates { get; set; } = new List<ForumDate>();

        public virtual ICollection<ForumPost> ForumPosts { get; set; } = new List<ForumPost>();

        public virtual TeacherPlan TeacherPlan { get; set; }
    }
}
