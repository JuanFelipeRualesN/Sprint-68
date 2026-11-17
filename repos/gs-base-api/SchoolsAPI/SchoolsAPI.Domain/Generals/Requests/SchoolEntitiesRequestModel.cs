using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Request;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// School entities request to filter by entities details.
    /// </summary>
    public class SchoolEntitiesRequestModel
    {
        /// <summary>
        /// Gets or sets entity modifier to filter.
        /// </summary>
        public EntityModifierModel EntityModifier { get; set; }

        /// <summary>
        /// Gets or sets page from.
        /// </summary>
        public int PageFrom { get; set; }

        /// <summary>
        /// Gets or sets the limiter.
        /// </summary>
        public int Limiter { get; set; }
    }
}