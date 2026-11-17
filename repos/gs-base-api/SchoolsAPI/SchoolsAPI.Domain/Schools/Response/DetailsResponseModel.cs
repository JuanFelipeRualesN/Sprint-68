using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response
{
    public class DetailsResponseModel : Request.EntityModifierModel
    {
        public List<EntityModel> Programs { get; set; }
        public List<EntityModel> Schools { get; set; }
        public List<EntityModel> Institutions { get; set; }
        public List<EntityModel> Companies { get; set; }
    }
}
