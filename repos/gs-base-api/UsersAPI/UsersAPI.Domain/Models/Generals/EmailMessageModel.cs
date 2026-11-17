using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsersAPI.UsersAPI.Domain.Models.Generals
{
	public class EmailMessageModel
	{
		public string Id { get; set; }
		public string Subject { get; set; }
		public string Header { get; set; }
		public string Body { get; set; }
		public string Url { get; set; }
		public string Footer { get; set; }
		public string LangCode { get; set; }
		public string Type { get; set; }
		public string ModifiedBy { get; set; }
		public string CreatedAt { get; set; }
		public string UpdatedAt { get; set; }

	}
}