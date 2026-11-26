using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the lesson averages (Not used in this API).
    /// </summary>
    public class UserLessonAverage
    {
        public int Id { get; set; }

        /// <summary>
        /// The user&apos;s id lesson averages. This was a part of the library, but it is not used in this project as 03.17.2021. This comes from gs_users_db_prod user&apos;s account table. 
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// The lesson&apos;s id. This is not used in this API. 
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// The user&apos;s average for the lesson. This is not used in this API. 
        /// </summary>
        public double? Average { get; set; }

        /// <summary>
        /// If the lesson was completed. This is not used in this API. 
        /// </summary>
        public int? Completed { get; set; }

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
    }
}
