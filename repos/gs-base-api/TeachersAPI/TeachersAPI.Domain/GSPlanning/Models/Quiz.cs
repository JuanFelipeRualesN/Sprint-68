using System;
using System.Collections.Generic;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the general data for quizzes (homework/assignment) created by a teacher.
    /// </summary>
    public class Quiz
    {
        public int Id { get; set; }

        /// <summary>
        /// The Id of the lesson. This was a part of the library but is not used in this project.  It is not used in this project as 03.17.2021
        /// </summary>
        public int? LessonId { get; set; }

        /// <summary>
        /// The quiz&apos;s title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A custom value that is assigned by the teacher for the homework. Example, if the homework has a total of 20 points, then this should be 20.
        /// </summary>
        public int? CustomValue { get; set; }

        /// <summary>
        /// The quiz&apos;s  instruction.
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// The level of knowledge. This was a part of the library, but it is not used in this project.  It is not used in this project as 03.17.2021
        /// </summary>
        public int? LevelOfKnowledge { get; set; }

        /// <summary>
        /// If it is an exam. It is not used in this project as 03.17.2021
        /// </summary>
        public int? IsExam { get; set; }

        /// <summary>
        /// The quiz type id from quiz_types. 
        /// </summary>
        public int? QuizTypeId { get; set; }

        /// <summary>
        /// If the quiz is ready to be published.  It is not used in this project as 03.17.2021
        /// </summary>
        public int? ReadyToPublish { get; set; }

        /// <summary>
        /// If the quiz is available (1) or not (0). 
        /// </summary>
        public int? Available { get; set; }

        /// <summary>
        /// The user who modified the row.
        /// </summary>
        public int? ModifiedBy { get; set; }

        /// <summary>
        /// The datetime of the creation of the row.
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// The datetime when the row was updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<QuizAttachment> QuizAttachments { get; set; } = new List<QuizAttachment>();

        public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();

        public virtual QuizType QuizType { get; set; }

        public virtual ICollection<TeacherPlanQuiz> TeacherPlanQuizzes { get; set; } = new List<TeacherPlanQuiz>();

        public virtual ICollection<UserQuizAuthorizationCode> UserQuizAuthorizationCodes { get; set; } = new List<UserQuizAuthorizationCode>();

        public virtual ICollection<UserQuizFile> UserQuizFiles { get; set; } = new List<UserQuizFile>();

        public virtual ICollection<UserQuizResult> UserQuizResults { get; set; } = new List<UserQuizResult>();
    }
}
