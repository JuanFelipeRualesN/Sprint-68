namespace GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition
{
    public class UserResponseModel
    {
        private string _id;
        private string _userId;
        private string _quizId;
        private string _subjectCode;
        private string _levelCode;
        private string _questionId;
        private string _responseId;
        private string _responseTime;
        private string _correct;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string QuizId { get { return _quizId; } set { _quizId = value; } }
        public string SubjectCode { get { return _subjectCode; } set { _subjectCode = value; } }
        public string LevelCode { get { return _levelCode; } set { _levelCode = value; } }
        public string QuestionId { get { return _questionId; } set { _questionId = value; } }
        public string ResponseId { get { return _responseId; } set { _responseId = value; } }
        public string ResponseTime { get { return _responseTime; } set { _responseTime = value; } }
        public string Correct { get { return _correct; } set { _correct = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}