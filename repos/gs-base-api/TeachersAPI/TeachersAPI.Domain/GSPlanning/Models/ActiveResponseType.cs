using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSPlanning.Models;

namespace GSPlanning.Models
{
    public class ActiveResponseType
    {
        /// <summary>
        /// Unique identifier of the strategy category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the strategy category.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A friendly text used to describe the strategy category.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The user who modified the strategy category.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the strategy category.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the strategy category was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<ActiveResponseType> ActiveResponseTypes { get; set; } = new List<ActiveResponseType>();

        public virtual ICollection<TeacherPlanActiveResponseDate> TeacherPlanActiveResponseDates { get; set; } = new List<TeacherPlanActiveResponseDate>();
    }
}
