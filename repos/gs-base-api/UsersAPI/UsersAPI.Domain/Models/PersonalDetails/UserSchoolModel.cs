using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersAPI.UsersAPI.Domain.Models.PersonalDetails
{
	public class UserSchoolModel
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public string SchoolId { get; set; }
		public string Identifier { get; set; }
		public string Confirmed { get; set; }
		public string ModifiedBy { get; set; }
		public string CreatedAt { get; set; }
		public string UpdatedAt { get; set; }

	}
}
