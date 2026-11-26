namespace GSAIServicesAPI.FeignAPIs.Models
{
    /// <summary>
    /// Represents a curriculum unit from the Athenas DB.
    /// </summary>
    public class UnitModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the unit.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the unit's name, which is its number.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unit's description.
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        /// Gets or sets the level's code.
        /// </summary>
        public string LevelCode { get; set; }

        /// <summary>
        /// Gets or sets the subject's code.
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the unit's duration.
        /// </summary>
        public string Duration { get; set; }
    }
}
