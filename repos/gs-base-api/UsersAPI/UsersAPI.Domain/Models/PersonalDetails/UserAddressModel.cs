namespace UsersAPI.UsersAPI.Domain.Models.PersonalDetails
{
	public class UserAddressModel
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public string PersonAddressName { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public string City { get; set; }
		public string StateIsoCode { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
		public string CountryIsoCode2 { get; set; }
		public string AddressExtra { get; set; }
		public string AddressPhone { get; set; }
		public string MainAddress { get; set; }
		public string Enable { get; set; }
		public string ModifiedBy { get; set; }
		public string CreatedAt { get; set; }
		public string UpdatedAt { get; set; }

	}
}