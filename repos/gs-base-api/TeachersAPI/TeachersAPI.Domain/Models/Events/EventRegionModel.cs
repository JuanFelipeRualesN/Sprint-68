namespace TeachersAPI.Domain.Models.Events
{
    public class EventRegionModel
    {
        public string Id { get; set; }
        public string EventId { get; set; }

        /// <summary>
        /// The region (ORE) id, as returned by SchoolsAPI's api/regions. Resolved to member
        /// SchoolIds via SchoolsAPI's api/ore/{oreId}/schools_ids — see EventAudienceResolver
        /// and SchoolsDetailsHandler.GetSchoolIdsForRegion.
        /// </summary>
        public string RegionId { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
    }
}
