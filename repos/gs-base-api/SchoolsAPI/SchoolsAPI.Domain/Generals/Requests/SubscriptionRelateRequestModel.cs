using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Dapper;
using SchoolsAPI.SchoolsAPI.Domain.Models.Schools;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Requests
{
    /// <summary>
    /// To handle subscription relations with schools and programs.
    /// </summary>
    public class SubscriptionRelateRequestModel : PermissionSessionRequestModel
    {
        /// <summary>
        /// Gets or sets subscription id.
        /// </summary>
        public string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the school ids.
        /// </summary>
        public List<string> SchoolIds { get; set; }

        /// <summary>
        /// Gets or sets the program ids.
        /// </summary>
        public List<string> ProgramIds { get; set; }

        /// <summary>
        /// Gets or sets the entity academic detail's id.
        /// </summary>
        public string EntityAcademicDetailId { get; set; }

        /// <summary>
        /// Gets or sets the academic period details.
        /// </summary>
        public List<AcademicPeriodModel> AcademicPeriods { get; set; }

        /// <summary>
        /// Gets or sets the academic details as full term start date, full term end date, etc. 
        /// </summary>
        public SchoolsAPI.Domain.Models.Generals.Dapper.EntityAcademicDetailModel EntityAcademicDetail { get; set; }
    }
}