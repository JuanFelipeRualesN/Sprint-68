namespace UsersAPI.UsersAPI.Domain.Models.Relations
{
	public class ProfileConnectionRequestModel
	{
		public string Id { get; set; }
		public string UserRequesterId { get; set; }
		public string UserRequestedId { get; set; }
		public string ModifiedBy { get; set; }
		public string CreatedAt { get; set; }
		public string UpdatedAt { get; set; }

	}
}