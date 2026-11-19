using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the available dates for the forums
    /// </summary>
    public class ForumDate
    {
        public int Id { get; set; }

        /// <summary>
        /// Forum’s id
        /// </summary>
        public int ForumId { get; set; }

        /// <summary>
        /// The weekday is available the forum
        /// </summary>
        public DateTime ImplementationDate { get; set; }

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
