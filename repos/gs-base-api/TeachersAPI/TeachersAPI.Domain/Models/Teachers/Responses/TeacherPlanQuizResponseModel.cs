using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersAPI.Domain.Models.Teachers.Responses
{
    /// <summary>
    /// To handle teacher plan Quiz Response details
    /// </summary>
    public class TeacherPlanQuizResponseModel : TeacherPlanQuizModel
    {
        /// <summary>
        /// Gets or sets the teacher plan model details
        /// </summary>
        public TeacherPlanModel TeacherPlanModel { get; set; }

        /// <summary>
        /// Gets or sets the subject code.
        /// </summary>
       // public string SubjectCode { get; set; }

        /// <summary>
        /// Gets or sets the level code.
        /// </summary>
        // public string LevelCode { get; set; }

        /// <summary>
        /// Gets or sets the current available tries.
        /// </summary>
        public int CurrentAvailableTries { get; set; }

        /// <summary>
        /// Gets or sets the tries used. 
        /// </summary>
        public int TriesUsed { get; set; }

        /// <summary>
        /// Gets or sets the user id of the plan created.
        /// </summary>
      //  public string UserIdPlanCreated { get; set; }

        /// <summary>
        /// Gets or sets the assignment results list. (Added on 2021.10.06 by POR. Only applies for performance improvement logic). 
        /// </summary>
        public List<PlanAssignmentUserResultModel> AssignmentResults { get; set; }

        /// <summary>
        /// To get or set the student Id.
        /// </summary>
        public string StudentId { get; set; }
    }
}
