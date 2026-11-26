using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the posts created in a forum.
    /// </summary>
    public class ForumPost
    {
        public int Id { get; set; }

        /// <summary>
        /// The forum&apos;s id.
        /// </summary>
        public int? ForumId { get; set; }

        /// <summary>
        /// The author&apos;s id.
        /// </summary>
        public int? Author { get; set; }

        /// <summary>
        /// If the forum post is available or not. If not available, it supposed to not be shown in frontend.
        /// </summary>
        public int? Available { get; set; }

        /// <summary>
        /// The forum post message.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// If the post is a reply to another post, this will be the id of the forum posts.
        /// </summary>
        public int? ParentPostId { get; set; }

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

        public virtual Forum Forum { get; set; }
    }
}
