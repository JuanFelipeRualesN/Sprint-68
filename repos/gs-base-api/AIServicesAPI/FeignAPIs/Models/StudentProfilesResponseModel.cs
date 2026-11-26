namespace GSAIServicesAPI.FeignAPIs.Models
{
    /// <summary>
    /// The subset of the users API <c>/api/profiles</c> response this service needs: the list of
    /// profiles under <c>UserInfo</c>. Every field the users API returns that is not read here is
    /// intentionally left out, since the only thing the alert count needs is each student's id.
    /// </summary>
    public class StudentProfilesResponseModel
    {
        /// <summary>The profiles returned for the requested page.</summary>
        public List<StudentProfileModel> UserInfo { get; set; } = [];
    }

    /// <summary>One profile from the users API, reduced to the id used to scope the alert count.</summary>
    public class StudentProfileModel
    {
        /// <summary>The student's user id — the shard key the prompt log is queried by.</summary>
        public string? UserId { get; set; }
    }
}
