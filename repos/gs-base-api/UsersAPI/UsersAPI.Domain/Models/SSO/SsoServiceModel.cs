namespace UsersAPI.UsersAPI.Domain.Models.SSO
{
    public class SsoServiceModel
    {
        public string Id { get; set; }
        public string CloudServicePlatform { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string Domain { get; set; }
        public string OpenIdConfigUrl { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

    }
}