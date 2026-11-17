using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeachersAPI.Domain.Models.Lessons
{
	public class CodeModel : IResponse
	{
            public bool Valid { get; set; }
            public System.Collections.Generic.List<string> Messages { get; set; }
            public string Id { get; set; }
            public string Code { get; set; }
            public string Description { get; set; }
            public string RelationCodeId { get; set; }
            public string TypeId { get; set; }
            public string ModifiedBy { get; set; }
            public string CreatedAt { get; set; }
            public string UpdatedAt { get; set; }
    }
}