namespace ReadingAPI.ReadingAPI.Domain.DTOs.Generals
{
    public class PermissionSessionRequestDto
    {
        public string Token { get; set; }
        public string PermissionName { get; set; }
        public string IpAddress { get; set; }
        public string UserId { get; set; }
    }
}
