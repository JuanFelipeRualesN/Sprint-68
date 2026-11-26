
namespace GSPlanning.Models
{
    public class TeacherPlanObservationDate
    {
        /// <summary>
        /// Unique identifier of the teacher plan observation date.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The related teacher plan observation id.
        /// </summary>
        public int TeacherPlanObservationId { get; set; }

        /// <summary>
        /// Date when the observation took place or is planned.
        /// </summary>
        public DateTime ImplementationDate { get; set; }

        /// <summary>
        /// Identifier of the user who last modified the record.
        /// Maps to column: modified_by
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// Datetime when the record was created.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Datetime when the record was last updated.
        /// </summary>

        public DateTime? UpdatedAt { get; set; }

    }
}
