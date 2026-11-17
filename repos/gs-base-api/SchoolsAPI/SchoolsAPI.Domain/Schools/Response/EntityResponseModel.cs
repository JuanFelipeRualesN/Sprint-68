using System.Collections.Generic;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Request;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response
{
    public class EntityResponseModel : ResponseModel
    {
        public int TotalPages { get; set; }
        public List<EntityModifierModel> Entities { get; set; }
    }
}
