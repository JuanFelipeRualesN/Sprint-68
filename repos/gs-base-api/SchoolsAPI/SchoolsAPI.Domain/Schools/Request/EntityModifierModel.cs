using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Request
{
    public class EntityModifierModel
    {
        public List<EntityModel> Entities { get; set; }
        public List<PhoneModel> Phones { get; set; }
        public List<AddressModel> Addresses { get; set; }
        public List<RelationModel> Relations { get; set; }
        public List<EntitySubscriptionModel> Subscriptions { get; set; }
    }
}
