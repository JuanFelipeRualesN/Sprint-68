using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Schools.Response
{
    /// <summary>
    /// To get the full details of an academic as schools, programs, etc. 
    /// </summary>
    public class EntityAcademicDetailFullResponseModel
    {
        public EntityAcademicDetailFullResponseModel()
        {
            EntityModel = new EntityModel();
            EntitySubscriptionModel = new EntitySubscriptionModel();
            EntityAcademicDetailModel = new EntityAcademicDetailModel();
            AcademicDetailModel = new AcademicDetailModel();
            AcademicPeriodModelList = new List<AcademicPeriodModel>();
        }
        /// <summary>
        /// Gets or sets the entity models.
        /// </summary>
        public EntityModel EntityModel { get; set; }

        /// <summary>
        /// Get or sets the academic details.
        /// </summary>
        public AcademicDetailModel AcademicDetailModel { get; set; }

        /// <summary>
        /// Get or sets the entity subscriptions.
        /// </summary>
        public EntitySubscriptionModel EntitySubscriptionModel { get; set; }

        /// <summary>
        /// Get or sets the academic details.
        /// </summary>
        public EntityAcademicDetailModel EntityAcademicDetailModel { get; set; }

        /// <summary>
        /// Gets or sets the academic periods.
        /// </summary>
        public List<AcademicPeriodModel> AcademicPeriodModelList { get; set; }
    }
}
