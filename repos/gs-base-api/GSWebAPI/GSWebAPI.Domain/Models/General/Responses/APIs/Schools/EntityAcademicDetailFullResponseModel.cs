using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSWebAPI.GSWebAPI.Domain.Models.General.Responses.APIs.Schools
{
    /// <summary>
    /// To handle the academic details, including periods.
    /// </summary>
    public class EntityAcademicDetailFullResponseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityAcademicDetailFullResponseModel"/> class.
        /// </summary>
        public EntityAcademicDetailFullResponseModel()
        {
            EntityModel = new EntityModel();
            EntitySubscriptionModel = new EntitySubscriptionModel();
            EntityAcademicDetailModel = new EntityAcademicDetailModel();
            AcademicPeriodModelList = new List<AcademicPeriodModel>();
        }

        /// <summary>
        /// Gets or sets the entity models.
        /// </summary>
        public EntityModel EntityModel { get; set; }

        /// <summary>
        /// Gets or sets the entity subscriptions.
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