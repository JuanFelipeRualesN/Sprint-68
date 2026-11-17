namespace GSWebAPI.GSWebAPI.Domain.Models.Lesson
{
    public class QuizQuestionResponseModel
    {
        private string _id;
        private string _quizQuestionId;
        private string _response;
        private string _correct;
        private string _value;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string QuizQuestionId { get { return _quizQuestionId; } set { _quizQuestionId = value; } }
        public string Response { get { return _response; } set { _response = value; } }
        public string Correct { get { return _correct; } set { _correct = value; } }
        public string Value { get { return _value; } set { _value = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}