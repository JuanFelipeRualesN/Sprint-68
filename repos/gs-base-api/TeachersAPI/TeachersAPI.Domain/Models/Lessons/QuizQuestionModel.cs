namespace TeachersAPI.Domain.Models.Lessons
{
    public class QuizQuestionModel 
    {
        private string _id;
        private string _quizId;
        private string _questionTypeId;
        private string _desc;
        private string _quizMatchInstructionId;
        private string _customValue;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;
        public bool Valid { get; set; }
        public System.Collections.Generic.List<string> Messages { get; set; }

        public string Id { get { return _id; } set { _id = value; } }
        public string QuizId { get { return _quizId; } set { _quizId = value; } }
        public string QuestionTypeId { get { return _questionTypeId; } set { _questionTypeId = value; } }
        public string Desc { get { return _desc; } set { _desc = value; } }
        public string QuizMatchInstructionId { get { return _quizMatchInstructionId; } set { _quizMatchInstructionId = value; } }
        public string CustomValue { get { return _customValue; } set { _customValue = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }
    }
}