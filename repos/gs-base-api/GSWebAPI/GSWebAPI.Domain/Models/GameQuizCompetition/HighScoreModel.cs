namespace GSWebAPI.GSWebAPI.Domain.Models.GameQuizCompetition
{
    public class HighScoreModel
    {
        private string _id;
        private string _userId;
        private string _score;
        private string _subjectCode;
        private string _levelCode;
        private string _modifiedBy;
        private string _createdAt;
        private string _updatedAt;

        public string Id { get { return _id; } set { _id = value; } }
        public string UserId { get { return _userId; } set { _userId = value; } }
        public string Score { get { return _score; } set { _score = value; } }
        public string SubjectCode { get { return _subjectCode; } set { _subjectCode = value; } }
        public string LevelCode { get { return _levelCode; } set { _levelCode = value; } }
        public string ModifiedBy { get { return _modifiedBy; } set { _modifiedBy = value; } }
        public string CreatedAt { get { return _createdAt; } set { _createdAt = value; } }
        public string UpdatedAt { get { return _updatedAt; } set { _updatedAt = value; } }

    }
}