namespace TeachersAPI.Domain.Models.Lessons.Response
{
    /// <summary>
    /// To handle response of assignments created by a teacher to let they know if they can edit
    /// the quiz or not.
    /// </summary>
    public class QuizEditModel : QuizModel
    {
        /// <summary>
        /// Gets or sets whether the quiz can be edit or not.
        /// </summary>
        public bool CanBeEdited { get; set; }

        public QuizEditModel()
        {
                
        }

        public QuizEditModel(QuizModel quizModel)
        {
            // Map properties from QuizModel to QuizEditModel
            Valid = quizModel.Valid;
            Messages = quizModel.Messages;
            Id = quizModel.Id;
            LessonId = quizModel.LessonId;
            Title = quizModel.Title;
            Instruction = quizModel.Instruction;
            LevelOfKnowledge = quizModel.LevelOfKnowledge;
            IsExam = quizModel.IsExam;
            QuizTypeId = quizModel.QuizTypeId;
            CustomValue = quizModel.CustomValue;
            ReadyToPublish = quizModel.ReadyToPublish;
            Available = quizModel.Available;
            ModifiedBy = quizModel.ModifiedBy;
            CreatedAt = quizModel.CreatedAt;
            UpdatedAt = quizModel.UpdatedAt;
        }
    }
}