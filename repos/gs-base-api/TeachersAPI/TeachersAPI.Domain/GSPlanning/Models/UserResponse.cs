using System;

namespace GSPlanning.Domain
{
    /// <summary>
    /// To handle the user responses of a quiz/homework created by the teacher.
    /// </summary>
    public class UserResponse
    {
        public int Id { get; set; }

        /// <summary>
        /// User quiz result id.
        /// </summary>
        public int? UserQuizResultId { get; set; }

        /// <summary>
        /// The question&apos;s id of the quiz created by the teacher.
        /// </summary>
        public int? QuestionId { get; set; }

        /// <summary>
        /// The response&apos;s id of the quiz created by the teacher and selected by the student. It can be null, since if it is a free explanation, it is going to be in response extra.
        /// </summary>
        public int? ResponseId { get; set; }

        /// <summary>
        /// 1 if correct, 0 if not. If null, it is because is not an autocorrected quiz.
        /// </summary>
        public int? Correct { get; set; }

        /// <summary>
        /// The student&apos;s response explanation. It could be used in True or False and free quiz type. (as 03.17.2021). 
        /// </summary>
        public string ResponseExtra { get; set; }

        /// <summary>
        /// How much time the user spent answering the question. As 2021.03.17 it is not used for this project.
        /// </summary>
        public float? ResponseTime { get; set; }

        /// <summary>
        /// The assigned value. This is automatically set when the homework is an autocorrection type as select and match. If the homework type is select and match, this is calculated by the total value / total questions. If the homework is not auto-correct, then this value is assigned by the teacher. 
        /// </summary>
        public string AssignedValue { get; set; }

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

        public virtual QuizQuestion Question { get; set; }

        public virtual QuizQuestionResponse Response { get; set; }

        public virtual UserQuizResult UserQuizResult { get; set; }
    }
}
