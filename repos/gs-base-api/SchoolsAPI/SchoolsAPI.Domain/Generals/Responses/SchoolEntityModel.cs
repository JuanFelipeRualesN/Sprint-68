using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SchoolsAPI.SchoolsAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// To handle the responses of schools.
    /// </summary>
    public class SchoolEntityModel
    {
        /// <summary>
        /// Gets or sets the school entities.
        /// </summary>

        public List<EntityGeneralDetailsResponseModel> SchoolEntities { get; set; }

        /// <summary>
        /// Gets or sets the total pages.
        /// </summary>
        public int PageTotals { get; set; }
    }
}