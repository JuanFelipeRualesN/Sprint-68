namespace ReadingAPI.ReadingAPI.Domain.DTOs.Generals
{
    public class ValidationResponseDto
    {
        public string IpAddress { get; set; }
        public string LangCode { get; set; }
        public bool SessionValid { get; set; }
        public List<string> Messages { get; set; }
        public string UserId { get; set; }
        public bool IsStaff { get; set; }
        public bool IsDirector { get; set; }
        public string Token { get; set; }
        public string RoleId { get; set; }
        public string OreId { get; set; }
        public string UserTimeZone { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
