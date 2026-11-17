using System.Collections.Generic;

namespace TeachersAPI.Domain.Models.Generals.Responses
{
    /// <summary>
    /// Handles homework details.
    /// </summary>
    public class HomeworkDetail : StudentHomeworkDetail
    {
        /// <summary>
        /// Gets or sets the teacher plan lesson id.
        /// </summary>
        public string TeacherPlanLessonId { get; set; }

        /// <summary>
        /// Gets or sets the total questions.
        /// </summary>
        public string TotalQuestions { get; set; }

        /// <summary>
        /// Gets or sets the total correct.
        /// </summary>
        public string TotalCorrect { get; set; }

        /// <summary>
        /// Gets or sets the total obtained by the student.
        /// </summary>
        public string TotalPercent { get; set; }

        /// <summary>
        /// Gets or sets the student name.
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// Gets or sets the current tries available.
        /// </summary>
        public int CurrentAvailableTries { get; set; }
        
        /// <summary>
        /// Gets or sets whether the homework was manually corrected by the teacher.
        /// </summary>
        public bool ManuallyCorrectedByTeacher { get; set; }
        
        /// <summary>
        /// Gets or sets the maximum score.
        /// </summary>
        public string MaxScore { get; set; }
        
        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        public string Score { get; set; }

    }
}