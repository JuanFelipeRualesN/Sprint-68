namespace GSPlanning.Models.Translations
{
    public class EvaluationTypeTranslation
    {
        /// <summary>
        /// Unique identifier of the accommodation type.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identifier of the accommodation type.
        /// </summary>
        public int EvaluationTypeId { get; set; }

        /// <summary>
        /// Language code of the translation.
        /// </summary>
        public string Lang { get; set; }

        /// <summary>
        /// The name of the accommodation type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A friendly text used to describe the accommodation type.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The user who modified the accommodation type.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the accommodation type.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the accommodation type was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
