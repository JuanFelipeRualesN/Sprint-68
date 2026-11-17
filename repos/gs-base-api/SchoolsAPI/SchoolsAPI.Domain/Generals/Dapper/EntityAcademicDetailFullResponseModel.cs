using System.Collections.Generic;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Dapper
{
    public class EntityAcademicDetailFullResponseModel
    {
        public EntityAcademicDetailFullResponseModel()
        {
            EntityModel = new EntityModel();
            EntitySubscriptionModel = new EntitySubscriptionModel();
            EntityAcademicDetailModel = new EntityAcademicDetailModel();
            AcademicPeriodModelList = new List<AcademicPeriodModel>();
        }

        public EntityModel EntityModel { get; set; }
        public EntitySubscriptionModel EntitySubscriptionModel { get; set; }
        public EntityAcademicDetailModel EntityAcademicDetailModel { get; set; }
        public List<AcademicPeriodModel> AcademicPeriodModelList { get; set; }

    }
}