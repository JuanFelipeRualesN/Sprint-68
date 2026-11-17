namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class QuizQuestionModel
    {
        private string _id;
        private string _quizId;
        private string _desc;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string QuizId { get { return _quizId; } set { _quizId = value; } }
        public string Desc { get { return _desc; } set { _desc = value; } }
        public string LearningSheetId { get; set; }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}