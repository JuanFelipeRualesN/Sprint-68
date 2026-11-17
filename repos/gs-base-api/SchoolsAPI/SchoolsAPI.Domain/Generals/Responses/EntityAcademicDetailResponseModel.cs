using System;
using System.Collections.Generic;
using SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Dapper;

namespace SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Responses
{
    public class EntityAcademicDetailResponseModel
    {
        /// <summary>
        /// Gets or sets the page totals (of a list of subscription).
        /// </summary>
        public int PageTotals { get; set; }

        /// <summary>
        /// Gets or sets the entity academic details models.
        /// </summary>
        public List<EntityAcademicDetailModel> EntityAcademicDetailModels { get; set; }
    }
}