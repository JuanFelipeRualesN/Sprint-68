using System.Collections.Generic;
using TeachersAPI.Domain.Models.Teachers;
using TeachersAPI.Domain.Models.FeignAPI;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    public class TeacherPlanningDetailResponseModel
    {
        public TeacherPlanningDetailResponseModel()
        {
            LessonAttachedModelList = new List<PlanningAttachedDetailsModel>();
            QuizzesAttachedModelList = new List<PlanningAttachedDetailsModel>();
            GroupsAttachedList = new List<TeacherPlanGroupResponseModel>();
            Messages = new List<string>();            
        }

        /// <summary>
        /// Gets or sets the teacher plan.
        /// </summary>
        public TeacherPlanModel TeacherPlanModel { get; set; }

        /// <summary>
        /// Gets or sets the lesson model list. 
        /// </summary>
        public List<PlanningAttachedDetailsModel> LessonAttachedModelList { get; set; }

        /// <summary>
        /// Gets or sets the quizzes model list. 
        /// </summary>
        public List<PlanningAttachedDetailsModel> QuizzesAttachedModelList { get; set; }

        /// <summary>
        /// Gets or sets the groups attached.
        /// </summary>
        public List<TeacherPlanGroupResponseModel> GroupsAttachedList { get; set; }

        /// <summary>
        /// To get the full academic period details.
        /// </summary>
        public FeignAPI.AcademicDetailRequestModel AcademicPeriodFullDetail { get; set; }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Total lessons for planning
        /// </summary>
        public int TotalLessons { get; set; } = 0;

        /// <summary>
        /// Total Homeworks for planning
        /// </summary>
        public int TotalAssignments { get; set; } = 0;
    }

    /// <summary>
    /// To handle the teachers' groups.
    /// </summary>
    public class TeacherPlanGroupResponseModel
    {
        /// <summary>
        /// Gets or sets the teacher's plan group id.
        /// </summary>
        public string TeacherPlanGroupId { get; set; }

        /// <summary>
        /// Gets or sets the group's id.
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Gets or sets the group's name.
        /// </summary>
        public string GroupName { get; set; }
    }

    public class PlanningAttachedDetailsModel
    {
        /// <summary>
        /// Gets or sets the ID of the Item
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the item Id. 
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the DueDate
        /// </summary>
        public string DueDate { get; set; }

        /// <summary>
        /// Gets or sets the due date timezone.
        /// </summary>
        public string DueDateTimezone { get; set; }

        /// <summary>
        /// Gets or sets if it is for grading or not.
        /// </summary>
        public string ForGrading { get; set; }

        /// <summary>
        /// Gets or sets the tries quantity.
        /// </summary>
        public string TriesQuantity { get; set; }
        
        /// <summary>
        /// Gets or sets if is gap closing.
        /// </summary>
        public string isGapClosing { get; set; }
        
        /// <summary>
        /// Gets or sets if is required Pre and Post test
        /// </summary>
        public string PrePostRequired { get; set; }

        /// <summary>
        /// Gets or sets the teacher plan date models.
        /// </summary>
        public List<TeacherPlanLessonDateModel> TeacherPlanLessonDateModels { get; set; } = new List<TeacherPlanLessonDateModel>();

        /// <summary>
        /// Gets or sets the plan lesson quizzes.
        /// </summary>
        public List<Domain.Models.Teachers.TeacherPlanLessonQuizModel> TeacherPlanLessonQuizModels { get; set; }

    }
}