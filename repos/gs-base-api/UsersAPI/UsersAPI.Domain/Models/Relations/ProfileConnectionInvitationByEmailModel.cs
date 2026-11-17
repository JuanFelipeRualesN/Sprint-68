namespace UsersAPI.UsersAPI.Domain.Models.Relations
{
    public class ProfileConnectionInvitationByEmailModel
    {
		public string Id { get; set; }
		public string UserCodeToConnectWith { get; set; }
		public string Email { get; set; }
		public string Token { get; set; }
		public string CreatedBy { get; set; }
		public string ModifiedBy { get; set; }
		public string CreatedAt { get; set; }
		public string UpdatedAt { get; set; }
	}
}
